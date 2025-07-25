using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using office_automation_system.application.Contracts.Services.Notification;
using office_automation_system.application.Dto.Notification;


namespace office_automation_system.Api.Controllers.Admin.Notification
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]

    public class NotificationController : ControllerBase
    {
        private readonly INotificationGenericService _NotificationGenericService;

        public NotificationController(INotificationGenericService NotificationGenericService)
        {
            _NotificationGenericService = NotificationGenericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var notifications = await _NotificationGenericService.GetAllAsync();
                return Ok(notifications);
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var notification = await _NotificationGenericService.GetByIdAsync(id);
                if (notification == null)
                    return NotFound();

                return Ok(notification);
            }
            catch (Exception ex) { 
            
                return StatusCode(500, ex.Message);
            }
            
        }



       
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] NotificationFilterDto dto)
        {
            try
            {
                var result = await _NotificationGenericService.FindAsync(p => p.IsDeleted == false &&
                (dto.Title == null || p.Title.Contains(dto.Title)) &&
                (dto.UserId == null || p.UserId == dto.UserId));
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
        {
            try
            {
                var result = await _NotificationGenericService.CreateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Errors);

                return Ok("Notification created successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditNotificationDto dto)
        {
            try
            {
                var result = await _NotificationGenericService.EditAsync(id, dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Errors);

                return Ok("Notification updated successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        // DELETE: api/admin/AdministrativeProcesses/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var success = await _AdministrativeProcessGenericProcessService.DeleteAsync(id);
        //    if (!success)
        //        return NotFound();

        //    return Ok("Administrative Process deleted successfully.");
        //}


       
        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _NotificationGenericService.SoftDeleteAsync(id);
                if (!success)
                    return NotFound();

                return Ok("Notification soft-deleted successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
