﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot
{
    /// <summary>
    /// Exception thrown inside of IotApi.
    /// </summary>
    public class IotApiException : Exception
    {
        /// <summary>
        /// List of messages related to error.
        /// </summary>
        public IList<object> ReceivedMessages { get; protected set; }

        public IotApiException(string message) : base(message)
        {

        }

        public IotApiException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public IotApiException(string message, Exception innerException, IList<object> receivedMessages) : base(message, innerException)
        {
            this.ReceivedMessages = receivedMessages;
        }
    }
}
