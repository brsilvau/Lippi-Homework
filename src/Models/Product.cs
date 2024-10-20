using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lippi_homework.Models
{
    public record Producto
    {
        [Key]
        public int Id { get; init; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor no negativo.")]
        public int Stock { get; init; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        public required string Nombre { get; init; }
    }
}
