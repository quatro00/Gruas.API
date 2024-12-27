using DocumentFormat.OpenXml.Drawing.Charts;
using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Pagos;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Models.DTO.Reportes;
using Gruas.API.Models.Enums;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Gruas.API.Repositories.Implementation
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly GruasContext context;
        public ReportesRepository(GruasContext context) => this.context = context;

        public async Task<ResponseModel> GetGruas(GetGruas_Request model)
        {
            ResponseModel rm = new ResponseModel();

            try
            {

                var result = await context.Gruas.Include(x=>x.TipoGrua).Include(x => x.Proveedor)
                    .Where(x =>
                        (x.ProveedorId == model.proveedorId || model.proveedorId == null) &&
                        (x.TipoGruaId == model.tipoGruaId || model.tipoGruaId == null) &&
                        (x.Placas.ToUpper().Contains(model.placas ?? "") || model.placas == null))
                    .Select(s => new GetGruas_Response()
                    {
                        id = s.Id,
                        proveedor = $"{s.Proveedor.RazonSocial}",
                        tipoGrua = s.TipoGrua.Descripcion,
                        placas = s.Placas,
                        marca = s.Marca,
                        modelo = s.Modelo,
                        anio = s.Anio,
                        activo = s.Activo ? 1 : 0
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

        public async Task<ResponseModel> GetPagos(GetPagos_Request model)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                
                var result = await context.Pagos.Include(x=>x.Proveedor).Include(x=>x.EstatusPago).Include(x=>x.PagoDetalles)
                    
                    .Where(x => 
                        (x.EstatusPagoId == model.estatusPagoId || model.estatusPagoId == null) &&
                        (x.ProveedorId == model.proveedorId || model.proveedorId == null) &&
                        (x.FechaCreacion >= model.fechaInicio || model.fechaInicio == null) &&
                        (x.FechaCreacion <= model.fechaTermino || model.fechaTermino == null))
                    .Select(s => new GetPagos_Response()
                    {
                        id = s.Id,
                        folio = s.Folio,
                        proveedor = $"{s.Proveedor.RazonSocial}",
                        fechaCreacion = s.FechaCreacion,
                        fechaPago = s.FechaPago,
                        referencia = s.Referencia ?? "",
                        concepto = s.Concepto,
                        estatusPago = s.EstatusPago.Descripcion,
                        cantidadServicios = s.PagoDetalles.Count(),
                        subTotal = s.PagoDetalles.Sum(x => x.SubTotal),
                        comision = s.PagoDetalles.Sum(x => x.Comision),
                        total = s.PagoDetalles.Sum(x => x.Total),
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

        public async Task<ResponseModel> GetPagosProveedor(GetPagosProveedor_Request model, Guid? proveedorId)
        {
            ResponseModel rm = new ResponseModel();


            DateTime fechaInicial = DateTime.ParseExact(model.periodo, "yyyy-MM", CultureInfo.InvariantCulture);
            DateTime fechaTermino = fechaInicial.AddMonths(1);


            try
            {
                var cuenta = await this.context.Cuenta.Where(x=>x.Id == proveedorId).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null) {
                    proveedor = cuenta.ProveedorId;
                }
                var result = await context.Pagos
                    .Where(x =>
                        (x.EstatusPagoId == model.estatusPago || model.estatusPago == 0) &&
                        ((x.FechaCreacion >= fechaInicial && x.FechaCreacion <= fechaTermino)) &&
                        (x.ProveedorId == proveedor || proveedor == null)
                        )
                    .Select(s => new GetPagosProveedor_Result()
                    {
                        id = s.Id,
                        folio = s.Folio,
                        razonSocial = s.Proveedor.RazonSocial,
                        concepto = s.Concepto,
                        monto = s.Monto,
                        referencia = s.Referencia ?? "",
                        fechaPago = s.FechaPago,
                        fechaRegistro = s.FechaCreacion
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

        public async Task<ResponseModel> GetProveedores(GetProveedores_Request model)
        {
            ResponseModel rm = new ResponseModel();

            if(model.descripcion == null)
            {
                model.descripcion = "";
            }
            try
            {

                var result = await context.Proveedors
                    .Where(x =>
                        (x.EstadoId == model.estadoId || model.estadoId == null) &&
                        ((x.RazonSocial.ToUpper().Contains(model.descripcion.ToUpper())) || (x.NoProveedor.ToString().ToUpper().Contains(model.descripcion.ToUpper()))
                        
                        ))
                    .Select(s => new GetProveedores_Response()
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
                        comision = s.Comision,
                        estado = s.Estado.Nombre,
                        activo = s.Activo,
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

        public async Task<ResponseModel> GetServicios(GetServicios_Request model)
        {
            ResponseModel rm = new ResponseModel();

            try
            {

                var result = await context.Servicios
                    .Include(x => x.Proveedor)
                    .Include(x => x.EstatusServicio)
                    .Include(x=>x.Municipio).ThenInclude(x=>x.Estado)
                    .Include(x=>x.Grua).ThenInclude(x=>x.TipoGrua)
                    .Include(x=>x.Cotizacions)
                    .Where(x =>
                        (x.EstatusServicioId == model.estatusServicioId || model.estatusServicioId == null) &&
                        (x.ProveedorId == model.proveedorId || model.proveedorId == null) &&
                        (x.FechaCreacion >= model.fechaInicio || model.fechaInicio == null) &&
                        (x.FechaCreacion <= model.fechaTermino || model.fechaTermino == null))
                    .Select(s => new GetServicios_Response()
                    {
                        id = s.Id,
                        folio = s.Folio,
                        cliente = s.Cliente.Nombre,
                        telefono = s.Cliente.Telefono,
                        estado = s.Municipio != null ? s.Municipio.Estado.Nombre : string.Empty,
                        municipio = s.Municipio != null ? s.Municipio.Nombre : string.Empty,
                        origen = s.OrigenDireccion,
                        destino = s.DestinoDireccion,
                        total = s.Total ?? 0,
                        totalSugerido = s.TotalSugerido,
                        comision = 0,
                        fecha = s.Fecha,
                        estatus = s.EstatusServicio.Descripcion,
                        proveedor = s.Proveedor != null ? s.Proveedor.RazonSocial : string.Empty,//s.,
                        grua = s.Grua != null ? s.Grua.Placas : string.Empty,
                        tipo = s.Grua != null ? s.Grua.TipoGrua.Descripcion : string.Empty,
                        numCotizaciones = s.Cotizacions.Count,
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

        public async Task<ResponseModel> GetServiciosProveedor(GetServiciosProveedor_Request model, Guid? proveedorId)
        {
            ResponseModel rm = new ResponseModel();


            DateTime fechaInicial = DateTime.ParseExact(model.periodo, "yyyy-MM", CultureInfo.InvariantCulture);
            DateTime fechaTermino = fechaInicial.AddMonths(1);


            try
            {
                var cuenta = await this.context.Cuenta.Where(x => x.Id == proveedorId).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null)
                {
                    proveedor = cuenta.ProveedorId;
                }

                var result = await context.Servicios.Include(x=>x.EstatusServicio)
                    .Where(x =>
                        (x.EstatusServicioId == model.estatusServicioId || (model.estatusServicioId ?? 0) == 0) &&
                        (x.TipoServicioId == model.tipoServicioId || (model.tipoServicioId ?? 0) == 0) &&
                        ((x.Fecha >= fechaInicial && x.Fecha <= fechaTermino)) &&
                        (x.ProveedorId == proveedor || proveedor == null)
                        )
                    .Select(s => new GetServiciosProveedor_Result()
                    {
                        id = s.Id,
                        folio = s.Folio,
                        fecha = s.Fecha,
                        origen = $"{s.OrigenDireccion}, {s.OrigenMunicipio}",
                        destino = $"{s.DestinoDireccion}, {s.DestinoMunicipio}",
                        distancia = s.Distancia,
                        cuotaKm = s.CuotaKm,
                        banderazo = s.Banderazo,
                        total = s.Total ?? 0,
                        comision = 0,
                        estatus = s.EstatusServicio.Descripcion,
                        placas = s.Grua != null ? s.Grua.Placas : string.Empty,
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
    }
}
