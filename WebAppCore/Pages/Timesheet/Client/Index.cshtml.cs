using BusinessServices.Contracts;
using Common.Constants;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using User = Models.BO.User;

namespace WebAppCore.Pages.Timesheet.Client
{
    [Authorize(Roles = "Client")]
    public class IndexModel : PageModel
    {

        // Constants need to be set:
        //private const string accessToken = "";
        //private const string accountId = "";
        private const string signerName = "Pranay Mohite";
        private const string signerEmail = "pranay@uciny.com";
        //private const string docName = "World_Wide_Corp_lorem.pdf";

        // Additional constants
        //private const string signerClientId = "1000";
        //private const string basePath = "https://demo.docusign.net/restapi";

        // Change the port number in the Properties / launchSettings.json file:
        //     "iisExpress": {
        //        "applicationUrl": "http://localhost:5050",
        //private const string returnUrl = "";
        //private const string returnUrl = "ucitims.azurewebsites.net/Timesheet/Client/DSReturn";


        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor accessor;

        [BindProperty]
        public WeeksOfYearVm WeeksOfYearVm { get; set; }

        [BindProperty]
        public string CurrentWeekNumber { get; set; }

        public IList<User> Users { get; set; }
        public IList<UserVm> UserVms { get; set; }

        [BindProperty]
        public IList<TimesheetVm> TimesheetVm { get; set; }


        [BindProperty]
        public string SelectedUser { get; set; }

        [BindProperty]
        public bool isApproved { get; set; }

        [BindProperty]
        public string ApproverComments { get; set; }

        [BindProperty]
        public int TotalWorkingHours { get; set; }
        [BindProperty]
        public int TotalLunchHours { get; set; }

        [BindProperty]
        public DateTime CurrentDate { get; set; } = DateTime.Now.Date;

        [BindProperty]
        public DateTime TimesheetStartDate { get; set; } = DateTime.Now.Date.AddDays(-6);

        [BindProperty]
        public DateTime TimesheetEndDate { get; set; } = DateTime.Now.Date;

        public IndexModel(IServiceFactory serviceFactory, UserManager<User> userManager,
            IHttpContextAccessor accessor)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
            this.accessor = accessor;
        }

        public async Task OnPostDelete()
        {
            TempData["SelectedUser"] = SelectedUser;
            CurrentWeekNumber = TempData["CurrentWeekNumber"].ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;

            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(CurrentWeekNumber));

