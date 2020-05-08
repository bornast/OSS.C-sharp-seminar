using Sindikat.Identity.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAll();
    }
}
