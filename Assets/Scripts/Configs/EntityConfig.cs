using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityConfig", menuName = "Config/Entity")]
public class EntityConfig : Config
{
    public List<EntityBase> Entities;
    private Dictionary<string, EntityBase> _entities;
    
    public override IEnumerator Init()
    {
        _entities = new Dictionary<string, EntityBase>();

        foreach (var entity in Entities)
        {
            _entities.Add(entity.KEY_ID, entity);
        }

        yield return new WaitForSeconds(0.1f);
    }

    public bool TryGetEntity(string key, out EntityBase entityBase)
    {
        entityBase = null;

        if (_entities.ContainsKey(key))
        {
            entityBase = _entities[key];
        }

        return entityBase != null;
    }

}
