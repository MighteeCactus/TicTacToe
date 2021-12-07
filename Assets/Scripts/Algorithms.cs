namespace HB.TicTacToe
{
    public static class Algorithms
    {
        /// <summary>
        /// Call it each time player ends turn to check if player won. 
        /// </summary>
        /// <param name="progress">Flat representation of game field</param>
        /// <param name="shape">Last placed shape</param>
        /// <param name="id">Where exactly on flat field it has been placed</param>
        /// <returns></returns>
        public static bool WinCheckOnTurnEnd(TicTac[] progress, int id, TicTac shape)
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
    }
}