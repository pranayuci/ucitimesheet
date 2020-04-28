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
    public class EmployeeService : IBaseService<EmployeeVm>, IEmployeeService<EmployeeVm>
    {
        IRepositoryWrapper _repositoryWrapper;

        public EmployeeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public EmployeeVm Create(EmployeeVm entity)
        {
            var mappedBO = Mapper.Map<Employee>(entity);
            var result = _repositoryWrapper.EmployeeRepository.Create(mappedBO);
            _repositoryWrapper.EmployeeRepository.SaveChanges();
            return Mapper.Map<EmployeeVm>(mappedBO);
        }

        public EmployeeVm Delete(Guid entityId)
        {
            var employeeVm = GetById(entityId);
            var mappedBO = Mapper.Map<Employee>(employeeVm);
            var result = _repositoryWrapper.EmployeeRepository.Delete(mappedBO);
            _repositoryWrapper.EmployeeRepository.SaveChanges();
            return employeeVm;
        }

        public IEnumerable<EmployeeVm> GetAll()
        {
            var result = _repositoryWrapper.EmployeeRepository.GetAll();
            return Mapper.Map<IEnumerable<EmployeeVm>>(result);
        }

        public EmployeeVm GetById(Guid entityId)
        {
            var employee = _repositoryWrapper.EmployeeRepository.GetById<Guid>(entityId);
            return Mapper.Map<EmployeeVm>(employee);
        }

        public EmployeeVm Update(EmployeeVm entity)
        {
            var mappedBO = Mapper.Map<Employee>(entity);
            var empolyee = _repositoryWrapper.EmployeeRepository.Update(mappedBO);
            _repositoryWrapper.EmployeeRepository.SaveChanges();
            return Mapper.Map<EmployeeVm>(empolyee);
        }
    }
}
