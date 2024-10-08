using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Marca
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;
}
