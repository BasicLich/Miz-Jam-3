using DG.Tweening;
using UnityEngine;

namespace MizJam
{
    public class FPSGrabber : MonoBehaviour
    {
        [SerializeField]
        private float grabDistance, throwForce;

        [SerializeField]
        private LayerMask grabbable;

        [SerializeField]
        private Transform holdPosition;

        [SerializeField]
        private new Camera camera;

        private Grabbable grabbed;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if(this.grabbed)
                    this.Throw();
                else
                    this.TryGrab();
            }
        }

        private void TryGrab()
        {
            Ray ray = new Ray(this.camera.transform.position, this.camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, this.grabDistance, this.grabbable))
                this.Grab(hit.collider.GetComponentInParent<Grabbable>());
        }

        private void Grab(Grabbable toGrab)
        {
            toGrab.GetGrabbed();

            this.grabbed = toGrab;
            toGrab.transform.SetParent(this.holdPosition);
            toGrab.transform.DOLocalMove(Vector3.zero, 0.5f);
        }
        private void Throw()
        {
            this.grabbed.transform.SetParent(null);
            this.grabbed.GetThrown(this.throwForce * this.transform.forward);
            this.grabbed = null;
        }
    }
}