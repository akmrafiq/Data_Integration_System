using Data_Integration.Core.Entities;
using Data_Integration.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.Repositories
{
    public class DataDetailsRepository : Repository<DataDetails>,IDataDetailsRepository
    {
        public DataDetailsRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
