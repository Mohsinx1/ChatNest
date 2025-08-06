using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    public async Task SendSignal(object data)
    {
        await Clients.Others.SendAsync("ReceiveSignal", data);
    }
}
