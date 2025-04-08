using Contarcts.Common.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;


namespace BusinessService.Infrastructure.Notifications
{
    public class NotHub : Hub
    {
        private readonly ILogger<NotHub> _logger;

        public NotHub(ILogger<NotHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var drId = httpContext.Request.Query["drId"].ToString();
                    if (!string.IsNullOrEmpty(drId))
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, drId);
                        _logger.LogInformation($"Doctor {drId} connected: {Context.ConnectionId}");
                    }

                    var userId = httpContext.Request.Query["userId"].ToString();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                        _logger.LogInformation($"User {userId} connected: {Context.ConnectionId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnConnectedAsync: {ex.Message}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var drId = httpContext.Request.Query["drId"].ToString();
                    if (!string.IsNullOrEmpty(drId))
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, drId);
                        _logger.LogInformation($"Doctor {drId} disconnected: {Context.ConnectionId}");
                    }

                    var userId = httpContext.Request.Query["userId"].ToString();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                        _logger.LogInformation($"User {userId} disconnected: {Context.ConnectionId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OnDisconnectedAsync: {ex.Message}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToDoctor(string drId, NotificationResDto notification)
        {
            if (string.IsNullOrEmpty(drId))
            {
                _logger.LogWarning("SendNotificationToDoctor: drId is null or empty.");
                return;
            }

            try
            {
                await Clients.Group(drId).SendAsync("receivenotificationdr", notification);
                _logger.LogInformation($"Notification sent to doctor {drId}: {notification.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending notification to doctor {drId}: {ex.Message}");
            }
        }

        public async Task SendNotificationToPatient(string userId, NotificationResDto notification)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("SendNotificationToPatient: userId is null or empty.");
                return;
            }

            try
            {
                await Clients.Group(userId).SendAsync("receivenotificationpatient", notification);
                _logger.LogInformation($"Notification sent to doctor {userId}: {notification.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending notification to doctor {userId}: {ex.Message}");
            }
        }
    }
}
