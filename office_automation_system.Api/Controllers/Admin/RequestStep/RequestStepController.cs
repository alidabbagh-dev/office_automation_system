using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.domain.Enums;
using office_automation_system.Infrastructure.Services.Request;

namespace office_automation_system.Api.Controllers.Admin.Request
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
            var result = await _RequestStepGenericService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _RequestStepGenericService.GetByIdAsync(id);
            if (result == null)
                return NotFound("Request Step not found");
            return Ok(result);
        }


        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] RequestStepFilterDto filter)
        {

            var result = await _RequestStepGenericService.FindAsync(p =>
                p.IsDeleted == false &&
                (filter.RequestId == null || p.RequestId == filter.RequestId) &&
                (filter.OwnerId == null || p.OwnerId == filter.OwnerId) &&
                (filter.RoleId == null || p.RoleId == filter.RoleId) &&
                (filter.Title == null || p.Title == filter.Title)
            );

            return Ok(result);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditRequestStepDto dto)
        {
            var (success, errors) = await _RequestStepGenericService.EditAsync(id, dto);
            if (!success)
                return BadRequest(errors);
            return Ok("Request step updated successfully");
        }

        [HttpPatch("referback/{RequestStepId}")]
        public async Task<IActionResult> ReferToPreviousStep(Guid RequestStepId)
        {
            var (success,errors) = await _RequestStepGenericService.ReferToPreviousStep(RequestStepId);
            if(!success)
                return BadRequest(errors);

            return Ok("Request Step referred back successfully");

        }


        [HttpPatch("refernext/{RequestStepId}")]
        public async Task<IActionResult> ReferToNextStep(Guid RequestStepId)
        {
            var (success, errors) = await _RequestStepGenericService.ReferToNextStep(RequestStepId);
            if (!success)
                return BadRequest(errors);

            return Ok("Request Step referred to the next step successfully");

        }

        [HttpPatch("confirm-final-step/{RequestStepId}")]
        public async Task<IActionResult> ConfirmFinalStep(Guid RequestStepId)
        {
            var (success, errors) = await _RequestStepGenericService.ConfirmFinalStep(RequestStepId);
            if (!success)
                return BadRequest(errors);

            return Ok("Final Request Step Confirmed successfully");

        }










    }
}
