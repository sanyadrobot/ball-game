using UnityEngine;

namespace Mono.Services
{
    public interface IProjectileSpawnerService
    {
        void SetSpawnerPosition(Transform t);
        void SpawnProjectile(float scale);
    }
}