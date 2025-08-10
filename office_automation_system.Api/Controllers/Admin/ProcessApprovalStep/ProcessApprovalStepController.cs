using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Dto.ProcessApprovalStep;
using office_automation_system.application.Wrapper;
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
                return Ok(new ApiResponse<List<GetProcessApprovalStepDto>>(
                    true,"List of all process approval steps",result,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _ProcessApprovalStepGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<object>(
                        false,"ProcessApprovalStep not found",null,null    
                    ));
                return Ok(new ApiResponse<GetProcessApprovalStepDto>(
                    true,"Process Approval Step Received Successfully",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProcessApprovalStepDto dto)
        {
            try
            {
                var (success, errors) = await _ProcessApprovalStepGenericService.CreateAsync(dto);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        
                        false,null,null,errors    
                    ));
                return Ok(new ApiResponse<object>(
                    true,"Process Approval Step Created Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditProcessApprovalStepDto dto)
        {
            try
            {
                var (success, errors) = await _ProcessApprovalStepGenericService.EditAsync(id, dto);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false, null,null,errors    
                    ));
                return Ok(new ApiResponse<object>(
                    true,"Process Approval Step Edited Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    try
        //    {
        //        var success = await _ProcessApprovalStepGenericService.DeleteAsync(id);
        //        if (!success)
        //            return NotFound("Step not found");
        //        return Ok("Step deleted successfully");
        //    }
        //    catch (Exception ex) { 
        //        return StatusCode(500,ex.Message);
        //    }
            
        //}

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _ProcessApprovalStepGenericService.SoftDeleteAsync(id);
                if (!success)
                    return NotFound(new ApiResponse<object>(
                        false,"Process Approval Step Not Found",null,null    
                    ));
                return Ok(new ApiResponse<object>(
                    true,"Process Approval Step soft deleted Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null     
                ));
            }
            
        }

        [HttpGet("filter/{AdministrativeProcessId}")]
        public async Task<IActionResult> Filter(Guid AdministrativeProcessId)
        {
            try
            {
                var result = await _ProcessApprovalStepGenericService.FindAsync(p =>
                 p.IsDeleted == false && p.AdministrativeProcessId == AdministrativeProcessId);
                return Ok(new ApiResponse<List<GetProcessApprovalStepDto>>(
                    true,"List of Searched Process Approval Steps",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }
    }
}
