using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.DTO.Catalogo;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class CatalogoRepository : ICatalogoRepository
    {
        private readonly GruasContext context;
        public CatalogoRepository(GruasContext context) => this.context = context;
        public async Task<ResponseModel> GetTipoGrua()
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.TipoGruas.Select(s => new Catalogo_Response()
                {
                    id = s.Id,
                    descripcion = s.Descripcion,
                }).OrderBy(x => x.id).ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetEstatusServicio()
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.EstatusServicios.Select(s => new Catalogo_Response()
                {
                    id = s.Id,
                    descripcion = s.Descripcion,
                }).OrderBy(x=>x.id).ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetEstatusPago()
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.EstatusPagos.Select(s => new Catalogo_Response()
                {
                    id = s.Id,
                    descripcion = s.Descripcion,
                }).OrderBy(x => x.id).ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetEstados()
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.Estados.Select(s => new Catalogo_Response()
                {
                    id = s.Id,
                    descripcion = s.Nombre,
                }).OrderBy(x => x.id).ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetTipoServicio()
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.TipoServicios.Select(s => new Catalogo_Response()
                {
                    id = s.TipoServicioId,
                    descripcion = s.Descripcion,
                }).OrderBy(x => x.id).ToListAsync();

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }
    }
}
