using UnityEngine;

namespace Mono
{
    public class CameraFollow : MonoBehaviour {

        public Transform target;

        public float smoothSpeed = 0.05f;
        public Vector3 offset;

        private void FixedUpdate ()
        {
            var desiredPosition = target.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

    }
}