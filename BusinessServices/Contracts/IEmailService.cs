using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Contracts
{
    public interface IEmailService
    {
        void SendEmail(List<string> toList, List<string> ccList, string subject, string body);
    }
}
