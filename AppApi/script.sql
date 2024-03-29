USE [master]
GO
/****** Object:  Database [TCI]    Script Date: 3/18/2023 10:23:50 PM ******/
CREATE DATABASE [TCI]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TCI', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TCI.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TCI_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TCI_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TCI] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TCI].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TCI] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TCI] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TCI] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TCI] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TCI] SET ARITHABORT OFF 
GO
ALTER DATABASE [TCI] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TCI] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TCI] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TCI] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TCI] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TCI] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TCI] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TCI] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TCI] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TCI] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TCI] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TCI] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TCI] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TCI] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TCI] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TCI] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TCI] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TCI] SET RECOVERY FULL 
GO
ALTER DATABASE [TCI] SET  MULTI_USER 
GO
ALTER DATABASE [TCI] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TCI] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TCI] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TCI] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TCI] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TCI] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'TCI', N'ON'
GO
ALTER DATABASE [TCI] SET QUERY_STORE = OFF
GO
USE [TCI]
GO
/****** Object:  Schema [scott]    Script Date: 3/18/2023 10:23:50 PM ******/
CREATE SCHEMA [scott]
GO
/****** Object:  Table [scott].[Auth]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [scott].[Auth](
	[Email] [nvarchar](50) NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [scott].[Computer]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [scott].[Computer](
	[ComputerId] [int] IDENTITY(1,1) NOT NULL,
	[MotherBoard] [nvarchar](50) NULL,
	[CpuCores] [int] NULL,
	[HasWiFi] [bit] NULL,
	[HasLTE] [bit] NULL,
	[ReleaseDate] [date] NULL,
	[Price] [decimal](18, 4) NULL,
	[VideoCard] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComputerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [scott].[POSTS]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [scott].[POSTS](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PostTitle] [nvarchar](255) NULL,
	[PostContent] [nvarchar](max) NULL,
	[PostCreate] [datetime] NULL,
	[PostUpdated] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [CIX_POSTS_USERID_POSTID]    Script Date: 3/18/2023 10:23:50 PM ******/
