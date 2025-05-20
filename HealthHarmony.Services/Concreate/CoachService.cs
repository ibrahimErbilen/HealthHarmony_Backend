using HealthHarmony.Entities.DTOs.Coach;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Concreate
{
    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _coachRepository;

        public CoachService(ICoachRepository coachRepository)
        {
            _coachRepository = coachRepository;
        }

        public void AddCoach(CoachCreateDTO coachDto)
        {
            _coachRepository.AddCoach(coachDto);
        }

        public void UpdateCoach(CoachDTO coachDto)
        {
            // Güncellemede InvitationCode değişmemesi isteniyorsa bu satırı yorum satırına alabilirsin
            coachDto.InvitationCode ??= InvitationCodeGenerator.GenerateCode();

            _coachRepository.UpdateCoach(coachDto);
        }

        public void DeleteCoach(Guid coachId)
        {
            _coachRepository.DeleteCoach(coachId);
        }

        public List<CoachDTO> SearchCoachByName(string name)
        {
            return _coachRepository.SearchCoachByName(name);
        }

        public CoachDTO GetCoachByInvitationCode(string code)
        {
            return _coachRepository.GetCoachByInvitationCode(code);
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
