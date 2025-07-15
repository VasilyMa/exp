using Client;
using Fusion;
using System;
using UnityEngine;

namespace Statement
{
    public class TutorialState : BattleState
    {
        [SerializeField] private string gameSceneName = "GameScene";

        public override void Awake()
        {
            EcsHandler = new TutorEcsHandler();
        }

        public override void Start()
        {
            base.Start();

            Debug.Log("[TutorialState] Starting singleplayer session..."); 
        }

        public override void Update()
        {
            base.Update();
        }
    }
}