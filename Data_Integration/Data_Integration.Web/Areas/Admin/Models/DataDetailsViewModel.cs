using Autofac;
using Data_Integration.Web.Areas.Admin.Models;
using Data_Integration.Core.Services;
using Data_Integration.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Integration.Web;

namespace Data_Integration.Web.Areas.Admin.Models
{
    public class DataDetailsViewModel:BaseModel
    {
        private IDataDetailsService _dataDetailsService;
        public DataDetailsViewModel()
        {
            _dataDetailsService = Startup.AutofacContainer.Resolve<IDataDetailsService>();
        }

        public DataDetailsViewModel(IDataDetailsService dataDetailsService)
        {
            _dataDetailsService = dataDetailsService;
        }

        public object GetDataDetails(DataTablesAjaxRequestModel tableModel)
        {
            int total = 0;
            int totalFiltered = 0;
            var records = _dataDetailsService.GetDataDetails(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                out total,
                out totalFiltered);

            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                                record.Id.ToString(),
                                record.FileName,
                                record.CreateDate.ToString(),
                                record.Status,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
