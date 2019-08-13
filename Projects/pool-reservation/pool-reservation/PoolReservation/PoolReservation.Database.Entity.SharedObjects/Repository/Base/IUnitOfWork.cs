using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IHotelRepository Hotels { get; }
        IReservationRepository Reservations { get; }
        IVenuesRepository Venues { get; }
        IVenueItemsRepository VenueItems { get; }
        IPictureRepository Pictures { get; }
        IItemTypesRepository ItemTypes { get; }
        IPermissionsRepository Permissions { get; }

        IMiscellaneousRepository Miscellaneous { get; }

        int Complete();
    }
}
