using BusinessServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Models.BO;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServices.Implementations
{
    public class ServiceFactory : IServiceFactory
    {
        private EmployeeService _employeeService;
        private RolesService _rolesService;
        private UserService _userService;
        private TimesheetEntryService _timesheetEntryService;
        private ClientService _clientService;
        private WeeksOfYearService _weeksOfYearService;
        private SPTotalHoursConsultantWiseService _spTotalHoursConsultantWiseService;
        private EmailService _emailService;

        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public ServiceFactory(IRepositoryWrapper repositoryWrapper,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        public EmployeeService EmployeeService
        {
            get
            {
                if (_employeeService == null)
                {
                    _employeeService = new EmployeeService(this.repositoryWrapper);
                }
                return _employeeService;
            }
        }

        public TimesheetEntryService TimesheetEntryService
        {
            get
            {
                if (_timesheetEntryService == null)
                {
                    _timesheetEntryService = new TimesheetEntryService(this.repositoryWrapper);
                }
                return _timesheetEntryService;
            }
        }

        public UserService UserService
        {
            get
            {
                if (_userService == null)
                {
                    _userService = new UserService(this.repositoryWrapper, this.userManager);
                }
                return _userService;
            }
        }

        public RolesService RolesService
        {
            get
            {
                if (_rolesService == null)
                {
                    _rolesService = new RolesService(this.repositoryWrapper, this.roleManager);
                }
                return _rolesService;
            }
        }

        public ClientService ClientService
        {
            get
            {
                if (_clientService == null)
                {
                    _clientService = new ClientService(this.repositoryWrapper);
                }
                return _clientService;
            }
        }


        public WeeksOfYearService WeeksOfYearService
        {
            get
            {
                if (_weeksOfYearService == null)
                {
                    _weeksOfYearService = new WeeksOfYearService(this.repositoryWrapper);
                }
                return _weeksOfYearService;
            }
        }

        public SPTotalHoursConsultantWiseService SPTotalHoursConsultantWiseService
        {
            get
            {
                if (_spTotalHoursConsultantWiseService == null)
                {
                    _spTotalHoursConsultantWiseService = new SPTotalHoursConsultantWiseService(this.repositoryWrapper);
                }
                return _spTotalHoursConsultantWiseService;
            }
        }

        public EmailService EmailService
        {
            get
            {
                if (_emailService == null)
                {
                    _emailService = new EmailService(this.repositoryWrapper);
                }
                return _emailService;
            }
        }
    }
}
