using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Dto.Request;
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
                    return NotFound("this request is not related to you");
                }
                var result = await _RequestGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound("Request not found");
                return Ok(result);
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
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
                    return NotFound("No Request Found");
                }

                return Ok(relatedRequests);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
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
                    return BadRequest("you are not allowed to make request for this process");
                }
                var (success, errors) = await _RequestGenericService.CreateAsync(dto);
                if (!success)
                    return BadRequest(errors);

                return Ok("request created successfully");
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
                var success = await _RequestGenericService.DeleteAsync(id);
                if (!success)
                    return NotFound("Request not found");
                return Ok("Request deleted successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

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
                    return NotFound("Request not found");
                }

                if (Request.UserId != userIdGuid)
                {
                    return BadRequest("You are not allowed to delete this request");
                }
                var success = await _RequestGenericService.SoftDeleteAsync(id);
                if (!success)
                    return BadRequest("could not soft delete request");
                return Ok("request soft deleted successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
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
                    return NotFound("No Request Found");
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

                return Ok(AllowedResults);
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }






    }
}
