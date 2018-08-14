using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace EventSource4Net
{
    class ConnectingState : IConnectionState
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(ConnectingState));

        private Uri mUrl;
        private IWebRequesterFactory mWebRequesterFactory;
        public EventSourceState State { get { return EventSourceState.CONNECTING; } }
        
        public ConnectingState(Uri url, IWebRequesterFactory webRequesterFactory)
        {
            mUrl = url ?? throw new ArgumentNullException("Url cant be null");
            mWebRequesterFactory = webRequesterFactory ?? throw new ArgumentNullException("Factory cant be null");
        }

        public Task<IConnectionState> Run(Action<ServerSentEvent> donothing, CancellationToken cancelToken)
        {
            IWebRequester requester = mWebRequesterFactory.Create();
            var taskResp = requester.Get(mUrl);

            return taskResp.ContinueWith<IConnectionState>(tsk => 
            {
                if (tsk.Status == TaskStatus.RanToCompletion && !cancelToken.IsCancellationRequested)
                {
                    IServerResponse response = tsk.Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return new ConnectedState(response, mWebRequesterFactory);
                    }
                    else
                    {
                        _logger.Info("Failed to connect to: " + mUrl.ToString() + response ?? (" Http statuscode: " + response.StatusCode));
                    }
                }

                return new DisconnectedState(mUrl, mWebRequesterFactory);
            });
        }
    }
}
