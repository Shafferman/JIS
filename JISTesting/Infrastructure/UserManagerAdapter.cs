using JISTesting.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JISTesting.Infrastructure
{
    public class UserManagerAdapter : IUserManager
    {
        private readonly UserManager<IdentityUser> _manager;

        public UserManagerAdapter(UserManager<IdentityUser> manager)
        {
            _manager = manager;
        }
    }
}
