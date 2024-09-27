using System;
using System.Collections.Generic;

namespace CRUDAPI.models;

/// <summary>
/// Criação da tabela &quot;Departamento&quot;
/// </summary>
public partial class Departamento
{
    public int DeptoId { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Empregado> Emprs { get; set; } = new List<Empregado>();
}
