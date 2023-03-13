using Zenject;

namespace SandBox.Arko.TileMap_Experiment.Scripts
{
    public class TileMapController
    {
        private readonly TileMapGenerator _tileMapGenerator;

        public TileMapController(TileMapGenerator tileMapGenerator)
        {
            _tileMapGenerator = tileMapGenerator;
        }

        [Inject]
        public void Initialize()
        {
            _tileMapGenerator.GenerateContinentMap();
        }
    }
}