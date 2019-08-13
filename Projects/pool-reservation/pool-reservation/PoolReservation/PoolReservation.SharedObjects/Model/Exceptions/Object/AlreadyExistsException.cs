using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Model.Exceptions.Object
{
    public class AlreadyExistsException : BaseException
    {
        public AlreadyExistsException() : base()
        {

        }

        public AlreadyExistsException(string message) : base(message)
        {

        }

        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
