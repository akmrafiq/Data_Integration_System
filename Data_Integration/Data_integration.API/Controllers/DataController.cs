using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Data_Integration.Core.Entities;
using Data_Integration.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data_integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DataController : ControllerBase
    {
        private IDataDetailsService _dataDetailsService;
        public DataController()
        {
            _dataDetailsService = Startup.AutofacContainer.Resolve<IDataDetailsService>();
        }
        [HttpGet]
        public IEnumerable<DataDetails> Get()
        {
                return _dataDetailsService.DataDetailsList();
        }
    }
}