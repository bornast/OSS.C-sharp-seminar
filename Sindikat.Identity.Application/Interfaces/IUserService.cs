using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserForListDto>> GetAll();
        Task<UserForDetailedDto> GetOne(string userId);
        Task Update(string userId, UserForUpdateDto userForUpdate);
        Task Delete(string id);
    }
}
