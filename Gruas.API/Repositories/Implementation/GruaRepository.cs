using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Models.Grua;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class GruaRepository : IGruaRepository
    {
        private readonly GruasContext dbContext;
        public GruaRepository(GruasContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ResponseModel> Create(CreateGrua_Request model, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                Grua grua = new Grua()
                {
                    Id = Guid.NewGuid(),
                    ProveedorId = model.proveedorId,
                    Placas = model.placas.ToUpper(),
                    Marca = model.marca.ToUpper(),
                    Modelo = model.modelo.ToUpper(),
                    TipoGruaId = model.tipoGruaId,
                    Anio = model.anio,
                    Activo = model.activo == 1 ? true : false,
                    UsuarioCreadionId = Guid.Parse(usuarioId),
                    FechaCreacion = DateTime.Now
                };

                await this.dbContext.Gruas.AddAsync(grua);
                await this.dbContext.SaveChangesAsync();

                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> Get()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<GetGrua_Response> gruas = await
                this.dbContext.Gruas.Include(x=> x.Proveedor).Include(x=>x.TipoGrua).Select(x => new GetGrua_Response()
                {
                    id = x.Id,
                    proveedorId = x.ProveedorId,
                    tipoGruaId = x.TipoGruaId,
                    proveedor = x.Proveedor.RazonSocial,
                    tipoGrua = x.TipoGrua.Descripcion,
                    placas = x.Placas,
                    marca = x.Marca,
                    modelo = x.Modelo,
                    anio = x.Anio,
                    activo = x.Activo ? 1 : 0,
                }).ToListAsync();
                
                rm.result = gruas;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> Get(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                GetGrua_Response? grua = await
                this.dbContext.Gruas.Include(x => x.Proveedor).Include(x => x.TipoGrua).Where(x=>x.Id == id).Select(x => new GetGrua_Response()
                {
                    id = x.Id,
                    proveedorId = x.ProveedorId,
                    tipoGruaId = x.TipoGruaId,
                    proveedor = x.Proveedor.RazonSocial,
                    tipoGrua = x.TipoGrua.Descripcion,
                    placas = x.Placas,
                    marca = x.Marca,
                    modelo = x.Modelo,
                    anio = x.Anio,
                    activo = x.Activo ? 1 : 0,
                }).FirstOrDefaultAsync();

                rm.result = grua;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> GetGruasProveedor(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var cuenta = await this.dbContext.Cuenta.Where(x => x.Id == id).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null)
                {
                    proveedor = cuenta.ProveedorId;
                }


                List<GetGrua_Response> gruas = await
                this.dbContext.Gruas.Include(x => x.Proveedor).Include(x => x.TipoGrua).Where(x => x.ProveedorId == proveedor && x.Activo == true).Select(x => new GetGrua_Response()
                {
                    id = x.Id,
                    proveedorId = x.ProveedorId,
                    tipoGruaId = x.TipoGruaId,
                    proveedor = x.Proveedor.RazonSocial,
                    tipoGrua = x.TipoGrua.Descripcion,
                    placas = x.Placas,
                    marca = x.Marca,
                    modelo = x.Modelo,
                    anio = x.Anio,
                    activo = x.Activo ? 1 : 0,
                }).ToListAsync();

                rm.result = gruas;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, "Ocurrio un error inesperado.");
            }
            return rm;
        }

        public async Task<ResponseModel> Update(UpdateGrua_Request model, Guid id, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var results = await dbContext.Gruas.Where(x => x.Id == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.Placas, t => model.placas.ToUpper())
                    .SetProperty(t => t.Marca, t => model.marca.ToUpper())
                    .SetProperty(t => t.Modelo, t => model.modelo.ToUpper())
                    .SetProperty(t => t.TipoGruaId, t => model.tipoGruaId)
                    .SetProperty(t => t.Anio, t => model.anio)
                    .SetProperty(t => t.Activo, t => model.activo == 1 ? true : false)
                    .SetProperty(t => t.FechaModificacion, t => DateTime.Now)
                    .SetProperty(t => t.UsuarioModificacionId, t => Guid.Parse(usuarioId))
                );

                await dbContext.SaveChangesAsync();

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
