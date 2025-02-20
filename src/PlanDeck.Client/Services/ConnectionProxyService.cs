using Grpc.Net.Client;
using PlanDeck.Contracts.Room.State;
using System.Threading.Channels;
using PlanDeck.Contracts.Room;
using ProtoBuf.Grpc.Client;
using PlanDeck.Contracts.Dtos;

namespace PlanDeck.Client.Services;

/// <summary>
/// Service for receiving messages from the server.
/// </summary>
public interface IConnectionProxyService : IAsyncDisposable
{
    event Action<ServerStreamMessage> OnServerMessage;
    bool Connect(UserDto user);
}

/// <summary>
/// Service for receiving messages from the server.
/// </summary>
public class ConnectionProxyService : IConnectionProxyService
{
    private readonly GrpcChannel channel;
    private IGrpcRoomService roomStreamService;
    private Task listeningTask;
    private bool connected;
    private UserDto currentUser;
    private PeriodicTimer pingTimer;
    private CancellationTokenSource pingCts;

    /// <summary>
    /// Event triggered for each message from the server
    /// </summary>
    public event Action<ServerStreamMessage> OnServerMessage;

    public ConnectionProxyService(GrpcChannel channel)
    {
        UnboundedChannelOptions options = new()
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = true,
        };

        this.channel = channel;
    }

    /// <summary>
    /// Establishes a bidirectional gRPC connection with the server
    /// and starts listening for messages from the server.
    /// </summary>
    public bool Connect(UserDto user)
    {
        if (connected) return true; // already connected

        // Create client
        roomStreamService = channel.CreateGrpcService<IGrpcRoomService>();

        currentUser = user;
        SubscribeRequest connectionRequest = new()
        {
            RoomId = currentUser.LastPlanningRoom!,
            User = currentUser
        };
        var serverStream = roomStreamService.Connect(connectionRequest);

        // Start listening in an asynchronous loop
        listeningTask = ListenToServerAsync(serverStream);

        connected = true;

        StartPinging();
        return connected;
    }

    /// <summary>
    /// Listens for messages in a loop and triggers the event.
    /// </summary>
    private async Task ListenToServerAsync(IAsyncEnumerable<ServerStreamMessage> serverStream)
    {
        try
        {
            await foreach (var msg in serverStream)
            {
                // Trigger event or handling logic
                OnServerMessage?.Invoke(msg);
            }
        }
        catch (Exception ex)
        {
            // Error reading stream
            OnServerMessage?.Invoke(new ServerStreamMessage
            {
                EventType = RoomEventTypes.USER_LEFT,
                ErrorMessage = $"Stream error: {ex.Message}"
            });
        }
    }

    private void StartPinging()
    {
        // Create Timer every x seconds
        pingTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        pingCts = new CancellationTokenSource();

        Task.Run(async () =>
        {
            while (await pingTimer.WaitForNextTickAsync(pingCts.Token))
            {
            }
        });
    }

    /// <summary>
    /// Closes the stream (client-side).
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            // Disable PING timer
            pingCts?.Cancel();
            pingTimer?.Dispose();
        }
        catch { /* Ignore */ }

        if (listeningTask != null)
        {
            await listeningTask;
        }
    }
}
