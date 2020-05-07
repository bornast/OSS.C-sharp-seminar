using AutoMapper;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _repo;
        private readonly IMapper _mapper;

        public UserService(IBaseRepository<User> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserForListDto>> GetAll()
        {
            var users = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<UserForListDto>>(users);
        }

        public async Task<UserForDetailedDto> GetOne(string userId)
        {
            var user = await _repo.FindAsync(userId);

            if (user == null)
                throw new NotFoundException();

            var userDto = _mapper.Map<UserForDetailedDto>(user);
            return userDto;
        }

        public async Task Update(string userId, UserForUpdateDto userForUpdate)
        {
            var user = await _repo.FindAsync(userId);

            _mapper.Map(userForUpdate, user);

            await _repo.FlushAsync();
        }

        public async Task Delete(string id)
        {
            var user = await _repo.FindAsync(id);

            _repo.Delete(user);

            await _repo.FlushAsync();
        }

    }
}
