using UnityEngine;

namespace HB.TicTacToe
{
    public class PlayerInput : MonoBehaviour
    {
        private GameplayManager _manager;

        private RaycastHit[] _hits = new RaycastHit[10];

        private static int _cellLayerMask = -1; 
        
        private void Awake()
        {
            _manager = FindObjectOfType<GameplayManager>();

            if (_cellLayerMask == -1)
            {
                _cellLayerMask = LayerMask.GetMask(new[] {GlobalConstants.kCellLayerName});
            }
        }

        private void Update()
        {
            if (_manager.State.IsGameOver) { return; }
            if (!Input.GetMouseButtonUp(0)) { return;}

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var res = Physics.RaycastNonAlloc(ray, _hits,1000f, _cellLayerMask);
            if (res == 0) { return; }

            _hits[0].collider.GetComponent<Cell>().OnClick();
        }
    }
    
    public static class GlobalConstants
    {
        public const string kCellLayerName = "Cell";
    }
}

