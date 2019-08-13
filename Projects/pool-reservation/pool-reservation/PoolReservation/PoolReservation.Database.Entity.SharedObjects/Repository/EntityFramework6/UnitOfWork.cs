using PoolReservation.Database.Entity.SharedObjects.Repository.Base;
using PoolReservation.SharedObjects.Model.Exceptions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories;
using System.Diagnostics;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6
{
    public class UnitOfWork : IUnitOfWork
    {
        protected PoolReservationEntities dbContext { get; private set; }


        [DebuggerStepThrough]
        public UnitOfWork()
        {
            this.dbContext = new PoolReservationEntities();
            this.InitializeFields();
        }

        [DebuggerStepThrough]
        public UnitOfWork(PoolReservationEntities context)
        {
            this.dbContext = context;
            this.InitializeFields();
        }

        private void InitializeFields()
        {
            this.Users = new UserRepository(this.dbContext, this);

            this.Hotels = new HotelRepository(this.dbContext, this);

            this.Reservations = new ReservationRepository(this.dbContext, this);

            this.Venues = new VenuesRepository(this.dbContext, this);

            this.VenueItems = new ItemsRepository(this.dbContext, this);

            this.VenueTypes = new VenueTypesRepository(this.dbContext, this);

            this.ItemTypes = new ItemTypeRepository(this.dbContext, this);

            this.Pictures = new PictureRepository(this.dbContext, this);

            this.Permissions = new PermissionsRepository(this.dbContext, this);

            this.Miscellaneous = new MiscellaneousRepository(this.dbContext, this);
        }



        public IUserRepository Users { get; private set; }

        public IHotelRepository Hotels { get; private set; }

        public IReservationRepository Reservations { get; private set; }

        public IVenuesRepository Venues { get; private set; }

        public IVenueItemsRepository VenueItems { get; private set; }

        public IVenueTypesRepository VenueTypes { get; private set; }

        public IItemTypesRepository ItemTypes { get; private set; }

        public IPictureRepository Pictures { get; private set; }

        public IPermissionsRepository Permissions { get; private set; }
        public IMiscellaneousRepository Miscellaneous { get; private set; }

        public void Dispose()
        {
            if (this.dbContext != null)
            {
                dbContext.Dispose();
            }
        }

        public int Complete()
        {
            if (this.dbContext != null)
            {
                return dbContext.SaveChanges();
            }
            else
            {
                throw new AlreadyDisposedException() { Action = "dbcontextNull" };
            }
        }
    }
}
