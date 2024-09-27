using System;
using System.Collections.Generic;

namespace CRUDAPI.models;

/// <summary>
/// Criação da tabela &quot;Empregado&quot;
/// </summary>
public partial class Empregado
{
    public int EmprId { get; set; }

    public string Nome { get; set; } = null!;

    public string? Email { get; set; }

    public string? Cargo { get; set; }

    public string DeptoId { get; set; } = null!;

    public virtual ICollection<Departamento> Deptos { get; set; } = new List<Departamento>();
}
