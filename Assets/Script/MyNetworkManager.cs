using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public Transform[] spawnPoints;
    // we are not including spawn points on layer 2 because layer triggers are added on stairs. to be able to add spawn points on upper layers we either need layer trigger on that location or manually adjust player layer. both are feasable but later.

    void onStart() { }

    public override void OnStartServer()
    {
        print("server started");
    }

    public void onStopServer()
    {
        print("server ended");
    }

    public override void OnClientConnect()
    {
        /*
        this must be used to add prefabs that need to be controlled and synced.
        other way is to add them directly in inspector spwnPrefabs list
        These can include player characters, enemies, pickups, projectiles, or any other interactive objects that need to be synchronized across the network.
        */
        NetworkClient.RegisterPrefab(serverPrefab);
        NetworkClient.RegisterPrefab(clientPrefab);
        print("Client connected");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // print(spawnPrefabs);
        // print(spawnPrefabs[0].name);
        print("isHost " + (NetworkServer.active && NetworkClient.active));
        print("isHost " + (NetworkClient.activeHost));
        print("isHost " + (conn.identity == null));
        print("isHost " + (conn.connectionId == 0));
        print("connectionId " + conn.connectionId);
        GameObject playerPrefab = conn.connectionId == 0 ? serverPrefab : clientPrefab;
        if (playerPrefab != null)
        {
            // Get a random spawn point index
            int randomIndex = Random.Range(0, spawnPoints.Length - 1);
            Vector3 spawnPosition = Vector3.zero; // Set a default spawn position
            if (conn.connectionId != 0 && spawnPoints[randomIndex] != null)
            {
                spawnPosition = spawnPoints[randomIndex].position; // Use the spawn point position
            }
            print("spawn location: " + spawnPosition);
            GameObject playerObject = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            // NetworkServer.Spawn(playerObject, conn);
            NetworkServer.AddPlayerForConnection(conn, playerObject);
            // Assign client authority to the player object
            NetworkIdentity identity = playerObject.GetComponent<NetworkIdentity>();
            identity.AssignClientAuthority(conn);
        }
    }

    public void OnClientDisconnect(NetworkConnection conn)
    {
        print("client disconnected");
    }
}
