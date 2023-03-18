using AppApi.Data;
using AppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserCompleteController : ControllerBase
{
    private readonly DataContextDapper _dataContext;
    private readonly IConfiguration _config;
    public UserCompleteController(IConfiguration config)
    {
        _config = config;
        _dataContext = new DataContextDapper(_config);
    }

    [HttpGet("[action]/{userId}/{isActive}")]
    public IEnumerable<UserComplete> GetUsers(int userId,bool isActive)
    {
        string sql = @"EXEC [scott].[spUsers_Get]";
        string parameters = "";
        if (userId != 0)
        {
            parameters += ", @UserId=" + userId.ToString();
        }
        if (isActive)
        {
            parameters += ", @Active=" + isActive.ToString();
        }
        sql += parameters.Substring(1);

        return _dataContext.LoadData<UserComplete>(sql,null);
    }

    [HttpPut("[action]")]
    public IActionResult UpsertUser(UserComplete userComplete)
    {
        string sql = @"EXEC [scott].[spUser_Upsert]
                @FirstName = '" + userComplete.FirstName + "'," +
                "@LastName = '" + userComplete.LastName + "'," +
                "@Email = '" + userComplete.Email + "'," +
                "@Gender = '" + userComplete.Gender + "'," +
                "@Active = '" + userComplete.Active + "'," +
                "@JobTitle = '" + userComplete.JobTitle + "'," +
                "@Department = '" + userComplete.Department + "'," +
                "@Salary = '" + userComplete.Salary + "'," +
                "@UserId = '" + userComplete.UserId + "'";
        if(_dataContext.ExecuteSql(sql,null))
        {
            return Ok();
        }
        return BadRequest("Not able to complete the task!");
    }
}
