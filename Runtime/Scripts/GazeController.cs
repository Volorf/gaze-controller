using UnityEngine;
using UnityEngine.Events;

namespace Volorf.GazeController
{
    public class GazeController : MonoBehaviour
    {
        [SerializeField] private Direction targetDirection = Direction.Forward;
        
        [Space(5)] [Header("Elements")] 
        public Transform target;
        public Transform head;

        [Space(5)] [Header("Trigger Settings")] 
        [SerializeField] private float lookAtTrigger = 0.8f;
        [SerializeField] private float lookAwayTrigger = 0.6f;
        [SerializeField] private bool hideAtStart;

        [Space(5)] [Header("Events")] 
        public UnityEvent onShow;
        public UnityEvent onHide;

        [Space(5)] [Header("Debugging")] 
        [SerializeField] private bool printDotProduct;
        
        bool _hasBeenShown;
        bool _hasBeenShownForSecond;
        Vector3 _targetDir;

        private void Start()
        {
            if (!hideAtStart) onHide.Invoke();
        }

        private void Update()
        {
            
            switch (targetDirection)
            {
                case Direction.Right: _targetDir = target.right; break;
                case Direction.Left: _targetDir = target.right * -1; break;
                case Direction.Up: _targetDir = target.up; break;
                case Direction.Down: _targetDir = target.up * -1; break;
                case Direction.Forward: _targetDir = target.forward; break;
                case Direction.Back: _targetDir = target.forward * -1; break;
            }
            
            Vector3 targetDir = _targetDir;
            Vector3 dirToHead = head.position - target.position;
            float dotProd = Vector3.Dot(targetDir, dirToHead.normalized);
            
            if (printDotProduct) Debug.Log($"Dot product: {dotProd}");

            if (dotProd > lookAtTrigger)
            {
                if (!_hasBeenShown)
                {
                    onShow.Invoke();
                    _hasBeenShown = true;
                }
            }

            if (dotProd < lookAwayTrigger)
            {
                if (_hasBeenShown)
                {
                    onHide.Invoke();
                    _hasBeenShown = false;
                }
            }
        }
    }
}
