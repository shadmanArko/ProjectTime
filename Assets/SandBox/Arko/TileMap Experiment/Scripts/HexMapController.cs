using Zenject;

namespace SandBox.Arko.TileMap_Experiment.Scripts
{
    public class HexMapController
    {
        private readonly HexMap _hexMap;

        public HexMapController(HexMap hexMap)
        {
            _hexMap = hexMap;
        }

        [Inject]
        public void Initialize()
        {
            _hexMap.GenerateMap();
        }
    }
}