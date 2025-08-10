using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Wrapper;
using office_automation_system.domain.Enums;

namespace office_automation_system.Api.Controllers.Admin.Request
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestGenericService _RequestGenericService;

        public RequestController(IRequestGenericService RequestGenericService)
        {
            _RequestGenericService = RequestGenericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _RequestGenericService.GetAllAsync();
                return Ok(new ApiResponse<List<GetRequestDto>>(
                    true,"List of all requests",result,null    
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
                var result = await _RequestGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<object>(
                        false,"Request Not Found",null,null    
                    ));
                return Ok(new ApiResponse<GetRequestDto>(
                    true, "Request Received Successfully",result,null   
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
                return StatusCode(500,new ApiResponse<object>(
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
                var success = await _RequestGenericService.SoftDeleteAsync(id);
                if (!success)
                    return BadRequest(new ApiResponse<object>(
                        false,"Could not soft delete this request",null,null    
                    ));
                return Ok(new ApiResponse<object>(
                    true,"Request Soft deleted Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false, ex.Message,null,null    
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

                return Ok(new ApiResponse<List<GetRequestDto>>(
                    true,"List of searched requests",result,null    
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
