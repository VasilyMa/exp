using UnityEngine;

namespace Client 
{
    struct ReceiveSpawnEvent 
    {
        public Vector3 SpawnPos;
        public int PlayerOwner;
        public string SpawnKeyID;
        public string EntityKey; 
    }
}