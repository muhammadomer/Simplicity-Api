using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;


namespace SimplicityOnlineWebApi.Commons
{
    public class GoogleAPIKeys
    {
        public string client_Id { get; set; }
        public string project_id { get; set; }
        public string client_secret { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
    }

    public class GoogleAPI
    {
        public static AuthenticationToken getToken(string scope, HttpRequest request)
        {
            AuthenticationToken token = new AuthenticationToken();
            token.token = "AIzaSyAAYHvbw881ht3qpq8BZSFJi-Psf3GZ0kw";
            return token;
        }
    }
}
