namespace SolidarMap.DTO
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
        public string TipoUsuarioNome { get; set; }
    }
}
