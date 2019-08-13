using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Model.Exceptions.Object
{
    public class AlreadyDisposedException : BaseException
    {
        public AlreadyDisposedException() : base()
        {

        }

        public AlreadyDisposedException(string message) : base(message)
        {

        }

        public AlreadyDisposedException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
