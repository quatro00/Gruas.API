using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Servicio;
using Gruas.API.Models.Enums;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System;
using System.Net;
using System.Net.Mail;
using Azure;
using DocumentFormat.OpenXml.Office2013.Excel;

namespace Gruas.API.Repositories.Implementation
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly GruasContext context;
        public ServicioRepository(GruasContext context) => this.context = context;

        public async Task<ResponseModel> AsignarCotizacion(AsignarCotizaciones_Request model, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var cotizacion = await this.context.Cotizacions.Include(x=>x.Grua).Where(x=>x.Id == model.cotizacionId).FirstOrDefaultAsync();
                cotizacion.Seleccionada = true;

                var servicio = await this.context.Servicios.FindAsync(cotizacion.ServicioId);
                servicio.ProveedorId = cotizacion.Grua.ProveedorId;
                servicio.GruaId = cotizacion.GruaId;
                servicio.Total = cotizacion.Cotizacion1;
                servicio.TiempoLlegada = cotizacion.TiempoArrivo;
                servicio.EstatusServicioId = (int)EstatusServicio_Enum.Aceptado;

                await this.context.SaveChangesAsync();
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

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

        public async Task<ResponseModel> EnviarCotizacionProveedor(EnviarCotizacionProveedor_Request model, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que no haya una cotizacion de ese proveedor para ese servicio
                var cuenta = await this.context.Cuenta.Where(x => x.Id == usuarioId).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null)
                {
                    proveedor = cuenta.ProveedorId;
                }


                if (await this.context.Cotizacions.Where(x=>x.ServicioId == model.servicioId && x.Grua.ProveedorId == proveedor).AnyAsync()) 
                {
                    rm.SetResponse(false, "El servicio ya se encuentra cotizado, favor de modificar la cotización.");
                    return rm;
                }
                Cotizacion cotizacion = new Cotizacion()
                {
                    Id = Guid.NewGuid(),
                    Seleccionada = false,         
                    ServicioId = model.servicioId,
                    GruaId = model.gruaId,
                    TiempoArrivo= model.tiempo,
                    Cotizacion1 = model.costo,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = usuarioId

                };

                await this.context.Cotizacions.AddAsync(cotizacion);
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

        public async Task<ResponseModel> GetCotizaciones(Guid id)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var result = await context.Cotizacions
                    
                    .Include(x => x.Grua)
                    .ThenInclude(m => m.Proveedor)
                    .Include(x => x.Grua).ThenInclude(m => m.TipoGrua)
                    .Where(x => x.ServicioId == id)
                    .Select(s => new GetCotizaciones_Response()
                {
                    id = s.Id,
                    tiempoCotizado = s.TiempoArrivo,
                    costoCotizado = s.Cotizacion1,
                    seleccionada = s.Seleccionada ? 1 : 0,
                    razonSocial = s.Grua.Proveedor.RazonSocial,
                    proveedorTelefono1 = s.Grua.Proveedor.Telefono1,
                    proveedorTelefono2 = s.Grua.Proveedor.Telefono2,
                    proveedorCorreo = "",
                    gruaPlacas = s.Grua.Placas,
                    gruaMarca = s.Grua.Marca,
                    gruaAnio = s.Grua.Anio,
                    gruaTipo  = s.Grua.TipoGrua.Descripcion
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

        public async Task<ResponseModel> GetServiciosDisponibles(Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                var cuenta = await this.context.Cuenta.Where(x => x.Id == usuarioId).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null)
                {
                    proveedor = cuenta.ProveedorId;
                }


                var result = await context.Servicios
                    .Include(x=>x.Cotizacions)
                    .Include(x=>x.TipoServicio)
                    .Include(x => x.EstatusServicio)
                    .Include(x => x.Municipio)
                    .ThenInclude(m => m.Estado)
                    .Where(x => x.EstatusServicioId == 3)
                    .Select(s => new GetServiciosDisponibles_Request()
                {
                        id = s.Id,
                        folio = s.Folio.ToString(),
                        estatus = s.EstatusServicio.Descripcion,
                        origen = s.OrigenMunicipio,
                        destino = s.DestinoMunicipio,
                        kms = s.Distancia,
                        maniobras = s.Maniobras ? "Si" : "No",
                        tipoServicio = s.TipoServicio.Descripcion,
                        tipoVehiculo = s.TipoVehiculo,
                        totalSugerido = s.TotalSugerido,
                        totalCotizado = 0,
                        tiempoCotizado = 0,
                        latOrigen = s.OrigenLat,
                        lonOrigen = s.OrigenLon,
                        fecha = s.Fecha.ToString("dd MMMM yyyy", new CultureInfo("es-ES")),
                        hora = s.Fecha.ToString("h:mm tt", new CultureInfo("es-ES"))
                }).OrderBy(x=>x.kms).ToListAsync();

                foreach(var item in result)
                {
                    var cotizacion = await this.context.Cotizacions.Include(x=>x.Grua).Where(x=>x.ServicioId == item.id && x.Grua.ProveedorId == proveedor).FirstOrDefaultAsync();
                    if(cotizacion != null)
                    {
                        item.cotizacionId = cotizacion.Id;
                        item.estatus = "Cotizado";
                        item.totalCotizado = cotizacion.Cotizacion1;
                        item.tiempoCotizado = cotizacion.TiempoArrivo;
                    }
                    else
                    {
                        item.estatus = "Por cotizar";
                    }
                }

                rm.result = result;
                rm.SetResponse(true);
            }
            catch (Exception)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> GetServiciosProximos(Guid usuarioId)
        {
           
            ResponseModel rm = new ResponseModel();

            try
            {
                var cuenta = await this.context.Cuenta.Where(x => x.Id == usuarioId).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null)
                {
                    proveedor = cuenta.ProveedorId;
                }


                var result = await context.Servicios
                    .Include(x => x.Cotizacions)
                    .Include(x => x.TipoServicio)
                    .Include(x => x.EstatusServicio)
                    .Include(x => x.Municipio)
                    .ThenInclude(m => m.Estado)
                    .Where(x => (x.EstatusServicioId == 5 || x.EstatusServicioId == 6 || x.EstatusServicioId == 7) && x.ProveedorId == proveedor && x.Fecha >= DateTime.Now.Date)
                    .Select(s => new GetServiciosProximos_Response()
                    {
                        id = s.Id,
                        folio = s.Folio.ToString(),
                        tipoServicio = s.TipoServicio.Descripcion,
                        fecha = s.Fecha,
                        fechaDia = s.Fecha.Day,
                        fechaMes = s.Fecha.ToString("MMM", new CultureInfo("es-ES")),
                        fechaHora = s.Fecha.ToString("h:mm tt", new CultureInfo("es-ES")),
                        cliente = s.Cliente.Nombre +" " + s.Cliente.Apellidos,
                        clienteTelefono = s.Cliente.Telefono,
                        origen = s.OrigenDireccion,
                        origenMunicipio = s.OrigenMunicipio,
                        destino = s.DestinoDireccion,
                        destinoMunicipio = s.DestinoMunicipio,
                        origenLat = s.OrigenLat,
                        origenLon = s.OrigenLon,
                        destinoLat = s.DestinoLat,
                        destinoLon = s.DestinoLon,
                        origenReferencia = s.OrigenReferencia,
                        destinoReferencia = s.DestinoReferencia,
                        vehiculoAccidentado = s.VehiculoAccidentado,
                        fugaCombustible = s.FugaCombustible,
                        llantasGiran = s.LlantasGiran,
                        puedeNeutral = s.PuedeNeutral,
                        personasEnVehiculo = s.PersonasEnVehiculo,
                        lugarUbicuidad = s.LugarUbicuidad,
                        carreteraCarril = s.CarreteraCarril ?? 0,
                        carreteraKm = s.CarreteraKm ?? 0,
                        carreteraDestino = s.CarreteraDestino ?? "",
                        estacionamientoTipo = s.EstacionamientoTipo ?? "",
                        estacionamientoPiso = s.EstacionamientoPiso ?? "",
                        vehiculoMarca = s.VehiculoMarca ?? "",
                        vehiculoAnio = s.VehiculoAnio,
                        vehiculoModelo = s.VehiculoModelo,
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
                        estatusServicio = s.EstatusServicio.Descripcion,
                        razonSocial = s.Proveedor.RazonSocial,
                        proveedorTelefono = "",
                        grua = s.Grua.Marca + " " + s.Grua.Modelo,
                        gruaPlacas = s.Grua.Placas,
                        total = s.Total ?? 0,
                        tiempoLlegada = s.TiempoLlegada ?? 0,
                        direccionCongestion = s.DireccionCongestion ?? 0,
                        direccionKmTotales = s.DireccionKmTotales ?? 0,
                        direccionMinsNormal = s.DireccionMinsNormal ?? 0,
                        direccionMinsTrafico = s.DireccionMinsTrafico ?? 0
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

        public async Task<ResponseModel> EnviarCorreoServicio(Guid id)
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                //buscamos el servicio
                var servicio = await this.context.Servicios.Where(x => x.Id == id).FirstOrDefaultAsync();



                Configuracion? configuracion_urlservicio = await this.context.Configuracions.FindAsync(1);

                Configuracion? configuracion_correosmtp = await this.context.Configuracions.FindAsync(2);
                Configuracion? configuracion_passwordsmtp = await this.context.Configuracions.FindAsync(3);
                Configuracion? configuracion_servidorsmtp = await this.context.Configuracions.FindAsync(4);
                Configuracion? configuracion_puertosmtp = await this.context.Configuracions.FindAsync(5);
                Configuracion? configuracion_remitentesmtp = await this.context.Configuracions.FindAsync(6);

                // Configuración del cliente SMTP
                SmtpClient smtpClient = new SmtpClient(configuracion_servidorsmtp.ValorString)
                {
                    Port = 587, // Puerto SMTP para Gmail
                                //Credentials = new NetworkCredential(configuracion_correosmtp.ValorString, "ltmd cgnj ortf ytaw"), // Tu correo y contraseña
                    Credentials = new NetworkCredential(configuracion_correosmtp.ValorString, configuracion_passwordsmtp.ValorString), // Tu correo y contraseña
                    EnableSsl = true, // Usar conexión segura
                };

                // Configuración del mensaje
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(configuracion_correosmtp.ValorString, configuracion_remitentesmtp.ValorString), // Correo remitente
                    Subject = $"Solicitud de servicio de grúa No. {servicio.Folio.ToString()}",
                    Body =
                    $"🚨 ¡Nueva solicitud de servicio de grúa! 🚨 \n" +
                    $"📍 Origen: {servicio.OrigenDireccion} \n" +
                    $"📍 Destino: {servicio.DestinoDireccion} \n" +
                    $"🛣️ Distancia total: {servicio.Distancia.ToString("N")} Km. \n" +
                    $"💰 Precio sugerido: {servicio.TotalSugerido.ToString("C")} \n" +
                    $"🚗 Tipo de vehículo: {servicio.TipoVehiculo} \n" +
                    $"🔗 Detalle completo y opciones: {configuracion_urlservicio.ValorString + "/" + id.ToString()} \n" +
                    "📲 ¡Responde pronto para asegurar este servicio! 😊 \n",




                    IsBodyHtml = false, // Cambiar a true si quieres enviar HTML
                };

                // Destinatarios
                mailMessage.To.Add("josecarlosgarciadiaz@gmail.com");

                // Enviar correo
                smtpClient.Send(mailMessage);
            }
            catch(Exception ioe)
            {
                rm.SetResponse(false, ioe.Message);
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
                await this.EnviarCorreoServicio(servicio.Id);

                rm.SetResponse(true);
            }
            catch (Exception e)
            {
                rm.SetResponse(false);
            }

            return rm;
        }

        public async Task<ResponseModel> ModificarCotizacionProveedor(ModificarCotizacionProveedor_Request model, Guid usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que no haya una cotizacion de ese proveedor para ese servicio
                var cuenta = await this.context.Cuenta.Where(x => x.Id == usuarioId).FirstOrDefaultAsync();
                Guid? proveedor = null;

                if (cuenta != null)
                {
                    proveedor = cuenta.ProveedorId;
                }


                var cotizacion = await this.context.Cotizacions.Where(x => x.Id == model.cotizacionId).FirstOrDefaultAsync();
                
                
                cotizacion.GruaId = model.gruaId;
                cotizacion.TiempoArrivo = model.tiempo;
                cotizacion.Cotizacion1 = model.costo;
                cotizacion.FechaModificacion = DateTime.Now;
                cotizacion.UsuarioModificacion = usuarioId;
               
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
