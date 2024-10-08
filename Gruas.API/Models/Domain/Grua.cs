using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Grua
{
    public Guid Id { get; set; }

    public Guid ProveedorId { get; set; }

    public string Placas { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public int TipoGruaId { get; set; }

    public int Anio { get; set; }

    public bool Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public Guid? UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual ICollection<GruaImagen> GruaImagens { get; set; } = new List<GruaImagen>();

    public virtual ICollection<Propuestum> Propuesta { get; set; } = new List<Propuestum>();

    public virtual Proveedor Proveedor { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual TipoGrua TipoGrua { get; set; } = null!;
}
