using Data_Integration.Core.Entities;
using Data_Integration.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Integration.Core.Services
{
    public class DataDetailsService : IDataDetailsService
    {
        private IDataUnitOfWork _dataUnitOfWork;

        public DataDetailsService(IDataUnitOfWork dataUnitOfWork)
        {
            _dataUnitOfWork = dataUnitOfWork;
        }
        public void AddDataDetails(DataDetails dataDetails)
        {
            if (dataDetails == null || string.IsNullOrWhiteSpace(dataDetails.FileName))
                throw new InvalidOperationException("Data File name is missing");

            _dataUnitOfWork.DataDetailsRepository.Add(dataDetails);
            _dataUnitOfWork.Save();
        }

        public IEnumerable<DataDetails> DataDetailsList()
        {
            return _dataUnitOfWork.DataDetailsRepository.GetAll();
        }

        public void DeleteDataDetails(int id)
        {
            _dataUnitOfWork.DataDetailsRepository.Remove(id);
            _dataUnitOfWork.Save();
        }

        public void EditDataDetails(DataDetails dataDetails)
        {
            var oldData = _dataUnitOfWork.DataDetailsRepository.GetById(dataDetails.Id);
            oldData.Status = dataDetails.Status;
            _dataUnitOfWork.Save();
        }

        public DataDetails GetDataDetail(int id)
        {
            return _dataUnitOfWork.DataDetailsRepository.GetById(id);
        }

        public IEnumerable<DataDetails> GetDataDetails(
            int pageIndex, 
            int pageSize,
            string searchText, 
            out int total, 
            out int totalFiltered
            )
        {

            return _dataUnitOfWork.DataDetailsRepository.Get(
                out total,
                out totalFiltered,
                x => x.FileName.Contains(searchText),
                null,
                "",
                pageIndex,
                pageSize,
                true);
        }
    }
}
