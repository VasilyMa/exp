using Client;
using Fusion;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Statement
{
    public class BattleState : State
    {
        public static new BattleState Instance
        {
            get
            {
                return (BattleState)State.Instance;
            }
        }

        [HideInInspector] public EcsRunHandler EcsHandler;

        public EntityBase PlayerEntityBase;
        protected Dictionary<string, EcsPackedEntity> _localKeyMap = new();
        protected Dictionary<string, EcsPackedEntity> _netKeyMap = new();

        protected Dictionary<int, PlayerRef> dictionaryPlayers = new Dictionary<int, PlayerRef>();
        /// <summary>
        /// Invoke when host send any clients message of start game
        /// </summary>
        public virtual void OnStarted() { Debug.Log("[Start game] game started"); }
        /// <summary>
        /// Invoke when scene loaded with addressables
        /// </summary>
        public virtual void OnSceneLoaded() => EcsHandler = new MainEcsHandler();
        public virtual void ShutdownEcsHandler() => EcsHandler.Dispose();
        public override void Start() { }
        public override void Update() => EcsHandler.Run();
        public override void FixedUpdate() => EcsHandler.FixedRun();
        public override void OnDestroy()
        {
            base.OnDestroy();   
            EcsHandler.Dispose();
        }

        public virtual void AddEntity(string localKey, string netKey, int entity)
        {
            var packed = EcsHandler.World.PackEntity(entity);

            if (!string.IsNullOrEmpty(localKey) && !_localKeyMap.ContainsKey(localKey))
                _localKeyMap[localKey] = packed;

            if (!string.IsNullOrEmpty(netKey) && !_netKeyMap.ContainsKey(netKey))
                _netKeyMap[netKey] = packed;
        }

        public virtual void AddEntity(string localKey, int entity)
        {
            var packed = EcsHandler.World.PackEntity(entity);

            if (!string.IsNullOrEmpty(localKey) && !_localKeyMap.ContainsKey(localKey))
                _localKeyMap[localKey] = packed;
        }

        public virtual bool TryGetEntity(string key, out EcsPackedEntity packedEntity)
        {
            return _localKeyMap.TryGetValue(key, out packedEntity)
                || _netKeyMap.TryGetValue(key, out packedEntity);
        }

        public virtual bool TryGetEntity(string key, out int unpackedEntity)
        {
            if (TryGetEntity(key, out EcsPackedEntity packed) && packed.Unpack(EcsHandler.World, out int entity))
            {
                unpackedEntity = entity;
                return true;
            }

            unpackedEntity = -1;
            return false;
        }

        public virtual void RemoveByLocalKey(string localKey)
        {
            if (_localKeyMap.TryGetValue(localKey, out var packed))
            {
                _localKeyMap.Remove(localKey);

                // Также удалим из _netKeyMap, если такой же packed найден
                var netKeyToRemove = _netKeyMap.FirstOrDefault(kvp => kvp.Value.Equals(packed)).Key;
                if (!string.IsNullOrEmpty(netKeyToRemove))
                    _netKeyMap.Remove(netKeyToRemove);
            }
        }

        public virtual void RemoveByNetKey(string netKey)
        {
            if (_netKeyMap.TryGetValue(netKey, out var packed))
            {
                _netKeyMap.Remove(netKey);

                // Также удалим из _localKeyMap, если такой же packed найден
                var localKeyToRemove = _localKeyMap.FirstOrDefault(kvp => kvp.Value.Equals(packed)).Key;
                if (!string.IsNullOrEmpty(localKeyToRemove))
                    _localKeyMap.Remove(localKeyToRemove);
            }
        }

        /*
                public virtual void AddEntity(string localKey, string netKey, int entity)
                {
                    if (dictionaryEntities.ContainsKey(key)) return;

                    dictionaryEntities.Add(key, EcsHandler.World.PackEntity(entity));
                }
                public virtual bool TryGetEntity(string key, out EcsPackedEntity packedEntity)
                {
                    if (dictionaryEntities.ContainsKey(key))
                    {
                        packedEntity = dictionaryEntities[key];
                        return true;
                    }

                    packedEntity = default(EcsPackedEntity);
                    return false;
                }
                public virtual bool TryGetEntity(string key, out int unpackedEntity)
                {
                    if (dictionaryEntities.ContainsKey(key))
                    {
                        if (dictionaryEntities[key].Unpack(EcsHandler.World, out int entity))
                        {
                            unpackedEntity = entity;
                            return true;
                        }
                    }

                    unpackedEntity = -1;
                    return false;
                }*/

        public void SendRequest<TRequest>(TRequest request) where TRequest : struct, IRequestable
        {
            var world = EcsHandler.World;
            var entityEvent = world.NewEntity();

            world.GetPool<RequestEvent>().Add(entityEvent);
            ref var eventComp = ref world.GetPool<TRequest>().Add(entityEvent);

            eventComp = request;
        }
        /// <summary>
        /// Register player on scene from PhotonInitializer
        /// </summary>
        /// <param name="data"></param>
        public void AddPlayer(NetworkPlayerData data)
        {
            if (!dictionaryPlayers.ContainsKey(data.PlayerOwn))
            {    
                var players = PhotonInitializer.Instance.Runner.ActivePlayers;

                foreach (var player in players)
                {
                    if (player.PlayerId == data.PlayerOwn)
                    {
                        dictionaryPlayers.Add(data.PlayerOwn, player);
                    }
                }
            }

            Debug.Log($"Player add {data.PlayerOwn}");

            if (PhotonRunHandler.Instance.SessionData.TargetPlayerCount == dictionaryPlayers.Count)
            {
                Debug.Log($"Player count is max {PhotonRunHandler.Instance.SessionData.TargetPlayerCount} and {PhotonRunHandler.Instance.SessionData.TargetPlayerCount == dictionaryPlayers.Count}");
                PhotonRunHandler.Instance.SendRequestStartGameRPC();
            }
        }

        protected void InitEcsHandler()
        {
            EcsHandler = PhotonRunHandler.Instance.Runner.IsServer ? new ServerRunHandler() : new ClientRunHandler();

            EcsHandler.Init();
        }
    }
}