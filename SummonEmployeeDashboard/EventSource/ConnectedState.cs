using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace EventSource4Net
{
    class ConnectedState : IConnectionState
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(ConnectedState));
        private readonly IWebRequesterFactory mWebRequesterFactory;
        private ServerSentEvent mSse = null;
        private string mRemainingText = string.Empty;   // the text that is not ended with a lineending char is saved for next call.
        private IServerResponse mResponse;
        public EventSourceState State { get { return EventSourceState.OPEN; } }

        public ConnectedState(IServerResponse response, IWebRequesterFactory webRequesterFactory)
        {
            mResponse = response;
            mWebRequesterFactory = webRequesterFactory;
        }

        public Task<IConnectionState> Run(Action<ServerSentEvent> msgReceived, CancellationToken cancelToken)
        {
            Task<IConnectionState> t = new Task<IConnectionState>(() =>
            {
                var stream = mResponse.GetResponseStream();
                {
                    byte[] buffer = new byte[1024 * 8];
                    int bytesRead = 0;
                    try
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception e)
                    {
                        _logger.Warn("Failed to read from stream", e);
                    }
                    if (bytesRead > 0) // stream has not reached the end yet
                    {
                        string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        text = mRemainingText + text;
                        string[] lines = StringSplitter.SplitIntoLines(text, out mRemainingText);
                        foreach (string line in lines)
                        {
                            if (cancelToken.IsCancellationRequested) break;

                            // Dispatch message if empty lne
                            if (string.IsNullOrEmpty(line.Trim()) && mSse != null)
                            {
                                _logger.Debug("Message received");
                                msgReceived(mSse);
                                mSse = null;
                            }
                            else if (line.StartsWith(":"))
                            {
                                // This a comment, just log it.
                                _logger.Debug("A comment was received: " + line);
                            }
                            else
                            {
                                string fieldName = String.Empty;
                                string fieldValue = String.Empty;
                                if (line.Contains(':'))
                                {
                                    int index = line.IndexOf(':');
                                    fieldName = line.Substring(0, index);
                                    fieldValue = line.Substring(index + 1).TrimStart();
                                }
                                else
                                {
                                    fieldName = line;
                                }

                                if (String.Compare(fieldName, "event", true) == 0)
                                {
                                    mSse = mSse ?? new ServerSentEvent();
                                    mSse.EventType = fieldValue;
                                }
                                else if (String.Compare(fieldName, "data", true) == 0)
                                {
                                    mSse = mSse ?? new ServerSentEvent();
                                    mSse.Data = fieldValue + '\n';
                                }
                                else if (String.Compare(fieldName, "id", true) == 0)
                                {
                                    mSse = mSse ?? new ServerSentEvent();
                                    mSse.LastEventId = fieldValue;
                                }
                                else if (String.Compare(fieldName, "retry", true) == 0)
                                {
                                    if (int.TryParse(fieldValue, out int parsedRetry))
                                    {
                                        mSse = mSse ?? new ServerSentEvent();
                                        mSse.Retry = parsedRetry;
                                    }
                                }
                                else
                                {
                                    // Ignore this, just log it
                                    _logger.Warn("A unknown line was received: " + line);
                                }
                            }
                        }

                        if (!cancelToken.IsCancellationRequested)
                        {
                            return this;
                        }
                    }
                    return new DisconnectedState(mResponse.ResponseUri, mWebRequesterFactory);
                }
            });

            t.Start();
            return t;
        }
    }
}
