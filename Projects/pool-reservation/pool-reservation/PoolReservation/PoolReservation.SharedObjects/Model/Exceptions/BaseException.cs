using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Model.Exceptions
{
    [Serializable]
    public abstract class BaseException : Exception
    {
        public BaseException() : base()
        {

        }

        public BaseException(string message) : base(message)
        {

        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public string Action { get; set; }

        public string DetailedAction { get; set; }

        public string DetailedMessage { get; set; }
    }
}
