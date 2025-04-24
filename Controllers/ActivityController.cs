using Api.Dtos;
using Api.DTOs;
using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/groups/{groupId}/activities")]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }


        [HttpGet("{activityId}")]
        public async Task<ActionResult<ActivityResDto>> GetById(int groupId, int activityId)
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);
                var activity = await _activityService.GetByIdAsync(groupId, activityId, userId);
                return Ok(activity);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityResDto>>> GetAll(
            int groupId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 8)
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);
                var activities = await _activityService.GetAllByGroupIdAsync(groupId, userId, pageNumber, pageSize);
                return Ok(activities);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ActivityResDto>> Create(int groupId, [FromBody] ActivityReqDto dto)
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);
                var created = await _activityService.CreateAsync(groupId, userId, dto);
                return CreatedAtAction(nameof(GetById), new { groupId, activityId = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao criar atividade: {ex.Message}" });
            }
        }

        [HttpPut("{activityId}")]
        public async Task<IActionResult> Update(int groupId, int activityId, [FromBody] ActivityReqDto dto)
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);
                var success = await _activityService.UpdateAsync(activityId, groupId, userId, dto);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao atualizar atividade: {ex.Message}" });
            }
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(int groupId, int activityId)
        {
            try
            {
                var success = await _activityService.DeleteAsync(activityId, groupId);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao excluir atividade: {ex.Message}" });
            }
        }

        [HttpPost("{activityId}/comments")]
        public async Task<ActionResult<CommentResDto>> AddComment(int groupId, int activityId, [FromBody] CommentReqDto dto)
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);
                var comment = await _activityService.CreateCommentAsync(groupId, activityId, userId, dto);
                return Ok(comment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("ranking")]
        public async Task<ActionResult<IEnumerable<ActivityRankingDto>>> GetRanking(int groupId)
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);
                var ranking = await _activityService.GetRankingByGroupIdAsync(groupId, userId);
                return Ok(ranking);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
