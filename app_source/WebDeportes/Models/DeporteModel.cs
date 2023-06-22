using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDeportes.Models
{
    /// <summary>
    /// Clase que representa un deporte: su nombre, país de origen, característica principal, cantidad de jugadores por equipo y si es un juego olímpico o no.
    /// </summary>
    public class DeporteModel
    {
        /// <summary>
        /// Identificador del deporte. Funciona para actualizar los datos en la base de datos más fácilmente.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Representa el nombre del deporte.
        /// </summary>
        [Required(ErrorMessage = "Es necesario asignar un nombre")]
        [DisplayName("Nombre")]
        public string? Nombre { get; set; }

        /// <summary>
        /// Representa el país de donde surgió el deporte.
        /// </summary>
        [Required(ErrorMessage = "Es necesario asignar un país")]
        [DisplayName("País de origen")]
        public string? PaisOrigen { get; set; }

        /// <summary>
        /// Representa una característica del deporte.
        /// </summary>
        [Required(ErrorMessage = "Es necesario asignar una característica")]
        [DisplayName("Característica principal")]
        public string? CaracteristicaPrincipal { get; set; }

        /// <summary>
        /// Representa la cantidad de jugadores que puede tener cada equipo.
        /// </summary>
        [Required(ErrorMessage = "Es necesario asignar un valor")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Ingrese una cantidad válida")]
        [DisplayName("Jugadores por equipo")]
        public int JugadoresPorEquipo { get; set; }

        /// <summary>
        /// Indica si es un deporte olímpico (participa en las olimpiadas mundiales) o no.
        /// </summary>
        [Required]
        [DisplayName("Es un deporte olímpico")]
        public bool EsOlimpico { get; set; }
    }
}
