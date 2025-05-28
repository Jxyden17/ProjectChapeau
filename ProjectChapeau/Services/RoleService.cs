using ProjectChapeau.Models;
using ProjectChapeau.Repositories;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }
    }
}
