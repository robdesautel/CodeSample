using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Models.Permissions.Outgoing;
using PoolReservation.Models.User.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PoolReservation.api
{
    public class UserController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("api/User/Search/Minimal")]
        [ResponseType(typeof(OutgoingHotelPermissions))]
        public HttpResponseMessage SearchMinimalUsers(string query)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (userId == null)
                {
                    throw new Exception("User not found.");
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var users = unitOfWork.Users.SearchUserByIdOrEmailMINIMAL(userId, query);

                    var outgoingUsers = users.ToList().Select(x => OutgoingIdEmailUser.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingUsers, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }
    }
}
