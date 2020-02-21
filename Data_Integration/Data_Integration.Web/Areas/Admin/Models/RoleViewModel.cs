using Autofac;
using Microsoft.AspNetCore.Identity;
using Data_Integration.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Integration.Web.Areas.Admin.Models
{
    public class RoleViewModel:BaseModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleViewModel()
        {
            _roleManager = Startup.AutofacContainer.Resolve<RoleManager<IdentityRole>>();
        }

        public object GetRoles(DataTablesAjaxRequestModel tableModel)
        {
            int total = 0;
            int totalFiltered = 0;
            var start = (tableModel.PageIndex - 1) * tableModel.PageSize;
            IEnumerable<IdentityRole> records = null;
            if (string.IsNullOrWhiteSpace(tableModel.SearchText))
                records = _roleManager.Roles.Skip(start).Take(tableModel.PageSize);
            else
                records = _roleManager.Roles.Where(x => x.Name.Contains(tableModel.SearchText));

            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                                record.Id.ToString(),
                                record.Name,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
