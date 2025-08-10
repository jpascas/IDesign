using IDesign.Access.Identity;
using IDesign.Access.Identity.JWT;
using IDesign.Access.Repositories;
using IDesign.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDesign.Manager
{
    public class LoginManager : ILoginManager
    {
        private readonly IUserRepository repository;
        private readonly IJwtProvider jwtProvider;
        private readonly IPasswordHasher passwordHasher;

        public LoginManager(IUserRepository repository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
        {
            this.repository = repository;
            this.jwtProvider = jwtProvider;
            this.passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<string>> HandleLoginAsync(LoginUserDto dto)
        {
            // check if user already exists
            var existentUser = await repository.GetByEmailAsync(dto.Email);

            if (existentUser == null)
                return OperationResult<string>.FailureResult("User doesnt exists", 401);

            if (!passwordHasher.VerifyHashedPassword(existentUser.PasswordHash, dto.Password))
                return OperationResult<string>.FailureResult("Password is incorrect", 401);

            return OperationResult<string>.SuccessResult(jwtProvider.Generate(existentUser)); // return JWT
        }
    }
}
