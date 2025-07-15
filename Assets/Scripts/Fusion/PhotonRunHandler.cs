using Client;
using Fusion;
using MemoryPack;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations; 
using UnityEngine.SceneManagement;
using Statement;

public class PhotonRunHandler : NetworkBehaviour
{
    public static PhotonRunHandler Instance { get; private set; } 
    private NetworkRunner runner;
    public NetworkSessionData SessionData;
     
    public override void Spawned()
    {
        base.Spawned();

        if (Instance == null)
        {
            Instance = this;
        }

        runner = PhotonInitializer.Instance.Runner;
        
        Debug.Log("[PhotonRunHandler] Init called. Runner: " + runner?.name);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SendUnitEntitySpawnRPC(byte[] recieve)
    {
        var data = MemoryPackSerializer.Deserialize<NetworkUnitEntitySpawnEvent>(recieve);
        BattleState.Instance.SendRequest(data);
        Debug.Log($"Receive spawn event {data.EntityKey}");
    }

    [Rpc(RpcSources.Proxies, RpcTargets.StateAuthority)]
    public void SendRequestDamageEffectRPC(byte[] recieve)
    {
        var data = MemoryPackSerializer.Deserialize<NetworkDamageEffectEvent>(recieve);
        BattleState.Instance.SendRequest(data);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    public void SendRequestHealthUpdateRPC(byte[] recieve)
    {
        var data = MemoryPackSerializer.Deserialize<NetworkHealthUpdateEvent>(recieve);
        BattleState.Instance.SendRequest(data);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    public void SendRequestConfirmDamageRPC(byte[] recieve)
    {
        var data = MemoryPackSerializer.Deserialize<NetworkConfirmDamageEvent>(recieve);

    } 

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SendRequestTransformRPC(byte[] recieve)
    {
        var data = MemoryPackSerializer.Deserialize<NetworkTransformEvent>(recieve);
        BattleState.Instance.SendRequest(data);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void SendRequestReadyToStartRPC(byte[] receive)
    {
        var data = MemoryPackSerializer.Deserialize<NetworkPlayerData>(receive);

        BattleState.Instance.AddPlayer(data);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void SendRequestStartGameRPC()
    {
        BattleState.Instance.OnStarted();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void StartGameSceneRPC(byte[] rawData)
    {
        try
        {
            SessionData = MemoryPackSerializer.Deserialize<NetworkSessionData>(rawData);
            StartCoroutine(LoadSceneRoutine(SessionData.ScenePath));
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PhotonRunHandler] Scene RPC failed: {ex}");
        }
    }

    private IEnumerator LoadSceneRoutine(string scenePath)
    {
        yield return PrepareForSceneUnload();

        var loadHandle = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        yield return loadHandle;

        if (loadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[PhotonRunHandler] Scene load failed: {scenePath}");
            yield break;
        }

        Debug.Log($"[PhotonRunHandler] Scene loaded: {loadHandle.Result.Scene.name}");
        yield return InitializeScenePostLoad();
    }

    private IEnumerator PrepareForSceneUnload()
    {
        BattleState.Instance?.ShutdownEcsHandler();
        yield return null;
    }

    private IEnumerator InitializeScenePostLoad()
    {
        BattleState.Instance?.OnSceneLoaded();
        yield return null;
    }
}
