using AutoMapper;
using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Notification;
using office_automation_system.application.Dto.ProcessApprovalStep;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.application.Dto.RequestStepFile;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // AdministrativeProcess Mappings
            CreateMap<AdministrativeProcess, CreateAdministrativeProcessDto>().ReverseMap();
            CreateMap<AdministrativeProcess, EditAdministrativeProcessDto>().ReverseMap();
            CreateMap<AdministrativeProcess, GetAdministrativeProcessDto>().ReverseMap();

            //ApplicationUser Mappings
            CreateMap<ApplicationUser, CreateApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUser, EditApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUser, GetApplicationUserDto>().ReverseMap();


            //Notification Mappings
            CreateMap<Notification,CreateNotificationDto>().ReverseMap();
            CreateMap<Notification, EditNotificationDto>().ReverseMap();
            CreateMap<Notification, GetNotificationDto>().ReverseMap();

            //ProcessApprovalStep  Mappings
            CreateMap<ProcessApprovalStep, CreateProcessApprovalStepDto>().ReverseMap();
            CreateMap<ProcessApprovalStep, EditProcessApprovalStepDto>().ReverseMap();
            CreateMap<ProcessApprovalStep, GetProcessApprovalStepDto>().ReverseMap();
           

            //Request Mappings
            CreateMap<Request, CreateRequestDto>().ReverseMap();
            CreateMap<Request, EditRequestDto>().ReverseMap();
            CreateMap<Request, GetRequestDto>().ReverseMap();

            //RequestStep Mappings
            CreateMap<RequestStep, CreateRequestStepDto>().ReverseMap();
            CreateMap<RequestStep, EditRequestStepDto>().ReverseMap();
            CreateMap<RequestStep, GetRequestStepDto>().ReverseMap();

            //RequestStepFile Mappings
            CreateMap<RequestStepFile, CreateRequestStepFileDto>().ReverseMap();
            CreateMap<RequestStepFile, EditRequestStepFileDto>().ReverseMap();
            CreateMap<RequestStepFile, GetRequestStepFileDto>().ReverseMap();


        }
    }

}
