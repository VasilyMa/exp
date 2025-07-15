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
using System.Threading.Tasks;

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
    public async void StartGameSceneRPC(byte[] rawData)
    {
        try
        {
            SessionData = MemoryPackSerializer.Deserialize<NetworkSessionData>(rawData);

            // Подготовка перед выгрузкой сцены
            await PrepareForSceneUnloadAsync();

            // Создаём и используем SceneProviderModule
            var sceneProvider = new SceneProviderModule();

            // Опционально подписываемся на прогресс (если нужно)
            sceneProvider.ProgressChanged += progress =>
            {
                Debug.Log($"Loading progress: {progress * 100f:0.0}%");
                // Можно обновить UI
            };

            // Загружаем сцену по пути, ждём завершения
            await sceneProvider.LoadSceneAsync(SessionData.ScenePath, LoadSceneMode.Single);

            Debug.Log($"[PhotonRunHandler] Scene loaded: {SessionData.ScenePath}");

            // Инициализация после загрузки сцены
            await InitializeScenePostLoadAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PhotonRunHandler] Scene RPC failed: {ex}");
        }
    }

    // Асинхронная версия PrepareForSceneUnload
    private Task PrepareForSceneUnloadAsync()
    {
        BattleState.Instance?.ShutdownEcsHandler();
        return Task.CompletedTask; // здесь можно вставить await, если будут async операции
    }

    // Асинхронная версия InitializeScenePostLoad
    private Task InitializeScenePostLoadAsync()
    {
        BattleState.Instance?.OnSceneLoaded();
        return Task.CompletedTask;
    }
    /*[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
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
    }*/
}
