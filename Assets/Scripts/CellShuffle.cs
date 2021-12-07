using UnityEngine;

namespace HB.TicTacToe
{
    public class CellShuffle : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        
        public void Do()
        {
            // transform.SetParent(null);
            
            _rb.isKinematic = false;
            _rb.useGravity  = true;
            
            var shift = 0.3f;
            _rb.velocity = new Vector3(Random.Range(-shift, shift), 3f, Random.Range(-shift, shift));
            _rb.AddTorque(Vector3.one * Random.Range(-shift, shift));
        }
    }
}