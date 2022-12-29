﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using NetHub.Application.Features.Public.Users.Dto;
using NetHub.Application.Tools;
using NetHub.Data.SqlServer.Entities.Identity;

namespace NetHub.Application.Features.Public.Users.Info;

internal sealed class GetUsersInfoHandler : DbHandler<GetUsersInfoRequest, UserDto[]>
{
	public GetUsersInfoHandler(IServiceProvider serviceProvider) : base(serviceProvider)
	{
	}


	public override async Task<UserDto[]> Handle(GetUsersInfoRequest request,
		CancellationToken ct)
	{
		var users = await Database.Set<AppUser>()
			.Where(u => request.UserNames
				.Select(u => u.ToUpper())
				.Contains(u.NormalizedUserName))
			.ToArrayAsync(ct);

		return users.Select(u => u.Adapt<UserDto>()).ToArray();
	}
}