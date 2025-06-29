using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Dto.ProcessApprovalStep;
using office_automation_system.Infrastructure.Services.ProcessApprovalStep;
using System;
using System.Threading.Tasks;

namespace office_automation_system.api.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class ProcessApprovalStepController : ControllerBase
    {
        private readonly IProcessApprovalStepGenericService _ProcessApprovalStepGenericService;

        public ProcessApprovalStepController(IProcessApprovalStepGenericService ProcessApprovalStepGenericService)
        {
            _ProcessApprovalStepGenericService = ProcessApprovalStepGenericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ProcessApprovalStepGenericService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ProcessApprovalStepGenericService.GetByIdAsync(id);
            if (result == null)
                return NotFound("Step not found");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProcessApprovalStepDto dto)
        {
            var (success, errors) = await _ProcessApprovalStepGenericService.CreateAsync(dto);
            if (!success)
                return BadRequest(errors);
            return Ok("Step created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditProcessApprovalStepDto dto)
        {
            var (success, errors) = await _ProcessApprovalStepGenericService.EditAsync(id, dto);
            if (!success)
                return BadRequest(errors);
            return Ok("Step updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _ProcessApprovalStepGenericService.DeleteAsync(id);
            if (!success)
                return NotFound("Step not found");
            return Ok("Step deleted successfully");
        }

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var success = await _ProcessApprovalStepGenericService.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Step not found");
            return Ok("Step soft deleted successfully");
        }

        [HttpGet("filter/{AdministrativeProcessId}")]
        public async Task<IActionResult> Filter(Guid AdministrativeProcessId)
        {
            var result = await _ProcessApprovalStepGenericService.FindAsync(p =>
                 p.IsDeleted == false && p.AdministrativeProcessId == AdministrativeProcessId);
                
           
            

            return Ok(result);
        }
    }
}
