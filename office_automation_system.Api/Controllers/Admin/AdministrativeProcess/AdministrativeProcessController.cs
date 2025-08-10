using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Wrapper;

namespace office_automation_system.Api.Controllers.Admin.AdministrativeProcess
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    
    public class AdministrativeProcessController : ControllerBase
    {
        private readonly IAdministrativeProcessGenericService _AdministrativeProcessGenericProcessService;

        public AdministrativeProcessController(IAdministrativeProcessGenericService AdministrativeProcessGenericService)
        {
            _AdministrativeProcessGenericProcessService = AdministrativeProcessGenericService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var processes = await _AdministrativeProcessGenericProcessService.GetAllAsync();
                var apiResponse = new ApiResponse<List<GetAdministrativeProcessDto>>(
                    true,"list of all processes", processes, null
                );
                return Ok(apiResponse);
            }
            catch (Exception ex) {
                var apiResponse = new ApiResponse<object>(false,ex.Message,null,null);
                return StatusCode(500, apiResponse);
            }
            
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var process = await _AdministrativeProcessGenericProcessService.GetByIdAsync(id);
                if (process == null)
                {
                    
                    return NotFound(new ApiResponse<object>(false, "process not found", null, null));
                }

                
                return Ok(new ApiResponse<GetAdministrativeProcessDto>(true,
                    "process successfully received",
                    process, null
                ));
            }
            catch (Exception ex) {
                
                return StatusCode(500, new ApiResponse<object>(false, ex.Message, null, null));
            }
            
        }



        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.FindAsync(p => p.Title.Contains(title));
                var apiResponse = new ApiResponse<List<GetAdministrativeProcessDto>>(true,"list of searched processes",result,null);
                return Ok(apiResponse);
            }
            catch (Exception ex) {
                var apiResponse = new ApiResponse<object>(false,ex.Message,null,null); 
                return StatusCode(500, apiResponse);
            }
            
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdministrativeProcessDto dto)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.CreateAsync(dto);
                if (!result.IsSuccess)
                {

                    return BadRequest(new ApiResponse<object>(false,null,null,result.Errors));
                }
                    

                return Ok(new ApiResponse<object>(true,"process created successfully",null,null));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(false,ex.Message,null,null));
            }
            
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditAdministrativeProcessDto dto)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.EditAsync(id, dto);
                if (!result.IsSuccess)
                    return BadRequest(new ApiResponse<object>(false,null,null,result.Errors));

                return Ok(new ApiResponse<object>(true,"Process Updated Successfully",null,null));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(false,ex.Message,null,null));
            }
            
        }

        // DELETE: api/admin/AdministrativeProcesses/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var success = await _AdministrativeProcessGenericProcessService.DeleteAsync(id);
        //    if (!success)
        //        return NotFound();

        //    return Ok("Administrative Process deleted successfully.");
        //}

        
        
        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _AdministrativeProcessGenericProcessService.SoftDeleteAsync(id);
                if (!success)
                    return NotFound(new ApiResponse<object>(false,"Process not found",null,null));

                return Ok(new ApiResponse<object>(true,"Process soft deleted successfully",null,null));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(false,ex.Message,null,null));
            
            }
            
        }
    }

}

