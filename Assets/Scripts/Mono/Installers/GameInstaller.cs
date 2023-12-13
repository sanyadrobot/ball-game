using Mono.Services;
using Mono.Services.Game;
using UnityEngine;
using Zenject;

namespace Mono.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Player player;
        [SerializeField] private Exit exit;
        public override void InstallBindings()
        {
            Container.Bind<ObstacleSpawnerService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ProjectileSpawnerService>().AsSingle().NonLazy();
            Container.Bind<Player>().FromInstance(player).AsSingle();
            Container.Bind<Exit>().FromInstance(exit).AsSingle();
        }
    }
}