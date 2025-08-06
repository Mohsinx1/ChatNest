using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private static Dictionary<string, string> userMap = new(); // connectionId -> username

    public override Task OnConnectedAsync()
    {
        var username = Context.User.Identity.Name ?? Context.ConnectionId;
        userMap[Context.ConnectionId] = username;
        Clients.Others.SendAsync("UserJoined", username);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (userMap.TryGetValue(Context.ConnectionId, out var username))
        {
            Clients.Others.SendAsync("UserLeft", username);
            userMap.Remove(Context.ConnectionId);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendSignal(string targetUsername, object data)
    {
        var targetConn = userMap.FirstOrDefault(x => x.Value == targetUsername).Key;
        var fromUsername = userMap[Context.ConnectionId];
        if (targetConn != null)
        {
            await Clients.Client(targetConn).SendAsync("ReceiveSignal", fromUsername, data);
        }
    }

    public async Task RequestUserList()
    {
        var fromUsername = userMap[Context.ConnectionId];
        var users = userMap.Values.Where(u => u != fromUsername).ToList();
        await Clients.Caller.SendAsync("UserList", users);
    }
}
