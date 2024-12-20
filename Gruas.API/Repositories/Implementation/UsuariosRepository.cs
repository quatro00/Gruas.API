using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.StoredProcedures;
using Gruas.API.Models.DTO.Usuarios;
using Gruas.API.Models.Grua;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly GruasContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        public UsuariosRepository(GruasContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<ResponseModel> CreateUsuarioProveedor(CreateUsuario_Request model, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var id = Guid.NewGuid();

                //validamos que el correo electronico no este registrado
                if(await this.dbContext.AspNetUsers.Where(x=>x.PhoneNumber.ToUpper() == model.telefono.ToUpper()).AnyAsync())
                {
                    rm.SetResponse(false, "El teléfono ya esta registrado.");
                    return rm;
                }
                //validamos que el telefono no este registrado

                if (await this.dbContext.AspNetUsers.Where(x => x.Email.ToUpper() == model.correoElectronico.ToUpper()).AnyAsync())
                {
                    rm.SetResponse(false, "El correo electrónico ya esta registrado.");
                    return rm;
                }


                if (await this.dbContext.AspNetUsers.Where(x => x.UserName.ToUpper() == model.nombreUsuario.ToUpper()).AnyAsync())
                {
                    rm.SetResponse(false, "El nombre de usuario ya esta registrado.");
                    return rm;
                }

                IdentityUser user1 = new IdentityUser()
                {
                    Id =id.ToString().ToUpper(),
                    UserName = model.nombreUsuario,
                    NormalizedUserName = model.nombreUsuario.ToUpper(),
                    Email = model.correoElectronico,
                    NormalizedEmail = model.correoElectronico.ToUpper(),
                    EmailConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = model.telefono,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                };


                string password = this.userManager.PasswordHasher.HashPassword(user1, model.password);
                user1.PasswordHash = password;
                
                Cuentum cuenta = new Cuentum()
                {
                    Id = id,
                    NombreUsuario = model.nombreUsuario,
                    Nombre = model.nombre,
                    Apellidos = model.apellidos,
                    CorreoElectronico = model.correoElectronico,
                    Telefono = model.telefono,
                    ProveedorId = model.proveedorId,
                    Activo = true,
                    UsuarioCreadionId = Guid.Parse(usuarioId),
                    FechaCreacion = DateTime.Now
                };

                //creamos el usuario
                await this.userManager.CreateAsync(user1);
                await this.userManager.AddToRoleAsync(user1, "Colaborador");
                await this.dbContext.Cuenta.AddAsync(cuenta);
                await this.dbContext.SaveChangesAsync();

                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> Get(GetUsuarios_Request model)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<SpqGetUsuariosResult> result = await dbContext.SpqGetUsuariosResults
                   .FromSqlRaw(
                        "EXECUTE dbo.SPQ_GetUsuarios @Tipo = {0}, @CorreoElectronico = {1}, @Telefono = {2}",
                        model.tipo ?? (object)DBNull.Value,
                        model.correoElectronico ?? (object)DBNull.Value,
                        model.telefono ?? (object)DBNull.Value
                    )
                    .ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public Task<ResponseModel> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> Update(UpdateUsuario_Request model, Guid id, string usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
