using Data_Integration.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.Services
{
    public interface IDataDetailsService
    {
        IEnumerable<DataDetails> GetDataDetails(
           int pageIndex,
           int pageSize,
           string searchText,
           out int total,
           out int totalFiltered);
        void AddDataDetails(DataDetails dataDetails);
        DataDetails GetDataDetail(int id);
        void EditDataDetails(DataDetails dataDetails);
        void DeleteDataDetails(int id);
    }
}
