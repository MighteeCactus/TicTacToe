using System;
using UnityEngine;

namespace HB.TicTacToe
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _shapeCross;
        [SerializeField] private GameObject _shapeRound;
        
        private int _id;
        private GameplayManager _manager;
        
        private bool _isClicked;

        private void Awake()
        {
            _shapeCross.gameObject.SetActive(false);
            _shapeRound.gameObject.SetActive(false);
        }

        public void Initialize(int i, GameplayManager manager)
        {
            _id = i;
            _manager = manager;

            name = $"Cell {i}";
        }

        public void OnClick()
        {
            if (_isClicked) { return; }
            _isClicked = true;

            var shape = _manager.State.NextTurnShape;
            
            _manager.SetLastTurn(shape, _id);
            
            switch (shape)
            {
                case TicTac.Cross:
                    _shapeCross.gameObject.SetActive(true);
                    // Instantiate(_shapeCross, transform);
                    break;
                case TicTac.Round:
                    _shapeRound.gameObject.SetActive(true);
                    // Instantiate(_shapeRound, transform);
                    break;
                case TicTac.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
            }
        }
    }
}
