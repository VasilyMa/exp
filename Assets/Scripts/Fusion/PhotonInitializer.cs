using UnityEngine;
using Fusion;
using System.Threading.Tasks;
using MemoryPack;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class PhotonInitializer : MonoBehaviour, INetworkRunnerCallbacks
{
    public static PhotonInitializer Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private NetworkObject runHandlerPrefab; 
    [HideInInspector] public NetworkRunner Runner;
    
    private PhotonRunHandler currentHandler;
    private List<PlayerRef> connectedPlayers = new List<PlayerRef>();
    private SessionParams sessionParams;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (runHandlerPrefab == null)
        {
            Debug.LogError("[PhotonInitializer] RunHandler prefab is not assigned!");
        }
    }
    public async Task StartSession(SessionParams session)
    {
        if (Runner != null && Runner.IsRunning)
            await Runner.Shutdown();

        Runner = gameObject.AddComponent<NetworkRunner>();
        Runner.ProvideInput = session.ProvideInput;

        var sceneManager = Runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        var args = new StartGameArgs
        {
            GameMode = session.Mode,
            SessionName = session.RoomName,
            Scene = SceneRef.FromIndex(session.SceneIndex),
            SceneManager = sceneManager
        };

        sessionParams = session;

        var result = await Runner.StartGame(args);

        if (result.Ok == false)
        {
            Debug.LogError($"[PhotonInitializer] Failed to start game: {result.ShutdownReason}");
            return;
        }

        if (Runner.IsServer) // Спавним только если это сервер / мастер
        {
            var obj = Runner.Spawn(runHandlerPrefab, Vector3.zero, Quaternion.identity, inputAuthority: Runner.LocalPlayer, flags: NetworkSpawnFlags.DontDestroyOnLoad);

            if (obj == null)
            {
                Debug.LogError("[PhotonInitializer] Failed to spawn RunHandler.");
                return;
            }

            currentHandler = obj.GetComponent<PhotonRunHandler>();
            if (currentHandler == null)
            {
                Debug.LogError("[PhotonInitializer] Spawned object does not have PhotonRunHandler.");
                return;
            }
        } 
    }
     
    public async Task EndSession()
    {
        if (currentHandler != null)
        {
            if (currentHandler.Object != null && currentHandler.Object.IsValid)
                Runner.Despawn(currentHandler.Object);

            currentHandler = null;
        }

        if (Runner != null)
        {
            await Runner.Shutdown();
            Destroy(Runner);
            Runner = null;
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!connectedPlayers.Contains(player))
            connectedPlayers.Add(player);

        Debug.Log($"[PhotonInitializer] Player joined: {player}, total: {connectedPlayers.Count}");

        if (runner.IsServer && connectedPlayers.Count >= sessionParams.TargetPlayerCount)
        {
            NetworkSessionData sessionData = new NetworkSessionData()
            {
                ScenePath = sessionParams.ScenePath,
                TargetPlayerCount = sessionParams.TargetPlayerCount,
            };

            Debug.Log("[PhotonInitializer] Required players joined, starting game...");
            PhotonRunHandler.Instance?.StartGameSceneRPC(MemoryPack.MemoryPackSerializer.Serialize(sessionData));
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (connectedPlayers.Contains(player))
            connectedPlayers.Remove(player);

        Debug.Log($"[PhotonInitializer] Player left: {player}, total: {connectedPlayers.Count}");
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}

public struct SessionParams
{
    public GameMode Mode;
    public string RoomName;
    public string ScenePath;
    public int SceneIndex;
    public int TargetPlayerCount;
    public bool ProvideInput;
}

[MemoryPackable]
public partial struct NetworkSessionData
{
    public string ScenePath;
    public int TargetPlayerCount;

    public NetworkSessionData(string scenePath,int targetPlayerCount)
    {
        ScenePath = scenePath;
        TargetPlayerCount = targetPlayerCount;
    }
}

[MemoryPackable]
public partial struct NetworkPlayerData
{
    public int PlayerOwn;

    public NetworkPlayerData(int player)
    {
        PlayerOwn = player;
    }
}