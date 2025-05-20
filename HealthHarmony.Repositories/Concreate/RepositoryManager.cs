using HealthHarmony.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Concreate
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IUserRepository _userRepository;

        public RepositoryManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IUserRepository User => _userRepository;
    }
}
