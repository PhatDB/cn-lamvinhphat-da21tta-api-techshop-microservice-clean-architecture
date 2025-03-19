using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Application.DTOs;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;

namespace UserService.Application.Queries.Users.GetUserInfomation
{
    public class GetUserInformationQueryHandler : IQueryHandler<GetUserInfomationQuery, UserDTO>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserInformationQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> Handle(GetUserInfomationQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.AsQueryable().AsNoTracking().Include(u => u.UserAddresses).Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return Result.Failure<UserDTO>(UserError.UserNotFound);

            UserDTO? userDto = _mapper.Map<UserDTO>(user);

            return Result.Success(userDto);
        }
    }
}