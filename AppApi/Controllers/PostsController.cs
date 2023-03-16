using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{


    [HttpGet("[action]")]
    public IActionResult GetAllPosts()
    {
        string sqlGetAllPosts = @"
                SELECT 
                   [PostId]
                  ,[UserId]
                  ,[PostTitle]
                  ,[PostContent]
                  ,[PostCreate]
                  ,[PostUpdated]
              FROM [TCI].[scott].[POSTS]
            ";
        return Ok();
    }


}
