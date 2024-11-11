﻿using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Pagos;
using Gruas.API.Models.DTO.Reportes;
using Gruas.API.Models.Enums;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly GruasContext context;
        public ReportesRepository(GruasContext context) => this.context = context;
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
    }
}