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
            
            if (CheckWin(_progress, shape, id))
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

        private bool CheckWin(TicTac[] progress, TicTac shape, int id)
        {
            var gs = GameSettings.Instance;

            var hor = 1;
            var vert = 1;
            var diagLtR = 1;
            var diagRtL = 1;

            bool horLFail = false, horRFail = false,
                vertLFail  = false, vertRFail  = false,
                diag1LFail = false, diag1RFail = false,
                diag2LFail = false, diag2RFail = false;

            for (var i = 1; i < gs.WinLength; i++)
            {
                // vertical check
                var l = id - i * gs.Cols;
                var r = id + i * gs.Cols;
                
                if (!vertLFail && l > 0               && progress[l] == shape) { vert++; } else { vertLFail = true; }
                if (!vertRFail && r < progress.Length && progress[r] == shape) { vert++; } else { vertRFail = true; }
                
                if (vert >= gs.WinLength) { return true; }
                
                
                // horizontal check
                l = id % gs.Cols - i >= 0      ? id - i : int.MinValue;
                r = id % gs.Cols + i < gs.Cols ? id + i : int.MinValue;
                if (!horLFail && l != int.MinValue && progress[l] == shape) { hor++; } else { horLFail = true; }
                if (!horRFail && r != int.MinValue && progress[r] == shape) { hor++; } else { horRFail = true; }
                
                if (hor >= gs.WinLength) { return true; }
                
                // diagonal right to left /
                // nasty edge case, check overshoot
                l = id + gs.Cols * i < progress.Length && id % gs.Cols + i < gs.Cols ? id + i + gs.Cols * i : int.MinValue;
                r = id - gs.Cols * i > 0               && id % gs.Cols - i >= 0      ? id - i - gs.Cols * i : int.MinValue;
                
                if (!diag1LFail && l != int.MinValue && progress[l] == shape) { diagLtR++; } else { diag1LFail = true; }
                if (!diag1RFail && r != int.MinValue && progress[r] == shape) { diagLtR++; } else { diag1RFail = true; }
                
                if (diagLtR >= gs.WinLength) { return true; }
                
                // diagonal left to right \
                // nasty edge case, check overshoot
                l = id + gs.Cols * i < progress.Length && id % gs.Cols - i >= 0      ? id - i + gs.Cols * i : int.MinValue;
                r = id - gs.Cols * i > 0               && id % gs.Cols + i < gs.Cols ? id + i - gs.Cols * i : int.MinValue;
                
                if (!diag2LFail && l != int.MinValue && progress[l] == shape) { diagRtL++; } else { diag2LFail = true; }
                if (!diag2RFail && r != int.MinValue && progress[r] == shape) { diagRtL++; } else { diag2RFail = true; }
                
                if (diagRtL >= gs.WinLength) { return true; }
            }
            
            return false;
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
