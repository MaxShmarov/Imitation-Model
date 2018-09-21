
using UnityEngine;

namespace LittleWorld
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private bool _setCameraAsTarget = false;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            if (_target == null && _setCameraAsTarget)
            {
                _target = Camera.allCameras[0].transform;
            }
        }

        private void LateUpdate()
        {
            _transform.rotation = Quaternion.LookRotation(_target.forward, _target.up);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}
