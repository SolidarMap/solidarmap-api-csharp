namespace SolidarMap.DTO
{
    public class AjudaResponseDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TipoRecursoId { get; set; }
        public string NomeUsuario { get; set; }
        public string Recurso { get; set; }
        public string Descricao { get; set; }
        public char Status { get; set; }
        public DateTime DataPublicacao { get; set; }
    }
}
