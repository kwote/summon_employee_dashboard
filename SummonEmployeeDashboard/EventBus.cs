using EvtSource;
using Newtonsoft.Json;
using SummonEmployeeDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    public class EventBus
    {
        private Subject<SummonRequestUpdate> subject = new Subject<SummonRequestUpdate>();
        private IObservable<SummonRequestUpdate> eventBus;
        private EventSourceReader evt;

        public EventBus()
        {
            eventBus = subject.AsObservable();
        }

        private const string Format = "{{\"where\":{{\"or\":[{{\"targetId\":{0}}},{{\"callerId\":{1}}}]}}}}";

        public async void Initialize(AccessToken accessToken)
        {
            if (evt != null) return;
            var condition = string.Format(Format, accessToken.UserId, accessToken.UserId);
            Uri url = new Uri(
                App.GetApp().URL + "summonrequests/change-stream?access_token=" + accessToken.Id + "&options=" + condition
            );
            evt = new EventSourceReader(url);
            evt.MessageReceived += (object sender, EventSourceMessageEventArgs e) => {
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
                evt.Start(); // Reconnect to the same URL
            };
            await Task.Run(() => { evt = evt.Start(); });
        }

        public void Dispose()
        {
            if (evt == null) return;
            evt.Dispose();
            evt = null;
        }

        public IDisposable Subscribe(IObserver<SummonRequestUpdate> observer)
        {
            return eventBus.Subscribe(observer);
        }
    }
}
