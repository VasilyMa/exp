using System.Collections.Generic;
using UnityEngine;

namespace Client 
{
    struct SpawnComponent 
    {
        public float Delay;
        public List<Transform> SpawnPoints;
        public float SpawnTick;
        public float SpawnTimeRemaining;
    }
}