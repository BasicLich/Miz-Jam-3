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
        private bool canGrab = true;
        private bool canThrow = false;

        private void Update()
        {
            if (this.canGrab && Input.GetMouseButtonDown(1))
            {
                if(this.grabbed && this.canThrow)
                    this.Throw();
                else
                    this.TryGrab();
            }
        }

        private void TryGrab()
        {
            Ray ray = new Ray(this.camera.transform.position, this.camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, this.grabDistance, this.grabbable))
            {
                Grabbable g = hit.collider.GetComponentInParent<Grabbable>();
                if (g)
                    this.Grab(g);
            }
        }

        private void Grab(Grabbable toGrab)
        {
            toGrab.GetGrabbed();

            this.canThrow = false;
            this.grabbed = toGrab;
            toGrab.transform.SetParent(this.holdPosition);
            toGrab.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => this.canThrow = true);

            this.SendMessage("OnGrab");
        }

        private void Throw()
        {
            this.grabbed.transform.SetParent(null);
            this.grabbed.GetThrown(this.throwForce * this.transform.forward);
            this.grabbed = null;

            this.SendMessage("OnThrow");
        }

        private void OnAttackBegin()
        {
            this.canGrab = false;
        }

        private void OnAttackEnd()
        {
            this.canGrab = true;
        }
    }
}