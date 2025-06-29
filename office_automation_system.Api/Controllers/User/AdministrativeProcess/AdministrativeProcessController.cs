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

        // GET: api/user/AdministrativeProcesses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var processes = await _AdministrativeProcessGenericProcessService.GetAllAsync();
            return Ok(processes);
        }

        // GET: api/user/AdministrativeProcesses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var process = await _AdministrativeProcessGenericProcessService.GetByIdAsync(id);
            if (process == null)
                return NotFound();

            return Ok(process);
        }



        // GET: api/user/AdministrativeProcesses/search?title="hello"
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            var result = await _AdministrativeProcessGenericProcessService.FindAsync(p => p.Title.Contains(title));
            return Ok(result);
        }

    }
}
