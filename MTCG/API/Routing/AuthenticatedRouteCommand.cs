﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;

namespace MTCG.API.Routing
{
    public abstract class AuthenticatedRouteCommand: IRouteCommand
    {
        public User Identity { get; init; }

        protected AuthenticatedRouteCommand(User identity)
        {
            Identity = identity;
        }

        public abstract HttpResponse Execute();
    }
}
