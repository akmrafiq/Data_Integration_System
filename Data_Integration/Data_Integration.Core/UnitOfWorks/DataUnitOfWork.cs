using Data_Integration.Core.Contexts;
using Data_Integration.Core.Repositories;
using Data_Integration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.UnitOfWorks
{
    public class DataUnitOfWork : UnitOfWork<DataContext>, IDataUnitOfWork
    {
        public IDataDetailsRepository DataDetailsRepository { get ; set; }
        public DataUnitOfWork(string connectionString, string migrationAssemblyName)
          : base(connectionString, migrationAssemblyName)
        {
            DataDetailsRepository = new DataDetailsRepository(_dbContext);
        }
    }
}
