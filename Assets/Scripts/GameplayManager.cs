using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HB.TicTacToe
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private BasicMenu _menu;
        [SerializeField] private Cell _cellPrefab;

        public RoundState State { get; private set; }

        private TicTac[] _progress;

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            var gs = GameSettings.Instance;

            _grid.Clear();

            for (var i = 0; i < gs.TotalCells; i++)
            {
                var cell = Instantiate(_cellPrefab);
                cell.Initialize(i, this);

                _grid.AddCell(cell);
            }

            _progress = new TicTac[gs.TotalCells];
            State = new RoundState();
            
            _menu.UpdateRule(gs.WinLength);
            _menu.UpdateTurn(State.NextTurnShape.ToString());
        }

        public void SetLastTurn(TicTac shape, int id)
        {
            State.SetLastTurn(shape);

            _progress[id] = shape;
            
            if (Algorithms.WinCheckOnTurnEnd(_progress, id, shape))
            {
                Debug.Log($"Win {shape}");
                State.Win(shape);
                _menu.ShowWin(shape.ToString());
                StartCoroutine(ReloadScene());
                
                _grid.ShuffleCells();
                
                return;
            }

            if (!State.IsGameOver && State.TurnsCount == GameSettings.Instance.TotalCells)
            {
                Debug.Log("Draw");
                State.Win(TicTac.None);
                _menu.ShowWin("Draw");
                StartCoroutine(ReloadScene());
                
                _grid.ShuffleCells();
                
                return;
            }
            
            _menu.UpdateTurn(State.NextTurnShape.ToString());
        }

        private IEnumerator ReloadScene()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public class RoundState
    {
        public int TurnsCount { get; private set; }
        
        public TicTac LastTurnShape { get; private set; }
        public TicTac NextTurnShape { get; private set; }

        public bool IsGameOver { get; private set; }
        public TicTac Winner   { get; private set; }

        public RoundState()
        {
            SetLastTurn(TicTac.None);
            TurnsCount = 0;
        }

        public void SetLastTurn(TicTac shape)
        {
            LastTurnShape = shape;

            switch (shape)
            {
                case TicTac.Cross:
                    NextTurnShape = TicTac.Round;
                    break;
                case TicTac.Round:
                    NextTurnShape = TicTac.Cross;
                    break;
                case TicTac.None:
                    NextTurnShape = TicTac.Cross;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
            }

            TurnsCount++;
        }

        public void Win(TicTac shape)
        {
            IsGameOver = true;
            Winner     = shape;
        }
    }

    public enum TicTac
    {
        None,
        Cross,
        Round
    }
}
