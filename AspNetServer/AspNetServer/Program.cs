using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseHttpsRedirection();

app.UseWebSockets();

app.MapGet("/", async (context) =>
{
   if (!context.WebSockets.IsWebSocketRequest)
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
	else
	{
		while (true)
		{
			using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
			var data = Encoding.ASCII.GetBytes($" Data atual: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

			await webSocket.SendAsync(
				data,
				WebSocketMessageType.Text,
				true,
				CancellationToken.None);

			await Task.Delay(1000);
		}
	}
});

await app.RunAsync();