CREATE CLUSTERED INDEX [CIX_POSTS_USERID_POSTID] ON [scott].[POSTS]
(
	[UserId] ASC,
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Table [scott].[UserJobInfo]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [scott].[UserJobInfo](
	[UserId] [int] NULL,
	[JobTitle] [nvarchar](100) NULL,
	[Department] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [scott].[USERS]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [scott].[USERS](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Gender] [nvarchar](10) NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [scott].[UserSalary]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [scott].[UserSalary](
	[UserId] [int] NULL,
	[Salary] [decimal](18, 0) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [scott].[spLoginConfirmation_Get]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [scott].[spLoginConfirmation_Get]
    @Email NVARCHAR(50)
AS

BEGIN
    SELECT 
        [auth].[PasswordHash],
        [auth].[PasswordSalt]
    from [scott].[Auth] as auth
    WHERE auth.Email=@Email
    
END
GO
/****** Object:  StoredProcedure [scott].[spPosts_Delete]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [scott].[spPosts_Delete]
    @PostId INT,
    @UserId INT
AS

BEGIN
    DELETE from [scott].[POSTS]
    where PostId = @PostId AND UserId=@UserId
END
GO
/****** Object:  StoredProcedure [scott].[spPosts_Get]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [scott].[spPosts_Get]
    @UserId INT=null,
    @SearchValue NVARCHAR(MAX)=null,
    @PostId INT =NULL
AS

BEGIN
        SELECT
            [posts].[PostId],
            [posts].[UserId],
            [posts].[PostTitle],
            [posts].[PostContent],
            [posts].[PostCreate],
            [posts].[PostUpdated] 
        from scott.POSTS as Posts
            WHERE 
                UserId= ISNULL(@UserId,Posts.UserId)
                AND (
                    @SearchValue IS  NULL OR (
                    [posts].[PostTitle] LIKE '%'+@SearchValue+'%' OR
                    [posts].[PostContent] LIKE '%'+@SearchValue+'%' )
                )
                AND PostId =ISNULL(@PostId,Posts.PostId)
END
GO
/****** Object:  StoredProcedure [scott].[spPosts_UpSert]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE   PROCEDURE [scott].[spPosts_UpSert]
    @UserId INT,
    @PostTitle NVARCHAR(255),
    @PostContent NVARCHAR(MAX),
    @PostId INT=null
 AS

 BEGIN
    IF EXISTS (SELECT PostId from [scott].POSTS where PostId = @PostId)
        BEGIN
            UPDATE [scott].POSTS
            SET 
                PostTitle = @PostTitle,
                PostContent = @PostContent,
                PostUpdated = GETDATE()
            WHERE
                PostId = @PostId
        END
        ELSE
            BEGIN
                INSERT INTO [scott].POSTS (
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreate],
                    [PostUpdated]
                ) VALUES (
                    @UserId,
                    @PostTitle,
                    @PostContent,
                    GETDATE(),
                    GETDATE()
                )
            END
 END
GO
/****** Object:  StoredProcedure [scott].[spRegistration_Upsert]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [scott].[spRegistration_Upsert]
    @Email NVARCHAR(50),
    @PasswordHash varbinary(MAX),
    @PasswordSalt varbinary(MAX)
AS

BEGIN
    IF EXISTS (SELECT * FROM [scott].[Auth] WHERE Email=@Email)
        BEGIN
            UPDATE [scott].[Auth]
            SET 
                Email=@Email,
                PasswordHash=@PasswordHash,
                PasswordSalt=@PasswordSalt
            WHERE Email=@Email

        END
    ELSE
        BEGIN
            INSERT INTO [scott].[Auth] (
                    Email,
                    PasswordHash,
                    PasswordSalt

            ) VALUES (
                    @Email,
                    @PasswordHash,
                    @PasswordSalt
            )
        END
END
GO
/****** Object:  StoredProcedure [scott].[spUser_Upsert]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [scott].[spUser_Upsert]

    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Gender NVARCHAR(10),
    @Active BIT = 1,
    @JobTitle NVARCHAR(50),
    @Department NVARCHAR(50),
    @Salary DECIMAL(18,2),
    @UserId INT=0

AS 
    BEGIN
        IF NOT EXISTS (SELECT * FROM [scott].[USERS] WHERE [users].[UserId] = @UserId)
            BEGIN
                IF NOT EXISTS (SELECT * FROM [scott].[USERS] WHERE [Email] = @Email)
                    BEGIN
                        DECLARE @outputUserId INT

                        INSERT INTO [scott].[USERS] (
                                    [FirstName],
                                    [LastName],
                                    [Email],
                                    [Gender],
                                    [Active]
                        ) VALUES (
                                    @FirstName,
                                    @LastName,
                                    @Email,
                                    @Gender,
                                    @Active
                        )
                        set @outputUserId =@@IDENTITY

                        INSERT INTO [scott].[UserJobInfo] (
                            [UserId],
                            [JobTitle],
                            [Department]
                        ) VALUES (
                            @outputUserId,
                            @JobTitle,
                            @Department
                        )

                        INSERT INTO [scott].[UserSalary] (
                             [UserId],
                             [Salary]
                        ) VALUES (
                            @outputUserId,
                            @Salary
                        )
                    END
            END
        ELSE
            BEGIN
                UPDATE [scott].[USERS] SET
                     [FirstName] = @FirstName,
                     [LastName] = @LastName,
                     [Email] = @Email,
                     [Gender] = @Gender,
                     [Active] = @Active
                WHERE [USERS].[UserId] = @UserId

                UPDATE [scott].[UserSalary] SET
                    [Salary]=@Salary
                WHERE [UserSalary].[UserId] = @UserId

                UPDATE [scott].[UserJobInfo] SET
                    [JobTitle]=@JobTitle,
                    [Department]=@Department
                WHERE [UserJobInfo].[UserId] = @UserId

            END
    END
GO
/****** Object:  StoredProcedure [scott].[spUsers_delete]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [scott].[spUsers_delete]
    @UserId INT
AS

BEGIN
    DELETE from [scott].USERS
    where UserId = @UserId

    DELETE from [scott].[UserSalary]
    WHERE UserId = @UserId

    DELETE from [scott].[UserJobInfo]
    WHERE UserId = @UserId

END
GO
/****** Object:  StoredProcedure [scott].[spUsers_Get]    Script Date: 3/18/2023 10:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE   PROCEDURE [scott].[spUsers_Get]
  -- EXEC SCOTT.spUsers_Get
  @UserId INT = NULL,
  @Active BIT = NULL
  AS
  BEGIN
      drop TABLE if EXISTS #AvgDepSalary

      SELECT 
        userJobInfo.Department,
        AVG(userSalary.Salary) as averageSalary
        into #AvgDepSalary
        from scott.USERS as users
        LEFT JOIN scott.UserSalary as userSalary
          on userSalary.UserId= users.UserId
        LEFT JOIN scott.UserJobInfo as userJobInfo
          on userJobInfo.UserId= users.UserId
        GROUP BY userJobInfo.Department

        CREATE CLUSTERED INDEX cix_AvgDepSalary_department on #AvgDepSalary(Department)

      select 
        [users].[UserId],
        [users].[FirstName],
        [users].[LastName],
        [users].[Email],
        [users].[Gender],
        [users].[Active],
        [Salary].[Salary],
        [userJobInfo].[Department],
        [userJobInfo].[JobTitle],
        [avgSalary].[averageSalary]
      from scott.users as users
      LEFT JOIN [scott].[UserSalary] AS Salary 
        on Salary.UserId=users.UserId
      left join [scott].[UserJobInfo] as userJobInfo
        on userJobInfo.UserId=users.UserId
      left join #AvgDepSalary as avgSalary
        on avgSalary.Department=userJobInfo.Department
        -- OUTER APPLY(
        --         SELECT 
        --           AVG(userSalary2.Salary) as averageSalary
        --           from scott.USERS as users
        --           LEFT JOIN scott.UserSalary as userSalary2
        --             on userSalary2.UserId= users.UserId
        --           LEFT JOIN scott.UserJobInfo as userJobInfo2
        --             on userJobInfo2.UserId= users.UserId
        --           where userJobInfo2.Department=userJobInfo.Department
        --           GROUP BY userJobInfo2.Department
        -- )as AvgSalary
      WHERE [users].[UserId] = ISNULL(@UserId,[users].[UserId])
        AND ISNULL([users].[Active],0) = COALESCE(@Active,[users].[Active],0)
  END
GO
USE [master]
GO
ALTER DATABASE [TCI] SET  READ_WRITE 
GO
