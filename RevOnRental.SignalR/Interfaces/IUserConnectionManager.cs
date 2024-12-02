using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.SignalR.Interfaces
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(int userId, string connectionId);

        void RemoveUserConnection(string connectionId);

        HashSet<string> GetUserConnections(int userId);

        IEnumerable<int> OnlineUsers { get; }
    }
}
