using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain;

public class CurvePoint
{
    public int Id { get; set; }
    [Required(ErrorMessage = "L'identifiant de la courbe ne doit pas etre vide.")]
    public byte? CurveId { get; set; }
    public DateTime? AsOfDate { get; set; }
    public double? Term { get; set; }
    public double? CurvePointValue { get; set; }
    public DateTime? CreationDate { get; set; }
}