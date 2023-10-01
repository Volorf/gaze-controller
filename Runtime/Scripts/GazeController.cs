using UnityEngine;
using UnityEngine.Events;

namespace Volorf.GazeController
{
    public class GazeController : MonoBehaviour
    {
        [SerializeField] private Direction targetDirection = Direction.Forward;
        
        [Space(5)] [Header("Elements")] 
        public Transform head;
        public Transform target;

        [Space(5)] [Header("Trigger Settings")] 
        [SerializeField] private float lookAtAngle = 30f;
        [SerializeField] private float lookAwayAngle = 60f;
        [SerializeField] private bool hideAtStart;

        [Space(5)] [Header("Events")] 
        public UnityEvent onShow;
        public UnityEvent onHide;

        [Space(5)] [Header("Debugging")] 
        [SerializeField] private bool printAngle;
        [SerializeField] private bool printEvents;
        
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
            float angle = Vector3.Angle(dirToHead, targetDir);

            if (printAngle)
            {
                Debug.Log($"Angle: {angle}");
            }

            if (angle < lookAtAngle)
            {
                if (!_hasBeenShown)
                {
                    onShow.Invoke();
                    _hasBeenShown = true;
                    if (printEvents) Debug.Log($"Looking at");
                }
            }

            if (angle > lookAwayAngle)
            {
                if (_hasBeenShown)
                {
                    onHide.Invoke();
                    _hasBeenShown = false;
                    if (printEvents) Debug.Log($"Looking away");
                }
            }
        }
    }
}
