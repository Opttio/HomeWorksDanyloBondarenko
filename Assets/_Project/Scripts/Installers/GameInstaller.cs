using _Project.Scripts.Environment.Platforms;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LocationGeneratorFromPool _locationGenerator;
        [SerializeField] private PlatformPoolManager _platformPoolManager;
        public override void InstallBindings()
        {
            HierarchyBinding<LocationGeneratorFromPool>(_locationGenerator);
            HierarchyBinding<PlatformPoolManager>(_platformPoolManager);
        }

        private void HierarchyBinding<T>(T bind)
        {
            Container
                .Bind<T>()
                .FromInstance(bind)
                .AsSingle();
        }
    }
}