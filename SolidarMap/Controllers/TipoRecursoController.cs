using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os tipos de recurso.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TipoRecursoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoRecursoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os tipos de recurso cadastrados.
        /// </summary>
        /// <response code="200">Retorna todos os tipos de recurso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoRecursoResponseDTO>>> GetTipos()
        {
            return await _context.TiposRecurso
                .Select(r => new TipoRecursoResponseDTO
                {
                    Id = r.Id,
                    Recurso = r.Recurso
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um tipo de recurso pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de recurso.</param>
        /// <response code="200">Tipo de recurso encontrado.</response>
        /// <response code="404">Tipo de recurso não encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoRecursoResponseDTO>> GetTipo(int id)
        {
            var tipo = await _context.TiposRecurso.FindAsync(id);
            if (tipo == null)
                return StatusCode(404, $"Não foi encontrado Tipo de Recurso com ID: {id}");

            return new TipoRecursoResponseDTO
            {
                Id = tipo.Id,
                Recurso = tipo.Recurso
            };
        }

        /// <summary>
        /// Cadastra um novo tipo de recurso.
        /// </summary>
        /// <param name="dto">Dados do tipo de recurso a ser criado.</param>
        /// <response code="201">Tipo de recurso criado.</response>
        /// <response code="500">Erro ao salvar no banco.</response>
        [HttpPost]
        public async Task<ActionResult<TipoRecursoResponseDTO>> PostTipo(TipoRecursoRequestDTO dto)
        {
            var tipo = new TipoRecurso
            {
                Recurso = dto.Recurso
            };

            try
            {
                _context.TiposRecurso.Add(tipo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetTipo), new { id = tipo.Id }, new TipoRecursoResponseDTO
            {
                Id = tipo.Id,
                Recurso = tipo.Recurso
            });
        }

        /// <summary>
        /// Atualiza um tipo de recurso existente.
        /// </summary>
        /// <param name="id">ID do tipo de recurso a ser atualizado.</param>
        /// <param name="dto">Novos dados do tipo de recurso.</param>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="404">Tipo de recurso não encontrado.</response>
        /// <response code="500">Erro ao atualizar no banco.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipo(int id, TipoRecursoRequestDTO dto)
        {
            var tipo = await _context.TiposRecurso.FindAsync(id);
            if (tipo == null)
                return StatusCode(404, $"Não foi encontrado Tipo de Recurso com ID: {id}");

            tipo.Recurso = dto.Recurso;

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
        /// Remove um tipo de recurso pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de recurso a ser removido.</param>
        /// <response code="204">Tipo de recurso removido com sucesso.</response>
        /// <response code="404">Tipo de recurso não encontrado.</response>
        /// <response code="500">Erro ao remover no banco.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            var tipo = await _context.TiposRecurso.FindAsync(id);
            if (tipo == null)
                return StatusCode(404, $"Não foi encontrado Tipo de Recurso com ID: {id}");

            try
            {
                _context.TiposRecurso.Remove(tipo);
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
