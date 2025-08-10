using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Wrapper;
using System.Security.Claims;

namespace office_automation_system.Api.Controllers.User.Request
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestGenericService _RequestGenericService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestController(IRequestGenericService RequestGenericService, IHttpContextAccessor httpContextAccessor)
        {
            _RequestGenericService = RequestGenericService;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                
                var IsUserAllowedToAccessRequest = await _RequestGenericService.IsUserAllowedToAccessRequest(id);
                
                if (!IsUserAllowedToAccessRequest)
                {
                    return NotFound(new ApiResponse<object>(
                        false,"Request Not Found",null,null
                    ));
                }
                var result = await _RequestGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<object>(
                        false,"Request Not Found",null,null    
                    ));

                return Ok(new ApiResponse<GetRequestDto>(
                    true,"Request Received Successfully",result,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            


        }

        [HttpGet("related-requests")]
        public async Task<IActionResult> GetUserRelatedRequests()
        {
            try
            {
                var relatedRequests = await _RequestGenericService.GetUserRelatedRequestsAsync();
                if (relatedRequests == null || !relatedRequests.Any())
                {
                    return NotFound(new ApiResponse<object>(
                        false,"Request Not Found",null,null    
                    ));
                }

                return Ok(new ApiResponse<List<GetRequestDto>>(
                    true,"List Of Searched Requests",relatedRequests,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }
            
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
            try
            {
                var IsVerified = await _RequestGenericService.CheckIsVerified(dto);
                if (!IsVerified)
                {
                    return BadRequest(new ApiResponse<object>(
                        false,"You are not allowed to make this request",null,null    
                    ));
                }
                var (success, errors) = await _RequestGenericService.CreateAsync(dto);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"Request Created Successfully",null,null    
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
        //        var success = await _RequestGenericService.DeleteAsync(id);
        //        if (!success)
        //            return NotFound("Request not found");
        //        return Ok("Request deleted successfully");
        //    }
        //    catch (Exception ex) {
        //        return StatusCode(500, ex.Message);
        //    }
            
        //}

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var userObj = _httpContextAccessor?.HttpContext?.User;
                var userIdString = userObj?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userIdGuid = Guid.Parse(userIdString);
                var Request = await _RequestGenericService.GetByIdAsync(id);
                if (Request == null)
                {
                    return NotFound(new ApiResponse<object>(
                        false,"Request Not Found",null,null    
                    ));
                }

                if (Request.UserId != userIdGuid)
                {
                    return BadRequest(new ApiResponse<object>(
                        false,"You are not allowed to make this request",null,null    
                    ));
                }
                var success = await _RequestGenericService.SoftDeleteAsync(id);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false,"Could not soft delete this request",null,null    
                        
                    ));
                return Ok(new ApiResponse<object>(
                    true,"Request soft deleted successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] RequestFilterDto filter)
        {
            try
            {
                var result = await _RequestGenericService.FindAsync(p =>
                p.IsDeleted == false &&
                (filter.ProcessId == null || p.ProcessId == filter.ProcessId) &&
                (filter.UserId == null || p.UserId == filter.UserId) &&
                (filter.Status == null || p.Status == filter.Status)
            );

                if (result == null || !result.Any())
                {
                    return NotFound(new ApiResponse<object>(
                        false,"Request not found",null,null    
                    ));
                }

                var AllowedResults = new List<GetRequestDto>();
                foreach (var Request in result)
                {
                    var IsUserAllowedToAccessRequest = await _RequestGenericService.IsUserAllowedToAccessRequest(Request.Id);
                    if (IsUserAllowedToAccessRequest)
                    {
                        AllowedResults.Add(Request);
                    }
                }
                
                return Ok(new ApiResponse<List<GetRequestDto>>(
                    true,"List of Searched Requests",AllowedResults,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }






    }
}
