using Fusion;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class MainMenuState : State
    {
        public static new MainMenuState Instance
        {
            get
            {
                return (MainMenuState)State.Instance;
            }
        }
        public override void FixedUpdate()
        { 
        }

        public override void Start()
        { 
        }

        public override void Update()
        { 

        }

        public async void StartMatchmaking()
        {
            var session = new SessionParams
            {
                Mode = GameMode.AutoHostOrClient,
                RoomName = $"MatchMaking",
                ScenePath = "battle_scene_01",
                SceneIndex = 4,
                ProvideInput = true,
                TargetPlayerCount = 2
            };

            await PhotonInitializer.Instance.StartSession(session);
        }

        async void TutorialStart()
        { 
            var session = new SessionParams
            {
                Mode = GameMode.Single,
                RoomName = "TutorialRoom",

                ProvideInput = true 
            };

            await PhotonInitializer.Instance.StartSession(session);
        }
    }
}