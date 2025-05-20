using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
    }
}
