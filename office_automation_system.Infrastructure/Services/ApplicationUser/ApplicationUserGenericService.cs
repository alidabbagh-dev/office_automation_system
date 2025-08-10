using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using office_automation_system.application.Contracts.Services.ApplicationUser;
using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Role;
using office_automation_system.application.Validator.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using office_automation_system.application.Validator.Role;
namespace office_automation_system.Infrastructure.Services.ApplicationUser
{
    public class ApplicationUserGenericService : IApplicationUserGenericService
    {
        private readonly UserManager<office_automation_system.domain.Entities.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;
        private readonly CreateApplicationUserDtoValidator _createValidator;
        private readonly EditApplicationUserDtoValidator _editValidator;
        private readonly AssignRoleDtoValidator _assignRoleValidator;

        public ApplicationUserGenericService(IMapper mapper,UserManager<office_automation_system.domain.Entities.ApplicationUser> userManager,
                                     RoleManager<IdentityRole<Guid>> roleManager,
                                     EditApplicationUserDtoValidator editValidator,
                                     CreateApplicationUserDtoValidator createValidator,
                                     AssignRoleDtoValidator assignRoleValidator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _editValidator = editValidator;
            _createValidator = createValidator;
            _assignRoleValidator = assignRoleValidator;
        }

        public async Task<List<GetApplicationUserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<GetApplicationUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleIds = new List<Guid>();
                
                foreach (var roleName in roles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        roleIds.Add(role.Id);
                    }
                }
                result.Add(new GetApplicationUserDto
                {
                    Id = user.Id,
                    Email = user?.Email,
                    RoleId = roleIds.FirstOrDefault(),
                    IsLocked = user?.LockoutEnd != null && user?.LockoutEnd > DateTimeOffset.UtcNow,
                    UserCode = user?.UserCode
                });
            }

            return result;
        }

        public async Task<GetApplicationUserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = new List<Guid>();
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roleIds.Add(role.Id);
                }
            }
            return new GetApplicationUserDto
            {
                Id = user.Id,
                Email = user?.Email,
                RoleId = roleIds.FirstOrDefault(),
                IsLocked = user?.LockoutEnd != null && user?.LockoutEnd > DateTimeOffset.UtcNow,
                UserCode = user?.UserCode
            };
        }

        public async Task<List<GetApplicationUserDto>> FilterUsersAsync(UserFilterDto filter)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter?.Email))
                query = query.Where(u => u.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter?.UserName))
                query = query.Where(u => u.UserName.Contains(filter.UserName));

            if (!string.IsNullOrWhiteSpace(filter?.UserCode))
                query = query.Where(u => u.UserCode.Contains(filter.UserCode));

            var users = await query.ToListAsync();
            return _mapper.Map<List<GetApplicationUserDto>>(users);
        }


        public async Task<(bool Success , List<string> Errors)> CreateUserAsync(CreateApplicationUserDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var user = new office_automation_system.domain.Entities.ApplicationUser
            {
                UserName = dto?.Email,
                Email = dto?.Email,
                UserCode = dto?.UserCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return (false, ["Could not Create User"]);

            var AssignRoleDto = new AssignRoleDto { UserId = user?.Id, RoleId = dto?.RoleId };
            await AssignRoleToUserAsync(AssignRoleDto);

            return (true,[]) ;
        }

        public async Task<(bool Success , List<string> Errors)> UpdateUserAsync(Guid Id , EditApplicationUserDto dto)
        {

            FluentValidation.Results.ValidationResult validation = await _editValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }


            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return (false,["Did not Find the User"]) ;

            user.Email = dto?.Email;
            user.UserName = dto?.Email;
            user.UserCode = dto?.UserCode;
            var AssignRoleDto = new AssignRoleDto { UserId = user?.Id, RoleId = dto?.RoleId };
            await AssignRoleToUserAsync(AssignRoleDto);

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                return (false, ["Could not Update user"]);
            }

            return (true, []);
        }

        public async Task<bool> LockUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UnlockUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }






        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<(bool Success,List<string> Errors)> AssignRoleToUserAsync(AssignRoleDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _assignRoleValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }
            var user = await _userManager.FindByIdAsync(dto?.UserId.ToString());
            if (user == null) return (false,["User Not Found"]);

            var existingRoles = await _userManager.GetRolesAsync(user);
            if (existingRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, existingRoles);
            }

            var role = await _roleManager.FindByIdAsync(dto?.RoleId.ToString());
            if (role == null) return (false,["Role Not Found"]) ;

            var result = await _userManager.AddToRoleAsync(user, role.Name!);
            return (result.Succeeded,[]) ;
        }
    }

}
