using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public abstract class Repository : IRepository
    {
        protected PoolReservationEntities dbContext { get; private set; }
        protected UnitOfWork unitOfWork { get; private set; }

        public Repository(PoolReservationEntities context, UnitOfWork unitOfWork)
        {
            this.dbContext = context;
            this.unitOfWork = unitOfWork;
        }
    }
}
