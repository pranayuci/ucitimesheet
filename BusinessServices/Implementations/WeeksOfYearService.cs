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
    public class WeeksOfYearService : IBaseService<WeeksOfYearVm>, IWeeksOfYearService<WeeksOfYearVm>
    {
        IRepositoryWrapper _repositoryWrapper;

        public WeeksOfYearService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public WeeksOfYearVm Create(WeeksOfYearVm entity)
        {
            var mappedBO = Mapper.Map<WeeksOfYear>(entity);
            var result = _repositoryWrapper.WeeksOfYearRepository.Create(mappedBO);
            _repositoryWrapper.WeeksOfYearRepository.SaveChanges();
            return Mapper.Map<WeeksOfYearVm>(mappedBO);
        }

        public WeeksOfYearVm Delete(Guid entityId)
        {
            var WeeksOfYearVm = GetById(entityId);
            var mappedBO = Mapper.Map<WeeksOfYear>(WeeksOfYearVm);
            var result = _repositoryWrapper.WeeksOfYearRepository.Delete(mappedBO);
            _repositoryWrapper.WeeksOfYearRepository.SaveChanges();
            return WeeksOfYearVm;
        }

        public IEnumerable<WeeksOfYearVm> GetAll()
        {
            var result = _repositoryWrapper.WeeksOfYearRepository.GetAll();
            return Mapper.Map<IEnumerable<WeeksOfYearVm>>(result);
        }

        public WeeksOfYearVm GetById(Guid entityId)
        {
            var WeeksOfYear = _repositoryWrapper.WeeksOfYearRepository.GetById<Guid>(entityId);
            return Mapper.Map<WeeksOfYearVm>(WeeksOfYear);
        }

        public WeeksOfYearVm GetWeekByDate(DateTime dateTime)
        {
            var result = _repositoryWrapper.WeeksOfYearRepository.GetWeekByDate(dateTime);
            return Mapper.Map<WeeksOfYearVm>(result);
        }

        public WeeksOfYearVm GetWeekByWeekNumber(int weekNumber)
        {
            var result = _repositoryWrapper.WeeksOfYearRepository.GetWeekByWeekNumber(weekNumber);
            return Mapper.Map<WeeksOfYearVm>(result);
        }

        public WeeksOfYearVm Update(WeeksOfYearVm entity)
        {
            var mappedBO = Mapper.Map<WeeksOfYear>(entity);
            var empolyee = _repositoryWrapper.WeeksOfYearRepository.Update(mappedBO);
            _repositoryWrapper.WeeksOfYearRepository.SaveChanges();
            return Mapper.Map<WeeksOfYearVm>(empolyee);
        }
    }
}
