using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventSource4Net
{
    class DisconnectedState : IConnectionState 
    {
        private Uri mUrl;
        private IWebRequesterFactory mWebRequesterFactory;
        public EventSourceState State
        {
            get { return EventSourceState.CLOSED; }
        }

        public DisconnectedState(Uri url, IWebRequesterFactory webRequesterFactory)
        {
            mUrl = url ?? throw new ArgumentNullException("Url cant be null");
            mWebRequesterFactory = webRequesterFactory;
        }

        public Task<IConnectionState> Run(Action<ServerSentEvent> donothing, CancellationToken cancelToken)
        {
            if(cancelToken.IsCancellationRequested)
                return Task.Factory.StartNew<IConnectionState>(() => { return new DisconnectedState(mUrl, mWebRequesterFactory); });
            else
                return Task.Factory.StartNew<IConnectionState>(() => { return new ConnectingState(mUrl, mWebRequesterFactory); });
        }
    }
}
