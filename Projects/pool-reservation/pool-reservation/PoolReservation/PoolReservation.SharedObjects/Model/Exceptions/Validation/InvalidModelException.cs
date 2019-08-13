using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Model.Exceptions.Validation
{
    public class InvalidModelException : BaseException
    {
        public InvalidModelException() : base()
        {

        }

        public InvalidModelException(string message) : base(message)
        {

        }

        public InvalidModelException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
