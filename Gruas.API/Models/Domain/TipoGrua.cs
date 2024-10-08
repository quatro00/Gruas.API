using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class TipoGrua
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Grua> Gruas { get; set; } = new List<Grua>();
}
