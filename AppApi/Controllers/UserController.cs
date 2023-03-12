using AppApi.Data;
using AppApi.Helper;
using AppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContextDapper _dataContext;
    private readonly IConfiguration _config;
    public UserController(IConfiguration config)
    {
        _config = config;
        _dataContext = new DataContextDapper(_config);
    }

    [HttpGet("[action]")]
    public IEnumerable<UsersDto> GetAllUsers() {
        var mapper= MapperProfile.Initialize();
        var usersFromDb=_dataContext.LoadData<Users>("SELECT * FROM SCOTT.USERS",null);
        var results=mapper.Map<IEnumerable<Users>, List<UsersDto>>(usersFromDb);
        return results;
    }

    [HttpGet("[action]")]
    public UsersDto GetUsers(string? firstName,string? lastName,string? email)
    {
        var mapper= MapperProfile.Initialize();

        string selectQuary = @"SELECT FirstName, LastName, Email, Gender, Active
                FROM SCOTT.USERS
                WHERE  ";
        string queryExtension = @"";
        string queryString = "";
        var userFromDb = new Users();

        if (firstName!=null && lastName!=null && email != null)
        {
            queryExtension = @"FirstName=@firstName AND LastName=@lastName AND Email=@email";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new { firstName, lastName, email });
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else if (firstName != null && lastName != null)
        {
            queryExtension = @"users.FirstName=@firstName AND users.LastName=@lastName";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new { firstName, lastName});
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else if (firstName != null && email != null)
        {
            queryExtension = @"users.FirstName=@firstName AND users.Email=@email";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new { firstName, email });
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else if (lastName != null && email != null)
        {
            queryExtension = @"users.LastName=@lastName AND users.Email=@email";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new { lastName, email });
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else if(firstName != null)
        {
            queryExtension = @"users.FirstName=@firstName";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new { firstName});
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else if (lastName != null)
        {
            queryExtension = @"users.LastName=@lastName";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new { lastName});
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else if (email != null)
        {
            queryExtension = @" users.Email=@email";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadDataSingle<Users>(queryString, new {email });
            return mapper.Map<Users, UsersDto>(userFromDb);
        }
        else
        {
            return  mapper.Map<Users, UsersDto>(userFromDb); ;
        }
    }

    [HttpPost("[action]")]
    public int SaveUsers(List<Users> userList)
    {
        var mapper=MapperProfile.Initialize();
        List<UsersDto> usersDtoList=mapper.Map< List < Users > ,List <UsersDto>>(userList);
        string insertAllQuary = @"INSERT INTO SCOTT.USERS (
                FirstName,
                LastName,
                Email,
                Gender,
                Active
            ) VALUES (@FirstName, @LastName, @Email, @Gender, @Active)
        ";

        if(userList.Count > 0)
        {
            return _dataContext.ExecuteSqlWithRowCount(insertAllQuary, usersDtoList);
        }

        return 0;
    }

}
