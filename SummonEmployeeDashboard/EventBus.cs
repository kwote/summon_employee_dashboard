using EventSource4Net;
using Newtonsoft.Json;
using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    public class EventBus
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EventBus));
        public static EventBus Instance { get { return instance ?? (instance = new EventBus()); } }

        public void Register(object listener)
        {
            if (!listeners.Any(l => l.Listener == listener))
            {
                listeners.Add(new EventListenerWrapper(listener));
            }
        }

        public void Unregister(object listener)
        {
            listeners.RemoveAll(l => l.Listener == listener);
        }

        public void PostEvent(object e)
        {
            listeners.Where(l => l.EventType == e.GetType()).ToList().ForEach(l => l.PostEvent(e));
        }

        private static EventBus instance;

        private EventBus() { }

        private List<EventListenerWrapper> listeners = new List<EventListenerWrapper>();

        private class EventListenerWrapper
        {
            public object Listener { get; private set; }
            public Type EventType { get; private set; }

            private MethodBase method;

            public EventListenerWrapper(object listener)
            {
                Listener = listener;

                Type type = listener.GetType();

                method = type.GetMethod("OnEvent");
                if (method == null)
                {
                    throw new ArgumentException("Class " + type.Name + " does not contain method OnEvent");
                }

                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length != 1)
                {
                    throw new ArgumentException("Method OnEvent of class " + type.Name + " have invalid number of parameters (should be one)");
                }

                EventType = parameters[0].ParameterType;
            }

            public void PostEvent(object e)
            {
                method.Invoke(Listener, new[] { e });
            }
        }
        // TODO add message bus
        private EventSource evt;
        private bool connected = false;

        private CancellationTokenSource listenToken = new CancellationTokenSource();

        private Timer pingTimer;

        private const string Format = "{{\"where\":{{\"or\":[{{\"targetId\":{0}}},{{\"callerId\":{1}}}]}}}}";
        public const int PING_PERIOD = 60;

        public void Initialize(AccessToken accessToken)
        {
            CloseConnection();
            OpenConnection(accessToken);
            SchedulePing();
        }

        private void SchedulePing()
        {
            pingTimer?.Dispose();
            var autoEvent = new AutoResetEvent(false);
            pingTimer = new Timer(o =>
            {
                Task.Factory.StartNew(() =>
                {
                    App app = App.GetApp();
                    var accessToken = app.AccessToken;
                    var valid = Ping(accessToken.Id, app);
                    if (valid && !connected)
                    {
                        OpenConnection(accessToken);
                    } else if (!valid && connected)
                    {
                        CloseConnection();
                        /*syncContext.Post(o =>
                        {
                            Login();
                        }, null);*/
                    }
                });
            }, autoEvent, PING_PERIOD * 1000, PING_PERIOD * 1000);
        }

        private bool Ping(string accessToken, App app)
        {
            try
            {
                return app.GetService<PeopleService>().Ping(accessToken);
            } catch (Exception e)
            {
                log.Error(e);
            }
            return false;
        }

        private void OpenConnection(AccessToken accessToken)
        {
            var condition = string.Format(Format, accessToken.UserId, accessToken.UserId);
            Uri url = new Uri(
                App.GetApp().URL + "summonrequests/change-stream?access_token=" + accessToken.Id + "&options=" + condition
            );
            evt = new EventSource(url, 30);
            evt.EventReceived += (object sender, ServerSentEventReceivedEventArgs e) =>
            {
                Console.WriteLine($"{e.Message}");
                var message = JsonConvert.DeserializeObject<SummonRequestMessage>(e.Message.Data);
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
                    Instance.PostEvent(update);
                }
            };
            listenToken = new CancellationTokenSource();
            Task.Factory.StartNew(() => { evt?.Start(listenToken.Token); });
            connected = true;
        }

        private void CloseConnection()
        {
            connected = false;

            listenToken.Cancel();
            evt = null;
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
