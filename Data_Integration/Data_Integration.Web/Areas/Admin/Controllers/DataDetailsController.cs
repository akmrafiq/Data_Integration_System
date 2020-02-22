using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Data_Integration.Core.Services;
using Data_Integration.Web.Areas.Admin.Models;
using Data_Integration.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data_Integration.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DataDetailsController : Controller
    {
        private IDataDetailsService _dataDetailsService;

        public DataDetailsController(IDataDetailsService dataDetailsService)
        {
            _dataDetailsService = dataDetailsService;
        }
        public IActionResult Index()
        {
            var model = new DataDetailsViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile dataFile)
        {
            var randomName = Path.GetRandomFileName().Replace(".", "");
            var newFileName = $"{ randomName }{ Path.GetExtension(dataFile.FileName)}";

            var path = $"upload/{randomName}{Path.GetExtension(dataFile.FileName)}";

            if (!System.IO.File.Exists(path))
            {
                using (var csvFile = System.IO.File.OpenWrite(path))
                {
                    using (var uploadedFile = dataFile.OpenReadStream())
                    {
                        uploadedFile.CopyTo(csvFile);
                    }
                }
            }
            var client = new AmazonS3Client();
            var file = new FileInfo(path);
            var request = new PutObjectRequest
            {
                BucketName = "datafilecontainer",
                FilePath = file.FullName,
                Key = newFileName
            };
            var response = await client.PutObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                var model = new DataDetailsUpdateModel();
                model.AddNewDataFile(newFileName);
                file.Delete();
            }
            return View();
        }

        public IActionResult GetDataDetails()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new DataDetailsViewModel();
            var data = model.GetDataDetails(tableModel);
            return Json(data);
        }
    }
}