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
                var usuarios = new List<SPQ_GetUsuarios>();

                using var connection = dbContext.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "EXEC SPQ_GetUsuarios @Tipo, @CorreoElectronico, @Telefono";

                // Agregar parámetros
                var tipoParam = command.CreateParameter();
                tipoParam.ParameterName = "@Tipo";
                tipoParam.Value = model.tipo ?? (object)DBNull.Value;
                command.Parameters.Add(tipoParam);

                var correoParam = command.CreateParameter();
                correoParam.ParameterName = "@CorreoElectronico";
                correoParam.Value = model.correoElectronico ?? (object)DBNull.Value;
                command.Parameters.Add(correoParam);

                var telefonoParam = command.CreateParameter();
                telefonoParam.ParameterName = "@Telefono";
                telefonoParam.Value = model.telefono ?? (object)DBNull.Value;
                command.Parameters.Add(telefonoParam);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    usuarios.Add(new SPQ_GetUsuarios
                    {
                        id = Guid.Parse(reader["Id"]?.ToString() ?? ""),
                        userName = reader["UserName"]?.ToString() ?? "",
                        nombre = reader["Nombre"]?.ToString() ?? "",
                        apellidos = reader["Apellidos"]?.ToString() ?? "",
                        correoElectronico = reader["CorreoElectronico"]?.ToString() ?? "",
                        telefono = reader["Telefono"]?.ToString() ?? "",
                        razonSocial = reader["RazonSocial"]?.ToString() ?? "",
                        Roles = reader["Roles"]?.ToString() ?? "",
                        activo = reader["Activo"] != DBNull.Value && Convert.ToBoolean(reader["Activo"]) ? 1 : 0
                    });
                }

                rm.result = usuarios;

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
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
