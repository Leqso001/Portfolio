using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using TourOperator.Services;
using System.Net.Mail;

namespace EmailSender.Service.Interface
{
    public interface IEmailClient
    {
        void Send(MailMessage message);
    }
}
