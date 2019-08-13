using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Exceptions;
using PoolReservation.Infrastructure.Http;
using PoolReservation.SharedObjects.Model.Exceptions;
using PoolReservation.SharedObjects.Model.Exceptions.Object;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PoolReservation.Infrastructure.Errors
{
    public class ErrorFactory
    {
        //[DebuggerStepThrough]
        public static HttpResponseMessage Handle(Func<HttpResponseMessage> functionToRun, HttpRequestMessage request)
        {
            try
            {
                return functionToRun();
            }
            catch (BeginTransactionException e)
            {
                return JsonFactory.CreateJsonMessage(e.Summary, HttpStatusCode.InternalServerError, request);
            }
            catch (AlreadyDisposedException e)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = e.Message, DetailedMessage = e.DetailedMessage, Action = e.Action, DetailedAction = e.DetailedAction }, HttpStatusCode.InternalServerError, request);
            }
            catch (UnauthorizedAccessException)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "You are not authorized to view this content.", Action = "unauthorized"}, HttpStatusCode.Forbidden, request);
            }
            catch (BaseException e)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = e.Message, DetailedMessage = e.DetailedMessage, Action = e.Action, DetailedAction = e.DetailedAction }, HttpStatusCode.InternalServerError, request);
            }
            catch (Exception e)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = e.Message }, HttpStatusCode.InternalServerError, request);
            }
        }

        //[DebuggerStepThrough]
        public static async Task<HttpResponseMessage> Handle(Func<Task<HttpResponseMessage>> functionToRun, HttpRequestMessage request)
        {
            try
            {
                return await functionToRun();
            }
            catch (BeginTransactionException e)
            {
                return JsonFactory.CreateJsonMessage(e.Summary, HttpStatusCode.InternalServerError, request);
            }
            catch (AlreadyDisposedException e)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = e.Message, DetailedMessage = e.DetailedMessage, Action = e.Action, DetailedAction = e.DetailedAction }, HttpStatusCode.InternalServerError, request);
            }
            catch (UnauthorizedAccessException)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "You are not authorized to view this content.", Action = "unauthorized" }, HttpStatusCode.Forbidden, request);
            }
            catch (BaseException e)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = e.Message, DetailedMessage = e.DetailedMessage, Action = e.Action, DetailedAction = e.DetailedAction }, HttpStatusCode.InternalServerError, request);
            }
            catch (Exception e)
            {
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = e.Message }, HttpStatusCode.InternalServerError, request);
            }
        }
    }
}