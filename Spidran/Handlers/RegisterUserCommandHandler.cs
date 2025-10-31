using Microsoft.EntityFrameworkCore;
using MyMediator.Interfaces;
using Spidran.DB;
using Spidran.Command;


namespace Spidran.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly SpidranContext _context;

        public RegisterUserCommandHandler(SpidranContext context)
        {
            _context = context;
        }

        public async Task<Result> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            {
                return Result.Fail("Пользователь с таким Email уже существует");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Phone = request.Phone,
                Info = request.Info,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok("Пользователь успешно зарегистрирован", new { userId = user.Id });
        }
    }

}
