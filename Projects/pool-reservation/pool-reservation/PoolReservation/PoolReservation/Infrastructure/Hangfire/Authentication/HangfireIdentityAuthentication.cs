using Hangfire.Dashboard;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire.Annotations;

namespace PoolReservation.Infrastructure.Hangfire.Authentication
{
    public class HangfireIdentityAuthentication : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var theContext = context.GetOwinEnvironment();

            var owinContext = new OwinContext(theContext);

            using (var unitOfWork = new UnitOfWork())
            {
                var userId = owinContext.Authentication.User.Identity.GetUserId();

                return unitOfWork.Users.IsUserDeveloperAdmin(userId);
            }
        }
    }
}