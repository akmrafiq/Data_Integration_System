using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
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
        private static string apiUrl = "http://localhost:50850/api/";
        private static string authirizeToken;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (string.IsNullOrEmpty(authirizeToken))
                    Login();

                if (!string.IsNullOrEmpty(authirizeToken))
                {
                    CheckingFileStatus();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
        public string CheckingFileStatus()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authirizeToken);
            var httpResponse = client.GetAsync(apiUrl+ "DataProcess").Result;
            var result = httpResponse.Content.ReadAsStringAsync().Result;
            return result;
        }

        public void Login()
        {
            try
            {
                authirizeToken = null;
                authirizeToken = GetTokens("kay@gmail.com", "Rafiq@123");
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
                var result = "";
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
    }
}
