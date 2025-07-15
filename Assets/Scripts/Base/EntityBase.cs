using System.Collections.Generic;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Entities/NewEntity")]
public class EntityBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
    [Header("Main attributes entity")]
    [SerializeReference] public List<IComponent> Components;

    [Space(5f)]
    [Header("Combat stats of entity")]
    [SerializeReference] public List<IStat> Stats;



    public void InitEntity(EcsWorld world, int entity)
    {
        foreach (var component in Components)
        {
            component.AddComponent(world, entity);
        }

        foreach (var stat in Stats)
        {
            stat.Init(world, entity);
        }
    }

    public void OnAfterDeserialize()
    { 
    }

    public void OnBeforeSerialize()
    {
        KEY_ID = name;
    }
}
