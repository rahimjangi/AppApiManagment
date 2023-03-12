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
    public IEnumerable<UsersDto> GetSUsers(string? firstName,string? lastName,string? email)
    {
        var mapper= MapperProfile.Initialize();

        string selectQuary = @"SELECT FirstName, LastName, Email, Gender, Active
                FROM SCOTT.USERS
                WHERE  ";
        string queryExtension = @"";
        string queryString = "";
        IEnumerable<Users> userFromDb = new List<Users>();

        if (firstName!=null && lastName!=null && email != null)
        {
            queryExtension = @"FirstName LIKE '%"+@firstName +"'AND LastName LIKE '%"+@lastName +"'AND Email LIKE '%"+@email+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new { firstName, lastName, email });
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else if (firstName != null && lastName != null)
        {
            queryExtension = @"users.FirstName LIKE '%"+@firstName +"'AND users.LastName LIKE '%"+@lastName+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new { firstName, lastName});
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else if (firstName != null && email != null)
        {
            queryExtension = @"users.FirstName LIKE '%"+@firstName +"' AND users.Email LIKE '%"+@email+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new { firstName, email });
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else if (lastName != null && email != null)
        {
            queryExtension = @"users.LastName LIKE '%"+@lastName +"' AND users.Email LIKE '%"+@email+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new { lastName, email });
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else if(firstName != null)
        {
            queryExtension = @"users.FirstName LIKE '%"+@firstName+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new { firstName});
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else if (lastName != null)
        {
            queryExtension = @"users.LastName LIKE '%"+@lastName+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new { lastName});
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else if (email != null)
        {
            queryExtension = @" users.Email LIKE '%"+@email+"'";
            queryString = selectQuary + queryExtension;
            Console.WriteLine(queryString);
            userFromDb = _dataContext.LoadData<Users>(queryString, new {email });
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
        }
        else
        {
            return mapper.Map<IEnumerable<Users>, IEnumerable<UsersDto>>(userFromDb);
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

    [HttpPost("[action]")]
    public int SaveUser(UsersDto user)
    {
        string sql = @"INSERT INTO SCOTT.USERS (
                    FirstName, 
                    LastName,
                    Email,
                    Gender,
                    Active
                ) VALUES (
                    @FirstName, 
                    @LastName,
                    @Email,
                    @Gender,
                    @Active
                    )
                    ";
        if (ModelState.IsValid)
        {

            return _dataContext.ExecuteSqlWithRowCount(sql, user);
        }
        else
        {
            return 0;
        }
    }

    [HttpPut("[action]")]
    public int UpdateUser(int userId,UsersDto user)
    {
        string sql = @"UPDATE SCOTT.USERS
                       SET 
                        FirstName='" + user.FirstName + "'," +
                        "LastName='" + user.LastName + "'," +
                        "Email = '"+user.Email + "'," +
                        "Gender ='"+user.Gender + "'," +
                        "Active ='"+user.Active+"' WHERE USERID="+userId;

        string existingUserQuary = @"SELECT * FROM SCOTT.USERS WHERE USERID=" + userId;


        if (ModelState.IsValid)
        {
            var result =_dataContext.ExecuteSqlWithRowCount(sql,null);
            return result;
        }
        else
        {
            return 0;
        }
    }

    [HttpDelete("[action]")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"DELETE SCOTT.USERS WHERE USERID="+userId;
        if(_dataContext.ExecuteSql(sql, null))
        {
            return Ok();
        }
        throw new Exception("There is a problem");
    }

}
