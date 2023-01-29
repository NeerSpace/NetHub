﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetHub.Shared.Api;
using NetHub.Shared.Api.Abstractions;
using NetHub.Data.SqlServer.Context;
using NetHub.Data.SqlServer.Entities.Identity;
using NetHub.Models.Me;
using NetHub.Shared.Api.Constants;

namespace NetHub.Api.Endpoints.Users;

[Tags(TagNames.Users)]
[ApiVersion(Versions.V1)]
public sealed class UserCheckUsernameEndpoint : Endpoint<string, CheckUsernameResult>
{
    private readonly ISqlServerDatabase _database;

    public UserCheckUsernameEndpoint(ISqlServerDatabase database)
    {
        _database = database;
    }


    [HttpGet("users/{username}")]
    public override async Task<CheckUsernameResult> HandleAsync([FromRoute] string username, CancellationToken ct)
    {
        var isExist = await _database.Set<AppUser>().AnyAsync(u => u.UserName == username, ct);

        return new(!isExist);
    }
}