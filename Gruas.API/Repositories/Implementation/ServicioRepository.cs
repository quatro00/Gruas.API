using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Servicio;
using Gruas.API.Models.Enums;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly GruasContext context;
        public ServicioRepository(GruasContext context) => this.context = context;

        public async Task<ResponseModel> AsignarGrua(AsignarGrua_Request request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {

                var grua = await this.context.Gruas.FindAsync(request.gruaId);
                var servicio = await this.context.Servicios.FindAsync(request.servicioId);

                if (servicio != null)
                {
                    servicio.ProveedorId = grua.ProveedorId;
                    servicio.GruaId = grua.Id;
                    servicio.Total = request.costo;
                    servicio.TiempoLlegada = request.tiempo;
                    servicio.EstatusServicioId = (int)EstatusServicio_Enum.Aceptado;
                    servicio.FechaModificacion = DateTime.Now;
                    servicio.UsuarioModificacionId = usuarioId;
                }

                await this.context.SaveChangesAsync();
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> CancelarServicio(CancelarServicio_Request request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var servicio = await this.context.Servicios.FindAsync(request.servicioId);

                if (servicio != null)
                {
                    servicio.EstatusServicioId = (int)EstatusServicio_Enum.Cancelado;
                    
                    servicio.FechaCancelacion = DateTime.Now;
                    servicio.UsuarioCancelacion = usuarioId;
                    servicio.MotivoCancelacion = request.motivo;

                    servicio.FechaModificacion = DateTime.Now;
                    servicio.UsuarioModificacionId = usuarioId;
                }

                await this.context.SaveChangesAsync();
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> ColocarEnPropuesta(ColocarEnPropuesta_Request request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var servicio = await this.context.Servicios.FindAsync(request.servicioId);

                if (servicio != null)
                {
                    servicio.EstatusServicioId = (int)EstatusServicio_Enum.EnPropuesta;

                    servicio.FechaModificacion = DateTime.Now;
                    servicio.UsuarioModificacionId = usuarioId;
                }

                await this.context.SaveChangesAsync();
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetServicio(Guid id)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.Servicios.Include(x=>x.EstatusServicio).Include(x => x.Municipio).ThenInclude(m => m.Estado).Select(s => new GetServicio_Response()
                {
                    id = s.Id,
                    tipo = s.TipoVehiculo,
                    placas = s.VehiculoCuentaPlacas,
                    marca = s.VehiculoMarca,
                    modelo = s.VehiculoModelo,
                    anio = s.VehiculoAnio,
                    estatusServicio = s.EstatusServicioId,
                    numPersonas = s.PersonasEnVehiculo,
                    origen = s.OrigenDireccion,
                    destino = s.DestinoDireccion,
                    estado = s.Municipio != null ? s.Municipio.Estado.Nombre : string.Empty,
                    municipio = s.Municipio != null ? s.Municipio.Nombre : string.Empty,
                    kilometros = s.DireccionKmTotales ?? 0,
                    tiempoEstimado = s.DireccionMinsTrafico ?? 0,
                    fugaCombustible = s.FugaCombustible ? "Si" : "No",
                    llantasGiran = s.LlantasGiran ? "Si" : "No",
                    esPosibleNeutral = s.PuedeNeutral ? "Si" : "No",
                    personasEnVehiculo = s.PersonasEnVehiculo,
                    lugar = s.LugarUbicuidad,
                    carril = s.CarreteraCarril ?? 0,
                    kilometro = s.CarreteraKm ?? 0,
                    tipoDeEstacionamiento = s.EstacionamientoTipo ?? string.Empty,
                    pisoEstacionamiento = s.EstacionamientoPiso ?? string.Empty,
                    vehiculoAccidentado = s.VehiculoAccidentado ? "Si" : "No",
                    tarifaInicial = s.Banderazo,
                    costoPorKm = s.CuotaKm,
                    tarifaDinamica = 0,
                    maniobras = s.ManiobrasCosto,
                    totalSugerido = s.TotalSugerido,
                    estatus = s.EstatusServicio.Descripcion,
                    razonSocial = s.Proveedor != null ? s.Proveedor.RazonSocial : string.Empty,
                    telefono_1 = s.Proveedor != null ? s.Proveedor.Telefono1 : string.Empty,
                    telefono_2 = s.Proveedor != null ? s.Proveedor.Telefono2 : string.Empty,
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

        public async Task<ResponseModel> GetServicios(int? estatusServicioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.Servicios.Where(x=>x.EstatusServicioId == estatusServicioId || estatusServicioId == null).Include(x=>x.Proveedor).Include(x=>x.Grua).Include(x=>x.Municipio).ThenInclude(m => m.Estado).Select(s => new GetServicios_Response()
                {
                    id = s.Id,
                    folio = s.Folio.ToString(),
                    estatusServicioId = s.EstatusServicioId,
                    cliente = s.Cliente.Nombre + " " + s.Cliente.Apellidos,
                    telefono = s.Cliente.Telefono,
                    estado = s.Municipio != null ? s.Municipio.Estado.Nombre : string.Empty,
                    origen = s.OrigenDireccion,
                    destino = s.DestinoDireccion,
                    costo = s.TotalSugerido.ToString("C"),
                    estatus = s.EstatusServicio.Descripcion,
                    fechaCreacion = s.FechaCreacion,
                    fecha = s.Fecha,
                    proveedor = s.Proveedor != null ? s.Proveedor.RazonSocial : string.Empty,
                    gruaPlaca = s.Grua != null ? s.Grua.Placas : string.Empty,
                    gruaMarca = s.Grua != null ? s.Grua.Marca : string.Empty,
                    gruaModelo = s.Grua != null ? s.Grua.Modelo : string.Empty,
                    gruaTipo = s.Grua != null ? s.Grua.TipoGrua.Descripcion : string.Empty,
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

        public async Task<ResponseModel> InsServicio(RegistraServicio_Request model, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                int carril = 0;
                if((model.carril ??"") != "")
                {
                    carril = int.Parse(model.carril);
                }

                int km = 0;
                if ((model.km ?? "") != "")
                {
                    km = int.Parse(model.km);
                }

                int folio = 1;
                try
                {
                    folio = this.context.Servicios.Max(x => x.Folio) + 1;
                }
                catch(Exception ioe)
                {
                    folio = 1;
                }
                //buscamos la distancia entre el origen y el destino
                string origen = $"{model.LatOrigen},{model.lngOrigen}";
                string destino = $"{model.LatDestino},{model.lngDestino}";

                var distancia = await Gruas.API.Utils.Helpers.GetDrivingDistance(origen, destino);

                //buscamos al cliente con el telefono
                Cuentum? cuenta = await this.context.Cuenta.Where(x => x.CorreoElectronico == model.correoElectronico).FirstOrDefaultAsync();
                //if(cuenta == null) { cuenta = await this.context.Cuenta.Where(x => x.CorreoElectronico == model.correoElectronico).FirstOrDefaultAsync(); }

                //si no existe la cuenta la creamos
                if(cuenta == null)
                {
                    cuenta = new Cuentum()
                    {
                        Id = Guid.NewGuid(),
                        NombreUsuario = model.correoElectronico,
                        Nombre = model.nombre,
                        Apellidos = model.apellidos,
                        CorreoElectronico = model.correoElectronico,
                        Telefono = model.telefono,
                        ProveedorId = null,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreadionId = usuarioId

                    };

                    await this.context.Cuenta.AddAsync(cuenta);
                }

                //obtenemos el municipio
                var municipio = await this.context.Municipios.Where(x => x.Nombre == model.municipioOrigen).FirstOrDefaultAsync();
                var estado = await this.context.Estados.Where(x=>x.NombreCorto == "NL").FirstOrDefaultAsync();
                decimal total = 0;
                //determinamos si usa maniobras
                bool maniobras = false;

                if(model.lugarUbicuidad == "Cochera" || model.lugarUbicuidad == "Estacionamiento" || model.llantasGiran == 0 || model.puedeNeutral == 0 || model.fugaCombustible == 1)
                {
                    maniobras = true;
                    total = estado.Maniobras;
                }


                //calculamos el total sugerido
                total = total + (distancia.kmTotales * estado.CuotaKm) + estado.Banderazo;
                

                Servicio servicio = new Servicio()
                {
                    Id = Guid.NewGuid(),
                    Folio = folio,
                    ClienteId = cuenta.Id,
                    MunicipioId = municipio.Id,
                    TipoVehiculo = model.tipoVehiculo,

                    OrigenMunicipio = model.municipioOrigen,
                    OrigenDireccion = model.origen,
                    OrigenLat = model.LatOrigen.ToString(),
                    OrigenLon = model.lngOrigen.ToString(),
                    OrigenReferencia = model.referenciaOrigen,

                    DestinoMunicipio = model.municipioDestino,
                    DestinoDireccion = model.destino,
                    DestinoLat = model.LatDestino.ToString(),
                    DestinoLon = model.lngDestino.ToString(),
                    DestinoReferencia = model.referenciaDestino,

                    VehiculoAccidentado = model.accidente == 1,
                    FugaCombustible = model.fugaCombustible == 1,
                    LlantasGiran = model.llantasGiran == 1,
                    PuedeNeutral = model.puedeNeutral == 1,
                    PersonasEnVehiculo = model.cantidadPersonas,
                    LugarUbicuidad = model.lugarUbicuidad,
                    CarreteraCarril = carril,
                    CarreteraKm = km,
                    CarreteraDestino = model.destino,
                    EstacionamientoTipo = model.tipoEstacionamiento,
                    EstacionamientoPiso = model.piso,
                    VehiculoMarca = model.marca,
                    VehiculoModelo = model.modelo,
                    VehiculoAnio = model.anio,
                    VehiculoColor = model.color,
                    VehiculoCuentaPlacas = model.permiso,
                    //placas
                    VehiculoCuentaModificaciones = model.modificaciones == 1,
                    VehiculoDescripcionModificaciones = model.modificacionesEspecificacion,
                    Distancia = distancia.kmTotales,
                    CuotaKm = estado.CuotaKm,
                    Banderazo = estado.Banderazo,
                    Maniobras = maniobras,
                    ManiobrasCosto = estado.Maniobras,
                    TotalSugerido = total,
                    EstatusServicioId = 1,
                    ProveedorId = null,
                    GruaId = null,
                    Total = null,
                    TiempoLlegada = null,
                    DireccionCongestion =(decimal)distancia.congestionPercentage,
                    DireccionKmTotales = distancia.kmTotales,
                    DireccionMinsNormal = distancia.minsNomal,
                    DireccionMinsTrafico = distancia.minsTrafico,
                    Fecha = model.fecha,
                    TipoServicioId = 1, //servicio de grua
                    VehiculoPlacas = model.placaPermiso ?? "",

                    CorreoElectronico = model.correoElectronico,
                    Telefono = model.telefono,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreadionId = usuarioId

                };

                await this.context.Servicios.AddAsync(servicio);
                await this.context.SaveChangesAsync();

                //enviamos notificacion via correo electronico o sms
                
                rm.SetResponse(true);
            }
            catch (Exception e)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> SolicitarCotizaciones(SolicitarCotizacion_Request request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var servicio = await this.context.Servicios.FindAsync(request.servicioId);

                if (servicio != null)
                {
                    servicio.EstatusServicioId = (int)EstatusServicio_Enum.EnEsperaDeCotizaciones;

                    servicio.FechaModificacion = DateTime.Now;
                    servicio.UsuarioModificacionId = usuarioId;
                }

                await this.context.SaveChangesAsync();
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> TerminarServicio(TerminarServicio_Request request, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var servicio = await this.context.Servicios.FindAsync(request.servicioId);

                if (servicio != null)
                {
                    servicio.EstatusServicioId = (int)EstatusServicio_Enum.Terminado;

                    servicio.FechaModificacion = DateTime.Now;
                    servicio.UsuarioModificacionId = usuarioId;
                }

                await this.context.SaveChangesAsync();
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
