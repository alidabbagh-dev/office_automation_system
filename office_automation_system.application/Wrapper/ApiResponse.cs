using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Wrapper
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse(bool success, string? message = null, T? data = default, List<string>? errors = null) { 
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
        }
    }
}
