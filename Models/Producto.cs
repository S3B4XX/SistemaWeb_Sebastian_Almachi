using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_WEB.Models
{
    // Modelo = estructura de los datos. EF Core crea la tabla "Productos" a partir de esta clase.
    // Los atributos [Required], [StringLength], [Range] validan el formulario (en el navegador y en el servidor).
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede superar los 50 caracteres.")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = string.Empty;

        // decimal(18,2): hasta 18 dígitos con 2 decimales, adecuado para dinero.
        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor que 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (USD)")]
        public decimal Precio { get; set; }

        [Range(0, 100000, ErrorMessage = "El stock debe ser un número entero entre 0 y 100000.")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        // "string?" = campo opcional (puede quedar vacío).
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}
