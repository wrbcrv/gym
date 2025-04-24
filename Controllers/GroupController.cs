using Api.Dtos;
using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/groups")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupSummaryResDto>>> GetAll()
        {
            try
            {
                var userIdClaim = UserUtils.GetCurrentUserId(HttpContext);
                var groups = await _groupService.GetAllAsync(userIdClaim);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupResDto>> GetById(int id)
        {
            try
            {
                var userIdClaim = UserUtils.GetCurrentUserId(HttpContext);
                var group = await _groupService.GetByIdAsync(id, userIdClaim);
                if (group == null)
                    return NotFound();

                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<GroupResDto>> Create(GroupReqDto dto)
        {
            try
            {
                var userIdClaim = UserUtils.GetCurrentUserId(HttpContext);
                var created = await _groupService.CreateAsync(dto, userIdClaim);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar grupo: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GroupReqDto dto)
        {
            try
            {
                var success = await _groupService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar grupo: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _groupService.DeleteAsync(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir grupo: {ex.Message}");
            }
        }

        [HttpPost("{groupId}/members/{userId}")]
        public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
        {
            try
            {
                var success = await _groupService.AddUserToGroupAsync(groupId, userId);
                if (!success)
                    return BadRequest("Grupo ou usuário inválido, ou o usuário já está no grupo.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar usuário ao grupo: {ex.Message}");
            }
        }

        [HttpDelete("{groupId}/members/{userId}")]
        public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            try
            {
                var success = await _groupService.RemoveUserFromGroupAsync(groupId, userId);
                if (!success)
                    return NotFound("Usuário ou grupo não encontrado, ou o usuário não está no grupo.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover usuário do grupo: {ex.Message}");
            }
        }
    }
}
