using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Dto.ProcessApprovalStep;
using office_automation_system.Infrastructure.Services.ProcessApprovalStep;
using System;
using System.Threading.Tasks;

namespace office_automation_system.Api.Controllers.Admin.ProcessApprovalStep
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
            try
            {
                var result = await _ProcessApprovalStepGenericService.GetAllAsync();
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
                var result = await _ProcessApprovalStepGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound("Step not found");
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProcessApprovalStepDto dto)
        {
            try
            {
                var (success, errors) = await _ProcessApprovalStepGenericService.CreateAsync(dto);
                if (!success)
                    return BadRequest(errors);
                return Ok("Step created successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditProcessApprovalStepDto dto)
        {
            try
            {
                var (success, errors) = await _ProcessApprovalStepGenericService.EditAsync(id, dto);
                if (!success)
                    return BadRequest(errors);
                return Ok("Step updated successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _ProcessApprovalStepGenericService.DeleteAsync(id);
                if (!success)
                    return NotFound("Step not found");
                return Ok("Step deleted successfully");
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _ProcessApprovalStepGenericService.SoftDeleteAsync(id);
                if (!success)
                    return NotFound("Step not found");
                return Ok("Step soft deleted successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("filter/{AdministrativeProcessId}")]
        public async Task<IActionResult> Filter(Guid AdministrativeProcessId)
        {
            try
            {
                var result = await _ProcessApprovalStepGenericService.FindAsync(p =>
                 p.IsDeleted == false && p.AdministrativeProcessId == AdministrativeProcessId);
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
