using HealthHarmony.Entities.DTOs.Daily;
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
    public class DailyDataRepository : IDailyDataRepository
    {
        private readonly string _connectionString;

        public DailyDataRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void SaveOrUpdateDailyData(DailyData data)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_SaveOrUpdateDailyData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", data.UserId);
                cmd.Parameters.AddWithValue("@Date", data.Date.Date);
                cmd.Parameters.AddWithValue("@StepCount", data.StepCount);
                cmd.Parameters.AddWithValue("@CaloriesBurned", data.CaloriesBurned);
                cmd.Parameters.AddWithValue("@CaloriesConsumed", data.CaloriesConsumed);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public DailyData? GetTodayData(Guid userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("usp_GetTodayDailyData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Null kontrolü yaparak veri okuma işlemi
                            if (!reader.IsDBNull(0)) // DailyDataId null ise
                            {
                                return new DailyData
                                {
                                    DailyDataId = reader.GetInt32(0), // id okuma
                                    UserId = reader.GetGuid(1),        // UserId okuma
                                    Date = reader.GetDateTime(2),
                                    StepCount = reader.GetInt32(3),
                                    CaloriesBurned = reader.GetInt32(4),
                                    CaloriesConsumed = reader.GetInt32(5)
                                };
                            }
                            else
                            {
                                // DailyDataId null olduğunda ne yapılacağına karar verebilirsiniz.
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını loglama
                Console.WriteLine(ex.Message);
                return null;
            }

            return null;
        }

        public async Task<List<DailyData>> GetLast6DaysCaloriesBurnedAsync(Guid userId)
        {
            var results = new List<DailyData>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetLast6DaysCaloriesBurned", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new DailyData
                        {
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            StepCount = reader.GetInt32(reader.GetOrdinal("StepCount")),
                            CaloriesBurned = reader.GetInt32(reader.GetOrdinal("CaloriesBurned")),
                            CaloriesConsumed = reader.GetInt32(reader.GetOrdinal("CaloriesConsumed")),
                            UserId = userId // UserId stored procedure'de dönmüyor, biz kendimiz ekliyoruz
                        });
                    }
                }
            }

            return results;
        }

        public async Task UpdateCaloriesConsumedAsync(Guid userId, int caloriesConsumed)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("UpdateLatestCaloriesConsumed", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                command.Parameters.Add(new SqlParameter("@CaloriesConsumed", SqlDbType.Int) { Value = caloriesConsumed });

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateCaloriesBurnedAsync(Guid userId, int caloriesBurned)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("sp_UpdateCaloriesBurned", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                command.Parameters.Add(new SqlParameter("@CaloriesBurned", SqlDbType.Int) { Value = caloriesBurned });

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
