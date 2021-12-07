using UnityEngine;

namespace HB.TicTacToe
{
    [CreateAssetMenu(menuName = "Create GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public int Rows = 3;
        public int Cols = 3;
        public int WinLength = 3;

        public Vector2 CellSize = Vector2.one;
        
        public int TotalCells => Rows * Cols;
        
        #region Semi singleton
        
        private static GameSettings _instance;
        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameSettings>("game_settings");
                }

                return _instance;
            }
        }

        #endregion Semi singleton
    }
}
