using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de avaliações.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvaliacaoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Exibe todas as avaliações cadastradas.
        /// </summary>
        /// <response code="200">Retorna a lista de avaliações.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvaliacaoResponseDTO>>> GetAvaliacoes()
        {
            return await _context.Avaliacoes
                .Include(a => a.Ajuda)
                .Include(a => a.Usuario)
                .Select(a => new AvaliacaoResponseDTO
                {
                    Id = a.Id,
                    AjudaId = a.AjudaId,
                    UsuarioId = a.UsuarioId,
                    Nota = a.Nota,
                    Comentario = a.Comentario,
                    DataAvaliacao = a.DataAvaliacao,
                    NomeUsuario = a.Usuario.Nome
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna uma avaliação específica pelo ID.
        /// </summary>
        /// <param name="id">ID da avaliação.</param>
        /// <response code="200">Avaliação encontrada.</response>
        /// <response code="404">Avaliação não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AvaliacaoResponseDTO>> GetAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Ajuda)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacao == null)
                return StatusCode(404, $"Não foi encontrada avaliação com ID: {id}");

            return new AvaliacaoResponseDTO
            {
                Id = avaliacao.Id,
                AjudaId = avaliacao.AjudaId,
                UsuarioId = avaliacao.UsuarioId,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                DataAvaliacao = avaliacao.DataAvaliacao,
                NomeUsuario = avaliacao.Usuario.Nome
            };
        }

        /// <summary>
        /// Retorna todas as avaliações vinculadas a uma ajuda específica.
        /// </summary>
        /// <param name="ajudaId">ID da ajuda.</param>
        /// <response code="200">Lista de avaliações encontrada.</response>
        /// <response code="404">Ajuda não encontrada.</response>
        [HttpGet("ajuda/{ajudaId}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoResponseDTO>>> GetAvaliacoesPorAjuda(int ajudaId)
        {
            var existeAjuda = await _context.Ajudas.AnyAsync(a => a.Id == ajudaId);
            if (!existeAjuda)
                return StatusCode(404, $"Ajuda com ID {ajudaId} não encontrada.");

            var avaliacoes = await _context.Avaliacoes
                .Where(a => a.AjudaId == ajudaId)
                .Include(a => a.Usuario)
                .Select(a => new AvaliacaoResponseDTO
                {
                    Id = a.Id,
                    AjudaId = a.AjudaId,
                    UsuarioId = a.UsuarioId,
                    Nota = a.Nota,
                    Comentario = a.Comentario,
                    DataAvaliacao = a.DataAvaliacao,
                    NomeUsuario = a.Usuario.Nome
                })
                .ToListAsync();

            return avaliacoes;
        }

        /// <summary>
        /// Cadastra uma nova avaliação.
        /// </summary>
        /// <param name="dto">Dados da avaliação.</param>
        /// <response code="201">Avaliação criada.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="500">Erro ao salvar no banco.</response>
        [HttpPost]
        public async Task<ActionResult<AvaliacaoResponseDTO>> PostAvaliacao(AvaliacaoRequestDTO dto)
        {
            var ajudaValida = await _context.Ajudas.AnyAsync(a => a.Id == dto.AjudaId);
            var usuarioValido = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);

            if (!ajudaValida || !usuarioValido)
                return StatusCode(400, "AjudaId ou UsuarioId inválido.");

            var avaliacao = new Avaliacao
            {
                AjudaId = dto.AjudaId,
                UsuarioId = dto.UsuarioId,
                Nota = dto.Nota,
                Comentario = dto.Comentario,
                DataAvaliacao = DateTime.Now
            };

            try
            {
                _context.Avaliacoes.Add(avaliacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetAvaliacao), new { id = avaliacao.Id }, new AvaliacaoResponseDTO
            {
                Id = avaliacao.Id,
                AjudaId = avaliacao.AjudaId,
                UsuarioId = avaliacao.UsuarioId,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                DataAvaliacao = avaliacao.DataAvaliacao,
                NomeUsuario = (await _context.Usuarios.FindAsync(avaliacao.UsuarioId))?.Nome
            });
        }

        /// <summary>
        /// Atualiza uma avaliação existente.
        /// </summary>
        /// <param name="id">ID da avaliação.</param>
        /// <param name="dto">Novos dados.</param>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Avaliação não encontrada.</response>
        /// <response code="500">Erro ao atualizar no banco.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvaliacao(int id, AvaliacaoRequestDTO dto)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
                return StatusCode(404, $"Não foi encontrada avaliação com ID: {id}");

            var ajudaValida = await _context.Ajudas.AnyAsync(a => a.Id == dto.AjudaId);
            var usuarioValido = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!ajudaValida || !usuarioValido)
                return StatusCode(400, "AjudaId ou UsuarioId inválido.");

            avaliacao.AjudaId = dto.AjudaId;
            avaliacao.UsuarioId = dto.UsuarioId;
            avaliacao.Nota = dto.Nota;
            avaliacao.Comentario = dto.Comentario;

            try
            {
                _context.Entry(avaliacao).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma avaliação pelo ID.
        /// </summary>
        /// <param name="id">ID da avaliação.</param>
        /// <response code="204">Avaliação excluída com sucesso.</response>
        /// <response code="404">Avaliação não encontrada.</response>
        /// <response code="500">Erro ao remover do banco.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
                return StatusCode(404, $"Não foi encontrada avaliação com ID: {id}");

            try
            {
                _context.Avaliacoes.Remove(avaliacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao remover no banco de dados: {ex.Message}");
            }

            return NoContent();
        }
    }
}
