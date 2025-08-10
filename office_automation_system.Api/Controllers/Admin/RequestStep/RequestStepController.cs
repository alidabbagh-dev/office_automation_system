using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.application.Wrapper;
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
                return Ok(new ApiResponse<List<GetRequestStepDto>>(
                        true,"list of all request steps",result,null
                    )
                );
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
                var result = await _RequestStepGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<object>(
                        false,"request step not found",null,null    
                    ));
                return Ok(new ApiResponse<GetRequestStepDto>(
                    true,"request step received successfully",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
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

                return Ok(new ApiResponse<List<GetRequestStepDto>>(
                    true,"list of searched request steps",result,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                    
                ));
            }
            
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditRequestStepDto dto)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.EditAsync(id, dto);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,errors       
                    ));
                return Ok(new ApiResponse<object>(
                    true,"request step updated successfully",null,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpPatch("referback/{RequestStepId}")]
        public async Task<IActionResult> ReferToPreviousStep(Guid RequestStepId)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.ReferToPreviousStep(RequestStepId);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"request step referred back successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }
            

        }


        [HttpPatch("refernext/{RequestStepId}")]
        public async Task<IActionResult> ReferToNextStep(Guid RequestStepId)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.ReferToNextStep(RequestStepId);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false, null,null,errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"request step referred to next step successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            

        }

        [HttpPatch("confirm-final-step/{RequestStepId}")]
        public async Task<IActionResult> ConfirmFinalStep(Guid RequestStepId)
        {
            try
            {
                var (success, errors) = await _RequestStepGenericService.ConfirmFinalStep(RequestStepId);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"request step confirmed as final step",null,null    
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
