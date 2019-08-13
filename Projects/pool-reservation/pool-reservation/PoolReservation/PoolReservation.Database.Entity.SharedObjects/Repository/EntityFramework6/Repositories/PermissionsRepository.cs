using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class PermissionsRepository : Repository, IPermissionsRepository
    {
        public PermissionsRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public AspNetUsers GetUserByIdWithPermissions(string userId)
        {
            return this.dbContext.AspNetUsers
                .Include(x => x.SitePermissions.HotelPermissions)
                .Include(x => x.SitePermissions.IconPermissions)
                .Include(x => x.SitePermissions.OtherReservationsPermissions)
                .Include(x => x.SitePermissions.PersonalReservationsPermissions)
                .Include(x => x.SitePermissions.PricePermissions)
                .FirstOrDefault(x => x.Id == userId);
        }
    }
}
