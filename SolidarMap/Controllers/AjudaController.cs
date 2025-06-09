using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de Ajudas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AjudaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AjudaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Exibe todos as ajudas cadastradas.
        /// </summary>
        /// /// <response code="200">Retorna lista com objetos das ajudas encontradas.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AjudaResponseDTO>>> GetAjudas()
        {
            return await _context.Ajudas
                .Include(a => a.Usuario)
                .Include(a => a.TipoRecurso)
                .Select(a => new AjudaResponseDTO
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    TipoRecursoId = a.RecursoId,
                    NomeUsuario = a.Usuario.Nome,
                    Recurso = a.TipoRecurso.Recurso,
                    Descricao = a.Descricao,
                    Status = a.Status,
                    DataPublicacao = a.DataPublicacao
                })
                .ToListAsync();
        }

        /// <summary>
        /// Exibe o objeto da ajuda cadastrada no id informado.
        /// </summary>
        /// <param name="id">ID da ajuda a ser buscado.</param>
        /// <returns>
        /// Retorna <see cref="NoContentResult"/> (200) se o id da ajuda for encontrada,
        /// ou <see cref="NotFoundResult"/> (404) se a ajuda não for encontrada.
        /// </returns>
        /// <response code="200">Ajuda encontradA.</response>
        /// <response code="404">Ajuda não encontrada no banco de dados.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AjudaResponseDTO>> GetAjuda(int id)
        {
            var ajuda = await _context.Ajudas
                .Include(a => a.Usuario)
                .Include(a => a.TipoRecurso)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ajuda == null) return StatusCode(404, $"Não foi encontrado Ajuda com ID: {id}");

            return new AjudaResponseDTO
            {
                Id = ajuda.Id,
                UsuarioId = ajuda.UsuarioId,
                TipoRecursoId = ajuda.RecursoId,
                NomeUsuario = ajuda.Usuario.Nome,
                Recurso = ajuda.TipoRecurso.Recurso,
                Descricao = ajuda.Descricao,
                Status = ajuda.Status,
                DataPublicacao = ajuda.DataPublicacao
            };
        }

        /// <summary>
        /// Exibe uma lista de ajudas cadastradas no ID do usuário informado.
        /// </summary>
        /// <param name="usuarioId">ID do Usuário para buscar as ajudas.</param>
        /// <returns>
        /// Retorna <see cref="NoContentResult"/> (200) se for encontrado ajudas com ID do usuário,
        /// ou <see cref="NotFoundResult"/> (404) se não forem encontradas ajudas.
        /// </returns>
        /// <response code="200">Ajudas encontradas.</response>
        /// <response code="404">Ajudas não encontradas no banco de dados.</response>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<AjudaResponseDTO>>> GetAjudasPorUsuario(int usuarioId)
        {
            var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
            if (!existeUsuario)
                return StatusCode(404, $"Usuário com ID {usuarioId} não encontrado.");

            var ajudas = await _context.Ajudas
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.Usuario)
                .Include(a => a.TipoRecurso)
                .Select(a => new AjudaResponseDTO
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    TipoRecursoId = a.RecursoId,
                    NomeUsuario = a.Usuario.Nome,
                    Recurso = a.TipoRecurso.Recurso,
                    Descricao = a.Descricao,
                    Status = a.Status,
                    DataPublicacao = a.DataPublicacao
                })
                .ToListAsync();

            return ajudas;
        }

        /// <summary>
        /// Cadastra uma nova ajuda na base de dados.
        /// </summary>
        /// <param name="dto">Objeto com os dados da ajuda a ser cadastrada.</param>
        /// <returns>
        /// Retorna <c>201 Created</c> com os dados da ajuda criada, 
        /// ou <c>400 BadRequest</c> se os dados forem inválidos.
        /// </returns>
        /// <response code="201">Ajuda inserida.</response>
        /// <response code="400">Requisição inválida.</response>
        [HttpPost]
        public async Task<ActionResult<AjudaResponseDTO>> PostAjuda(AjudaRequestDTO dto)
        {
            var usuarioValido = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
            var recursoValido = await _context.TiposRecurso.AnyAsync(r => r.Id == dto.TipoRecursoId);

            if (!usuarioValido || !recursoValido)
            {
                return StatusCode(400, "Usuário ou Tipo de Recurso inválido.");
            }

            var ajuda = new Ajuda
            {
                UsuarioId = dto.UsuarioId,
                RecursoId = dto.TipoRecursoId,
                Descricao = dto.Descricao,
                Status = dto.Status,
                DataPublicacao = DateTime.Now
            };

            try
            {
                _context.Ajudas.Add(ajuda);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetAjuda), new { id = ajuda.Id }, new AjudaResponseDTO
            {
                Id = ajuda.Id,
                UsuarioId = ajuda.UsuarioId,
                TipoRecursoId = ajuda.RecursoId,
                Descricao = ajuda.Descricao,
                Status = ajuda.Status,
                DataPublicacao = ajuda.DataPublicacao,
                NomeUsuario = (await _context.Usuarios.FindAsync(ajuda.UsuarioId))?.Nome,
                Recurso = (await _context.TiposRecurso.FindAsync(ajuda.RecursoId))?.Recurso
            });
        }


        /// <summary>
        /// Atualiza os dados de uma ajuda existente no banco de dados.
        /// </summary>
        /// <param name="id">ID da ajuda a ser atualizada. Deve coincidir com o ID do objeto fornecido.</param>
        /// <param name="dto">Objeto com os novos dados d.</param>
        /// <returns>
        /// Retorna <see cref="NoContentResult"/> (204) se a atualização for bem-sucedida,
        /// <see cref="BadRequestResult"/> (400) se Usuário ou Tipo de Recurso forem inválidos,
        /// ou <see cref="NotFoundResult"/> (404) se a ajuda não for encontrada.
        /// </returns>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="400">Usuário ou Tipo de Recurso são inválidos.</response>
        /// <response code="404">AJuda não encontrada no banco de dados.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAjuda(int id, AjudaRequestDTO dto)
        {
            var ajuda = await _context.Ajudas.FindAsync(id);
            if (ajuda == null) return StatusCode(404, $"Não foi encontrado Ajuda com ID: {id}");

            var usuarioValido = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
            var recursoValido = await _context.TiposRecurso.AnyAsync(r => r.Id == dto.TipoRecursoId);
            if (!usuarioValido || !recursoValido)
            {
                return BadRequest("Usuário ou Tipo de Recurso inválido.");
            }

            ajuda.UsuarioId = dto.UsuarioId;
            ajuda.RecursoId = dto.TipoRecursoId;
            ajuda.Descricao = dto.Descricao;
            ajuda.Status = dto.Status;

            try
            {
                _context.Entry(ajuda).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma ajuda existente pelo seu id.
        /// </summary>
        /// <param name="id">ID da ajuda a ser removida.</param>
        /// <returns>
        /// Retorna <c>204 NoContent</c> se a exclusão for bem-sucedida, 
        /// ou <c>404 NotFound</c> se a ajuda não for encontrada.
        /// </returns>
        /// <response code="204">Ajuda excluída.</response>
        /// <response code="404">Ajuda não encontrado no banco de dados.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAjuda(int id)
        {
            var ajuda = await _context.Ajudas.FindAsync(id);
            if (ajuda == null) return StatusCode(404, $"Não foi encontrado Ajuda com ID: {id}");

            try
            {
                _context.Ajudas.Remove(ajuda);
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
