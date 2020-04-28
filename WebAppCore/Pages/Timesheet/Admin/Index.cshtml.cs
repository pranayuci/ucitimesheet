using BusinessServices.Contracts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using Models.BO.ViewModels;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Pages.Timesheet.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor accessor;

        public IList<TimesheetVm> TimesheetVm { get; set; }
        public IList<User> Users { get; set; }
        public IList<TotalHoursConsultantWiseVm> TotalHoursConsultantWiseVms { get; set; }
        public string Rpt { get; set; } = "C:\\Temp\\Consultant.pdf";


        [BindProperty]
        public bool isSubmitted { get; set; }

        [BindProperty]
        public bool DisableEntries { get; set; }

        [BindProperty]
        public WeeksOfYearVm WeeksOfYearVm { get; set; }

        [BindProperty]
        public string CurrentWeekNumber { get; set; }


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

            TotalHoursConsultantWiseVms = this.serviceFactory.SPTotalHoursConsultantWiseService
                                                .GetTotalHoursConsultantWise(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate).ToList();
            TempData["CurrentWeekNumber"] = WeeksOfYearVm.WeekNumber;
            await GetTimesheetForUI();
            return Page();
        }

        public async Task<IActionResult> OnPostMain(List<TimesheetVm> timesheetVm, string hdnTotalWorkHours, string hdnTotalLunchHours)
        {
            var tempTimesheetVm = new List<TimesheetVm>();
            tempTimesheetVm = timesheetVm;
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToPage("./NotFound");
            }

            foreach (var entry in tempTimesheetVm)
            {
                entry.isSubmitted = isSubmitted;
                entry.SubmittedBy = user.Id;
                entry.SubmittedDate = DateTime.Now;
                entry.SubmittedFrom = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            this.serviceFactory.TimesheetEntryService.CreateRange(tempTimesheetVm);

            return RedirectToPage("./Success");
        }


        public void OnPostEdit(string weekId, string weekId1)
        {
            CurrentWeekNumber = TempData["CurrentWeekNumber"].ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;

            //TimesheetVm = new List<TimesheetVm>();
            //WeeksOfYearVm = new WeeksOfYearVm();
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(weekId));


            if (weekId1 == "dec")
            {
                WeeksOfYearVm.WeekNumber -= 1;
                WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(WeeksOfYearVm.WeekNumber));

                TotalHoursConsultantWiseVms = this.serviceFactory.SPTotalHoursConsultantWiseService
                                           .GetTotalHoursConsultantWise(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate).ToList();

                //await GetTimesheetForUI();
            }
            else if (weekId1 == "inc")
            {
                WeeksOfYearVm.WeekNumber += 1;
                WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(WeeksOfYearVm.WeekNumber));

                TotalHoursConsultantWiseVms = this.serviceFactory.SPTotalHoursConsultantWiseService
                                           .GetTotalHoursConsultantWise(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate).ToList();

                //await GetTimesheetForUI();
            }
            else
            {

            }
        }

        private async Task GetTimesheetForUI()
        {
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(WeeksOfYearVm.WeekNumber);
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedTimesheetForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, user.Id).ToList();
            if (TimesheetVm.Count > 0)
            {
                if (TimesheetVm.Any(ts => ts.isSubmitted))
                {
                    DisableEntries = true;
                }
            }
            else
            {
                if (WeeksOfYearVm.WeekNumber.ToString() == CurrentWeekNumber)
                {
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
                    Comments = ""
                });
            }
        }


        public async Task<IActionResult> OnPostView(string weekId, string weekId1, string userId, string clientId)
        {
            var user = await userManager.FindByIdAsync(userId);
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(weekId));

            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedTimesheetForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, user.Id).ToList();

            var users = this.userManager.Users.Where(u => u.Client == clientId).ToList();
            User ClientUser = null;

            for (var i = 0; i < users.Count; i++)
            {
                if (await userManager.IsInRoleAsync(users[i], "Client"))
                {
                    ClientUser = users[i];
                    break;
                }
            }

            var content = WriteBytes(TimesheetVm, user, ClientUser);

            //return new FileContentResult(content, "application/msword");

            return new FileContentResult(content, "application/pdf");
        }
        private byte[] GeneratePdf(IList<TimesheetVm> timesheetVm, User user, User ClientUser)
        {
            var client = this.serviceFactory.ClientService.GetById(Guid.Parse(user.Client));

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                //var font = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);
                Font boldArial = FontFactory.GetFont("Arial", 12, Font.BOLD);
                //Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLDITALIC, new Color(125, 88, 15));

                PdfPTable tableHeader = new PdfPTable(1);
                PdfPCell titleCell = new PdfPCell(new Phrase("TIME SHEET", new Font { Size = 20 }));
                titleCell.Border = 0;
                titleCell.HorizontalAlignment = 1;
                tableHeader.AddCell(titleCell);

                document.Add(tableHeader);

                tableHeader.SpacingBefore = 20f;
                tableHeader.SpacingAfter = 15f;

                PdfPTable tableConsultantHeader = new PdfPTable(4);

                PdfPCell nameLabelCell = new PdfPCell(new Phrase("Consultant Name: ", boldArial));
                nameLabelCell.Border = 0;
                tableConsultantHeader.AddCell(nameLabelCell);
                PdfPCell nameCell = new PdfPCell(new Phrase(user.FirstName + " " + user.LastName));
                nameCell.Border = 0;
                tableConsultantHeader.AddCell(nameCell);


                PdfPCell clientLabelCell = new PdfPCell(new Phrase("Client: ", boldArial));
                clientLabelCell.Border = 0;
                tableConsultantHeader.AddCell(clientLabelCell);
                PdfPCell clientCell = new PdfPCell(new Phrase(client.Name));
                clientCell.Border = 0;
                tableConsultantHeader.AddCell(clientCell);

                PdfPCell periodLabelCell = new PdfPCell(new Phrase("Period Ending Date: ", boldArial));
                periodLabelCell.Border = 0;
                tableConsultantHeader.AddCell(periodLabelCell);
                var contractEndDate = user.ContractEndTime == null ? "" : user.ContractEndTime.Value.ToShortDateString();
                PdfPCell periodCell = new PdfPCell(new Phrase(contractEndDate));
                periodCell.Border = 0;
                tableConsultantHeader.AddCell(periodCell);

                PdfPCell supervisorLabelCell = new PdfPCell(new Phrase("Supervisor: ", boldArial));
                supervisorLabelCell.Border = 0;
                tableConsultantHeader.AddCell(supervisorLabelCell);
                PdfPCell supervisorCell = new PdfPCell(new Phrase(ClientUser?.FirstName + " " + ClientUser?.LastName));
                supervisorCell.Border = 0;
                tableConsultantHeader.AddCell(supervisorCell);

                PdfPCell departmentLableCell = new PdfPCell(new Phrase("Department: ", boldArial));
                departmentLableCell.Border = 0;
                tableConsultantHeader.AddCell(departmentLableCell);
                PdfPCell departmentCell = new PdfPCell(new Phrase(user.Department));
                departmentCell.Border = 0;
                tableConsultantHeader.AddCell(departmentCell);

                PdfPCell projectLableCell = new PdfPCell(new Phrase("Project Name: ", boldArial));
                projectLableCell.Border = 0;
                tableConsultantHeader.AddCell(projectLableCell);
                PdfPCell projectCell = new PdfPCell(new Phrase(user.ProjectName));
                projectCell.Border = 0;
                tableConsultantHeader.AddCell(projectCell);

                tableConsultantHeader.SpacingBefore = 20f;
                tableConsultantHeader.SpacingAfter = 30f;

                document.Add(tableConsultantHeader);

                PdfPTable table = new PdfPTable(3);
                PdfPCell dayCell = new PdfPCell(new Phrase("Day", boldArial));
                dayCell.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                table.AddCell(dayCell);
                PdfPCell dateCell = new PdfPCell(new Phrase("Date", boldArial));
                dateCell.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right
                table.AddCell(dateCell);
                PdfPCell hoursCell = new PdfPCell(new Phrase("Hours", boldArial));
                hoursCell.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right
                table.AddCell(hoursCell);

                float totalWorkingHours = 0;

                foreach (var ts in timesheetVm)
                {
                    table.AddCell(ts.Day);
                    table.AddCell(ts.Date.ToShortDateString());
                    table.AddCell(ts.WorkHours.ToString());
                    totalWorkingHours += ts.WorkHours;
                }

                PdfPCell totalHoursCell = new PdfPCell(new Phrase("Total", boldArial));
                table.AddCell(totalHoursCell);

                //table.AddCell("Total");
                table.AddCell("");
                table.AddCell(totalWorkingHours.ToString());

                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                document.Add(table);

                PdfPTable tableClientSignatureFooter = new PdfPTable(1);
                tableClientSignatureFooter.AddCell("Client: I certify that I am satisfied with and I accept the consultant’s work during the period stated above.");
                tableClientSignatureFooter.AddCell("Authorized Signature:____________________		Date:_________________");

                tableClientSignatureFooter.SpacingBefore = 20f;
                tableClientSignatureFooter.SpacingAfter = 30f;

                document.Add(tableClientSignatureFooter);

                PdfPTable tableConsultantSignatureFooter = new PdfPTable(1);
                tableConsultantSignatureFooter.AddCell("Consultant:  I certify that only actual hours worked have been recorded above.");
                tableConsultantSignatureFooter.AddCell("Authorized Signature:____________________		Date:_________________");

                document.Add(tableConsultantSignatureFooter);

                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }
        }
        public byte[] WriteBytes(IList<TimesheetVm> timesheetVm, User user, User ClientUser)
        {
            var FileContents = GeneratePdf(TimesheetVm, user, ClientUser);
            return FileContents;
            //Rpt = string.Format(Rpt);

            //DirectoryInfo fc = new DirectoryInfo(@"C:\\Temp\");
            //if (!fc.Exists)
            //{
            //    Directory.CreateDirectory(@"C:\\Temp\");
            //}
            //System.IO.File.WriteAllBytes(Rpt, FileContents);
            //return Rpt;
        }

    }
}