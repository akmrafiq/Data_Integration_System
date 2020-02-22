using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Autofac;
using Data_integration.API.Models;
using Data_Integration.API;
using Data_Integration.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data_Integration.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Data_integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DataProcessController : ControllerBase
    {
        private IDataDetailsService _dataDetailsService;
        public DataProcessController()
        {
            _dataDetailsService = Startup.AutofacContainer.Resolve<IDataDetailsService>();
        }
        [HttpGet]
        public IEnumerable<DataDetails> Get()
        {
            var list = _dataDetailsService.DataDetailsList();
            GetData(list);
            return list;
        }

      
        private async void GetData(IEnumerable<DataDetails> dataDetails)
        {
            
            foreach (var item in dataDetails)
            {
                if (item.Status=="Pending")
                {
                    var s3Client = new AmazonS3Client();

                    var s3Request = new GetObjectRequest
                    {
                        BucketName = "datafilecontainer",
                        Key = item.FileName
                    };

                    var response = await s3Client.GetObjectAsync(s3Request);
                    var token = new CancellationToken();
                    var storePath = $"file/{item.FileName}";
                    await response.WriteResponseStreamToFileAsync(storePath, false, token);

                    using (var streamReader = System.IO.File.OpenText(storePath))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();
                            var data = line.Split(new[] { ',' });
                            var dataModel = new DataModel();
                            dataModel.FileName = item.FileName;
                            dataModel.Name = data[0];
                            dataModel.Email = data[1];
                            dataModel.Phone = data[2];

                            await InsertDataAsync(dataModel);
                        }
                    }
                    UpdateStatus(item);
                }
            }
           
        }

        private async Task InsertDataAsync(DataModel dataModel)
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
        private void UpdateStatus(DataDetails dataDetails)
        {
            dataDetails.Id = dataDetails.Id;
            dataDetails.Status = "Complete";
            dataDetails.FileName = dataDetails.FileName;
            _dataDetailsService.EditDataDetails(dataDetails);
        }
    }
}