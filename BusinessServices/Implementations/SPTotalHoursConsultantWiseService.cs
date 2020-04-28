using AutoMapper;
using BusinessServices.Contracts;
using Common.Constants;
using Models.BO;
using Models.BO.ViewModels;
using Models.ViewModels;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BusinessServices.Implementations
{
    public class SPTotalHoursConsultantWiseService : IBaseService<TotalHoursConsultantWiseVm>, ISPTotalHoursConsultantWiseService<TotalHoursConsultantWiseVm>
    {
        IRepositoryWrapper _repositoryWrapper;

        public SPTotalHoursConsultantWiseService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public TotalHoursConsultantWiseVm Create(TotalHoursConsultantWiseVm entity)
        {
            throw new NotImplementedException();

        }

        public TotalHoursConsultantWiseVm Delete(Guid entityId)
        {
            throw new NotImplementedException();

        }

        public IEnumerable<TotalHoursConsultantWiseVm> GetAll()
        {
            throw new NotImplementedException();

        }

        public TotalHoursConsultantWiseVm GetById(Guid entityId)
        {
            throw new NotImplementedException();

        }

        public IQueryable<TotalHoursConsultantWiseVm> GetTotalHoursConsultantWise(DateTime starDate, DateTime endDate)
        {
            try
            {
                //    List<SqlParameter> parameters = new List<SqlParameter>()
                //{
                //    new SqlParameter("StartDate", starDate),
                //    new SqlParameter("EndDate", endDate)
                //};

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter startDateParameter = new SqlParameter("StartDate", SqlDbType.DateTime);
                startDateParameter.Value = starDate;
                parameters.Add(startDateParameter);
                SqlParameter endDateParameter = new SqlParameter("EndDate", SqlDbType.DateTime);
                endDateParameter.Value = endDate;
                parameters.Add(endDateParameter);


                //var result = _repositoryWrapper.SPTotalHoursConsultantWise.GetTotalHoursConsultantWise(StoredProcedureConstants.spGetTotalHoursConsultantWise, parameters.ToArray());
                var result = _repositoryWrapper.SPTotalHoursConsultantWise.GetTotalHoursConsultantWise(starDate, endDate);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TotalHoursConsultantWiseVm Update(TotalHoursConsultantWiseVm entity)
        {
            var mappedBO = Mapper.Map<Client>(entity);
            throw new NotImplementedException();

        }
    }
}
