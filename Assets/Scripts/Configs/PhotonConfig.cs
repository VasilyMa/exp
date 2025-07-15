using System.Collections;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "PhotonConfig", menuName = "Config/Photon")]
public class PhotonConfig : Config
{
    [Header("Connection Settings")]
    public string DefaultSessionName = "DefaultLobby";
    public int MaxPlayers = 4;
    public GameMode GameMode = GameMode.Shared;

    private NetworkRunner _runner;
    private GameObject _runnerHolder;
    private bool _isPhotonAvailable;

    public bool IsPhotonAvailable => _isPhotonAvailable;

    public override IEnumerator Init()
    {
        yield return null;
        //yield return CheckPhotonAvailability();
    }
    private IEnumerator CheckPhotonAvailability()
    {
        _runnerHolder = new GameObject("NetworkRunner");
        Object.DontDestroyOnLoad(_runnerHolder);

        _runner = _runnerHolder.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var checkTask = CheckConnectionAsync();

        while (!checkTask.IsCompleted)
        {
            yield return null;
        }

        _isPhotonAvailable = checkTask.Result.Ok;

        if (_isPhotonAvailable)
        {
            Debug.Log("Photon Fusion is available");
        }
        else
        {
            Debug.LogError($"Photon check failed: {checkTask.Result.ErrorMessage}");
        }

        Dispose();
    }

    private async Task<StartGameResult> CheckConnectionAsync()
    {
        return await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Single,
            SessionName = "ConnectionTest",
            PlayerCount = 1,
            SceneManager = null, // Отключаем SceneManager для проверки
            Scene = SceneRef.None
        });
    }

    public void Dispose()
    {
        if (_runner != null)
        {
            _runner.Shutdown();
            _runner = null;
        }

        if (_runnerHolder != null)
        {
            Object.Destroy(_runnerHolder);
            _runnerHolder = null;
        }
    }
}