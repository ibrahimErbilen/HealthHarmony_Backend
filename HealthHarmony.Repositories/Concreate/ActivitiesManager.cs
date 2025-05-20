using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Concreate
{
    public class ActivitiesManager : IActivitiesRepository
    {
        private readonly string _connectionString;
        public ActivitiesManager(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<Activity>> GetAllActivities()
        {
            var activities = new List<Activity>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllActivities", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                activities.Add(new Activity
                                {
                                    ActivityId = reader.GetInt32(0),
                                    ActivityName = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    EstimatedCaloriesBurn = reader.GetInt32(3),
                                    DifficultyLevel = reader.GetInt32(4),
                                    ImageUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    VideoUrl = reader.IsDBNull(6) ? null : reader.GetString(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                // Gerekirse loglama servisiyle kaydedebilirsin
            }

            return activities;
        }

        public async Task<int> GetEstimatedCaloriesBurnAsync(int activityId)
        {
            int estimatedCaloriesBurn = 0;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetEstimatedCaloriesBurn", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivityId", activityId);

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    estimatedCaloriesBurn = Convert.ToInt32(result);
                }
            }

            return estimatedCaloriesBurn;
        }
    }
}