            //TimesheetVm = this.serviceFactory.TimesheetEntryService.GetTimesheetsByUserId(SelectedUser).ToList();
            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedSubmittedTSForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, SelectedUser).ToList();

            var user = await this.userManager.FindByNameAsync(User.Identity.Name);
            PopulateUsers(user);
        }

        public async Task OnPostEdit(string weekId, string weekId1)
        {
            CurrentWeekNumber = TempData["CurrentWeekNumber"].ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;

            var user = await this.userManager.FindByNameAsync(User.Identity.Name);
            PopulateUsers(user);

            SelectedUser = TempData["SelectedUser"].ToString();
            TempData["SelectedUser"] = SelectedUser;

            TimesheetVm = new List<TimesheetVm>();
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(weekId));
            if (weekId1 == "dec")
            {
                WeeksOfYearVm.WeekNumber -= 1;
            }
            else if (weekId1 == "inc")
            {
                WeeksOfYearVm.WeekNumber += 1;
            }
            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(WeeksOfYearVm.WeekNumber);
            TempData["CurrentWeekNumber"] = WeeksOfYearVm.WeekNumber;

            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedSubmittedTSForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, SelectedUser).ToList();
        }

        private byte[] GeneratePdf(IList<TimesheetVm> timesheetVm, User user, User ClientUser)
        {
            var client = this.serviceFactory.ClientService.GetById(Guid.Parse(user.Client));

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10, 10, 10, 10);

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

                tableHeader.SpacingBefore = 50f;
                tableHeader.SpacingAfter = 15f;

                document.Add(tableHeader);



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
            return GeneratePdf(TimesheetVm, user, ClientUser);
        }

        public async Task<IActionResult> OnGet()
        {
            //if(urlUserId != null)
            //{

            //}
            var user = await this.userManager.FindByNameAsync(User.Identity.Name);

            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByDate(DateTime.Now.Date);
            CurrentWeekNumber = WeeksOfYearVm.WeekNumber.ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;

            if (user == null)
            {
                return RedirectToPage("./NotFound");
            }

            PopulateUsers(user);

            TimesheetVm = new List<TimesheetVm>();
            return Page();
        }

        private void PopulateUsers(User user)
        {
            var users = this.serviceFactory.UserService.GetAll().ToList();
            UserVms = new List<UserVm>();
            if (users.Count > 0)
            {
                Users = users.Where(u => u.Client.ToLower() == user.Client.ToLower()).ToList().Except(new List<User> { user }).ToList();
            }
            foreach (var ur in Users)
            {
                UserVms.Add(new UserVm { Id = ur.Id, Name = ur.FirstName + " " + ur.LastName });
            }
        }

        public async Task<ViewUrl> digiSign(IList<TimesheetVm> tempTimesheetVm, User user, User clientUser)
        {
            // Embedded Signing Ceremony
            // 1. Create envelope request obj
            // 2. Use the SDK to create and send the envelope
            // 3. Create Envelope Recipient View request obj
            // 4. Use the SDK to obtain a Recipient View URL
            // 5. Redirect the user's browser to the URL

            // 1. Create envelope request object
            //    Start with the different components of the request
            //    Create the document object


            DocuSign.eSign.Model.Document document = new DocuSign.eSign.Model.Document
            {
                DocumentBase64 = Convert.ToBase64String(WriteBytes(tempTimesheetVm, user, clientUser)),
                Name = user.FirstName + "_" + user.LastName + " Timesheet",
                FileExtension = "pdf",
                DocumentId = "1"
            };
            DocuSign.eSign.Model.Document[] documents = new DocuSign.eSign.Model.Document[] { document };

            // Create the signer recipient object 
            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = DocuSignConstants.SIGNER_CLIENT_ID,
                RecipientId = "1",
                RoutingOrder = "1"
            };

            // Create the sign here tab (signing field on the document)
            SignHere signHereTab = new SignHere
            {
                DocumentId = "1",
                PageNumber = "1",
                RecipientId = "1",
                TabLabel = "Sign Here Tab",
                XPosition = "195",
                YPosition = "400"
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            // Add the sign here tab array to the signer object.
            signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
            // Create array of signer objects
            Signer[] signers = new Signer[] { signer };
            // Create recipients object
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
            // Bring the objects together in the EnvelopeDefinition
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            {
                EmailSubject = "Please sign the document",
                Documents = new List<DocuSign.eSign.Model.Document>(documents),
                Recipients = recipients,
                Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(DocuSignConstants.BASE_PATH);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + DocuSignConstants.ACCESS_TOKEN);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = await envelopesApi.CreateEnvelopeAsync(DocuSignConstants.ACCOUNT_ID, envelopeDefinition);

            // 3. Create Envelope Recipient View request obj
            string envelopeId = results.EnvelopeId;
            RecipientViewRequest viewOptions = new RecipientViewRequest
            {
                ReturnUrl = DocuSignConstants.RETURN_URL,
                ClientUserId = DocuSignConstants.SIGNER_CLIENT_ID,
                AuthenticationMethod = "none",
                UserName = signerName,
                Email = signerEmail
            };

            // 4. Use the SDK to obtain a Recipient View URL
            ViewUrl viewUrl = await envelopesApi.CreateRecipientViewAsync(DocuSignConstants.ACCOUNT_ID, envelopeId, viewOptions);
            return viewUrl;
            //return Redirect(viewUrl.Url);
        }

        public async Task<IActionResult> OnPostMain(List<TimesheetVm> timesheetVm, string buttonId, string selecteduserId)
        {
            SelectedUser = TempData["SelectedUser"].ToString();
            TempData["SelectedUser"] = SelectedUser;
            CurrentWeekNumber = TempData["CurrentWeekNumber"].ToString();
            TempData["CurrentWeekNumber"] = CurrentWeekNumber;

            WeeksOfYearVm = this.serviceFactory.WeeksOfYearService.GetWeekByWeekNumber(Convert.ToInt32(CurrentWeekNumber));

            //TimesheetVm = this.serviceFactory.TimesheetEntryService.GetTimesheetsByUserId(selecteduserId).ToList();
            TimesheetVm = this.serviceFactory.TimesheetEntryService.GetUnApprovedSubmittedTSForWeek(WeeksOfYearVm.StartDate, WeeksOfYearVm.EndDate, SelectedUser).ToList();

            //var tempTimesheetVm = new List<TimesheetVm>();
            //tempTimesheetVm = timesheetVm;

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var consultant = await userManager.FindByIdAsync(SelectedUser);

            if (user == null)
            {
                return RedirectToPage("./NotFound");
            }

            if (buttonId == "approve")
            {
                foreach (var entry in TimesheetVm)
                {
                    //entry.isApproved = isApproved;
                    entry.isApproved = true;
                    entry.isRejected = false;
                    entry.ApprovedBy = user.Id;
                    entry.ApprovedDate = DateTime.Now;
                    entry.ApprovedFrom = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    entry.CommentFromApprover = ApproverComments;
                }

                SendEmail(user, consultant, buttonId);

            }
            if (buttonId == "reject")
            {
                foreach (var entry in TimesheetVm)
                {
                    entry.isRejected = true;
                    entry.isApproved = false;
                    entry.CommentFromApprover = ApproverComments;
                }
               
                SendEmail(user, consultant, buttonId);
            }

            this.serviceFactory.TimesheetEntryService.UpdateRange(TimesheetVm);

            //client
            User ClientUser = await GetClientForUser(user);

            if (buttonId == "approve")
            {
                var viewUrl = await digiSign(TimesheetVm, user, ClientUser);
                return Redirect(viewUrl.Url);
            }

            return RedirectToPage("./Success");
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

        private void SendEmail(User user, User consultant, string action)
        {
            List<string> toList = new List<string>();
            List<string> ccList = new List<string>();

            toList.Add(consultant.Email);//consultant
            ccList.Add(user.Email); //client
            ccList.Add(ApplicationContants.UCI_Admin_Email);//UCI admin

            string subject = $"Timesheet of {consultant.FirstName} {consultant.LastName} for week - {CurrentWeekNumber} is {action}ed.";

            string comment = (action == "reject" ? ApproverComments : string.Empty);
            string body = "";

            if (comment != string.Empty)
            {
                body = $"Dear <b>{consultant.FirstName} {consultant.LastName}, </b> " +
                         $"<br/> Your Timesheet for week - {CurrentWeekNumber} is {action}ed by <b>{user.FirstName} {user.LastName} " +
                         $"with following comments: <span style='color:red;font-weight:bold;'> {comment} </span>" +
                         $"<br/> <br/> Please login at {ApplicationContants.UCI_TMS_Web_Url} to take appropriate action.";
            }
            else
            {
                body = $"Dear <b>{consultant.FirstName} {consultant.LastName}, </b> " +
                         $"<br/> Your Timesheet for week - {CurrentWeekNumber} is {action}ed. by <b>{user.FirstName} {user.LastName}";
            }
           
            this.serviceFactory.EmailService.SendEmail(toList, ccList, subject, body);
        }
    }
}