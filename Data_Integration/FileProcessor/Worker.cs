using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using CsvHelper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static string apiUrl = "http://localhost:50850/api/dataprocess";
        private static string token;
        private static readonly HttpClient client = new HttpClient();
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var fileName = "egf41mhgclk.csv";
                var s3Client = new AmazonS3Client();
                
                var s3Request = new GetObjectRequest
                {
                    BucketName = "datafilecontainer",
                    Key = fileName
                };

                var response = await s3Client.GetObjectAsync(s3Request);
                var token = new CancellationToken();
                var storePath = $"file/{fileName}";
                await response.WriteResponseStreamToFileAsync(storePath, false, token);

                using (var streamReader = File.OpenText(storePath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var data = line.Split(new[] { ',' });
                        var dataList = new CSVHelper();
                        dataList.FileName = fileName;
                        dataList.Name = data[0];
                        dataList.Email = data[1];
                        dataList.Phone = data[2];

                        SendData(dataList);
                    }
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }
        public string CheckingFileStatus()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var authorizedResponse = client.GetAsync(apiUrl + "File").Result;
            var result = authorizedResponse.Content.ReadAsStringAsync().Result;
            return result;
        }

        public void Login()
        {
            try
            {
                token = null;
                token = GetTokens("kay@gmail.com", "Rafiq@123");
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex.Message);
            }
        }
        public string GetTokens(string username, string password)
        {
            try
            {
                var result = ""; dynamic item = "";
                var request = WebRequest.Create(apiUrl + "token?username=" + username + "&password=" + password + "");
                request.Method = "GET";
                request.ContentType = "application/json";
                using (var response = request.GetResponse())
                {
                    using (var streamItem = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(streamItem))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex) { throw new SystemException(ex.Message); };

        }

        public string SendData(CSVHelper cSVHelper)
        {
            try
            {
                var result = "";
                var request = WebRequest.Create(apiUrl + "dataModel="+cSVHelper);
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var response = request.GetResponse())
                {
                    using (var streamItem = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(streamItem))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex) { throw new SystemException(ex.Message); };

        }
    }
}