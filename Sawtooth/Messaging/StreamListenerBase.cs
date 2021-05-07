using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sawtooth.Messaging
{
    public abstract class StreamListenerBase : IStreamListener
    {
        readonly ConcurrentDictionary<string, TaskCompletionSource<Message>> Futures;
        /// <summary>
        /// The stream.
        /// </summary>
        protected readonly Stream Stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Messaging.StreamListenerBase"/> class.
        /// </summary>
        /// <param name="address">Address.</param>
        public StreamListenerBase(string address)
        {
            Stream = new Stream(address, this);
            Futures = new ConcurrentDictionary<string, TaskCompletionSource<Message>>();
        }

        /// <summary>
        /// Called when the stream receives a message
        /// </summary>
        /// <param name="message">Message.</param>
        public virtual void OnMessage(Message message)
        {
            if (Futures.TryGetValue(message.CorrelationId, out var source))
            {
                if (source.Task.Status != TaskStatus.RanToCompletion) source.SetResult(message);
                Futures.TryRemove(message.CorrelationId, out var _);
            }
        }

        /// <summary>
        /// Sends the message to the stream
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<Message> SendAsync(Message message, CancellationToken cancellationToken)
        {
            var source = new TaskCompletionSource<Message>();
            cancellationToken.Register(() => source.SetCanceled());

            if (Futures.TryAdd(message.CorrelationId, source))
            {
                Stream.Send(message);
                return source.Task;
            }
            if (Futures.TryGetValue(message.CorrelationId, out var task))
            {
                return task.Task;
            }
            throw new InvalidOperationException("Cannot get or set future context for this message.");
        }

        /// <summary>
        /// Connects to the stream
        /// </summary>
        protected void Connect() => Stream.Connect();

        /// <summary>
        /// Disconnects from the stream
        /// </summary>
        protected void Disconnect() => Stream.Disconnect();
    }
}
