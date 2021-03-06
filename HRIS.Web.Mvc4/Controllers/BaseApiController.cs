using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Project.Web.Mvc4.Controllers
{
    public class BaseApiController : ApiController
    {
        protected HttpResponseMessage BuildSuccessResult(HttpStatusCode statusCode)
        {
            return this.Request.CreateResponse(statusCode);
        }

        protected HttpResponseMessage BuildSuccessResult(HttpStatusCode statusCode, object data)
        {
            return data != null ? this.Request.CreateResponse(statusCode, data) : this.Request.CreateResponse(statusCode);
        }

        protected HttpResponseMessage BuildErrorResult(HttpStatusCode statusCode, string errorCode = null, string message = null)
        {
            return this.Request.CreateErrorResponse(statusCode, message);
        }
    }
}