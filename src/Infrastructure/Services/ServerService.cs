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

    public async Task<Server?> GetServer(string serverName)
    {
        return await _serverRepository.GetFirstAsync(s => s.Name == serverName);
    }

    public async Task RemoveMember(MemberRemoveDTO memberDelete)
    {
        await _serverRepository.UpdateOneAsync(
            c => c.Id == memberDelete.ServerId,
            Builders<Server>.Update.PullFilter(c => c.Members, c => c.Id == memberDelete.MemberId)
        );
    }
}
