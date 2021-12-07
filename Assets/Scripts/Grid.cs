using UnityEngine;

namespace HB.TicTacToe
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private int _currentIdx;

        private Vector3 _gridCenter;
        
        private void Awake()
        {
            var gs = GameSettings.Instance;
            _gridCenter = new Vector3(gs.Cols * gs.CellSize.x, 0f,gs.Rows * gs.CellSize.y) * 0.5f;
        }

        public void Clear()
        {
            foreach (Transform child in _container) { Destroy(child.gameObject); }
        }

        /// <summary>
        /// Cells are added from bottom left to top right.
        /// </summary>
        /// <param name="cell"></param>
        public void AddCell(Cell cell)
        {
            if (_container.childCount >= GameSettings.Instance.Rows * GameSettings.Instance.Cols) { return; }
            
            var t = cell.transform;
            t.SetParent(_container);

            var gs = GameSettings.Instance;
            var pos = new Vector2(_currentIdx % gs.Cols, _currentIdx / gs.Cols);
            t.localPosition = new Vector3(pos.x * gs.CellSize.x, 0f,pos.y * gs.CellSize.y) - _gridCenter;
            t.rotation = _container.rotation;

            _currentIdx++;
        }

        public void ShuffleCells()
        {
            foreach (Transform cell in _container)
            {
                var t = cell.GetComponent<CellShuffle>();
                if (t == null) { continue; }
                
                t.Do();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            var gs = GameSettings.Instance;
            Gizmos.DrawWireCube(transform.position, new Vector3(gs.Cols * gs.CellSize.x, 1f, gs.Rows * gs.CellSize.y));
        }
    }
}

