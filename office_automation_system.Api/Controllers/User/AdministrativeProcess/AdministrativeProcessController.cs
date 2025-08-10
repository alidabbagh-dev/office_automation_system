using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Wrapper;
namespace office_automation_system.Api.Controllers.User.AdministrativeProcess
{
    [Authorize]
    [Route("api/user/[controller]")]
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
                return Ok(new ApiResponse<List<GetAdministrativeProcessDto>>(true,"list of all processes",processes,null));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(false,ex.Message,null,null));
            }
            
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var process = await _AdministrativeProcessGenericProcessService.GetByIdAsync(id);
                if (process == null)
                    return NotFound(new ApiResponse<object>(false,"Process not found",null,null));

                return Ok(new ApiResponse<GetAdministrativeProcessDto>(true, "process received successfully"
                    ,process,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(false,ex.Message,null,null));
            }
            
        }



        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            try
            {
                var result = await _AdministrativeProcessGenericProcessService.FindAsync(p => p.Title.Contains(title));
                return Ok(new ApiResponse<List<GetAdministrativeProcessDto>>(true,"List of searched processes",
                    result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(false,ex.Message,null,null));            
            }
            
        }

    }
}
