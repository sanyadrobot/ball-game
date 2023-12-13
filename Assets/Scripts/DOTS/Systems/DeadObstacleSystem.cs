using DOTS.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Utils;

namespace DOTS.Systems
{
    [BurstCompile]
    public partial struct DeadObstacleSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var touchedEntities = Helper.GetSystemsManagedBurst<DeadObstacleProperty>(state.EntityManager);
            if(touchedEntities.Length <= 0) return;
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp); 
            foreach (var entity in touchedEntities)
            {
                var property = SystemAPI.GetComponent<DeadObstacleProperty>(entity);
                if (property.LifeTime > 0)
                {
                    property.LifeTime -= Time.deltaTime;
                    entityCommandBuffer.SetComponent(entity, property);
                }
                else
                {
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}