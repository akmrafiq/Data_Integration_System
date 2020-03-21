using Data_Integration.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.Contexts
{
    public interface IDataContext
    {
        DbSet<DataDetails> DataDetails { get; set; }
    }
}
