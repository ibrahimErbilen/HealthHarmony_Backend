using BCrypt.Net;
using HealthHarmony.Entities.DTOs.Coach;
using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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
    public class CoachRepository : ICoachRepository
    {
        private readonly string _connectionString;

        public CoachRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void AddCoach(CoachCreateDTO coachDto)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("AddCoach", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            string invitationCode = InvitationCodeGenerator.GenerateCode();

            // Şifre hashleniyor (bcrypt)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(coachDto.Password);

            cmd.Parameters.AddWithValue("@CoachName", coachDto.CoachName);
            cmd.Parameters.AddWithValue("@Email", coachDto.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.Parameters.AddWithValue("@InvitationCode", invitationCode);
            cmd.Parameters.AddWithValue("@ProfileImageUrl", coachDto.ProfileImageUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CoachType", (int)coachDto.CoachType);
            cmd.Parameters.AddWithValue("@Specialization", coachDto.Specialization ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Experience", coachDto.Experience ?? (object)DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        public void UpdateCoach(CoachDTO coachDto)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("UpdateCoach", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CoachId", coachDto.CoachId);
            cmd.Parameters.AddWithValue("@CoachName", coachDto.CoachName);
            cmd.Parameters.AddWithValue("@Email", coachDto.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", coachDto.PasswordHash);
            cmd.Parameters.AddWithValue("@InvitationCode", coachDto.InvitationCode);
            cmd.Parameters.AddWithValue("@ProfileImageUrl", coachDto.ProfileImageUrl);
            cmd.Parameters.AddWithValue("@CoachType", coachDto.CoachType);
            cmd.Parameters.AddWithValue("@Specialization", coachDto.Specialization);
            cmd.Parameters.AddWithValue("@Experience", coachDto.Experience);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteCoach(Guid coachId)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("DeleteCoach", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CoachId", coachId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public CoachDTO GetCoachByInvitationCode(string code)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("GetCoachByInvitationCode", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@InvitationCode", code);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new  CoachDTO
                {
                    CoachId = reader.GetGuid(0),
                    CoachName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    InvitationCode = reader.GetString(4),
                    ProfileImageUrl = reader.GetString(5),
                    CoachType = reader.GetInt32(6),
                    Specialization = reader.GetString(7),
                    Experience = reader.GetString(8)
                };
            }

            return null;
        }

        public List<CoachDTO> SearchCoachByName(string name)
        {
            List<CoachDTO> coaches = new();

            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("SearchCoachByName", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CoachName", name);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                coaches.Add(new CoachDTO
                {
                    CoachId = reader.GetGuid(0),
                    CoachName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    InvitationCode = reader.GetString(4),
                    ProfileImageUrl = reader.GetString(5),
                    CoachType = reader.GetInt32(6),
                    Specialization = reader.GetString(7),
                    Experience = reader.GetString(8)
                });
            }

            return coaches;
        }

        private static class InvitationCodeGenerator
        {
            private static readonly Random _random = new();

            public static string GenerateCode(int length = 6)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Range(1, length)
                    .Select(_ => chars[_random.Next(chars.Length)])
                    .ToArray());
            }
        }
    }
}
