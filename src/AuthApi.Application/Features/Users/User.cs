﻿using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;
using AuthApi.Application.DatabaseContext;

namespace AuthApi.Application.Features.Users;

public sealed class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Role { get; private set; }

    private User(Guid id, string name, string email, string password, string role)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public static Result<User> Create(string name, string email, string password, string role)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<User>(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(AuthApi_Resource.EMAIL_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure<User>(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        if (string.IsNullOrEmpty(role))
        {
            return Result.Failure<User>(AuthApi_Resource.ROLE_REQUIRED);
        }

        return Result.Success(new User(Guid.NewGuid(), name, email, password, role));
    }
}