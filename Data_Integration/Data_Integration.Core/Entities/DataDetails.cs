using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.Entities
{
    public class DataDetails
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
    }
}
