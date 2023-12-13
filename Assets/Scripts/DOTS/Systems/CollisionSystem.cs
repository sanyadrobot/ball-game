using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DOTS.Systems
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct CollisionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CollisionEventImpulseJob()
            {
                TouchLookup = SystemAPI.GetComponentLookup<Touched>(true),
                CommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                    .CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
        
        struct CollisionEventImpulseJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<Touched> TouchLookup;

            public EntityCommandBuffer CommandBuffer;

            private bool IsTouchable(Entity entity) => TouchLookup.HasComponent(entity);
            
            public void Execute(CollisionEvent collisionEvent)
            {
                if(!IsTouchable(collisionEvent.EntityA))
                {
                    var entity = CommandBuffer.CreateEntity();
                    CommandBuffer.AddComponent(entity, new Touched(collisionEvent));
                }
            }
        }
        public readonly struct Touched : IComponentData
        {
            public readonly CollisionEvent CollisionEvent;

            public Touched(CollisionEvent collisionEvent)
            {
                CollisionEvent = collisionEvent;
            }
        }
    }
}