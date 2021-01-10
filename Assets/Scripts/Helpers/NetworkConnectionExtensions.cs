using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class NetworkConnectionExtensions
{
    public static uint? GetId(this NetworkConnection conn)
    {
        var validController = conn.playerControllers.FirstOrDefault(controller => controller.IsValid);
        if (validController == null) return null;

        return validController.gameObject.GetComponent<NetworkIdentity>().netId.Value;
    }
}
