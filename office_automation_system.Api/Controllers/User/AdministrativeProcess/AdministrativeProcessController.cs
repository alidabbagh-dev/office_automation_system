using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Dto.AdministrativeProcess;

namespace office_automation_system.Api.Controllers.User.AdministrativeProcess
{
    [Authorize]
    [Route("api/user/[controller]")]
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
                return StatusCode(500,ex.Message);
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

    }
}
