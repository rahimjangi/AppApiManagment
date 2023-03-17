﻿using AppApi.Data;
using AppApi.Dto;
using AppApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly IConfiguration _config;

    public PostController(IConfiguration config)
    {
        _config = config;
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("[action]")]
    public IEnumerable<Post> GetAllPosts()
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


        return _dapper.LoadData<Post>(sqlGetAllPosts, null);
    }


    [HttpGet("[action]/{postId}")]
    public Post GetPostSingle(int postId)
    {
        string sqlGetPost = @"
                SELECT 
                    [PostId]
                    ,[UserId]
                    ,[PostTitle]
                    ,[PostContent]
                    ,[PostCreate]
                    ,[PostUpdated]
                FROM [TCI].[scott].[POSTS]
                    WHERE [PostId] = "+postId.ToString();


        return _dapper.LoadDataSingle<Post>(sqlGetPost, null);
    }


    [HttpGet("[action]/{userId}")]
    public IEnumerable<Post> GetPostsByUser(int userId)
    {
        string sqlGetPost = @"
                SELECT 
                    [PostId]
                    ,[UserId]
                    ,[PostTitle]
                    ,[PostContent]
                    ,[PostCreate]
                    ,[PostUpdated]
                FROM [TCI].[scott].[POSTS]
                    WHERE [UserId] = " + userId.ToString();


        return _dapper.LoadData<Post>(sqlGetPost, null);
    }

    [HttpGet("[action]")]
    public IEnumerable<Post> MyPosts()
    {
        string sqlGetPost = @"
                SELECT 
                    [PostId]
                    ,[UserId]
                    ,[PostTitle]
                    ,[PostContent]
                    ,[PostCreate]
                    ,[PostUpdated]
                FROM [TCI].[scott].[POSTS]
                    WHERE [UserId] = " + User.FindFirst("userId")?.Value;


        return _dapper.LoadData<Post>(sqlGetPost, null);
    }

    [HttpPost("[action]")]
    public IActionResult PostToAdd(PostToAddDto postToAddDto)
    {
        int logedUserId = Convert.ToInt32(User.FindFirst("userId")?.Value);
        string sqlPostToAdd = @"
                  INSERT into scott.posts (
                       [UserId]
                      ,[PostTitle]
                      ,[PostContent]
                      ,[PostCreate]
                  ) values (
                       @UserId
                      ,@PostTitle
                      ,@PostContent
                      ,@PostCreate
                  )";
        _dapper.ExecuteSql(sqlPostToAdd, new{
            UserId=logedUserId,
            PostTitle=postToAddDto.PostTitle,
            PostContent=postToAddDto.PostContent,
            PostCreate=DateTime.Now,});

        return Ok();
    }


    [HttpPost("[action]")]
    public IActionResult PostToEdit(PostToEditDto postToEdit)
    {
        string sqlPostToEdit = @"
                    UPDATE SCOTT.POSTS
                        SET PostTitle=@PostTitle , PostContent=@PostContent , PostUpdated ='"+DateTime.Now.ToString()
                        +"' WHERE [PostId] =" + postToEdit.PostId.ToString();
        _dapper.ExecuteSql(sqlPostToEdit, new
        {
            PostTitle = postToEdit.PostTitle,
            PostContent = postToEdit.PostContent,
        });

        return Ok();
    }

}
