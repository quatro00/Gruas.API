using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class SpqGetUsuariosResult
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Telefono { get; set; }

    public string? RazonSocial { get; set; }

    public string? Roles { get; set; }

    public bool? Activo { get; set; }
}
