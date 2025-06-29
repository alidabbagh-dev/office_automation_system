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

        // GET: api/admin/AdministrativeProcesses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var processes = await _AdministrativeProcessGenericProcessService.GetAllAsync();
            return Ok(processes);
        }

        // GET: api/admin/AdministrativeProcesses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var process = await _AdministrativeProcessGenericProcessService.GetByIdAsync(id);
            if (process == null)
                return NotFound();

            return Ok(process);
        }



        // GET: api/admin/AdministrativeProcesses/search?title="hello"
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            var result = await _AdministrativeProcessGenericProcessService.FindAsync(p => p.Title.Contains(title));
            return Ok(result);
        }

        // POST: api/admin/AdministrativeProcesses
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdministrativeProcessDto dto)
        {
            
            var result = await _AdministrativeProcessGenericProcessService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok("Administrative Process created successfully.");
        }

        // PUT: api/admin/AdministrativeProcesses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditAdministrativeProcessDto dto)
        {
            var result = await _AdministrativeProcessGenericProcessService.EditAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok("Administrative Process updated successfully.");
        }

        // DELETE: api/admin/AdministrativeProcesses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _AdministrativeProcessGenericProcessService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok("Administrative Process deleted successfully.");
        }

        
        //PATCH /api/AdministrativeProcesses/soft-delete/{id}
        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var success = await _AdministrativeProcessGenericProcessService.SoftDeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok("Administrative Process soft-deleted successfully.");
        }
    }

}

