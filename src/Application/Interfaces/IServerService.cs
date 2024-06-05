namespace Application.Interfaces;

public interface IServerService
{
    public Task<Server?> GetServer(Guid serverId);
    public Task<Server?> GetServer(string serverName, string serverPassword);
    public Task<Server?> CreateServer(ServerCreateDTO serverCreate);
    public Task<bool> DeleteServer(Guid serverId);
    public Task<Member> AddMember(MemberAddDTO memberAdd);
    public Task RemoveMember(MemberRemoveDTO memberDelete);
    public Task<Role?> CreateRole(RoleCreateDTO roleCreate);
    public Task<bool> DeleteRole(RoleDeleteDTO roleDelete);
}
