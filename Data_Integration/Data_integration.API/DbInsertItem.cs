using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.API
{
    public class DbInsertItem
    {
        public string TableName { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }
}
