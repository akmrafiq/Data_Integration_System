using Autofac;
using Data_Integration.Core.Entities;
using Data_Integration.Core.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Integration.Web.Areas.Admin.Models
{
    public class DataDetailsUpdateModel:BaseModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }


        private IDataDetailsService _dataDetailsService;

        public DataDetailsUpdateModel()
        {
            _dataDetailsService = Startup.AutofacContainer.Resolve<IDataDetailsService>();
        }

        public DataDetailsUpdateModel(IDataDetailsService dataDetailsService)
        {
            _dataDetailsService = dataDetailsService;
        }

        public void AddNewDataFile(string filename)
        {
            try
            {
                _dataDetailsService.AddDataDetails(new DataDetails
                {
                    FileName = filename
                });

                Notification = new NotificationModels("Success!", "File Uploaded", NotificationType.Success);
            }
            catch (InvalidOperationException iex)
            {
                Notification = new NotificationModels(
                    "Failed!",
                    "Failed to Upload File",
                    NotificationType.Fail);
            }
            catch (Exception ex)
            {
                Notification = new NotificationModels(
                    "Failed!",
                    "Failed to Upload File, please try again",
                    NotificationType.Fail);
            }
        }
    }
}
