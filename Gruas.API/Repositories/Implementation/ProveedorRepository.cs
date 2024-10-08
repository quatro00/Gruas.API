using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly GruasContext context;
        public ProveedorRepository(GruasContext context) => this.context = context;
        public async Task<ResponseModel> ActivarDesactivarProveedor(Guid id, bool activo, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {

                var results = await context.Proveedors.Where(x => x.Id == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.Activo, t => activo)
                    );

                await context.SaveChangesAsync();

                rm.result = results;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> GetProveedor(Guid id)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.Proveedors.Select(s => new GetProveedor_Response()
                {
                    id = s.Id,
                    noProveedor = s.NoProveedor,
                    razonSocial = s.RazonSocial,
                    direccion = s.Direccion,
                    telefono_1 = s.Telefono1,
                    telefono_2 = s.Telefono2,
                    rfc = s.Rfc,
                    banco = s.Banco,
                    cuenta = s.Cuenta
                }).Where(x=>x.id == id).FirstOrDefaultAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetProveedores()
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.Proveedors.Select(s => new GetProveedor_Response()
                {
                    id = s.Id,
                    noProveedor = s.NoProveedor,
                    razonSocial = s.RazonSocial,
                    direccion = s.Direccion,
                    telefono_1 = s.Telefono1,
                    telefono_2 = s.Telefono2,
                    rfc = s.Rfc,
                    banco = s.Banco,
                    cuenta = s.Cuenta,
                    activo = s.Activo
                }).ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> InsProveedor(InsProvedor_Request model, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                Proveedor item = new Proveedor()
                {
                    Id = Guid.NewGuid(),
                    RazonSocial = model.razonSocial,
                    Direccion = model.direccion,
                    Telefono1 = model.telefono_1,
                    Telefono2 = model.telefono_2,
                    Banco = model.banco,
                    Cuenta = model.cuenta,
                    Rfc = model.rfc,
                    EstadoId = Guid.Parse("B31F8E4F-DF16-489F-BE8A-ED370DCF5F29"),
                    Activo = false,
                    UsuarioCreadionId = usuarioId,
                    FechaCreacion = DateTime.Now,
                };

                await context.Proveedors.AddAsync(item);
                await context.SaveChangesAsync();

                rm.SetResponse(true);
            }
            catch (Exception e)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> UpdProveedor(UpdProvedor_Request model, Guid id, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {

                var results = await context.Proveedors.Where(x => x.Id == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.RazonSocial, t => model.razonSocial)
                    .SetProperty(t => t.Direccion, t => model.direccion)
                    .SetProperty(t => t.Telefono1, t => model.telefono_1)
                    .SetProperty(t => t.Telefono2, t => model.telefono_2)
                    .SetProperty(t => t.Rfc, t => model.rfc)
                    .SetProperty(t => t.Cuenta, t => model.cuenta)
                    .SetProperty(t => t.Banco, t => model.banco)
                    );

                await context.SaveChangesAsync();

                rm.result = results;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }
    }
}
