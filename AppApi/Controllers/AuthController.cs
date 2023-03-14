using AppApi.Data;
using AppApi.Dto;
using AppApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
        _dapper = new DataContextDapper(config);
    }

    [HttpPost("[action]")]
    public IActionResult Register(UserForRegistrationDto registrationDto)
    {
        if(registrationDto.Password ==registrationDto.PasswordConfirm)
        {
            string sql = @"SELECT 
                            [Email] 
                         FROM scott.Auth
                            WHERE Email = '@email'";
            IEnumerable<string> registredUsers=_dapper.LoadData<string>(sql, registrationDto.Email.ToString());
            if(registredUsers.Count() == 0)
            {
                byte[] passwordSalt= new byte[128];
                using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(passwordSalt);
                }
                string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value
                    + Convert.ToBase64String(passwordSalt);

                byte[] passwordHash = KeyDerivation.Pbkdf2(
                    password: registrationDto.Password,
                    salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256
                    );
                string sqlAddAuth = @"INSERT INTO scott.Auth (
                                             [Email]
                                            ,[PasswordHash]
                                            ,[PasswordSalt]
                                        ) VALUES (
                                            @Email, @PasswordHash, @PasswordSalt)";

                List<SqlParameter>sqlParameters = new List<SqlParameter>();

                SqlParameter emailParameter = new SqlParameter("@Email", SqlDbType.NVarChar);
                emailParameter.Value = registrationDto.Email;

                SqlParameter PasswordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                PasswordHashParameter.Value = passwordHash;

                SqlParameter PasswordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                PasswordSaltParameter.Value = passwordSalt;

                sqlParameters.Add(emailParameter);
                sqlParameters.Add(PasswordHashParameter);
                sqlParameters.Add(PasswordSaltParameter);

                if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to register the user!");
                }

            }
            else
            {
                return BadRequest("User with this email is already exist!");
            }

        }
        return BadRequest("Password do not match!");
    }

    [HttpPost("[action]")]
    public IActionResult Login(UserForLoginDto loginDto)
    {
        return Ok();
    }
}
