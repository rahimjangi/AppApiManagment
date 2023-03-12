using AppApi.Data;
using AppApi.Helper;
using AppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserEFController:ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly DataContextEF _contextEF;
    public UserEFController(IConfiguration configuration)
    {
        _configuration = configuration;
        _contextEF= new DataContextEF(configuration);
    }

    [HttpGet("[action]")]
    public IEnumerable<UsersDto> GetAllUsers() {
        var mapper= MapperProfile.Initialize();
        var ListOfUsers = _contextEF.Users.ToList();
        return mapper.Map<IEnumerable<Users>,IEnumerable<UsersDto>>(ListOfUsers);
    }

    [HttpGet("[action]")]
    public IEnumerable<UsersDto> GetSUsers(string? firstName, string? lastName, string? email) {
        var mapper = MapperProfile.Initialize();
        var ListOfUsers = _contextEF.Users.ToList();
        if (firstName != null && lastName != null && email != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u=>u.FirstName==firstName)
                .Where(u=>u.LastName==lastName)
                .Where(u=>u.Email==email)
                );
        }
        else if (firstName != null && lastName != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u => u.FirstName == firstName)
                .Where(u => u.LastName == lastName)
                );
        }
        else if (firstName != null && email != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u => u.FirstName == firstName)
                .Where(u => u.Email == email)
                );
        }
        else if (lastName != null && email != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u => u.LastName == lastName)
                .Where(u => u.Email == email)
                );
        }
        else if (firstName != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u => u.FirstName == firstName)
                );
        }
        else if (lastName != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u => u.LastName == lastName)
                );
        }
        else if (email != null)
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users
                .Where(u => u.Email == email)
                );
        }
        else
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(_contextEF.Users);
        }
    }

    [HttpPost("[action]")]
    public int SaveUser(UsersDto user) {
        var mapper = MapperProfile.Initialize();
        var savedUser=_contextEF.Users.Add(mapper.Map<UsersDto, Users>(user));
        _contextEF.SaveChanges();
        if(savedUser != null)
        {
            return 1;
        }
        return 0;
    }

    [HttpPut("[action]")]
    public int UpdateUser(int userId, UsersDto user) {
        var mapper= MapperProfile.Initialize();
        var userFromDb=_contextEF.Users.FirstOrDefault(u => u.UserId == userId);
        Users users = mapper.Map<UsersDto,Users>(user);

        if(userFromDb != null)
        {
            userFromDb.FirstName = user.FirstName;
            userFromDb.LastName = user.LastName;
            userFromDb.Email = user.Email;
            userFromDb.Gender = user.Gender;
            userFromDb.Active = user.Active;
            _contextEF.Update(userFromDb);
        }
        _contextEF.SaveChanges();
        return 1;
    }

    [HttpDelete("[action]")]
    public IActionResult DeleteUser(int userId) {
        var userFromDb = _contextEF.Users.FirstOrDefault(u => u.UserId == userId);
        if(userFromDb != null)
        {
            _contextEF.Users.Remove(userFromDb);
            _contextEF.SaveChanges();
            return Ok();
        }
        throw new Exception("User does not exist");
    }

}
