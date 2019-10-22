using System;
using Jasmine.Abs.Api.Dto;
using Jasmine.Abs.Entities;
using Jasmine.Abs.Entities.Models.Zeon;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Api.Dto.List;
using Jasmine.Abs.Api.Repositories.Exceptions;

namespace Jasmine.Abs.Api.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ZeonContext _context;

        public LoginRepository(ZeonContext context)
        {
            _context = context;
        }

        public async Task<List<LookupItem>> GetUsersAsync() => await _context.AspNetUsers.Select(x => new LookupItem
        {
            Id = x.EmployeeId,
            Name = x.Email
        }).ToListAsync().ConfigureAwait(false);

        public async Task<List<UserInfo>> GetUsersInfoAsync()
        {
            var users = await _context.AspNetUsers.Select(x => new UserInfo
            {
                Id = x.EmployeeId,
                Name = x.Email,
                UserName = x.EmployeeName
            }).ToListAsync().ConfigureAwait(false);
            return users;
        }

        public async Task<List<AbsApplicationInfo>> GetDivisionsAsync()
        {

            var items = await _context.AbsDivisions.ToListAsync();
            return await _context.AbsDivisions.Select(x => new AbsApplicationInfo
            {
                Id = x.Id,
                Name = x.Name,
                ApplicationType = x.ApplicationType
            }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> ValidateAsync(string email, string password)
        {
            var user = await _context.AspNetUsers.SingleOrDefaultAsync(i => i.Email == email);
            if (user == null) return false;

            var isAuthorizedUser = PasswordHelper.VerifyHashedPassword(user.PasswordHash, password);
            if (isAuthorizedUser) return true;
            return false;
        }
        public async Task<bool> UpdateAsync(UserDto userInfo)
        {
            var user = await _context.AspNetUsers.AsTracking().SingleOrDefaultAsync(i => i.Email == userInfo.Email);
            if (user != null)
            {
                var passwordHash = PasswordHelper.HashPassword(userInfo.Password);

                user.PasswordHash = passwordHash;
                user.SecurityStamp = passwordHash;
            }

            try
            {
                // Persist changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.AspNetUsers.Any(o => o.Id == user.Id))
                    throw new EntityNotFoundException();
                throw;
            }

            return true;
        }

  
        public async Task<bool> UpdateProfilePhotoAsync(int userId, byte[] photo)
        {
            var user = await _context.AspNetUsers.AsTracking().SingleOrDefaultAsync(i => i.EmployeeId == userId);
            if (user != null)
            {
                user.Photo = photo;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
            }
            return false;
        }
        public async Task<byte[]> GetProfilePhotoAsync(int userId)
        {
            var user = await _context.AspNetUsers.AsTracking().SingleOrDefaultAsync(i => i.EmployeeId == userId);
            if (user != null)
            {
                return user.Photo;
            }
            return null;
            
        }
        public async Task<bool> ClearProfilePhotoAsync(int userId)
        {
            var user = await _context.AspNetUsers.AsTracking().SingleOrDefaultAsync(i => i.EmployeeId == userId);
            if (user != null)
            {
                user.Photo = null;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
            }
            return false;
        }
        
    }
}