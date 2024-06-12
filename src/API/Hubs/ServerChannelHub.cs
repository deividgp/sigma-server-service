namespace API.Hubs;

public class ServerChannelHub(
    IChannelService channelService,
    IServerService serverService,
    IConfiguration config
) : Hub
{
    private readonly IChannelService _channelService = channelService;
    private readonly IServerService _serverService = serverService;
    private readonly IConfiguration _config = config;

    // Server
    public async Task<bool> SendCreateServer(ServerCreateDTO serverCreate)
    {
        try
        {
            Server? server = await _serverService.CreateServer(serverCreate);

            if (server is null)
                return false;

            await Clients.Caller.SendAsync(
                "ReceiveCreateServer",
                new PartialServer()
                {
                    UserId = serverCreate.OwnerId,
                    Id = server.Id,
                    Name = server.Name,
                    Icon = server.Icon
                }
            );

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendAddMember(SendAddMemberDTO sendAddMember)
    {
        try
        {
            Server? server = await _serverService.GetServer(
                sendAddMember.ServerName,
                sendAddMember.ServerPassword
            );

            if (server is null)
                return false;

            await _serverService.AddMember(
                new()
                {
                    ServerId = server.Id,
                    UserId = sendAddMember.UserId,
                    Username = sendAddMember.Username,
                }
            );
            await Clients.Caller.SendAsync(
                "ReceiveAddMember",
                new PartialServer()
                {
                    UserId = sendAddMember.UserId,
                    Id = server.Id,
                    Name = server.Name,
                    Icon = server.Icon
                }
            );

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendDeleteServer(Guid serverId)
    {
        try
        {
            Server? server = await _serverService.GetServer(serverId);

            if (server is null)
                return false;

            bool result = await _serverService.DeleteServer(serverId);

            if (!result)
                return false;

            await Clients.Caller.SendAsync("ReceiveServerDelete", serverId);

            HttpClient httpClient = new();
            var response = await httpClient.PatchAsJsonAsync(
                _config.GetValue<string>("User:Url")! + "RemoveFromServer",
                new ServerRemoveRequestDTO()
                {
                    ServerId = serverId,
                    UserIds = server.Members.Select(m => m.Id).ToList()
                }
            );

            response.EnsureSuccessStatusCode();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendRemoveMember(MemberRemoveDTO memberRemove)
    {
        try
        {
            await _serverService.RemoveMember(memberRemove);

            await Clients
                .User(memberRemove.MemberId.ToString())
                .SendAsync("ReceiveServerDelete", memberRemove.ServerId);
            await Clients.Caller.SendAsync("ReceiveMemberRemove", memberRemove);

            HttpClient httpClient = new();
            var response = await httpClient.PatchAsJsonAsync(
                _config.GetValue<string>("User:Url")! + "RemoveFromServer",
                new ServerRemoveRequestDTO()
                {
                    ServerId = memberRemove.ServerId,
                    UserIds = [memberRemove.MemberId]
                }
            );

            response.EnsureSuccessStatusCode();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendCreateRole(RoleCreateDTO roleCreate)
    {
        try
        {
            Role? role = await _serverService.CreateRole(roleCreate);

            if (role is null)
                return false;

            await Clients.Caller.SendAsync("ReceiveCreateRole", role);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendDeleteRole(RoleDeleteDTO roleDelete)
    {
        try
        {
            bool result = await _serverService.DeleteRole(roleDelete);

            if (!result)
                return false;

            await Clients.Caller.SendAsync("ReceiveDeleteRole", roleDelete.RoleId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Channel
    public async Task<bool> JoinChannel(string channelId)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LeaveChannel(string channelId)
    {
        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendMessage(MessageCreateDTO messageCreate)
    {
        try
        {
            Message message = await _channelService.AddMessage(messageCreate);
            await Clients
                .Group(messageCreate.ChannelId.ToString())
                .SendAsync("ReceiveChannelMessage", message);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendCreateChannel(ChannelCreateDTO channelCreate)
    {
        try
        {
            Channel channel = await _channelService.AddChannel(channelCreate);
            await Clients.Caller.SendAsync(
                "ReceiveChannelCreate",
                new PartialChannel() { Id = channel.Id, Name = channel.Name }
            );
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendDeleteChannel(ChannelDeleteDTO channelDelete)
    {
        try
        {
            await _channelService.DeleteChannel(channelDelete);
            await Clients.Caller.SendAsync("ReceiveChannelDelete", channelDelete);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
