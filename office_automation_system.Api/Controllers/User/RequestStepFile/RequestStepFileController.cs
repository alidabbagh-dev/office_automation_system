using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.RequestStepFile;
using office_automation_system.application.Dto.RequestStepFile;
using office_automation_system.application.Wrapper;

namespace office_automation_system.Api.Controllers.User.RequestStepFile
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestStepFileController : ControllerBase
    {
        private readonly IRequestStepFileGenericService _RequestStepFileGenericService;


        public RequestStepFileController(IRequestStepFileGenericService RequestStepFileGenericService)
        {
            _RequestStepFileGenericService = RequestStepFileGenericService;

        }

      

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _RequestStepFileGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<object>(
                        false,"request step file not found",null,null    
                    ));
                return Ok(new ApiResponse<GetRequestStepFileDto>(
                    true,"request step file received successfully",result,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
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
                    true,"List of searched request step files",result,null    
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
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
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
                        false,"could not delete request step file",null,null    
                    ));

                }

                return Ok(new ApiResponse<object>(
                    true,"request step file soft deleted successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }
            
        }

    }
}
