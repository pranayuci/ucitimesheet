using BusinessServices.Contracts;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebAppCore.Pages.Timesheet
{
    [Authorize(Roles = "Consultant, Admin")]
    public class IndexModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor accessor;

        public IList<TimesheetVm> TimesheetVm { get; set; }
        public UserVm UserVm { get; set; }

        [BindProperty]
        public bool isSubmitted { get; set; }

        [BindProperty]
        public bool isLastDayOfWeek { get; set; }


        [BindProperty]
        public bool isSaveDisabled { get; set; }

        [BindProperty]
        public bool DisableEntries { get; set; }

        [BindProperty]
        public WeeksOfYearVm WeeksOfYearVm { get; set; }

        [BindProperty]
        public string CurrentWeekNumber { get; set; }

        [BindProperty]
        public string RunningWeekNumber { get; set; }

        [BindProperty]
        public string TimesheetState { get; set; }

        public bool pageIsRejected { get; private set; }
        [BindProperty]
        public string RejectionMessage { get; private set; } = null;
        public bool pageIsApproved { get; private set; }
        public string ApprovedMessage { get; private set; }

        public IndexModel(IServiceFactory serviceFactory, UserManager<User> userManager,
            IHttpContextAccessor accessor)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
            this.accessor = accessor;
        }

        public async Task<IActionResult> OnGet()
        {
            DisableEntries = false;
            TimesheetVm = new List<TimesheetVm>();
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByDate(DateTime.Now.Date);
            CurrentWeekNumber = WeeksOfYearVm.WeekNumber.ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;
            RunningWeekNumber = CurrentWeekNumber;
            TempData["RunningWeekNumber"] = RunningWeekNumber;
            await GetUserInfo();
            await GetTimesheetForUI();
            return Page();
        }

        private async Task GetUserInfo()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var client = this.serviceFactory.ClientService.GetById(Guid.Parse(user.Client));
            UserVm = new UserVm();
            UserVm.Name = user.FirstName + " " + user.LastName;
            UserVm.ClientName = client.Name;
            UserVm.ProjectName = user.ProjectName;
            UserVm.Department = user.Department;
        }

        public async Task<IActionResult> OnPostMain(List<TimesheetVm> timesheetVm, string buttonId, string hdnTotalWorkHours, string hdnTotalLunchHours)
        {
            var tempTimesheetVm = new List<TimesheetVm>();
            tempTimesheetVm = timesheetVm;
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            //CurrentWeekNumber = TempData["CurrentWeekNumber"].ToString();
            //TempData["CurrentWeekNumber"] = CurrentWeekNumber;


            RunningWeekNumber = TempData["RunningWeekNumber"].ToString();
            TempData["RunningWeekNumber"] = RunningWeekNumber;


            //WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(CurrentWeekNumber));
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(RunningWeekNumber));


            if (user == null)
            {
                return RedirectToPage("./NotFound");
            }

            if (buttonId == "submit")
            {
                foreach (var entry in tempTimesheetVm)
                {
                    //entry.isSubmitted = isSubmitted;
                    entry.isSubmitted = true;
                    entry.isSaveDraft = false;
                    entry.SubmittedBy = user.Id;
                    entry.SubmittedDate = DateTime.Now;
                    entry.SubmittedFrom = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                TempData["SuccessMessage"] = "Your timesheet is successfully submitted.";
            }

            if (buttonId == "savedraft")
            {
                foreach (var entry in tempTimesheetVm)
                {
                    entry.isSubmitted = false;
                    entry.isSaveDraft = true;
                    entry.SubmittedBy = user.Id;
                    entry.SubmittedDate = DateTime.Now;
                    entry.SubmittedFrom = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                TempData["SuccessMessage"] = "Your timesheet is successfully saved.";
            }

            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedTimesheetForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, user.Id).ToList();

            if (TimesheetVm.Count > 0)
            {
                for (int i = 0, j = 0; i < tempTimesheetVm.Count; i++, j++)
                {
                    TimesheetVm[i].WorkHours = tempTimesheetVm[i].WorkHours;
                    TimesheetVm[i].ApprovedBy = tempTimesheetVm[i].ApprovedBy;
                    TimesheetVm[i].ApprovedDate = tempTimesheetVm[i].ApprovedDate;
                    TimesheetVm[i].ApprovedFrom = tempTimesheetVm[i].ApprovedFrom;
                    TimesheetVm[i].CommentFromApprover = tempTimesheetVm[i].CommentFromApprover;
                    TimesheetVm[i].Comments = tempTimesheetVm[i].Comments;
                    TimesheetVm[i].Date = tempTimesheetVm[i].Date;
                    TimesheetVm[i].Day = tempTimesheetVm[i].Day;
                    TimesheetVm[i].isApproved = tempTimesheetVm[i].isApproved;
                    TimesheetVm[i].isDisabled = tempTimesheetVm[i].isDisabled;
                    TimesheetVm[i].isRejected = tempTimesheetVm[i].isRejected;
                    TimesheetVm[i].isSaveDraft = tempTimesheetVm[i].isSaveDraft;
                    TimesheetVm[i].isSubmitted = tempTimesheetVm[i].isSubmitted;
                    TimesheetVm[i].LunchHours = tempTimesheetVm[i].LunchHours;
                    TimesheetVm[i].SubmittedBy = tempTimesheetVm[i].SubmittedBy;
                    TimesheetVm[i].SubmittedDate = tempTimesheetVm[i].SubmittedDate;
                    TimesheetVm[i].SubmittedFrom = tempTimesheetVm[i].SubmittedFrom;
                }
                this.serviceFactory.TimesheetEntryService.UpdateRange(TimesheetVm);
            }
            else
            {
                this.serviceFactory.TimesheetEntryService.CreateRange(tempTimesheetVm);
            }

            if (buttonId == "submit")
            {
                await SendEmail(user);
            }

            return RedirectToPage("./Success");
        }

        private async Task SendEmail(User user)
        {
            List<string> toList = new List<string>();
            List<string> ccList = new List<string>();

            //client
            User ClientUser = await GetClientForUser(user);
            toList.Add(ClientUser.Email);
            ccList.Add(user.Email);
            ccList.Add(ApplicationContants.UCI_Admin_Email);

            //string subject = $"Timesheet of {user.FirstName} {user.LastName} for week - {CurrentWeekNumber} is submitted.";

            //string body = $"Dear <b>{ClientUser.FirstName} {ClientUser.LastName}, </b> <br/><br/>" +
            //                $"<b> {user.FirstName} {user.LastName} </b> has submitted their Timesheet for week - {CurrentWeekNumber}. " +
            //                $"<br/> <br/> Kindly, Please login at {ApplicationContants.UCI_TMS_Web_Url} to take appropriate action.";

            string subject = $"Timesheet of {user.FirstName} {user.LastName} for week - {RunningWeekNumber} is submitted.";

            string body = $"Dear <b>{ClientUser.FirstName} {ClientUser.LastName}, </b> <br/><br/>" +
                            $"<b> {user.FirstName} {user.LastName} </b> has submitted their Timesheet for week - {RunningWeekNumber}. " +
                            $"<br/> <br/> Kindly, Please login at {ApplicationContants.UCI_TMS_Web_Url} to take appropriate action.";

            this.serviceFactory.EmailService.SendEmail(toList, ccList, subject, body);
        }

        private async Task<User> GetClientForUser(User user)
        {
            var users = this.userManager.Users.Where(u => u.Client == user.Client).ToList();
            User ClientUser = null;

            for (var i = 0; i < users.Count; i++)
            {
                if (await userManager.IsInRoleAsync(users[i], "Client"))
                {
                    ClientUser = users[i];
                    break;
                }
            }

            return ClientUser;
        }

        public async Task OnPostEdit(string weekId, string weekId1)
        {

            //this.serviceFactory.EmailService.SendEmail();
            CurrentWeekNumber = TempData["CurrentWeekNumber"].ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;

            RunningWeekNumber = TempData["RunningWeekNumber"].ToString();
            TempData["RunningWeekNumber"] = RunningWeekNumber;

            await GetUserInfo();
            TimesheetVm = new List<TimesheetVm>();

            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(weekId));

            if (weekId1 == "dec")
            {
                WeeksOfYearVm.WeekNumber -= 1;
                await GetTimesheetForUI();
            }
            else if (weekId1 == "inc")
            {
                WeeksOfYearVm.WeekNumber += 1;
                await GetTimesheetForUI();
            }
            else
            {
                CreateTimesheetUI();
            }
        }

        public User CurrentUser { get; set; }

        private async Task GetTimesheetForUI()
        {
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(WeeksOfYearVm.WeekNumber);

            RunningWeekNumber = WeeksOfYearVm.WeekNumber.ToString();
            TempData["RunningWeekNumber"] = RunningWeekNumber;

            isLastDayOfWeek = true;

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            CurrentUser = user;
            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedTimesheetForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, user.Id).ToList();
            if (TimesheetVm.Count > 0)
            {
                if (TimesheetVm.Any(ts => ts.isSubmitted))
                {
                    if (TimesheetVm.Any(ts => ts.isRejected))
                    {
                        //DisableEntries = false;
                        TimesheetState = ApplicationContants.TS_STATUS_PLACEHOLDER + ApplicationContants.TS_REJECTED;
                        pageIsRejected = true;
                        isLastDayOfWeek = false;
                        RejectionMessage = TimesheetVm[0].CommentFromApprover;
                        foreach (var tm in TimesheetVm)
                        {
                            tm.isDisabled = false;
                        }
                    }
                    else
                    {
                        pageIsRejected = false;
                        isLastDayOfWeek = true;
                        isSaveDisabled = isLastDayOfWeek;
                        TimesheetState = ApplicationContants.TS_STATUS_PLACEHOLDER + ApplicationContants.TS_PENDING_FOR_APPROVAL;

                        foreach (var tm in TimesheetVm)
                        {
                            tm.isDisabled = true;
                        }
                    }
                }
                else if (TimesheetVm.Any(ts => ts.isSaveDraft))
                {
                    //isLastDayOfWeek = (WeeksOfYearVm.EndDate >= DateTime.Now.Date);
                    isLastDayOfWeek = !(DateTime.Now.Date >= WeeksOfYearVm.EndDate);

                    foreach (var tm in TimesheetVm)
                    {
                        tm.isDisabled = false;
                    }
                }
            }
            else
            {
                var TimesheetVm = this.serviceFactory.TimesheetEntryService.GetTSForWeekForUser(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, user.Id).ToList();

                if (TimesheetVm.Any(ts => ts.isApproved))
                {
                    pageIsApproved = true;
                    isLastDayOfWeek = true;
                    isSaveDisabled = isLastDayOfWeek;
                    ApprovedMessage = ApplicationContants.GEN_TS_APPROVE_MESSAGE;
                    TimesheetState = ApplicationContants.TS_STATUS_PLACEHOLDER + ApplicationContants.TS_APPROVED;
                    foreach (var tm in TimesheetVm)
                    {
                        tm.isDisabled = true;
                    }
                }
                //else if (WeeksOfYearVm.WeekNumber.ToString() == CurrentWeekNumber)
                else
                {
                    pageIsApproved = false;
                    isLastDayOfWeek = !(DateTime.Now.Date >= WeeksOfYearVm.EndDate);

                    CreateTimesheetUI();
                }
            }

        }

        private void CreateTimesheetUI()
        {
            var days = (WeeksOfYearVm.EndDate - WeeksOfYearVm.StartDate).TotalDays + 1;

            for (int i = 0; i < days; i++)
            {
                var currDate = WeeksOfYearVm.StartDate.AddDays(i);

                TimesheetVm.Add(new TimesheetVm
                {
                    Day = currDate.ToString("dddd"),
                    Date = currDate,
                    WorkHours = 0F,
                    LunchHours = 0F,
                    Comments = "",
                    isDisabled = !(currDate >= CurrentUser.ContractStartTime)
                });
            }

            if (WeeksOfYearVm.WeekNumber == Convert.ToInt32(CurrentWeekNumber))
            {
                TimesheetState = ApplicationContants.TS_STATUS_PLACEHOLDER + ApplicationContants.TS_NEW;
            }

            //if previous week
            if (WeeksOfYearVm.WeekNumber < Convert.ToInt32(CurrentWeekNumber))
            {
                isLastDayOfWeek = !(TimesheetVm.Any(tm => !tm.isDisabled));
                isSaveDisabled = isLastDayOfWeek;
                if (!isLastDayOfWeek)
                {
                    TimesheetState = ApplicationContants.TS_STATUS_PLACEHOLDER + ApplicationContants.TS_PENDING_FOR_SUBMISSION;
                }

            }
            //if future week
            else if (WeeksOfYearVm.WeekNumber > Convert.ToInt32(CurrentWeekNumber))
            {
                isLastDayOfWeek = true;
                isSaveDisabled = isLastDayOfWeek;
            }

        }

    }
}