using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Dto.Request;
using office_automation_system.domain.Enums;

namespace office_automation_system.Api.Controllers.Admin.Request
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestGenericService _RequestGenericService;

        public RequestController(IRequestGenericService RequestGenericService)
        {
            _RequestGenericService = RequestGenericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _RequestGenericService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _RequestGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound("Request not found");
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            

            
        }

       

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
            try
            {
                var IsVerified = await _RequestGenericService.CheckIsVerified(dto);
                if (!IsVerified)
                {
                    return BadRequest("you are not allowed to make request for this process");
                }
                var (success, errors) = await _RequestGenericService.CreateAsync(dto);
                if (!success)
                    return BadRequest(errors);

                return Ok("request created successfully");
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }


        
        
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _RequestGenericService.DeleteAsync(id);
                if (!success)
                    return NotFound("Request not found");
                return Ok("Request deleted successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _RequestGenericService.SoftDeleteAsync(id);
                if (!success)
                    return BadRequest("could not soft delete request");
                return Ok("request soft deleted successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] RequestFilterDto filter)
        {
            try
            {
                var result = await _RequestGenericService.FindAsync(p =>
                p.IsDeleted == false &&
                (filter.ProcessId == null || p.ProcessId == filter.ProcessId) &&
                (filter.UserId == null || p.UserId == filter.UserId) &&
                (filter.Status == null || p.Status == filter.Status)
                );

                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
            
        }






    }
}
