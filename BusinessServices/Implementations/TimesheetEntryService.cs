using AutoMapper;
using BusinessServices.Contracts;
using Models.BO;
using Models.ViewModels;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServices.Implementations
{
    public class TimesheetEntryService : IBaseService<TimesheetVm>, ITimesheetEntryService<TimesheetVm>
    {
        IRepositoryWrapper _repositoryWrapper;

        public TimesheetEntryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public TimesheetVm Create(TimesheetVm entity)
        {
            var mappedBO = Mapper.Map<Employee>(entity);
            var result = _repositoryWrapper.EmployeeRepository.Create(mappedBO);
            _repositoryWrapper.EmployeeRepository.SaveChanges();
            return Mapper.Map<TimesheetVm>(mappedBO);
        }

        public TimesheetVm Delete(Guid entityId)
        {
            var TimesheetVm = GetById(entityId);
            var mappedBO = Mapper.Map<Employee>(TimesheetVm);
            var result = _repositoryWrapper.EmployeeRepository.Delete(mappedBO);
            _repositoryWrapper.EmployeeRepository.SaveChanges();
            return TimesheetVm;
        }

        public IEnumerable<TimesheetVm> GetAll()
        {
            var result = _repositoryWrapper.EmployeeRepository.GetAll();
            return Mapper.Map<IEnumerable<TimesheetVm>>(result);
        }

        public TimesheetVm GetById(Guid entityId)
        {
            var employee = _repositoryWrapper.EmployeeRepository.GetById<Guid>(entityId);
            return Mapper.Map<TimesheetVm>(employee);
        }

        public TimesheetVm Update(TimesheetVm entity)
        {
            var mappedBO = Mapper.Map<Employee>(entity);
            var empolyee = _repositoryWrapper.EmployeeRepository.Update(mappedBO);
            _repositoryWrapper.EmployeeRepository.SaveChanges();
            return Mapper.Map<TimesheetVm>(empolyee);
        }

        public void CreateRange(IEnumerable<TimesheetVm> timesheetVms)
        {
            try
            {
                var timesheetEntries = Mapper.Map<List<TimesheetEntry>>(timesheetVms);
                _repositoryWrapper.TimesheetEntryRepository.CreateRange(timesheetEntries);
            }
            catch (Exception ex)
            {

            }
        }

        //public void UpdateRange(IEnumerable<TimesheetVm> timesheetVms)
        //{
        //    try
        //    {
        //        var timesheetEntries = Mapper.Map<List<TimesheetEntry>>(timesheetVms);
        //        _repositoryWrapper.TimesheetEntryRepository.UpdateRange(timesheetEntries);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public void UpdateRange(IEnumerable<TimesheetVm> timesheetVms)
        {
            try
            {
                var timesheetEntries = Mapper.Map<List<TimesheetEntry>>(timesheetVms);
                _repositoryWrapper.TimesheetEntryRepository.UpdateRange(timesheetEntries);
            }
            catch (Exception ex)
            {

            }
        }

        public IEnumerable<TimesheetVm> GetTimesheetsByUserId(string userId)
        {
            var result = _repositoryWrapper.TimesheetEntryRepository.GetTimesheetsByUserId(userId);
            return Mapper.Map<List<TimesheetVm>>(result);
        }

        public IEnumerable<TimesheetVm> GetUnApprovedTimesheetForWeek(DateTime startDate, DateTime endDate, string userId)
        {
            var result = _repositoryWrapper.TimesheetEntryRepository.GetUnApprovedTimesheetForWeek(startDate, endDate, userId);
            return Mapper.Map<List<TimesheetVm>>(result);
        }

        public IEnumerable<TimesheetVm> GetTSForWeekForUser(DateTime startDate, DateTime endDate, string userId)
        {
            var result = _repositoryWrapper.TimesheetEntryRepository.GetTSForWeekForUser(startDate, endDate, userId);
            return Mapper.Map<List<TimesheetVm>>(result);
        }

        public IEnumerable<TimesheetVm> GetUnApprovedSubmittedTSForWeek(DateTime startDate, DateTime endDate, string userId)
        {
            var result = _repositoryWrapper.TimesheetEntryRepository.GetUnApprovedSubmittedTSForWeek(startDate, endDate, userId);
            return Mapper.Map<List<TimesheetVm>>(result);
        }
    }
}
