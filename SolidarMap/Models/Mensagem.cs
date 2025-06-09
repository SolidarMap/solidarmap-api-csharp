using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_MENSAGENS_C")]
    public class Mensagem
    {
        [Key]
        [Column("ID_MENSAGEM")]
        public int Id { get; set; }

        [Required]
        [Column("ID_AJUDA")]
        public int AjudaId { get; set; }

        [Required]
        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        [Required, MaxLength(500)]
        [Column("MENSAGEM")]
        public string Conteudo { get; set; }

        [Column("DATA_ENVIO")]
        public DateTime DataEnvio { get; set; }

        [ForeignKey("AjudaId")]
        public Ajuda Ajuda { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }
}
