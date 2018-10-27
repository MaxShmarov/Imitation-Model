
using UnityEngine;
using Lean.Touch;

namespace LittleWorld
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField]
        private Terrain _terrain;
        [SerializeField]
        private float _terrainOffset;
        [SerializeField]
        private float _terrainOffsetMin = 1.0f;
        [SerializeField]
        private float _terrainOffsetMax = 100.0f;
        [SerializeField]
        private float _offsetFromFloorIfNoTerrain;
        [SerializeField]
        private Vector2 _limitsX = new Vector2(Mathf.NegativeInfinity, Mathf.Infinity);
        [SerializeField]
        private Vector2 _limitsZ = new Vector2(Mathf.NegativeInfinity, Mathf.Infinity);
        [SerializeField]
        private Vector2 _limitsOffset = Vector2.zero;
        [SerializeField]
        private float _defaultOrthoSize;
        [SerializeField]
        private Vector2 _orthoLimits;

        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreGuiFingers = true;
        [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
        public int RequiredFingerCount;
        [Tooltip("The distance from the camera the world drag delta will be calculated from (this only matters for perspective cameras)")]
        public float MoveDistance = 1.0f;
        [Tooltip("The sensitivity of the movement, use -1 to invert")]
        public float MoveSensitivity = 1.0f;
        [Tooltip("How quickly the zoom reaches the target value")]
        public float MoveDampening = 10.0f;
        [Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
        [Range(-1.0f, 1.0f)]
        public float ZoomWheelSensitivity;

        private static CameraHandler _instance;
        public static CameraHandler Instance
        {
            get
            {
                return _instance;
            }
        }

        private Transform _transform;
        private Camera _camera;
        private Vector3 _moveRemainingDelta;
        private float _zoom;
        private bool _isMoving = false;
        private bool _isActive = true;

        private void Awake()
        {
            _instance = this;
            _transform = transform;
            _camera = GetComponent<Camera>();
            if (_camera.orthographic)
            {
                _camera.orthographicSize = _defaultOrthoSize;
                _zoom = _defaultOrthoSize;
            }
            else
            {
                _zoom = _terrainOffset;
            }
            _isMoving = false;
            _isActive = true;
        }

        protected virtual void LateUpdate()
        {
            if (!_isActive)
            {
                return;
            }
            var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);
            var screenDelta = LeanGesture.GetScreenDelta(fingers);
            screenDelta.x /= Screen.width;
            screenDelta.y /= Screen.height;
            var oldPosition = _transform.localPosition;
            _transform.Translate(new Vector3(screenDelta.x, 0f, screenDelta.y) * MoveSensitivity, Space.Self);
            _moveRemainingDelta += _transform.localPosition - oldPosition;
            if (_camera.orthographic)
            {
                _moveRemainingDelta.y = 0f;
            }
            var factor = LeanTouch.GetDampenFactor(MoveDampening, Time.deltaTime);
            var newDelta = Vector3.Lerp(_moveRemainingDelta, Vector3.zero, factor);
            _transform.localPosition = oldPosition + _moveRemainingDelta - newDelta;
            _isMoving = screenDelta.sqrMagnitude > 0.1f;
            _moveRemainingDelta = newDelta;
            if (_transform.position.x < _limitsOffset.x + _limitsX.x || _transform.position.x > _limitsOffset.x + _limitsX.y)
            {
                var limitedPos = _transform.localPosition;
                limitedPos.x = oldPosition.x;
                _transform.localPosition = limitedPos;
                _moveRemainingDelta.x = 0f;
            }
            if (_transform.position.z < _limitsOffset.y + _limitsZ.x || _transform.position.z > _limitsOffset.y + _limitsZ.y)
            {
                var limitedPos = _transform.localPosition;
                limitedPos.z = oldPosition.z;
                _transform.localPosition = limitedPos;
                _moveRemainingDelta.z = 0f;
            }
            var pinchRatio = LeanGesture.GetPinchRatio(fingers, ZoomWheelSensitivity);
            if (_camera.orthographic)
            {
                _zoom *= pinchRatio;
                _zoom = Mathf.Clamp(_zoom, _orthoLimits.x, _orthoLimits.y);
                _camera.orthographicSize = _zoom;
            }
            else
            {
                _zoom *= pinchRatio;
                _zoom = Mathf.Clamp(_zoom, _terrainOffsetMin, _terrainOffsetMax);
                var pos = _transform.position;
                var height = 0f;
                if (_terrain != null)
                {
                    height = _terrain.SampleHeight(pos) + _zoom;
                }
                else
                {
                    height = _offsetFromFloorIfNoTerrain + _zoom;
                }
                pos.y = height;
                _transform.position = pos;
            }
            var twistDegrees = LeanGesture.GetTwistDegrees(fingers);
            var rotation = _transform.localEulerAngles;
            rotation.y += twistDegrees;
            //_transform.localEulerAngles = rotation;
        }

        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void SetLimitsX(Vector2 limits)
        {
            _limitsX = limits;
        }

        public void SetLimitsZ(Vector2 limits)
        {
            _limitsZ = limits;
        }

        public void SetOffset(float offset)
        {
            _terrainOffset = offset;
        }

        public void SetOffsetMin(float offset)
        {
            _terrainOffsetMin = offset;
        }

        public void SetOffsetMax(float offset)
        {
            _terrainOffsetMax = offset;
        }
    }
}
