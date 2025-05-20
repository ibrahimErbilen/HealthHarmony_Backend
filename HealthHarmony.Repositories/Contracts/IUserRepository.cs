using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        User GetById(Guid id);
        User GetByRefreshToken(string refreshToken);
        void Update(User user);
        void Add(User user);
        bool SaveChanges();
    }
}
