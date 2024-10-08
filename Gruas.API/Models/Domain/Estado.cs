using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Estado
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NombreCorto { get; set; } = null!;

    public decimal CuotaKm { get; set; }

    public decimal Banderazo { get; set; }

    public decimal Maniobras { get; set; }

    public decimal CostoMinimo { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();

    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();
}
