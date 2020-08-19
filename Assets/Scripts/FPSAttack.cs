using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MizJam
{
    public class FPSAttack : MonoBehaviour
    {
        private struct BonePositioning
        {
            public Quaternion armsRotation;
            public Quaternion leftArmRotation;
            public Quaternion rightArmRotation;
            public Vector3 leftHandPosition;
            public Vector3 rightHandPosition;

            public static BonePositioning Lerp(BonePositioning a, BonePositioning b, float t)
            {
                t = Mathf.Clamp01(t);
                return new BonePositioning
                {
                    armsRotation = Quaternion.Lerp(a.armsRotation, b.armsRotation, t),
                    leftArmRotation = Quaternion.Lerp(a.leftArmRotation, b.leftArmRotation, t),
                    rightArmRotation = Quaternion.Lerp(a.rightArmRotation, b.rightArmRotation, t),
                    leftHandPosition = Vector3.Lerp(a.leftHandPosition, b.leftHandPosition, t),
                    rightHandPosition = Vector3.Lerp(a.rightHandPosition, b.rightHandPosition, t)
                };
            }
        }

        [SerializeField]
        private float attackRange = 12.0f;

        [SerializeField]
        private float attackRadius = 6.0f;

        [SerializeField]
        private float shoulderSize = 4.0f;

        [SerializeField]
        private float restArmSize = 5.0f;

        [SerializeField]
        private float timeToPrepare = 0.75f;

        [SerializeField]
        private float timeToAttack = 0.25f;

        [SerializeField]
        private float timeBeforeRest = 0.25f;

        [SerializeField]
        private float timeToRest = 0.25f;

        [SerializeField]
        private LayerMask attackable, enemies;

        [SerializeField]
        private Transform arms, leftArm, rightArm, leftHand, rightHand;

        [SerializeField]
        private new Camera camera;

        [SerializeField]
        private GameObject smashMarkPrefab;

        [SerializeField]
        private GameObject target;


        private bool isAttacking = false;
        private float counter = 0.0f;
        private BonePositioning bonePositioning;
        private Color targetColor;
        private Color targetOffColor;

        private void Awake()
        {
            this.bonePositioning = this.GetRestBonePositioning();
            this.targetColor = this.target.GetComponentInChildren<MeshRenderer>().material.GetColor("_BaseColor");
            this.targetOffColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }

        private void Update()
        {
            if (!this.isAttacking && Input.GetMouseButtonDown(0))
                StartCoroutine(this.AttackCoroutine());

            this.arms.localRotation = this.bonePositioning.armsRotation;
            this.leftArm.localRotation = this.bonePositioning.leftArmRotation;
            this.rightArm.localRotation = this.bonePositioning.rightArmRotation;
            this.leftHand.localPosition = this.bonePositioning.leftHandPosition;
            this.rightHand.localPosition = this.bonePositioning.rightHandPosition;

            Ray ray = new Ray(this.camera.transform.position, this.camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, this.attackable))
            {
                var material = this.target.GetComponentInChildren<MeshRenderer>().material;
                material.SetColor("_BaseColor", hit.distance <= this.attackRange ? this.targetColor : this.targetOffColor);

                this.target.transform.position = hit.point + 0.01f * hit.normal;
                this.target.transform.up = hit.normal;

                this.target.SetActive(true);
            } else
            {
                this.target.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            this.transform.localRotation = this.camera.transform.localRotation;
            this.leftHand.rotation = Quaternion.FromToRotation(this.leftHand.up, Vector3.up) * this.leftHand.rotation;
            this.rightHand.rotation = Quaternion.FromToRotation(this.rightHand.up, Vector3.up) * this.rightHand.rotation;
        }

        private IEnumerator AttackCoroutine()
        {
            this.isAttacking = true;

            float t = 0;
            this.counter = 0.0f;

            while (t <= 1.0f)
            {
                this.counter += Time.deltaTime;
                t = this.counter / this.timeToPrepare;
                this.bonePositioning = BonePositioning.Lerp(this.GetRestBonePositioning(), this.GetPrepareBonePositioning(), t);
                yield return null;
            }


            t = 0.0f;
            this.counter = 0.0f;

            while (t <= 1.0f)
            {
                this.counter += Time.deltaTime;
                t = this.counter / this.timeToAttack;
                this.bonePositioning = BonePositioning.Lerp(this.GetPrepareBonePositioning(), this.GetAttackBonePositioning(), t);
                yield return null;
            }

            this.Attack();

            t = 0.0f;
            this.counter = 0.0f;

            while (t <= 1.0f)
            {
                this.counter += Time.deltaTime;
                t = this.counter / this.timeBeforeRest;
                yield return null;
            }


            t = 0.0f;
            this.counter = 0.0f;
            
            while (t <= 1.0f)
            {
                this.counter += Time.deltaTime;
                t = this.counter / this.timeToRest;
                this.bonePositioning = BonePositioning.Lerp(this.GetAttackBonePositioning(), this.GetRestBonePositioning(), t);
                yield return null;
            }

            this.isAttacking = false;
        }

        private Vector3 GetAttackPosition()
        {
            Ray ray = new Ray(this.camera.transform.position, this.camera.transform.forward);
            Vector3 attackPosition = ray.GetPoint(this.attackRange);

            if (Physics.Raycast(ray, out RaycastHit hit, this.attackRange, this.attackable))
            {
                if (hit.distance < this.attackRange)
                    attackPosition = hit.point;
            }

            return attackPosition;
        }

        private void Attack()
        {
            Ray ray = new Ray(this.camera.transform.position, this.camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, this.attackRange, this.attackable))
            {
                if (hit.distance <= this.attackRange)
                {
                    var mark = Instantiate(this.smashMarkPrefab);
                    mark.transform.position = hit.point + 0.01f * hit.normal;
                    mark.transform.up = hit.normal;
                    mark.transform.Rotate(0.0f, Random.Range(0, 90.0f), 0.0f, Space.Self);

                    IEnumerable<Enemy> inRange = Physics.OverlapSphere(hit.point, this.attackRadius, this.enemies).Select(el => el.GetComponent<Enemy>()).Where(el => el != null);
                    foreach (Enemy enemy in inRange)
                        enemy.SufferImpact(hit.point);
                }
            }
        }


        private BonePositioning GetRestBonePositioning()
        {
            return new BonePositioning
            {
                armsRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f),
                leftArmRotation = Quaternion.identity,
                rightArmRotation = Quaternion.identity,
                leftHandPosition = this.restArmSize * Vector3.forward,
                rightHandPosition = this.restArmSize * Vector3.forward
            };
        }

        private BonePositioning GetPrepareBonePositioning()
        {
            Vector3 attackPosition = this.GetAttackPosition();
            float distance = Vector3.Distance(this.transform.position, attackPosition);
            float a2 = Mathf.Pow(this.shoulderSize / 2.0f, 2.0f);
            float b2 = Mathf.Pow(distance, 2.0f);
            float armSize = Mathf.Sqrt(a2 + b2);
            float armAngle = Mathf.Rad2Deg * Mathf.Acos(distance / armSize);

            return new BonePositioning
            {
                armsRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f),
                leftArmRotation = Quaternion.Euler(0.0f, armAngle - 7.5f, 0.0f),
                rightArmRotation = Quaternion.Euler(0.0f, -armAngle + 7.5f, 0.0f),
                leftHandPosition = armSize * Vector3.forward,
                rightHandPosition = armSize * Vector3.forward
            };
        }

        private BonePositioning GetAttackBonePositioning()
        {
            Vector3 attackPosition = this.GetAttackPosition();
            float distance = Vector3.Distance(this.transform.position, attackPosition);
            float a2 = Mathf.Pow(this.shoulderSize / 2.0f, 2.0f);
            float b2 = Mathf.Pow(distance, 2.0f);
            float armSize = Mathf.Sqrt(a2 + b2);
            float armAngle = Mathf.Rad2Deg * Mathf.Acos(distance / armSize);
            float xRotation = Vector3.SignedAngle(this.transform.forward, attackPosition - this.transform.position, Vector3.right);

            return new BonePositioning
            {
                armsRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f),
                leftArmRotation = Quaternion.Euler(0.0f, armAngle - 7.5f, 0.0f),
                rightArmRotation = Quaternion.Euler(0.0f, -armAngle + 7.5f, 0.0f),
                leftHandPosition = armSize * Vector3.forward,
                rightHandPosition = armSize * Vector3.forward
            };
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.camera.transform.position, this.attackRange);
        }
#endif
    }
}