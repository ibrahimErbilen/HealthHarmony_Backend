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
    public class FoodRepository : IFoodRepository
    {
        private readonly string _connectionString;

        public FoodRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddDailyFoodEat(DailyFoodEat dailyFood)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AddDailyFoodEat", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", dailyFood.UserId);
                cmd.Parameters.AddWithValue("@FoodName", dailyFood.FoodName);
                cmd.Parameters.AddWithValue("@Calories", dailyFood.Calories);
                cmd.Parameters.AddWithValue("@EatTime", dailyFood.EatTime);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task AddFood(Food food)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("AddFood", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FoodName", food.FoodName);
                cmd.Parameters.AddWithValue("@Calories", food.Calories);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public List<DailyFoodEat> GetTodayDailyFoodEatByUserId(Guid userId)
        {
            var list = new List<DailyFoodEat>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_GetTodayDailyFoodEatByUserId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new DailyFoodEat
                        {
                            DailyFoodEatID = Convert.ToInt32(reader["DailyFoodEatID"]),
                            UserId = (Guid)reader["UserId"],
                            FoodName = reader["FoodName"].ToString(),
                            Calories = Convert.ToInt32(reader["Calories"]),
                            EatTime = Convert.ToDateTime(reader["EatTime"])
                        };

                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public async Task<Food> SearchFoodByName(string name)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SearchFoodByName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FoodName", name);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Food
                        {
                            FoodId = reader.GetInt32(reader.GetOrdinal("FoodId")),
                            FoodName = reader.GetString(reader.GetOrdinal("FoodName")),
                            Calories = reader.GetInt32(reader.GetOrdinal("Calories"))
                        };
                    }
                }
            }

            return null;
        }
    }
}

