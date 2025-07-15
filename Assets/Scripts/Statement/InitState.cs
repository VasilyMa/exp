using MemoryPack;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class InitState : State
    {
        public static new InitState Instance
        {
            get
            {
                return (InitState)State.Instance;
            }
        } 
        public override void Awake()
        {
            UIModule.Initialize();

            EntityModule.Initialize();

            ConfigModule.Initialize(this, onConfigLoaded);
        }
        public override void Start()
        {

        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {

        }
        async void onConfigLoaded()
        {
            try
            { 
                var sceneProvider = new SceneProviderModule();

                // Опционально подписываемся на прогресс (если нужно)
                sceneProvider.ProgressChanged += progress =>
                {
                    Debug.Log($"Loading progress: {progress * 100f:0.0}%");
                    // Можно обновить UI
                };

                // Загружаем сцену по пути, ждём завершения
                await sceneProvider.LoadSceneAsync(1, LoadSceneMode.Single); 
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PhotonRunHandler] Scene RPC failed: {ex}");
            }
        } 
    }
}