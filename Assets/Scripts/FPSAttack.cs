﻿using System.Collections;
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
        private Transform arms, leftArm, rightArm, leftHand, rightHand;

        [SerializeField]
        private new Camera camera;



        private bool isAttacking = false;
        private int layerMask;
        private float counter = 0.0f;
        private BonePositioning bonePositioning;

        private void Awake()
        {
            this.layerMask = ~LayerMask.NameToLayer("Player");
            this.bonePositioning = this.GetRestBonePositioning();
        }

        private void Update()
        {
            this.arms.localRotation = this.bonePositioning.armsRotation;
            this.leftArm.localRotation = this.bonePositioning.leftArmRotation;
            this.rightArm.localRotation = this.bonePositioning.rightArmRotation;
            this.leftHand.localPosition = this.bonePositioning.leftHandPosition;
            this.rightHand.localPosition = this.bonePositioning.rightHandPosition;

            if (!this.isAttacking && Input.GetMouseButtonDown(0))
                StartCoroutine(this.AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            this.isAttacking = true;

            float t = 0;
            this.counter = 0.0f;

            while (t <= 1.0f)
            {
                this.bonePositioning = BonePositioning.Lerp(this.GetRestBonePositioning(), this.GetPrepareBonePositioning(), t);
                this.counter += Time.deltaTime;
                t = this.counter / this.timeToPrepare;
                yield return null;
            }


            t = 0.0f;
            this.counter = 0.0f;

            while (t <= 1.0f)
            {
                this.bonePositioning = BonePositioning.Lerp(this.GetPrepareBonePositioning(), this.GetAttackBonePositioning(), t);
                this.counter += Time.deltaTime;
                t = this.counter / this.timeToAttack;
                yield return null;
            }


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
                this.bonePositioning = BonePositioning.Lerp(this.GetAttackBonePositioning(), this.GetRestBonePositioning(), t);
                this.counter += Time.deltaTime;
                t = this.counter / this.timeToRest;
                yield return null;
            }

            this.isAttacking = false;
        }

        private Vector3 GetAttackPosition()
        {
            Ray ray = new Ray(this.camera.transform.position, this.camera.transform.forward);
            Vector3 attackPosition = ray.GetPoint(this.attackRange);

            if (Physics.Raycast(ray, out RaycastHit hit, this.attackRange, this.layerMask))
            {
                if (hit.distance < this.attackRange)
                    attackPosition = hit.point;
            }

            return attackPosition;
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
                leftArmRotation = Quaternion.Euler(0.0f, armAngle, 0.0f),
                rightArmRotation = Quaternion.Euler(0.0f, -armAngle, 0.0f),
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
                leftArmRotation = Quaternion.Euler(0.0f, armAngle, 0.0f),
                rightArmRotation = Quaternion.Euler(0.0f, -armAngle, 0.0f),
                leftHandPosition = armSize * Vector3.forward,
                rightHandPosition = armSize * Vector3.forward
            };
        }
    }
}