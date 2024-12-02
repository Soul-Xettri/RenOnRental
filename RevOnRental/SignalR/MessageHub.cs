using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RevOnRental.Domain.Constants;
using RevOnRental.SignalR.Interfaces;

namespace RevOnRental.SignalR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub
        {
            private readonly IUserConnectionManager _userConnectionManager;

            public MessageHub(IUserConnectionManager userConnectionManager)
            {
                _userConnectionManager = userConnectionManager;
            }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// Connect and Keep user in dictionary with signalId
        /// </summary>
        public override async Task OnConnectedAsync()
            {
            try
            {
                await Clients.All.SendAsync("onConnectedAsync", $"{Context.ConnectionId}");
                foreach (var claim in Context.User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }
                var test = Context.User.Claims.FirstOrDefault();
                //await base.OnConnectedAsync();

                // int userId = int.Parse(Context.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.JwtId).Value);
                // _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);
                //await Clients.All.SendAsync("onConnectedAsync", $"{Context.ConnectionId}");
                //await Clients.All.SendAsync("onUserActive", userId);
            }
            catch (Exception ex)
            {

                throw;
            }

            
            }

            /// <summary>
            /// Called when a connection with the hub is terminated.
            /// </summary>
            public override async Task OnDisconnectedAsync(Exception exception)
            {
                //Guid userId = Guid.Parse(Context.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.JwtId).Value);
                ////get the connectionId
                //var connectionId = Context.ConnectionId;
                //_userConnectionManager.RemoveUserConnection(connectionId);
                //var value = await Task.FromResult(0);
                //await Clients.All.SendAsync("onUserInActive", userId);
            }

            /// <summary>
            /// get all online user
            /// </summary>
            /// <returns></returns>
            public async Task GetOnlineUsers()
            {
            var userList = _userConnectionManager.OnlineUsers;
            await Clients.All.SendAsync("OnlineUsers", userList);
        }
        }
    }

