using RevOnRental.SignalR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.SignalR.Implementations
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private static Dictionary<int, HashSet<string>> userConnectionMap = new Dictionary<int, HashSet<string>>();
        private static string userConnectionMapLocker = string.Empty;

        public void KeepUserConnection(int userId, string connectionId)
        {
            lock (userConnectionMapLocker)
            {
                if (!userConnectionMap.ContainsKey(userId))
                {
                    userConnectionMap[userId] = new HashSet<string>();
                }
                userConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            //Remove the connectionId of user
            lock (userConnectionMapLocker)
            {
                foreach (var userId in userConnectionMap.Keys)
                {
                    if (userConnectionMap.ContainsKey(userId))
                    {
                        if (userConnectionMap[userId].Contains(connectionId))
                        {
                            userConnectionMap[userId].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }

        public HashSet<string> GetUserConnections(int userId)
        {
            var conn = new HashSet<string>();
            lock (userConnectionMapLocker)
            {
                conn = userConnectionMap.TryGetValue(userId, out conn) ? userConnectionMap[userId] : new HashSet<string>();
            }
            return conn;
        }

        public IEnumerable<int> OnlineUsers { get { return userConnectionMap.Keys; } }
    }
}
