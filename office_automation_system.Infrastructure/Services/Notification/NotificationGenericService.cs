using AutoMapper;
using office_automation_system.application.Contracts.Services.Notification;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.Notification;
using office_automation_system.application.Validator.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
namespace office_automation_system.Infrastructure.Services.Notification
{
    public class NotificationGenericService : INotificationGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateNotificationDtoValidator _createValidator;
        private readonly EditNotificationDtoValidator _editValidator;

        public NotificationGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            EditNotificationDtoValidator editValidator,
            CreateNotificationDtoValidator createValidator)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _editValidator = editValidator;
        }



        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateNotificationDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var Notification = _mapper.Map<office_automation_system.domain.Entities.Notification>(dto);
            await _unitOfWork.Notifications.AddAsync(Notification);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditNotificationDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _editValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var Notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (Notification == null) return (false, new List<string> { "Notification Not Found with this id" });

            _mapper.Map(dto, Notification);
            _unitOfWork.Notifications.Update(Notification);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<List<GetNotificationDto>> GetAllAsync()
        {
            var Notifications = await _unitOfWork.Notifications.GetAllAsync();
            return _mapper.Map<List<GetNotificationDto>>(Notifications);
        }



        public async Task<GetNotificationDto?> GetByIdAsync(Guid id)
        {
            var Notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            return Notification == null ? null : _mapper.Map<GetNotificationDto>(Notification);
        }


        public async Task<List<GetNotificationDto>> FindAsync(
        Expression<Func<office_automation_system.domain.Entities.Notification, bool>> predicate)
        {
            // 1. Use the generic repository to query entities
            var Notifications = await _unitOfWork.Notifications
                .FindAsync(predicate);

            // 2. Convert entities to DTOs using AutoMapper
            var dtos = _mapper.Map<List<GetNotificationDto>>(Notifications);

            // 3. Return the DTO list
            return dtos;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var Notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (Notification == null) return false;

            _unitOfWork.Notifications.Remove(Notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var Notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (Notification == null)
            {
                return false;
            }

            Notification.IsDeleted = true;
            _unitOfWork.Notifications.Update(Notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
