using Domain.Entities;

namespace Infrastructure.Services;

public class ServerService(IRepository<Server, Guid> serverRepository) : IServerService
{
    private readonly IRepository<Server, Guid> _serverRepository = serverRepository;

    public async Task<Server?> CreateServer(ServerCreateDTO serverCreate)
    {
        Member member = new() { Id = serverCreate.OwnerId, Username = serverCreate.OwnerUsername };

        Server server =
            new()
            {
                Id = Guid.NewGuid(),
                Name = serverCreate.ServerName,
                Password = BCrypt.Net.BCrypt.HashPassword(serverCreate.ServerPassword),
                OwnerId = serverCreate.OwnerId,
                Members = [member]
            };

        try
        {
            await _serverRepository.CreateAsync(server);
            return server;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Member> AddMember(MemberAddDTO memberAdd)
    {
        Member member = new() { Id = memberAdd.UserId, Username = memberAdd.Username };
        await _serverRepository.UpdateOneAsync(
            c => c.Id == memberAdd.ServerId,
            Builders<Server>.Update.Push(c => c.Members, member)
        );

        return member;
    }

    public async Task<bool> DeleteServer(Guid serverId)
    {
        try
        {
            await _serverRepository.RemoveAsync(serverId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Server?> GetServer(Guid serverId)
    {
        return await _serverRepository.GetByIdAsync(serverId);
    }

    public async Task<Server?> GetServer(string serverName, string? serverPassword)
    {
        Server? server = await _serverRepository.GetFirstAsync(s => s.Name == serverName);

        if (server is null)
            return null;

        if (server.Password is not null && serverPassword is null)
            return null;

        if (server.Password is null && (serverPassword is not null || serverPassword is null))
            return server;

        if (
            BCrypt.Net.BCrypt.Verify(
                serverPassword,
                server.Password,
                false,
                BCrypt.Net.HashType.SHA384
            )
        )
            return server;

        return null;
    }

    public async Task RemoveMember(MemberRemoveDTO memberDelete)
    {
        await _serverRepository.UpdateOneAsync(
            c => c.Id == memberDelete.ServerId,
            Builders<Server>.Update.PullFilter(c => c.Members, c => c.Id == memberDelete.MemberId)
        );
    }

    public async Task<Role?> CreateRole(RoleCreateDTO roleCreate)
    {
        Role role = new() { Id = Guid.NewGuid(), Name = roleCreate.RoleName };

        await _serverRepository.UpdateOneAsync(
            c => c.Id == roleCreate.ServerId,
            Builders<Server>.Update.Push(c => c.Roles, role)
        );

        return role;
    }

    public async Task<bool> DeleteRole(RoleDeleteDTO roleDelete)
    {
        try
        {
            await _serverRepository.UpdateOneAsync(
                c => c.Id == roleDelete.ServerId,
                Builders<Server>.Update.PullFilter(c => c.Roles, c => c.Id == roleDelete.RoleId)
            );

            Server? server = await _serverRepository.GetByIdAsync(roleDelete.ServerId);

            for (int i = 0; i < server!.Members.Count; i++)
            {
                await _serverRepository.UpdateOneAsync(
                    c => c.Id == roleDelete.ServerId,
                    Builders<Server>.Update.PullFilter(
                        c => c.Members[i].Roles,
                        c => c.Id == roleDelete.RoleId
                    )
                );
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
