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
                var result = await _RequestStepFileGenericService.GetByIdAsync(id);
                if (result == null)
                    return NotFound("Request Step file not found");
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
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

                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
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
                    return BadRequest(result.Errors);
                }
                else
                {
                    return Ok("Attachment File Uploaded Successfully");
                }
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
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
                    return BadRequest("Could not delete Request Step attachment File");

                }

                return Ok("Request Step Attachment file deleted successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

    }
}
