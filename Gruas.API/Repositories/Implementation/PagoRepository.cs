using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Pagos;
using Gruas.API.Models.DTO.Servicio;
using Gruas.API.Models.Enums;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class PagoRepository : IPagoRepository
    {
        private readonly GruasContext context;
        public PagoRepository(GruasContext context) => this.context = context;

        public async Task<ResponseModel> GetPagosMensuales(int anio, int mes, Guid proveedorId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var proveedor = await this.context.Cuenta.FindAsync(proveedorId);

                var result = await context.Pagos
                    .Include(x => x.EstatusPago)
                    .Where(x => x.FechaCreacion.Month == mes && x.FechaCreacion.Year == anio && x.ProveedorId == proveedor.ProveedorId)
                    .Select(s => new GetPagosMensuales_Response()
                    {
                        id = s.Id,
                        folio = s.Folio,
                        monto = s.Monto,
                        concepto = s.Concepto,
                        estatusPagoId = s.EstatusPagoId,
                        estatusPago = s.EstatusPago.Descripcion,
                        referencia = s.Referencia,
                        fechaPago = s.FechaPago,
                        fechaCreacion = s.FechaCreacion
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

        public async Task<ResponseModel> GetServiciosPorPagar(Guid proveedorId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var proveedor = await this.context.Proveedors.FindAsync(proveedorId);
                decimal comision = proveedor.Comision / 100;

                var result = await context.Servicios
                    .Include(x => x.EstatusServicio)
                    .Include(x=>x.Grua).ThenInclude(x=>x.TipoGrua)
                    .Include(x => x.Municipio).ThenInclude(m => m.Estado)
                    .Where(x=>x.EstatusServicioId == (int)EstatusServicio_Enum.Terminado && x.ProveedorId == proveedorId)
                    .Select(s => new GetServiciosPorPagar_Response()
                {
                    id = s.Id,
                    folio = s.Folio,
                    cliente = $"{s.Cliente.Nombre} {s.Cliente.Apellidos}",
                    telefono = s.Telefono ?? "",
                    fecha = s.Fecha,
                    grua = s.Grua != null ? s.Grua.Placas : string.Empty,
                    tipo = s.Grua != null ? s.Grua.TipoGrua.Descripcion : string.Empty,
                        estatus = s.EstatusServicio.Descripcion,
                        subTotal = s.Total ?? 0,
                        comision = (s.Total ?? 0) * (comision),
                        total = (s.Total ?? 0)-((s.Total ?? 0) * (comision))
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

        public async Task<ResponseModel> RegistrarPagoServicios(InsPagoServicios_Request model, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                Guid pagoId = Guid.NewGuid();
                int folio = await this.context.Pagos.CountAsync() + 1;
                decimal monto = model.servicios.Sum(x => x.total);

                //generamos el pago
                Pago pago = new Pago()
                {
                    Id = pagoId,
                    Folio = folio,
                    Monto = monto,
                    ProveedorId = model.proveedorId,
                    Concepto = model.concepto,
                    EstatusPagoId = (int)EstatusPago_Enum.PorPagar,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = usuarioId,
                    PagoDetalles = model.servicios.Select((x, index) => new PagoDetalle()
                    {
                        PagoId = pagoId,
                        ServicioId = x.servicioId,
                        Detalle = index,
                        SubTotal = x.subTotal,
                        Comision = x.comision,
                        Total = x.total,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = usuarioId
                    }).ToList()
                };
               
                //actualizamos los servicios a estatus en proceso de pagos
                foreach(var item in model.servicios)
                {
                    await this.context.Servicios.Where(x => x.Id == item.servicioId).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.EstatusServicioId, t => (int)EstatusServicio_Enum.EnProcesoDePago)
                );
                }

                await this.context.Pagos.AddAsync(pago);
                await this.context.SaveChangesAsync();

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
