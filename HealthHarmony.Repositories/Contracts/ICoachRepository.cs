using HealthHarmony.Entities.DTOs.Coach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface ICoachRepository
    {
        void AddCoach(CoachCreateDTO coachDto);
        void UpdateCoach(CoachDTO coachDto);
        void DeleteCoach(Guid coachId);
        List<CoachDTO> SearchCoachByName(string name);
        CoachDTO GetCoachByInvitationCode(string code);
    }
}
