using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os tipos de usuário.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoUsuarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os tipos de usuário cadastrados.
        /// </summary>
        /// <response code="200">Retorna todos os tipos de usuário.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUsuarioResponseDTO>>> GetTipos()
        {
            return await _context.TiposUsuario
                .Select(t => new TipoUsuarioResponseDTO
                {
                    Id = t.Id,
                    NomeTipo = t.NomeTipo
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um tipo de usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de usuário.</param>
        /// <response code="200">Tipo de usuário encontrado.</response>
        /// <response code="404">Tipo de usuário não encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsuarioResponseDTO>> GetTipo(int id)
        {
            var tipo = await _context.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return StatusCode(404, $"Não foi encontrado Tipo de Usuário com ID: {id}");

            return new TipoUsuarioResponseDTO
            {
                Id = tipo.Id,
                NomeTipo = tipo.NomeTipo
            };
        }

        /// <summary>
        /// Cadastra um novo tipo de usuário.
        /// </summary>
        /// <param name="dto">Dados do tipo de usuário a ser criado.</param>
        /// <response code="201">Tipo de usuário criado.</response>
        /// <response code="500">Erro ao salvar no banco.</response>
        [HttpPost]
        public async Task<ActionResult<TipoUsuarioResponseDTO>> PostTipo(TipoUsuarioRequestDTO dto)
        {
            var tipo = new TipoUsuario
            {
                NomeTipo = dto.NomeTipo
            };

            try
            {
                _context.TiposUsuario.Add(tipo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetTipo), new { id = tipo.Id }, new TipoUsuarioResponseDTO
            {
                Id = tipo.Id,
                NomeTipo = tipo.NomeTipo
            });
        }

        /// <summary>
        /// Atualiza um tipo de usuário existente.
        /// </summary>
        /// <param name="id">ID do tipo de usuário a ser atualizado.</param>
        /// <param name="dto">Novos dados do tipo de usuário.</param>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="404">Tipo de usuário não encontrado.</response>
        /// <response code="500">Erro ao atualizar no banco.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipo(int id, TipoUsuarioRequestDTO dto)
        {
            var tipo = await _context.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return StatusCode(404, $"Não foi encontrado Tipo de Usuário com ID: {id}");

            tipo.NomeTipo = dto.NomeTipo;

            try
            {
                _context.Entry(tipo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um tipo de usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de usuário a ser removido.</param>
        /// <response code="204">Tipo de usuário removido com sucesso.</response>
        /// <response code="404">Tipo de usuário não encontrado.</response>
        /// <response code="500">Erro ao remover no banco.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            var tipo = await _context.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return StatusCode(404, $"Não foi encontrado Tipo de Usuário com ID: {id}");

            try
            {
                _context.TiposUsuario.Remove(tipo);
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
