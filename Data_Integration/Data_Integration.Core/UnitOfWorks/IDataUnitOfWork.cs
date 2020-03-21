using Data_Integration.Core.Contexts;
using Data_Integration.Core.Repositories;
using Data_Integration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.UnitOfWorks
{
    public interface IDataUnitOfWork:IUnitOfWork<DataContext>
    {
        IDataDetailsRepository  DataDetailsRepository { get; set; }
    }
}
