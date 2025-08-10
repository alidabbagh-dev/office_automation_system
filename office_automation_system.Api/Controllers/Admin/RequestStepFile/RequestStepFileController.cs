using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Contracts.Services.RequestStepFile;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.application.Dto.RequestStepFile;
using office_automation_system.application.Wrapper;
using office_automation_system.domain.Enums;
using office_automation_system.Infrastructure.Services.Request;
using System.Security.Claims;

namespace office_automation_system.Api.Controllers.Admin.RequestStepFile
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RequestStepFileController : ControllerBase
    {
        private readonly IRequestStepFileGenericService _RequestStepFileGenericService;


        public RequestStepFileController(IRequestStepFileGenericService RequestStepFileGenericService)
        {
            _RequestStepFileGenericService = RequestStepFileGenericService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _RequestStepFileGenericService.GetAllAsync();
                return Ok(new ApiResponse<List<GetRequestStepFileDto>>(
                    true,"List of all request step files",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _RequestStepFileGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<object>(
                        false,"Request step file not found",null,null    
                    ));
                return Ok(new ApiResponse<GetRequestStepFileDto>(
                    true,"Request step file received successfully",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }

        }


        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] RequestStepFileFilterDto filter)
        {
            try
            {
                var result = await _RequestStepFileGenericService.FindAsync(p =>
                p.IsDeleted == false &&
                (filter.Title == null || p.Title.Contains(filter.Title)) &&
                (filter.FileUrl == null || p.FileUrl == filter.FileUrl) &&
                (filter.RequestStepId == null || p.RequestStepId == filter.RequestStepId)
            );

                return Ok(new ApiResponse<List<GetRequestStepFileDto>>(
                    true,"list of searched request step files",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateRequestStepFileDto dto)
        {

            try
            {
                var result = await _RequestStepFileGenericService.CreateAsync(dto);

                if (!result.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,result.Errors    
                    ));
                }
                else
                {
                    return Ok(new ApiResponse<object>(
                        true,"request step file created successfully",null,null    
                    ));
                }
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }

        }


        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteAsync(Guid id)
        {
            try
            {
                var success = await _RequestStepFileGenericService.SoftDeleteAsync(id);
                if (!success)
                {
                    return BadRequest(new ApiResponse<object>(
                        false,"Could not soft delete request step file",null,null    
                    ));

                }

                return Ok(new ApiResponse<object>(
                    true,"Request step file soft deleted successfully",null,null    
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
