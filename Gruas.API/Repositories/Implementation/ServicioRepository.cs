using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Gruas.API.Data;
using Gruas.API.Models;
using Gruas.API.Models.Domain;
using Gruas.API.Models.DTO.Servicio;
using Gruas.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Repositories.Implementation
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly GruasContext context;
        public ServicioRepository(GruasContext context) => this.context = context;
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
                Cuentum? cuenta = await this.context.Cuenta.Where(x => x.Telefono == model.telefono).FirstOrDefaultAsync();
                if(cuenta == null) { cuenta = await this.context.Cuenta.Where(x => x.CorreoElectronico == model.correoElectronico).FirstOrDefaultAsync(); }

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
    }
}
