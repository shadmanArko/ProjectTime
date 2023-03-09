using UnityEngine;
using Zenject;

namespace SandBox.Arko.TileMap_Experiment.Scripts
{
    public class DataTesing : MonoBehaviour
    {
        public int x;
        public int y;
        
        private  TileFinder _tileFinder;

        [Inject]
        public void Initialize(TileFinder tileFinder)
        {
            _tileFinder = tileFinder;
        }
        

        [ContextMenu("Get File Name")]
        public void GetTileName()
        {
            Debug.Log(_tileFinder.GetTile(x, y).name);
        }
    }
}