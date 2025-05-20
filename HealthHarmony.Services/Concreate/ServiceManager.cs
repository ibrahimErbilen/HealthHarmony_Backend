using HealthHarmony.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Concreate
{
    public class ServiceManager : IServiceManager
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public ServiceManager(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }


        public IAuthService AuthService => _authService;

        public ITokenService TokenService => _tokenService;
    }
}
