using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Dto.AdministrativeProcess;

namespace office_automation_system.Api.Controllers.Admin.AdministrativeProcess
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    
    public class AdministrativeProcessController : ControllerBase
    {
        private readonly IAdministrativeProcessGenericService _AdministrativeProcessGenericProcessService;

        public AdministrativeProcessController(IAdministrativeProcessGenericService AdministrativeProcessGenericService)
        {
            _AdministrativeProcessGenericProcessService = AdministrativeProcessGenericService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var processes = await _AdministrativeProcessGenericProcessService.GetAllAsync();
                return Ok(processes);
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
                var process = await _AdministrativeProcessGenericProcessService.GetByIdAsync(id);
                if (process == null)
                    return NotFound();

                return Ok(process);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }



        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.FindAsync(p => p.Title.Contains(title));
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdministrativeProcessDto dto)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.CreateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Errors);

                return Ok("Administrative Process created successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditAdministrativeProcessDto dto)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.EditAsync(id, dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Errors);

                return Ok("Administrative Process updated successfully.");
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
                var success = await _AdministrativeProcessGenericProcessService.SoftDeleteAsync(id);
                if (!success)
                    return NotFound();

                return Ok("Administrative Process soft-deleted successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            
            }
            
        }
    }

}

