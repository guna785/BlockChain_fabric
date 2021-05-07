using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sawtooth.Transactions
{
    public class InvalidTransactionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Processor.InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public InvalidTransactionException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Processor.InvalidTransactionException"/> class.
        /// </summary>
        public InvalidTransactionException() : base("Transaction was invalid")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Processor.InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="context">Context.</param>
        protected InvalidTransactionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Processor.InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidTransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
