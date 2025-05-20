using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HealthHarmony.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly string _connectionString;

        public UserActivityRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync()
        {
            var activities = new List<UserActivity>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("usp_GetAllUserActivities", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    activities.Add(new UserActivity
                    {
                        UserActivityId = (int)reader["UserActivityId"],
                        UserId = (Guid)reader["UserId"],
                        ActivityId = (int)reader["ActivityId"],
                        AddedDate = (DateTime)reader["AddedDate"],
                        CompletionDate = reader["CompletionDate"] as DateTime?,
                        IsCompleted = (bool)reader["IsCompleted"]
                    });
                }
            }

            return activities;
        }

        public async Task<UserActivity> GetUserActivityByIdAsync(Guid userActivityId)
        {
            UserActivity userActivity = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("usp_GetUserActivityById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserActivityId", userActivityId);

                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    userActivity = new UserActivity
                    {
                        UserActivityId = (int)reader["UserActivityId"],
                        UserId = (Guid)reader["UserId"],
                        ActivityId = (int)reader["ActivityId"],
                        AddedDate = (DateTime)reader["AddedDate"],
                        CompletionDate = reader["CompletionDate"] as DateTime?,
                        IsCompleted = (bool)reader["IsCompleted"]
                    };
                }
            }

            return userActivity;
        }

        public async Task<IEnumerable<UserActivity>> GetUserActivitiesByUserIdAsync(Guid userId)
        {
            var activities = new List<UserActivity>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("usp_GetUserActivitiesByUserId", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@UserId", userId);

                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        activities.Add(new UserActivity
                        {
                            UserActivityId = (int)reader["UserActivityId"],
                            UserId = (Guid)reader["UserId"],
                            ActivityId = (int)reader["ActivityId"],
                            ActivityName = reader["ActivityName"] as string ?? "No Name", // Default value if null
                            Description = reader["Description"] as string ?? "No Description", // Default value if null
                            AddedDate = (DateTime)reader["AddedDate"],
                            CompletionDate = reader["CompletionDate"] as DateTime?,
                            IsCompleted = (bool)reader["IsCompleted"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Buraya loglama yapabilir veya özel bir hata fırlatabilirsin
                // Örneğin: _logger.LogError(ex, "User activity verisi alınırken hata oluştu.");

                throw new Exception("Kullanıcı aktiviteleri alınırken bir hata oluştu.", ex);
            }

            return activities;
        }


        public async Task<int> AddUserActivityAsync(UserActivity userActivity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("usp_AddUserActivity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Pass the parameters to the stored procedure
                        command.Parameters.AddWithValue("@UserId", userActivity.UserId);
                        command.Parameters.AddWithValue("@ActivityId", userActivity.ActivityId);
                        command.Parameters.AddWithValue("@ActivityName", userActivity.ActivityName);
                        command.Parameters.AddWithValue("@Description", userActivity.Description);
                        command.Parameters.AddWithValue("@AddedDate", userActivity.AddedDate);
                        command.Parameters.AddWithValue("@CompletionDate", (object?)userActivity.CompletionDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IsCompleted", userActivity.IsCompleted);

                        // Execute the command and get the result
                        var result = await command.ExecuteScalarAsync();
                        if (result == null || result == DBNull.Value)
                        {
                            throw new Exception("Failed to retrieve UserActivityId.");
                        }

                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the full exception details, including stack trace and inner exception
                Console.WriteLine($"Error in AddUserActivityAsync: {ex.ToString()}");
                // Consider using a proper logging framework (Serilog, NLog) here
                throw new Exception("An error occurred while adding the user activity.", ex); // Keep re-throwing with inner ex
            }
        }



        public async Task<int> UpdateUserActivityAsync(UserActivity userActivity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("usp_UpdateUserActivity", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserActivityId", userActivity.UserActivityId);
                command.Parameters.AddWithValue("@UserId", userActivity.UserId);
                command.Parameters.AddWithValue("@ActivityId", userActivity.ActivityId);
                command.Parameters.AddWithValue("@AddedDate", userActivity.AddedDate);
                command.Parameters.AddWithValue("@CompletionDate", (object)userActivity.CompletionDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsCompleted", userActivity.IsCompleted);

                var result = await command.ExecuteScalarAsync();
                return (int)result; // Bu şekilde dönecek
            }
        }

        public async Task<Guid> DeleteUserActivityAsync(Guid userActivityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("usp_DeleteUserActivity", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserActivityId", userActivityId);

                var result = await command.ExecuteScalarAsync();
                return (Guid)result; // Bu şekilde dönecek
            }
        }

        public async Task<int> GetUserPendingCaloriesBurnAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new SqlCommand("GetUserPendingCaloriesBurn", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    var result = await cmd.ExecuteScalarAsync();
                    return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public async Task<bool> UpdateIsCompletedAsync(int userActivityId, bool isCompleted)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE UserActivities SET IsCompleted = @IsCompleted WHERE UserActivityId = @UserActivityId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsCompleted", isCompleted ? 1 : 0);
                    command.Parameters.AddWithValue("@UserActivityId", userActivityId);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


    }
}