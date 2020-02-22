using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Data_integration.API.Models;
using Data_Integration.API;
using Data_Integration.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data_integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProcessController : ControllerBase
    {
        private IDataDetailsService _dataDetailsService;
        public DataProcessController()
        {
            _dataDetailsService = Startup.AutofacContainer.Resolve<IDataDetailsService>();
        }

        [HttpGet]
        public async void Get(string filename, string name,string email, string phone )
        {
            var dataModel = new DataModel();
            dataModel.FileName = filename;
            dataModel.Name = name;
            dataModel.Email = email;
            dataModel.Phone = phone;
           await InsertDataAsync(dataModel);

        }

        public async Task InsertDataAsync(DataModel dataModel)
        {
            var dbHelper = new DynamoDbHelper();
            await dbHelper.Insert(new DbInsertItem
            {
                TableName = "datainfo",
                Values = new Dictionary<string, object>
                            {
                                {
                                    "id",Guid.NewGuid().ToString()
                                },
                                {
                                    "filename",dataModel.FileName
                                },
                                {
                                    "name",dataModel.Name
                                },
                                {
                                    "email",dataModel.Email
                                },
                                {
                                    "phone",dataModel.Phone
                                },
                                {
                                    "time",DateTimeOffset.Now
                                }
                            }
            });
        }
    }
}