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
                var result = await context.Pagos.Include(x=>x.EstatusPago).Include(x=>x.PagoDetalles)
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
                        fechaRegistro = s.FechaCreacion,
                        estatusPago = s.EstatusPago.Descripcion,
                        estatusPagoId = s.EstatusPago.Id,
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

        public async Task<ResponseModel> GetServiciosProveedorMensuales(GetServiciosProveedorMensuales_Request model, Guid? proveedorId)
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

                var result = await context.Servicios
                    .Include(x => x.EstatusServicio)
                    .Include(x => x.TipoServicio)
                    .Include(x => x.Cliente)
                    .Include(x => x.Municipio)
                    .Include(x=>x.EstatusServicio)
                    .Include(x=>x.Proveedor)
                    .Include(x=>x.Grua)
                    .Where(x =>
                        ((x.Fecha >= fechaInicial && x.Fecha <= fechaTermino)) &&
                        (x.ProveedorId == proveedor || proveedor == null)
                        )
                    .Select(s => new GetServiciosProveedorMensuales_Response()
                    {
                        id = s.Id,
                        folio = s.Folio,
                        tipoServicioId = s.TipoServicioId,
                        tipoServicio = s.TipoServicio.Descripcion,
                        fecha = s.Fecha,
                        clienteId = s.ClienteId,
                        clienteNombre = $"{s.Cliente.Nombre} {s.Cliente.Apellidos}",
                        clienteTelefono = s.Cliente.Telefono,
                        municipioId = s.MunicipioId,
                        municipio = s.Municipio != null ? s.Municipio.Nombre : string.Empty,
                        tipoVehiculo = s.TipoVehiculo,
                        origenMunicipio = s.OrigenMunicipio,
                        origenDireccion = s.OrigenDireccion,
                        origenLat = s.OrigenLat,
                        origenLon = s.OrigenLon,
                        origenReferencia = s.OrigenReferencia,

                        destinoMunicipio = s.DestinoMunicipio,
                        destinoDireccion = s.DestinoDireccion,
                        destinoLat = s.DestinoLat,
                        destinoLon = s.DestinoLon,
                        destinoReferencia = s.DestinoReferencia,

                        vehiculoAccidentado = s.VehiculoAccidentado,
                        fugaCombustible = s.FugaCombustible,
                        llantasGiran = s.LlantasGiran,
                        puedeNeutral = s.PuedeNeutral,
                        personasEnVehiculo = s.PersonasEnVehiculo,
                        lugarUbicuidad = s.LugarUbicuidad,
                        carreteraCarril = s.CarreteraCarril,
                        carreteraKm = s.CarreteraKm,
                        carreteraDestino = s.CarreteraDestino,
                        estacionamientoTipo = s.EstacionamientoTipo,
                        estacionamientoPiso = s.EstacionamientoPiso,
                        vehiculoMarca = s.VehiculoMarca,
                        vehiculoAnio = s.VehiculoAnio,
                        vehiculoColor = s.VehiculoColor,
                        vehiculoCuentaPlacas = s.VehiculoCuentaPlacas,
                        vehiculoPlacas = s.VehiculoPlacas,
                        vehiculoCuentaModificaciones = s.VehiculoCuentaModificaciones,
                        vehiculoDescripcionModificaciones = s.VehiculoDescripcionModificaciones,
                        distancia = s.Distancia,
                        cuotaKm = s.CuotaKm,
                        banderazo = s.Banderazo,
                        maniobras = s.Maniobras,
                        maniobrasCosto = s.ManiobrasCosto,
                        totalSugerido = s.TotalSugerido,
                        estatusServicioId = s.EstatusServicioId,
                        estatusServicio = s.EstatusServicio.Descripcion,
                        proveedorId = s.ProveedorId,
                        proveedorRfc = s.Proveedor != null ? s.Proveedor.Rfc : string.Empty,
                        proveedorRazonSocial = s.Proveedor != null ? s.Proveedor.RazonSocial : string.Empty,
                        proveedorTelefono = s.Proveedor != null ? s.Proveedor.Telefono1 : string.Empty,
                        gruaId = s.GruaId,
                        gruaPlacas = s.Grua != null ? s.Grua.Placas : string.Empty,
                        gruaMarca = s.Grua != null ? s.Grua.Marca : string.Empty,
                        gruaModelo = s.Grua != null ? s.Grua.Modelo : string.Empty,
                        total = s.Total,
                        tiempoLlegada = s.TiempoLlegada,
                        direccionCongestion = s.DireccionCongestion,
                        direccionKmsTotales = s.DireccionKmTotales,
                        direccionMinsNormal = s.DireccionMinsNormal,
                        direccionMinsTrafico = s.DireccionMinsTrafico,
                        motivoCancelacion = s.MotivoCancelacion,
                        fechaCancelacion = s.FechaCancelacion,
                        usuarioCancelacion = s.UsuarioCancelacion,
                        telefono = s.Telefono,
                        correoElectronico = s.CorreoElectronico,
                        fechaCreacion = s.FechaCreacion,
                    }).OrderByDescending(x=>x.fechaCreacion).ToListAsync();

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
