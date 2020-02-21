using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Integration.Web.Areas.Admin.Models
{
    public abstract class BaseModel
    {
        public NotificationModels Notification { get; set; }
    }
}
