using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.domain.Enums;
using office_automation_system.Infrastructure.Services.Request;

namespace office_automation_system.Api.Controllers.Admin.RequestStep
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RequestStepController : ControllerBase
    {
        private readonly IRequestStepGenericService _RequestStepGenericService;

        public RequestStepController(IRequestStepGenericService RequestStepGenericService)
        {
            _RequestStepGenericService = RequestStepGenericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _RequestStepGenericService.GetAllAsync();
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
                var result = await _RequestStepGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound("Request Step not found");
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] RequestStepFilterDto filter)
        {
            try
            {
                var result = await _RequestStepGenericService.FindAsync(p =>
                p.IsDeleted == false &&
                (filter.RequestId == null || p.RequestId == filter.RequestId) &&
                (filter.OwnerId == null || p.OwnerId == filter.OwnerId) &&
                (filter.RoleId == null || p.RoleId == filter.RoleId) &&
                (filter.Title == null || p.Title.Contains(filter.Title))
            );

                return Ok(result);
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditRequestStepDto dto)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.EditAsync(id, dto);
                if (!success)
                    return BadRequest(errors);
                return Ok("Request step updated successfully");
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpPatch("referback/{RequestStepId}")]
        public async Task<IActionResult> ReferToPreviousStep(Guid RequestStepId)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.ReferToPreviousStep(RequestStepId);
                if (!success)
                    return BadRequest(errors);

                return Ok("Request Step referred back successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            

        }


        [HttpPatch("refernext/{RequestStepId}")]
        public async Task<IActionResult> ReferToNextStep(Guid RequestStepId)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.ReferToNextStep(RequestStepId);
                if (!success)
                    return BadRequest(errors);

                return Ok("Request Step referred to the next step successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            

        }

        [HttpPatch("confirm-final-step/{RequestStepId}")]
        public async Task<IActionResult> ConfirmFinalStep(Guid RequestStepId)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.ConfirmFinalStep(RequestStepId);
                if (!success)
                    return BadRequest(errors);

                return Ok("Final Request Step Confirmed successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            

        }










    }
}
