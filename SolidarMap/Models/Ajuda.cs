using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_AJUDAS_C")]
    public class Ajuda
    {
        [Key]
        [Column("ID_AJUDA")]
        public int Id { get; set; }

        [Required]
        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("ID_RECURSO")]
        public int RecursoId { get; set; }

        [Required, MaxLength(256)]
        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Required]
        [Column("STATUS")]
        public char Status { get; set; }

        [Column("DATA_PUBLICACAO")]
        public DateTime DataPublicacao { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [ForeignKey("RecursoId")]
        public TipoRecurso TipoRecurso { get; set; }

    }
}
