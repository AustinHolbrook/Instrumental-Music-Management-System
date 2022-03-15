using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021IMMS.Services
{
    public interface IEmailService
    {
        Task SendEmail(string strToName, string strToAddress, string strSubject, string strBody);
    }
}
