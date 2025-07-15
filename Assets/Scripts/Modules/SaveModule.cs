using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine; 

public static class SaveModule
{
    private static Dictionary<Type, IDatable> _data = new Dictionary<Type, IDatable>();

    /// <summary>
    /// Загружает данные с PlayFab (корутина)
    /// </summary>
    public static IEnumerator LoadFromPlayFab<T>(Action<T> onComplete, Action<GetUserDataResult> onSuccess, Action<PlayFabError> onFailure) where T : IDatable, new()
    {
        string dataKey = typeof(T).Name;
        bool requestCompleted = false;

        T resultData = default;

        PlayFabError errorResult = null;
        GetUserDataResult successResult = null;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            successResult => {
                try
                {
                    if (successResult.Data != null && successResult.Data.ContainsKey(dataKey))
                    {
                        resultData = Deserialize<T>(successResult.Data[dataKey].Value);
                    }
                    else
                    {
                        resultData = new T(); 
                    }
                }
                catch (Exception ex)
                {
                    errorResult = new PlayFabError
                    {
                        Error = PlayFabErrorCode.UnknownError,
                        ErrorMessage = $"Deserialization error: {ex.Message}"
                    };
                }
                requestCompleted = true;
            },
            error => {
                errorResult = error;
                requestCompleted = true;
                onFailure?.Invoke(error);
            });

        yield return new WaitUntil(() => requestCompleted);

        if (errorResult != null)
        {
            Debug.LogError($"Load error ({dataKey}): {errorResult.GenerateErrorReport()}");
            onFailure?.Invoke(errorResult);
            yield break;
        }

        resultData?.ValidData();

        onComplete?.Invoke(resultData);
        onSuccess?.Invoke(successResult);

        if (!_data.ContainsKey(typeof(T)))
        {
            _data.Add(typeof(T), resultData);
        }
    }

    /// <summary>
    /// Сохраняет данные на PlayFab (корутина)
    /// </summary>
    public static IEnumerator SaveToPlayFab<T>(Action<UpdateUserDataResult> onSuccess, Action<PlayFabError> onFailure) where T : IDatable
    {
        string dataKey = typeof(T).Name;
        bool requestCompleted = false;
        PlayFabError errorResult = null;

        try
        {
            if (!_data.TryGetValue(typeof(T), out IDatable value))
            {
                value = Activator.CreateInstance<T>();
                _data[typeof(T)] = value;
            }

            if (value is not T data)
            {
                throw new InvalidCastException($"Stored data is not of type {typeof(T)}");
            }

            data.ProcessUpdateData();

            string serialized = Serialize(data);

            if (serialized.Length > 9000) // PlayFab limit ~10KB
            {
                throw new Exception($"Data too large ({serialized.Length} bytes)");
            }

            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> { { dataKey, serialized } },
                Permission = UserDataPermission.Public
            },
            successResult => { 
                requestCompleted = true;
                onSuccess?.Invoke(successResult);
            },
            error => {
                errorResult = error;
                requestCompleted = true;
                onFailure?.Invoke(errorResult);
            });
        }
        catch (Exception ex)
        {
            errorResult = new PlayFabError
            {
                Error = PlayFabErrorCode.UnknownError,
                ErrorMessage = ex.Message,
            };

            onFailure?.Invoke(errorResult);
            requestCompleted = true;
        }

        yield return new WaitUntil(() => requestCompleted);

        if (errorResult != null)
        {
            Debug.LogError($"Save error ({dataKey}): {errorResult.GenerateErrorReport()}");
        } 
    }

    private static string Serialize<T>(T data)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            new BinaryFormatter().Serialize(stream, data);
            return Convert.ToBase64String(stream.ToArray());
        }
    }

    private static T Deserialize<T>(string base64Data) where T : IDatable
    {
        byte[] bytes = Convert.FromBase64String(base64Data);
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            return (T)new BinaryFormatter().Deserialize(stream);
        }
    }
}

public interface IDatable
{
    string DATA_ID { get; }
    void ProcessUpdateData();
    void ValidData();
    void Dispose();
}

public interface ISavable
{
    string ID { get; }
    void LoadData();
    void SaveData();    
}