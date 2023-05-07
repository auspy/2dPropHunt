using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        print("server started");
    }

    public void onStopServer()
    {
        print("server ended");
    }

    public void OnClientConnect(NetworkConnection conn)
    {
        print("client connected");
    }

    public void OnClientDisconnect(NetworkConnection conn)
    {
        print("client disconnected");
    }
}
