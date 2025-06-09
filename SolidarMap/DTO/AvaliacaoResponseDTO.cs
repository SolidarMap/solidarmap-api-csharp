namespace SolidarMap.DTO
{
    public class AvaliacaoResponseDTO
    {
        public int Id { get; set; }
        public int AjudaId { get; set; }
        public int UsuarioId { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string NomeUsuario { get; set; }
    }
}
