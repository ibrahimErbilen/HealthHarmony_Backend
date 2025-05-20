using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Concreate
{
    public class GeminiRepository : IGeminiRepository
    {
        private readonly string _connectionString;

        public GeminiRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveResponseAsync(GeminiLog log)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SaveGeminiLog", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Saklı yordam parametrelerini ekleyelim
                    command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = log.UserId });
                    command.Parameters.Add(new SqlParameter("@Prompt", SqlDbType.NVarChar) { Value = log.Prompt });
                    command.Parameters.Add(new SqlParameter("@Response", SqlDbType.NVarChar) { Value = log.Response });
                    command.Parameters.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime) { Value = log.Timestamp });

                    // Saklı yordamı çalıştır
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
