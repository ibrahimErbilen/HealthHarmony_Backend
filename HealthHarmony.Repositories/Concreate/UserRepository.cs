using System;
using System.Data;
using Microsoft.Data.SqlClient;
using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace HealthHarmony.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private User _userToAdd;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetUserByEmail", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }
            return null;
        }

        public User GetById(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetUserById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }
            return null;
        }

        public void Add(User user)
        {
            _userToAdd = user;
        }

        public bool SaveChanges()
        {
            if (_userToAdd == null)
                return false;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("usp_AddUserAdd", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Username", _userToAdd.Username);
            command.Parameters.AddWithValue("@Email", _userToAdd.Email);
            command.Parameters.AddWithValue("@PasswordHash", _userToAdd.PasswordHash);
            command.Parameters.AddWithValue("@ProfileImageUrl", (object?)_userToAdd.ProfileImageUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@RegistrationDate", _userToAdd.RegistrationDate);
            command.Parameters.AddWithValue("@RefreshToken", (object?)_userToAdd.RefreshToken ?? DBNull.Value);
            command.Parameters.AddWithValue("@RefreshTokenExpiryTime", (object?)_userToAdd.RefreshTokenExpiryTime ?? DBNull.Value);

            connection.Open();
            var result = command.ExecuteScalar();
            _userToAdd.UserId = (Guid)result;
            _userToAdd = null;

            return true;
        }

        public void Update(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateUser", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@ProfileImageUrl", (object?)user.ProfileImageUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@RegistrationDate", user.RegistrationDate);
            command.Parameters.AddWithValue("@RefreshToken", (object?)user.RefreshToken ?? DBNull.Value);
            command.Parameters.AddWithValue("@RefreshTokenExpiryTime", (object?)user.RefreshTokenExpiryTime ?? DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public User GetByRefreshToken(string refreshToken)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetUserByRefreshToken", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RefreshToken", refreshToken);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }

            return null;
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = (Guid)reader["UserId"],
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                ProfileImageUrl = reader["ProfileImageUrl"]?.ToString(),
                RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                RefreshToken = reader["RefreshToken"]?.ToString(),
                RefreshTokenExpiryTime = reader["RefreshTokenExpiryTime"] as DateTime?
            };
        }
    }
}
