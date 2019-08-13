using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace PoolReservation.Infrastructure.Http
{
    public class JsonFactory
    {
        [DebuggerStepThrough]
        public static HttpResponseMessage CreateJsonMessage(object theObject, HttpStatusCode code, HttpRequestMessage request)
        {
            var jsonString = JsonConvert.SerializeObject(theObject);
            var response = request.CreateResponse(code);
            response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return response;
        }

        [DebuggerStepThrough]
        public static HttpResponseMessage CreateJSONResponseMessageNoSerialization(string theResponseMessage, HttpStatusCode code, HttpRequestMessage request)
        {
            var response = request.CreateResponse(code);
            response.Content = new StringContent(theResponseMessage, Encoding.UTF8, "application/json");
            return response;
        }

        [DebuggerStepThrough]
        public static HttpResponseMessage CreateHTMLResponseMessage(string theResponseMessage, HttpStatusCode code, HttpRequestMessage request)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(theResponseMessage, Encoding.UTF8, "text/html");
            return response;
        }
    }
}