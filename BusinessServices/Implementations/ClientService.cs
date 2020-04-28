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
    public class ClientService : IBaseService<ClientVm>, IClientService<ClientVm>
    {
        IRepositoryWrapper _repositoryWrapper;

        public ClientService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public ClientVm Create(ClientVm entity)
        {
            var mappedBO = Mapper.Map<Client>(entity);
            var result = _repositoryWrapper.ClientRepository.Create(mappedBO);
            _repositoryWrapper.ClientRepository.SaveChanges();
            return Mapper.Map<ClientVm>(mappedBO);
        }

        public ClientVm Delete(Guid entityId)
        {
            var ClientVm = GetById(entityId);
            var mappedBO = Mapper.Map<Client>(ClientVm);
            var result = _repositoryWrapper.ClientRepository.Delete(mappedBO);
            _repositoryWrapper.ClientRepository.SaveChanges();
            return ClientVm;
        }

        public IEnumerable<ClientVm> GetAll()
        {
            var result = _repositoryWrapper.ClientRepository.GetAll();
            return Mapper.Map<IEnumerable<ClientVm>>(result);
        }

        public ClientVm GetById(Guid entityId)
        {
            var Client = _repositoryWrapper.ClientRepository.GetById<Guid>(entityId);
            return Mapper.Map<ClientVm>(Client);
        }

        public ClientVm Update(ClientVm entity)
        {
            var mappedBO = Mapper.Map<Client>(entity);
            var empolyee = _repositoryWrapper.ClientRepository.Update(mappedBO);
            _repositoryWrapper.ClientRepository.SaveChanges();
            return Mapper.Map<ClientVm>(empolyee);
        }
    }
}
