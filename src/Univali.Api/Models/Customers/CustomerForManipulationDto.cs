using System.ComponentModel.DataAnnotations;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Models;

public abstract class CustomerForManipulationDto {
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}