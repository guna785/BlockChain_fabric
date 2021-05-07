using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sawtooth.Messaging
{
    public interface IStreamListener
    {
        /// <summary>
        /// Called when a new message is received from the validator
        /// </summary>
        /// <param name="message">Message.</param>
        void OnMessage(Message message);

        /// <summary>
        /// Send a message to the validator
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Message> SendAsync(Message message, CancellationToken cancellationToken);
    }
}
