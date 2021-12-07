using TMPro;
using UnityEngine;

namespace HB.TicTacToe
{
    public class BasicMenu : Menu
    {
        [SerializeField] private TextMeshProUGUI _rule;
        [SerializeField] private TextMeshProUGUI _turn;
        [SerializeField] private TextMeshProUGUI _win;
        
        private string _ruleTempl;
        private string _turnTempl;
        private string _winTempl;

        private void Awake()
        {
            _ruleTempl = _rule.text;
            _turnTempl = _turn.text;
            _winTempl  = _win.text;
            
            _win.gameObject.SetActive(false);
        }

        public void UpdateRule(int count)
        {
            _rule.text = string.Format(_ruleTempl, count);
        }
        
        public void UpdateTurn(string shape)
        {
            _turn.text = string.Format(_turnTempl, shape);
        }
        
        public void ShowWin(string shape)
        {
            _win.text = string.Format(_winTempl, shape);
            _win.gameObject.SetActive(true);
        }
    }
}