using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Exceptions
{
    public class BeginTransactionException : Exception
    {
        public BeginTransactionGroupSummary Summary { get; set; }
    }
}
