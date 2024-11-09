using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Servicio
{
    public Guid Id { get; set; }

    public int Folio { get; set; }

    public int TipoServicioId { get; set; }

    public DateTime Fecha { get; set; }

    public Guid ClienteId { get; set; }

    public Guid? MunicipioId { get; set; }

    public string TipoVehiculo { get; set; } = null!;

    public string OrigenMunicipio { get; set; } = null!;

    public string OrigenDireccion { get; set; } = null!;

    public string OrigenLat { get; set; } = null!;

    public string OrigenLon { get; set; } = null!;

    public string OrigenReferencia { get; set; } = null!;

    public string DestinoMunicipio { get; set; } = null!;

    public string DestinoDireccion { get; set; } = null!;

    public string DestinoLat { get; set; } = null!;

    public string DestinoLon { get; set; } = null!;

    public string DestinoReferencia { get; set; } = null!;

    public bool VehiculoAccidentado { get; set; }

    public bool FugaCombustible { get; set; }

    public bool LlantasGiran { get; set; }

    public bool PuedeNeutral { get; set; }

    public int PersonasEnVehiculo { get; set; }

    public string LugarUbicuidad { get; set; } = null!;

    public int? CarreteraCarril { get; set; }

    public int? CarreteraKm { get; set; }

    public string? CarreteraDestino { get; set; }

    public string? EstacionamientoTipo { get; set; }

    public string? EstacionamientoPiso { get; set; }

    public string VehiculoMarca { get; set; } = null!;

    public string VehiculoModelo { get; set; } = null!;

    public int VehiculoAnio { get; set; }

    public string VehiculoColor { get; set; } = null!;

    public string VehiculoCuentaPlacas { get; set; } = null!;

    public string VehiculoPlacas { get; set; } = null!;

    public bool VehiculoCuentaModificaciones { get; set; }

    public string VehiculoDescripcionModificaciones { get; set; } = null!;

    public decimal Distancia { get; set; }

    public decimal CuotaKm { get; set; }

    public decimal Banderazo { get; set; }

    public bool Maniobras { get; set; }

    public decimal ManiobrasCosto { get; set; }

    public decimal TotalSugerido { get; set; }

    public int EstatusServicioId { get; set; }

    public Guid? ProveedorId { get; set; }

    public Guid? GruaId { get; set; }

    public decimal? Total { get; set; }

    public decimal? TiempoLlegada { get; set; }

    public decimal? DireccionCongestion { get; set; }

    public decimal? DireccionKmTotales { get; set; }

    public decimal? DireccionMinsNormal { get; set; }

    public decimal? DireccionMinsTrafico { get; set; }

    public string? MotivoCancelacion { get; set; }

    public DateTime? FechaCancelacion { get; set; }

    public Guid? UsuarioCancelacion { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual Cuentum Cliente { get; set; } = null!;

    public virtual ICollection<Cotizacion> Cotizacions { get; set; } = new List<Cotizacion>();

    public virtual EstatusServicio EstatusServicio { get; set; } = null!;

    public virtual Grua? Grua { get; set; }

    public virtual Municipio? Municipio { get; set; }

    public virtual ICollection<Propuestum> Propuesta { get; set; } = new List<Propuestum>();

    public virtual Proveedor? Proveedor { get; set; }

    public virtual TipoServicio TipoServicio { get; set; } = null!;
}
