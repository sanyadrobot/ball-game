using Mono.Services.Game;
using Utils;
using Zenject;

namespace Mono.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameService>().AsSingle().NonLazy();
            Container.Bind<UnityMainThreadDispatcher>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
