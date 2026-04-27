using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSecurityExam.Models
{
    [Table("LIBROS")]
    public class Libros
    {
        [Key]
        [Column("IdLibro")]
        public int IdLibro { get; set; }

        [Column("Titulo")]
        public string Titulo { get; set; }

        [Column("Autor")]
        public string Autor { get; set; }

        [Column("Editorial")]
        public string Editorial { get; set; }

        [Column("Portada")]
        public string Portada { get; set; }

        [Column("Precio")]
        public int Precio { get; set; }

        [Column("IdGenero")]
        public int IdGenero { get; set; }
    }
}
