CREATE TABLE AspNetUsers 
( 
Id NVARCHAR(450) PRIMARY KEY,
 UserName NVARCHAR(256) NULL,
 NormalizedUserName NVARCHAR(256) NULL, 
 Email NVARCHAR(256) NULL,
 NormalizedEmail NVARCHAR(256) NULL,
 EmailConfirmed BIT NOT NULL,
 PasswordHash NVARCHAR(MAX) NULL, 
 SecurityStamp NVARCHAR(MAX) NULL, 
 ConcurrencyStamp NVARCHAR(MAX) NULL, 
 PhoneNumber NVARCHAR(MAX) NULL, 
 PhoneNumberConfirmed BIT NOT NULL, 
 TwoFactorEnabled BIT NOT NULL, 
 LockoutEnd DATETIMEOFFSET NULL, 
 LockoutEnabled BIT NOT NULL, 
 AccessFailedCount INT NOT NULL 
 ); 
 -------------------------------------------------------------------------------------------------------------
 CREATE TABLE AspNetRoles 
 ( 
 Id NVARCHAR(450) PRIMARY KEY,
 Name NVARCHAR(256) NULL, 
 NormalizedName NVARCHAR(256) NULL, 
 ConcurrencyStamp NVARCHAR(MAX) NULL 
 ); 
 -------------------------------------------------------------------------------------------------------------
 CREATE TABLE AspNetUserRoles 
 ( 
 UserId NVARCHAR(450) NOT NULL, 
 RoleId NVARCHAR(450) NOT NULL, 
 PRIMARY KEY (UserId, RoleId), 
 FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id), 
 FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) 
 ); 
 -------------------------------------------------------------------------------------------------------------
 CREATE TABLE AspNetUserClaims 
 ( 
 Id INT IDENTITY(1,1) PRIMARY KEY, 
 UserId NVARCHAR(450) NOT NULL, 
 ClaimType NVARCHAR(MAX) NULL, 
 ClaimValue NVARCHAR(MAX) NULL, 
 FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) 
 ); 
 -------------------------------------------------------------------------------------------------------------
 CREATE TABLE AspNetRoleClaims 
 ( 
 Id INT IDENTITY(1,1) PRIMARY KEY, 
 RoleId NVARCHAR(450) NOT NULL, 
 ClaimType NVARCHAR(MAX) NULL, 
 ClaimValue NVARCHAR(MAX) NULL, 
 FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) 
 ); 
 -------------------------------------------------------------------------------------------------------------
 CREATE TABLE AspNetUserLogins 
 ( 
 LoginProvider NVARCHAR(450) NOT NULL, 
 ProviderKey NVARCHAR(450) NOT NULL, 
 ProviderDisplayName NVARCHAR(MAX) NULL, 
 UserId NVARCHAR(450) NOT NULL, PRIMARY KEY (LoginProvider, ProviderKey), 
 FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) 
 ); 
 -------------------------------------------------------------------------------------------------------------
 CREATE TABLE AspNetUserTokens 
 ( 
 UserId NVARCHAR(450) NOT NULL, 
 LoginProvider NVARCHAR(450) NOT NULL, 
 Name NVARCHAR(450) NOT NULL, 
 Value NVARCHAR(MAX) NULL, PRIMARY KEY (UserId, LoginProvider, Name), 
 FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) 
 );
-------------------------------------------------------------------------------------------------------------
create table Department
(
Id int not null Primary Key Identity,
Name nvarchar(255) not null
)
-------------------------------------------------------------------------------------------------------------
create table Employee 
(
Id int Not NUll Identity Primary Key,
AspNetUserId nvarchar(450) not null Foreign Key References AspNetUsers(Id),
DepartmentId int not null Foreign Key References Department(Id),
Name nvarchar(255) not null,
Address nvarchar(max) null,
PhoneNumber nvarchar(255) null,
CreatedOn DateTime not null,
LastLoggedIn DateTime not null
)

-------------------------------------------------------------------------------------------------------------
create table Vacation
(
Id int not null Primary Key Identity,
EmployeeId int not null Foreign Key References Employee(Id),
NumberOfDays int not null
)
-------------------------------------------------------------------------------------------------------------
create table Payroll(
Id int not null Primary Key Identity,
AspNetUserId nvarchar(450) not null,
HoursWorked DECIMAL not null DEFAULT 0,
HourlyRate DECIMAL not null DEFAULT 0,
WorkedDate DateTime not null
)

-------------------------------------------------------------------------------------------------------------


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE sp_DeleteUser
	@userId nvarchar(450)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @EmployeeId int;

	SELECT @EmployeeId=Id FROM Employee
	WHERE AspNetUserId=@userId;

	DELETE FROM Vacation
	WHERE EmployeeId=@EmployeeId;

	DELETE FROM Payroll
	WHERE AspNetUserId=@userId;

	DELETE FROM AspNetUserRoles
	WHERE UserId=@userId;

	DELETE FROM Employee
	WHERE AspNetUserId=@userId;

	DELETE FROM AspNetUsers
	WHERE Id=@userId;
END
GO
