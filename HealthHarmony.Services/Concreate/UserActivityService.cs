using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthHarmony.Entities.DTOs;
using HealthHarmony.Entities.DTOs.User_Activity;
using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Services.Contracts;

namespace HealthHarmony.Service
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _repository;

        public UserActivityService(IUserActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserActivityDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllUserActivitiesAsync();
            return entities.Select(MapToDto);
        }

        public async Task<UserActivityDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetUserActivityByIdAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<IEnumerable<UserActivityDto>> GetByUserIdAsync(Guid userId)
        {
            var entities = await _repository.GetUserActivitiesByUserIdAsync(userId);
            return entities.Select(MapToDto);
        }

        public async Task<int> AddAsync(UserActivityDto dto)
        {
            var entity = MapToEntity(dto);
            return await _repository.AddUserActivityAsync(entity);
        }


        public async Task<int> UpdateAsync(UserActivityDto dto)
        {
            var entity = MapToEntity(dto);
            return await _repository.UpdateUserActivityAsync(entity); // Bu Guid döndürüyor
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _repository.DeleteUserActivityAsync(id);
        }

        // Mapping Methods
        private UserActivityDto MapToDto(UserActivity entity)
        {
            return new UserActivityDto
            {
                UserActivityId = entity.UserActivityId,
                UserId = entity.UserId,
                ActivityId = entity.ActivityId,
                AddedDate = entity.AddedDate,
                CompletionDate = entity.CompletionDate,
                IsCompleted = entity.IsCompleted,
                Description = entity.Description,
                ActivityName = entity.ActivityName
            };
        }

        private UserActivity MapToEntity(UserActivityDto dto)
        {
            return new UserActivity
            {
                UserActivityId = dto.UserActivityId,
                UserId = dto.UserId,
                ActivityId = dto.ActivityId,
                AddedDate = dto.AddedDate,
                CompletionDate = dto.CompletionDate,
                IsCompleted = dto.IsCompleted,
                Description = dto.Description,
                ActivityName = dto.ActivityName,
                
            };
        }

        public Task GetAllActivitiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetUserPendingCaloriesBurnAsync(Guid userId)
        {
            return await _repository.GetUserPendingCaloriesBurnAsync(userId);
        }

        public async Task<bool> UpdateIsCompletedAsync(int userActivityId, bool isCompleted)
        {
            return await _repository.UpdateIsCompletedAsync(userActivityId, isCompleted);
        }
    }
}
