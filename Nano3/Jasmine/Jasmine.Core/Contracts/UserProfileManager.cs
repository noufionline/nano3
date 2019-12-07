using Jasmine.Core.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Jasmine.Core.Repositories;

namespace Jasmine.Core.Contracts
{
    public interface IUserProfileManager
    {
        Task<bool> UpdateProfilePhoto(int userId, byte[] photo);
        Task<byte[]> GetProfilePhotoAsync(int userId);
        Task<bool> ClearProfilePhoto(int loggedUserId);
        byte[] Photo { get; set; }
    }

    public class UserProfileManager : IUserProfileManager
    {
        readonly IUserProfileManagerService _service;
        public UserProfileManager(IUserProfileManagerService service)
        {
            _service = service;
        }

        public Task<bool> UpdateProfilePhoto(int userId, byte[] photo)
        {
            return _service.UpdateProfilePhoto(userId, photo);
        }
        public async Task<byte[]> GetProfilePhotoAsync(int userId)
        {
            Photo = await _service.GetProfilePhoto(userId);
            return Photo;
        }
        public Task<bool> ClearProfilePhoto(int loggedUserId)
        {
            return _service.ClearProfilePhoto(loggedUserId);
        }

        public byte[] Photo { get; set; }
    }    


    public interface IUserProfileManagerService
    {
        Task<bool> UpdateProfilePhoto(int userId, byte[] photo);
        Task<byte[]> GetProfilePhoto(int userId);
        Task<bool> ClearProfilePhoto(int loggedUserId);
    }


    public class UserProfileManagerService : IUserProfileManagerService
    {
        readonly IUserProfileManagerRepository _repository;
        public UserProfileManagerService(IUserProfileManagerRepository repository)
        {
            _repository = repository;
        }
        public Task<bool> UpdateProfilePhoto(int userId, byte[] photo)
        {
            return _repository.UpdateProfilePhotoAsync(userId, photo);
        }
        public Task<byte[]> GetProfilePhoto(int userId)
        {
            return _repository.GetProfilePhotoAsync(userId);
        }
        public Task<bool> ClearProfilePhoto(int loggedUserId)
        {
            return _repository.ClearProfilePhotoAsync(loggedUserId);
        }
    }

    public interface IUserProfileManagerRepository
    {
        Task<bool> UpdateProfilePhotoAsync(int userId, byte[] photo);
        Task<byte[]> GetProfilePhotoAsync(int userId);
        Task<bool> ClearProfilePhotoAsync(int loggedUserId);

    }

    public class UserProfileManagerRepository :RestApiRepositoryBase, IUserProfileManagerRepository
    {
        
        
        public async Task<bool> UpdateProfilePhotoAsync(int userId, byte[] photo)
        {
            string requestUri = $"accounts/{userId}/photo/update";
            return await PutAndReturnStatusWithStreamAsync(requestUri, photo);
        }
        public async Task<byte[]> GetProfilePhotoAsync(int userId)
        {
            string request = $"accounts/{userId}/photo";
            return await ReadAsStreamAsync<byte[]>(request);
        }
        public async Task<bool> ClearProfilePhotoAsync(int loggedUserId)
        {
            string requestUri = $"accounts/{loggedUserId}/photo/clear";
            return await ReadAsAsync<bool>(requestUri);
        }

        public UserProfileManagerRepository(IHttpClientFactory factory) : base(factory)
        {
        }
    }


}
