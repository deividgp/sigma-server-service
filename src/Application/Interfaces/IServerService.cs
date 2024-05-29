namespace Application.Interfaces;

public interface IServerService
{
    public Task<Server?> GetServer(Guid serverId);
    public Task<Server?> GetServer(string serverName);
    public Task<Server?> CreateServer(ServerCreateDTO serverCreate);
    public Task<bool> DeleteServer(Guid serverId);
    public Task<Member> AddMember(MemberAddDTO memberAdd);
    public Task RemoveMember(MemberRemoveDTO memberDelete);
}
