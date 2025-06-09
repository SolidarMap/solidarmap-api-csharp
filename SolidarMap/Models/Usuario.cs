using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_USUARIOS_C")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int Id { get; set; }

        [Required]
        [Column("ID_TIPO_USUARIO")]
        public int TipoUsuarioId { get; set; }

        [Required, MaxLength(100)]
        [Column("NOME")]
        public string Nome { get; set; }

        [Required, MaxLength(100)]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required, MaxLength(256)]
        [Column("SENHA")]
        public string Senha { get; set; }

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; }

        [ForeignKey("TipoUsuarioId")]
        public TipoUsuario TipoUsuario { get; set; }

        public ICollection<Ajuda> Ajudas { get; set; }
        public ICollection<Avaliacao> Avaliacoes { get; set; }
        public ICollection<Mensagem> Mensagens { get; set; }
    }
}
