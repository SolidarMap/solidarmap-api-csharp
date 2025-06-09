using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_AVALIACOES_C")]
    public class Avaliacao
    {
        [Key]
        [Column("ID_AVALIACAO")]
        public int Id { get; set; }

        [Required]
        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("ID_AJUDA")]
        public int AjudaId { get; set; }

        [Required]
        [Column("NOTA")]
        public int Nota { get; set; }

        [MaxLength(150)]
        [Column("COMENTARIO")]
        public string Comentario { get; set; }

        [Column("DATA_AVALIACAO")]
        public DateTime DataAvaliacao { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [ForeignKey("AjudaId")]
        public Ajuda Ajuda { get; set; }
    }
}
