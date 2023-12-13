using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class Helper
    {
        public static void SetComponent<T>(this Entity e, T componentData) where T : unmanaged, IComponentData
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            entityCommandBuffer.SetComponent(e, componentData);
            entityCommandBuffer.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
            entityCommandBuffer.Dispose();
        }

        public static Entity GetSingletonEntity<T>() where T : IComponentData
        {
            var query = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(T));
            var entities = query.ToEntityArray(Allocator.Temp);
            return entities.Length > 0 ? entities.First() : Entity.Null;
        }
        
        public static NativeArray<Entity> GetSystemsManaged<T>() where T : struct, IComponentData
        {
            if (World.DefaultGameObjectInjectionWorld == null
                || !World.DefaultGameObjectInjectionWorld.IsCreated)
            {
                return new NativeArray<Entity>(0, Allocator.Temp);
            }
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
               
            var queryBuilder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<T>()
                .WithOptions(EntityQueryOptions.IncludeSystems);
            var entityQuery = entityManager.CreateEntityQuery(queryBuilder);
            var result = entityQuery.ToEntityArray(Allocator.Temp);
            queryBuilder.Dispose();
            entityQuery.Dispose();
            return result;
        }
        
        public static NativeArray<Entity> GetSystemsManagedBurst<T>(EntityManager entityManager) where T : struct, IComponentData
        {
            var queryBuilder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<T>()
                .WithOptions(EntityQueryOptions.IncludeSystems);
            var entityQuery = entityManager.CreateEntityQuery(queryBuilder);
            var result = entityQuery.ToEntityArray(Allocator.Temp);
            queryBuilder.Dispose();
            entityQuery.Dispose();
            return result;
        }
        
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, 
                    Task.Delay(timeout))) 
                throw new TimeoutException();
        }

        public static async Task LoadScene(string sceneName)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            await WaitUntil((() => loadSceneAsync.allowSceneActivation));
            await WaitUntil((() => loadSceneAsync.isDone));
            await WaitUntil((() => SceneManager.GetActiveScene().name == sceneName && SceneManager.GetActiveScene().isLoaded));
        }
    }
}