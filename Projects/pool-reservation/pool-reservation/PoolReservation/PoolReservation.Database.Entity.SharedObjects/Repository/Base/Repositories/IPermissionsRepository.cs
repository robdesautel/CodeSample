using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IPermissionsRepository: IRepository
    {
        AspNetUsers GetUserByIdWithPermissions(string userId);

    }
}
