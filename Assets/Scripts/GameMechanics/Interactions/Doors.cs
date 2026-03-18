using System;
using Core;
using DG.Tweening;
using GameMechanics.Interactions;
using GameMechanics.Player;
using UnityEngine;

namespace GameMechanics
{
    [RequireComponent(typeof(Collider))]
    public class Doors : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform leftDoor, rightDoor;
        private Collider _triggerCollider;
        [SerializeField] private float animationAngle;
        [SerializeField] private float animationDuration;
        private Quaternion _initialLeftRotation, _initialRightRotation;
        private bool _doorsOpened;

        private void Awake()
        {
            _initialLeftRotation = leftDoor.rotation;
            _initialRightRotation = rightDoor.rotation;
            _triggerCollider = GetComponent<Collider>();
        }

        public string GetName()
        {
            return "Open the door";
        }

        public void Interact()
        {
            if (_doorsOpened)
            {
                _doorsOpened = false;
                AnimateDoors(false, 0);
                return;
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;

            int direction = Vector3.Dot(forward, toPlayer) < 0 ? 1 : -1;

            _doorsOpened = true;
            AnimateDoors(true, direction);
        }

        private void AnimateDoors(bool open, int direction)
        {
            _triggerCollider.enabled = false;

            Quaternion leftTarget;
            Quaternion rightTarget;

            if (open)
            {
                leftTarget = _initialLeftRotation * Quaternion.AngleAxis(-animationAngle * direction, Vector3.up);
                rightTarget = _initialRightRotation * Quaternion.AngleAxis(animationAngle * direction, Vector3.up);
            }
            else
            {
                leftTarget = _initialLeftRotation;
                rightTarget = _initialRightRotation;
            }

            Sequence seq = DOTween.Sequence();

            seq.Join(leftDoor.DORotateQuaternion(leftTarget, animationDuration));
            seq.Join(rightDoor.DORotateQuaternion(rightTarget, animationDuration));

            seq.AppendCallback(() =>
            {
                var sound = open ? FMODEvents.Instance.saveBoardOpen : FMODEvents.Instance.saveBoardHover;
                AudioManager.Instance.PlayOneShot(sound, transform.position);

                _triggerCollider.enabled = true;
            });
        }
    }
}
