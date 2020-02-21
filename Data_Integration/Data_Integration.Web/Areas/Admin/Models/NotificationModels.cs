using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Integration.Web.Areas.Admin.Models
{
    public class NotificationModels
    {
        public string TypeCssClass { get; private set; }
        public string SignCssClass { get; private set; }
        public string Message { get; set; }
        public string HeaderText { get; set; }

        public NotificationModels(string headerText, string message, NotificationType type)
        {
            Message = message;
            HeaderText = headerText;
            TypeCssClass = type == NotificationType.Success ? "success" : "danger";
            SignCssClass = type == NotificationType.Success ? "check" : "ban";
        }
    }
    public enum NotificationType
    {
        Fail,
        Success
    }
}
