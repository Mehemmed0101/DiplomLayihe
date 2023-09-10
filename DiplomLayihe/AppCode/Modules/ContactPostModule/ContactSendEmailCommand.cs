using Back_End_Layihe.AppCode.InfraStructure;
using DiplomLayihe.Models.DataContext;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.ContactPostModule
{
    public class ContactSendEmailCommand : IRequest<CommandJsonResponse>
    {
        public int Id { get; set; }
        //public string Email { get; set; }
        //public string Answer { get; set; }
        //public string Comment { get; set; }
        //public string Subject { get; set; }

        public class ContactSendEmailCommandHandler : IRequestHandler<ContactSendEmailCommand, CommandJsonResponse>
        {
            readonly DiplomDbContext db;
            readonly IConfiguration configuration;
            readonly IActionContextAccessor ctx;
            public ContactSendEmailCommandHandler(DiplomDbContext db,
                IConfiguration configuration,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.configuration = configuration;
                this.ctx = ctx;
            }
            public async Task<CommandJsonResponse> Handle(ContactSendEmailCommand request, CancellationToken cancellationToken)
            {
                var displayName = configuration["emailAccount:displayName"];
                var smtpServer = configuration["emailAccount:smtpServer"];
                var smtpPort = Convert.ToInt32(configuration["emailAccount:smtpPort"]);
                var userName = configuration["emailAccount:userName"];
                var password = configuration["emailAccount:password"];
                var cc = configuration["emailAccount:cc"];

                var entity = await db.ContactUs
                    .FirstOrDefaultAsync(cp => cp.Id == request.Id, cancellationToken);

                try
                {

                    SmtpClient client = new SmtpClient(smtpServer, smtpPort);

                    client.Credentials = new NetworkCredential(userName, password);
                    client.EnableSsl = true;

                    //var from = ;
                    MailMessage message = new MailMessage(new MailAddress(userName, displayName), new MailAddress(entity.Email));
                    message.Subject = entity.Subject;
                    message.Body = entity.Answer;
                    message.IsBodyHtml = true;


                    string[] ccs = cc.Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in ccs)
                    {
                        message.Bcc.Add(item);

                    }


                    client.SendAsync(message, cancellationToken);
                    

                    entity.EmailSended = true;

                    await db.SaveChangesAsync(cancellationToken);


                }
                catch (Exception ex)
                {
                    return new CommandJsonResponse
                    {
                        Error = true,
                        Message = "Texniki problem chixdi brad uzurlu say !!!"
                    };
                }

                return new CommandJsonResponse
                {
                    Error = false,
                    Message = "Cavab Gonderildi"
                };
            }
        }
    }
}
