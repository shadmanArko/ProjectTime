using SandBox.Arko.TileMap_Experiment.Scripts;
using Unity.Entities;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "TileMapInstaller", menuName = "Installers/TileMapInstaller")]
public class TileMapInstaller : ScriptableObjectInstaller<TileMapInstaller>
{
    [SerializeField] private GameObject hexPrefab;
    public override void InstallBindings()
    {
        Container.Bind<HexMap>().AsSingle();
        Container.BindInstance(hexPrefab).WhenInjectedInto<HexMap>();
        Container.BindInterfacesAndSelfTo<HexMapController>().AsSingle().NonLazy();
    }
    
}