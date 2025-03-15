using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Interfaces;
using MediatR;


namespace AuthService.Application.Commands.Handlers.Pateint_authHandler
{
    public class PateintLoginCommandHandler : IRequestHandler<PatientLoginCommand, string>
    {
        private readonly IPatientRepo _repo;
        public PateintLoginCommandHandler(IPatientRepo repo)
        {
            _repo = repo;
        }

        public Task<string> Handle(PatientLoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //public Task<string> Handle(PatientLoginCommand request, CancellationToken cancellationToken)
        //{
        //    try
        //    {

        //    }
        //    catch (ValidationException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.InnerException?.Message ?? ex.Message);
        //    }
        //}

        //private string GenerateJwtToken(Agent? agent)
        //{
        //    if (agent == null)
        //    {
        //        throw new Exception("Agent not found.");
        //    }

        //    var securityKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

        //    if (string.IsNullOrEmpty(securityKey))
        //    {
        //        throw new Exception("Jwt Secret key is Missing");
        //    }

        //    var key = Encoding.UTF8.GetBytes(securityKey);
        //    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //       {
        //        new Claim(ClaimTypes.NameIdentifier, agent.Id.ToString()),
        //        new Claim(ClaimTypes.Name, agent.Email),
        //        new Claim(ClaimTypes.Role,"Agent")
        //    };

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: credentials
        //    );

        //    string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        //    return jwtToken;
        //}

        //private string GenerateRefreshToken()
        //{
        //    return Guid.NewGuid().ToString();
        //}
    }
}
