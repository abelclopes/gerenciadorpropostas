using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DOMAIN.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using INFRAESTRUCTURE;

namespace API.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IContext Context;
        protected readonly IMemoryCache MemoryCache;

        public BaseController(IContext context, IMemoryCache memoryCache)
        {
            Context = context;
            MemoryCache = memoryCache;
        }
    }
}
