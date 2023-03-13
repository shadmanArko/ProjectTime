using System;
using System.Collections.Generic;
using System.Linq;

namespace SandBox.Arko.TileMap_Experiment.Scripts
{
    public class TileFinder
    {
        private List<Tile> _tiles;

        public List<Tile> Tiles
        {
            get => _tiles;
            set => _tiles = value;
        }

        public TileFinder(List<Tile> tiles)
        {
            _tiles = tiles;
        }

        public Tile GetTile(int xPosition, int yPosition)
        {
            try
            {
                return _tiles.FirstOrDefault(tile => tile.xPosition == xPosition && tile.yPosition == yPosition);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}