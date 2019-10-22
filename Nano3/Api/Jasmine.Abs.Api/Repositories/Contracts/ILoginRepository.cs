using System.Collections.Generic;
using System.Threading.Tasks;
using Jasmine.Abs.Api.Dto;
using Jasmine.Abs.Api.Dto.List;
using Jasmine.Abs.Entities;

namespace Jasmine.Abs.Api.Repositories.Contracts
{
    public interface ILoginRepository
    {
        Task<List<LookupItem>> GetUsersAsync();
        Task<List<UserInfo>> GetUsersInfoAsync();
        Task<List<AbsApplicationInfo>> GetDivisionsAsync();
        Task<bool> ValidateAsync(string email, string password);
        Task<bool> UpdateAsync(UserDto userInfo);
        Task<bool> UpdateProfilePhotoAsync(int userId, byte[] photo);
        Task<byte[]> GetProfilePhotoAsync(int userId);
        Task<bool> ClearProfilePhotoAsync(int userId);
        
    }
}