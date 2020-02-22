using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_integration.API.Models;
using Data_Integration.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data_integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProcessController : ControllerBase
    {
        [HttpPost]
        public async void Post([FromBody] DataModel dataModel)
        {
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