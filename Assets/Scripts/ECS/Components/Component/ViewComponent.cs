using Leopotam.EcsLite;

using UnityEngine;
using UnityEngine.AI;

namespace Client 
{
    struct ViewComponent : IComponent
    {
        public GameObject RefObject;

        public void AddComponent(EcsWorld world, int entity)
        {
            var gameObject = GameObject.Instantiate(RefObject);

            ref var viewComp = ref world.GetPool<ViewComponent>().Add(entity);
            viewComp.RefObject = gameObject;

            ref var transformComp = ref world.GetPool<TransformComponent>().Add(entity);
            transformComp.Transform = gameObject.transform;

            if (gameObject.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
            {
                ref var navMeshComp = ref world.GetPool<NavMeshComponent>().Add(entity);
                navMeshComp.NavMeshAgent = navMeshAgent;
            }
            if (gameObject.TryGetComponent<CharacterController>(out var controller))
            {
                ref var characterControllerComp = ref world.GetPool<CharacterControllerComponent>().Add(entity);
                characterControllerComp.CharacterController = controller;
            }
             
            var transforms = gameObject.GetComponentsInChildren<Transform>(true);

            foreach (var transform in transforms)
            {
                if (transform.CompareTag("Top"))
                {
                    ref var topTransformComp = ref world.GetPool<TopTransformComponent>().Add(entity);
                    topTransformComp.Transform = transform;
                } 
            } 
        }
    }
}