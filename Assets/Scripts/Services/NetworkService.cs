using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class NetworkService
{
    public static bool IsServer()
    {
        return NetworkServer.active;
    }

    public static bool IsOffline()
    {
        return !NetworkClient.active
            && !NetworkServer.active;
    }
}
