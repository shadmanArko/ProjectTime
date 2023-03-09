using Zenject;

namespace SandBox.Arko.TileMap_Experiment.Scripts
{
    public class TileMapController
    {
        private readonly TileMap _tileMap;

        public TileMapController(TileMap tileMap)
        {
            _tileMap = tileMap;
        }

        [Inject]
        public void Initialize()
        {
            _tileMap.GenerateMap();
        }
    }
}