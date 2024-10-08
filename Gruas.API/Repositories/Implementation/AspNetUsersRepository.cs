using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.DTO.Auth;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Gruas.API.Utils;
using Gruas.API.Models.DTO.Usuarios;

namespace Gruas.API.Repositories.Implementation
{
    public class AspNetUsersRepository : IAspNetUsersRepository
    {
        private readonly GruasContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public AspNetUsersRepository(GruasContext context, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<ResponseModel> ForgotPassword(ForgotPasswordRequestDto model, string jwt)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var user = await context.AspNetUsers.Where(x => x.Email == model.Email && x.UserName == model.Username).FirstAsync();
                if (user is not null)
                {
                    SmtpClient smtpClient = new SmtpClient()
                    {
                        Host = configuration["Smtp:Host"]!,
                        EnableSsl = true,
                        Credentials = new NetworkCredential(configuration["Smtp:Email"], configuration["Smtp:Password"]),
                        Port = int.Parse(configuration["Smtp:Port"]!),
                    };

                    smtpClient.UseDefaultCredentials = false;

                    List<HtmlElement> htmlElements = new List<HtmlElement>() {
                        new HtmlElement()
                        {
                            id = "RestorePassword-Button",
                            element = "href",
                            elementValue = $"{configuration["Restore:Host"]}?token={jwt}"
                        }
                    };

                    MailMessage mailMessage = new MailMessage()
                    {
                        From = new MailAddress(configuration["Smtp:Email"]!),
                        Subject = "Recuperar Contraseña",
                        Body = new Html(configuration["Smtp:RestorePasswordTemplate"]!).Parse(htmlElements),
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(model.Email);

                    smtpClient.Send(mailMessage);

                    rm.SetResponse(true);
                }
            }
            catch (Exception ex)
            {
                rm.SetResponse(false, ex.Message);
            }

            return rm;
        }

        public async Task<ResponseModel> GetUserById(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var cuenta = await context.Cuenta.Where(x => x.Id == id).FirstOrDefaultAsync();
                var result = new UsuarioDto()
                {
                    nombre = cuenta.Nombre,
                    apellido = cuenta.Apellidos,
                    correo = cuenta.CorreoElectronico,
                    telefono = cuenta.Telefono,
                    ciudad = "",
                    descripcion = "",
                    activo = cuenta.Activo,
                   
                };
                var results = result;



                rm.result = results;
                rm.SetResponse(true);
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
            }

            return rm;
        }
    }
}
