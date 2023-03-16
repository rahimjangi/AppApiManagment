using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppApi.Helper;

public  class TokenHelper
{

    private readonly IConfiguration _config;

    public TokenHelper(IConfiguration config)
    {
        _config = config;
    }

    public  byte[] GetPasswordHash(byte[] passwordSalt, string password)
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

    public  string CreateToken(int userId)
    {
        string tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
        Claim[] claims = new Claim[]
        {
            new Claim("userId",userId.ToString())
        };
        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString));

        SigningCredentials signingCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires = DateTime.Now.AddDays(30)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
