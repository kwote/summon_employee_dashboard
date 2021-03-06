﻿using EvtSource;
using Newtonsoft.Json;
using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using SummonEmployeeDashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    public class EventBus
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EventBus));
        private Subject<SummonRequestUpdate> subject = new Subject<SummonRequestUpdate>();
        private Subject<string> messageBus = new Subject<string>();
        // TODO add message bus
        private EventSourceReader evt;
        private bool connected = false;

        private CancellationTokenSource pingToken = new CancellationTokenSource();

        public EventBus()
        {
        }

        private const string Format = "{{\"where\":{{\"or\":[{{\"targetId\":{0}}},{{\"callerId\":{1}}}]}}}}";
        public const int PING_PERIOD = 60;

        public async void Initialize(AccessToken accessToken)
        {
            CloseConnection();
            await OpenConnection(accessToken);
            SchedulePing();
        }

        private int pingCounter = 0;
        private const int RECONNECT_PINGS = 10;
        public const string DISCONNECTED = "disconnected";

        private void SchedulePing()
        {
            pingToken.Cancel();
            pingToken = new CancellationTokenSource();
            IObservable<long> observable = Observable.Interval(TimeSpan.FromSeconds(PING_PERIOD));

            // Subscribe the observable to the task on execution.
            observable.Subscribe(async x => {
                var accessToken = App.GetApp().AccessToken;
                pingCounter++;
                if (pingCounter == RECONNECT_PINGS)
                {
                    pingCounter = 0;
                    CloseConnection();
                    await OpenConnection(accessToken);
                    return;
                }
                var valid = false;
                try
                {
                    valid = await App.GetApp().GetService<PeopleService>().Ping(accessToken.Id);
                } catch (Exception e)
                {
                    log.Error("Ping failed", e);
                    valid = false;
                }
                if (valid && !connected)
                {
                    await OpenConnection(accessToken);
                } else if (!valid)
                {
                    CloseConnection();
                    messageBus.OnNext(DISCONNECTED);
                }
            }, pingToken.Token);
        }

        private async Task OpenConnection(AccessToken accessToken)
        {
            var condition = string.Format(Format, accessToken.UserId, accessToken.UserId);
            Uri url = new Uri(
                App.GetApp().URL + "summonrequests/change-stream?access_token=" + accessToken.Id + "&options=" + condition
            );
            evt = new EventSourceReader(url);
            evt.MessageReceived += (object sender, EventSourceMessageEventArgs e) =>
            {
                Console.WriteLine($"{e.Event} : {e.Message}");
                var message = JsonConvert.DeserializeObject<SummonRequestMessage>(e.Message);
                if (message != null)
                {
                    var request = message.Request();
                    var update = new SummonRequestUpdate { Request = request };
                    if (request.TargetId == accessToken.UserId)
                    {
                        switch (message.MessageType)
                        {
                            case "create":
                                update.UpdateType = UpdateType.Create;
                                break;
                            case "cancel":
                                update.UpdateType = UpdateType.Cancel;
                                break;
                            default:
                                return;
                        }
                    }
                    else if (request.CallerId == accessToken.UserId)
                    {
                        switch (message.MessageType)
                        {
                            case "accept":
                                update.UpdateType = UpdateType.Accept;
                                break;
                            case "reject":
                                update.UpdateType = UpdateType.Reject;
                                break;
                            default:
                                return;
                        }
                    }
                    else return;
                    subject.OnNext(update);
                }
            };
            evt.Disconnected += async (object sender, DisconnectEventArgs e) =>
            {
                Console.WriteLine($"Retry: {e.ReconnectDelay} - Error: {e.Exception.Message}");
                await Task.Delay(e.ReconnectDelay);
                evt = evt?.Start(); // Reconnect to the same URL
            };
            await Task.Run(() => { evt = evt?.Start(); });
            connected = true;
        }

        private void CloseConnection()
        {
            connected = false;

            evt?.Dispose();
            evt = null;
        }

        public void Dispose()
        {
            CloseConnection();
            messageBus.OnNext("disconnected");
            pingToken.Cancel();
        }

        public IDisposable Subscribe(IObserver<SummonRequestUpdate> observer)
        {
            return subject.Subscribe(observer);
        }

        public IDisposable SubscribeToMessage(IObserver<string> observer)
        {
            return messageBus.Subscribe(observer);
        }
    }
}
