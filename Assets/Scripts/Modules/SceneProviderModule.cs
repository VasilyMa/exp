using System;
using System.Threading;
using System.Threading.Tasks; 
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneProviderModule
{
    public event Action<float> ProgressChanged; // прогресс 0..1
    public event Action SceneLoaded;

    private CancellationTokenSource _cts;

    // Загрузка по индексу из BuildSettings
    public async Task LoadSceneAsync(int buildIndex, LoadSceneMode mode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
    {
        CancelCurrentLoading();

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var asyncOp = SceneManager.LoadSceneAsync(buildIndex, mode);
        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
        {
            ProgressChanged?.Invoke(asyncOp.progress);
            if (_cts.Token.IsCancellationRequested)
            {
                // Попытка отмены — пока SceneManager не умеет отменять загрузку, просто выйдем из метода
                return;
            }
            await Task.Yield();
        }

        ProgressChanged?.Invoke(1f);
        SceneLoaded?.Invoke();
    } 
    public async Task LoadSceneAsync(string address, LoadSceneMode mode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
    {
        CancelCurrentLoading();

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(address, mode, true);

        while (!handle.IsDone)
        {
            ProgressChanged?.Invoke(handle.PercentComplete);
            if (_cts.Token.IsCancellationRequested)
            {
                await AwaitHandle(Addressables.UnloadSceneAsync(handle, true));
                return;
            }
            await Task.Yield();
        }

        ProgressChanged?.Invoke(1f);
        SceneLoaded?.Invoke();
    }

    private static Task AwaitHandle(AsyncOperationHandle handle)
    {
        var tcs = new TaskCompletionSource<bool>();
        handle.Completed += _ => tcs.SetResult(true);
        return tcs.Task;
    }


    // Вызвать, чтобы прервать текущую загрузку (если нужна)
    public void CancelCurrentLoading()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}
