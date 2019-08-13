using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages
{
    public class Status<T>
    {
        public bool Valid { get; set; }

        public bool Completed { get; set; }

        public T FailedCode { get; set; }
    }
}
