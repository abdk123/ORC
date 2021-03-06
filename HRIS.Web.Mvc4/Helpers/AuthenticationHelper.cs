using Project.Web.Mvc4.APIAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Project.Web.Mvc4.Helpers
{
    public class AuthenticationHelper
    {
        public static BasicAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage Request)
        {
            string authHeader = null;
            var auth = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;
            if (auth != null)
            {
                authHeader = auth.First();
            }

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return null;
            }

            int index = authHeader.IndexOf(':');
            if (index < 0)
            {
                return null;
            }

            string username = authHeader.Substring(0, index);
            string password = authHeader.Substring(index + 1);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return new BasicAuthenticationIdentity(username, password);
        }

    }
}