using DAL;
using Repository.Contracts;

namespace Repository.Implementations
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IEmployeeRepository _employeeRepository;
        private IRolesRepository _rolesRepository;
        private ITimesheetEntryRepository _timesheetEntryRepository;
        private IClientRepository _clientRepository;
        private IWeeksOfYearRepository _weeksOfYearRepository;
        private ISPTotalHoursConsultantWise _spTotalHoursConsultantWise;

        private DataContext _dataContext;

        public RepositoryWrapper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_employeeRepository == null)
                {
                    _employeeRepository = new EmployeeRepository(_dataContext);
                }
                return _employeeRepository;
            }
        }

        public IRolesRepository RolesRepository
        {
            get
            {
                if (_rolesRepository == null)
                {
                    _rolesRepository = new RolesRepository(_dataContext);
                }
                return _rolesRepository;
            }
        }

        public ITimesheetEntryRepository TimesheetEntryRepository
        {
            get
            {
                if (_timesheetEntryRepository == null)
                {
                    _timesheetEntryRepository = new TimesheetEntryRepository(_dataContext);
                }
                return _timesheetEntryRepository;
            }
        }

        public IClientRepository ClientRepository
        {
            get
            {
                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_dataContext);
                }
                return _clientRepository;
            }
        }

        public IWeeksOfYearRepository WeeksOfYearRepository
        {
            get
            {
                if (_weeksOfYearRepository == null)
                {
                    _weeksOfYearRepository = new WeeksOfYearRepository(_dataContext);
                }
                return _weeksOfYearRepository;
            }
        }

        public ISPTotalHoursConsultantWise SPTotalHoursConsultantWise
        {
            get
            {
                if (_spTotalHoursConsultantWise == null)
                {
                    _spTotalHoursConsultantWise = new SPTotalHoursConsultantWiseRepo(_dataContext);
                }
                return _spTotalHoursConsultantWise;
            }
        }
    }
}