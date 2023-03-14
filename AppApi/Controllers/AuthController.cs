using AppApi.Data;
using AppApi.Dto;
using AppApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

                byte[] passwordHash = GetPasswordHash(passwordSalt,registrationDto.Email);

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
                    string sqlInsertUser = @"INSERT INTO SCOTT.USERS (
                                            FirstName, 
                                            LastName,
                                            Email,
                                            Gender,
                                            Active
                                        ) VALUES ('"
                                            +registrationDto.FirstName+"', '" 
                                            +registrationDto.LastName + "', '"
                                            + registrationDto.Email + "', '"
                                            + registrationDto.Gender + "', '"
                                            + 1+"')";

                    if(_dapper.ExecuteSql(sqlInsertUser, null))
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
        string sqlForHashAndSalt= @"select
                                        [PasswordHash],
                                        [PasswordSalt]
                                   from scott.Auth
                                         where Auth.Email ='"+loginDto.Email+ "'";

        var userForLoginConfirmationDto=_dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt,null);
        if (userForLoginConfirmationDto == null) { return BadRequest("User Does Not Exist") ; }

        byte[] passwordHash = GetPasswordHash(userForLoginConfirmationDto.PasswordSalt, loginDto.Email);
        for (int i = 0; i < passwordHash.Length; i++)
        {
            if (passwordHash[i]!= userForLoginConfirmationDto.PasswordHash[i])
            {
                return BadRequest("Email or password is not correct!");
            }
        }
        int userId = _dapper.LoadDataSingle<int>("  select [UserId] from scott.USERS where Email='"+ loginDto.Email+ "'", null);
        return Ok(new Dictionary<string, string>()
        {
            {"token",CreateToken(userId) }
        });
    }

    private byte[] GetPasswordHash(byte[] passwordSalt,string password)
    {
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value
            + Convert.ToBase64String(passwordSalt);

        byte[] passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256
            );
        return passwordHash;
    }

    private string CreateToken(int userId)
    {
        string tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
        Claim[] claims = new Claim[]
        {
            new Claim("userId",userId.ToString())
        };
        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString));

        SigningCredentials signingCredentials = new SigningCredentials(tokenKey,SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires= DateTime.Now.AddDays(30)
        };

        JwtSecurityTokenHandler tokenHandler= new JwtSecurityTokenHandler();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
