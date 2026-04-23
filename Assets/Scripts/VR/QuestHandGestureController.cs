using MyCasino.Slots.Core;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

namespace MyCasino.Slots.VR
{
    /// <summary>
    /// Gesture map (Meta Quest hand tracking):
    /// - Right index pinch: spin current slot.
    /// - Left hand pinch + horizontal swipe: switch slots.
    /// </summary>
    public class QuestHandGestureController : MonoBehaviour
    {
        [SerializeField] private VideoSlotMachine machine;
        [SerializeField] private SlotLobbyManager lobby;
        [SerializeField, Min(0.005f)] private float pinchDistanceThreshold = 0.025f;
        [SerializeField, Min(0.01f)] private float swipeDistanceThreshold = 0.08f;
        [SerializeField, Min(0.05f)] private float spinCooldownSeconds = 0.35f;

        private XRHandSubsystem _handSubsystem;
        private float _nextSpinTimestamp;
        private bool _leftPinchActive;
        private Vector3 _leftPinchStartPosition;

        private void OnEnable()
        {
            TryResolveHandSubsystem();
        }

        private void Update()
        {
            if (_handSubsystem == null || !_handSubsystem.running)
            {
                TryResolveHandSubsystem();
                return;
            }

            HandleRightSpinPinch();
            HandleLeftSwipeSelection();
        }

        private void HandleRightSpinPinch()
        {
            if (Time.time < _nextSpinTimestamp || machine == null)
            {
                return;
            }

            if (IsPinching(_handSubsystem.rightHand))
            {
                if (machine.TrySpin())
                {
                    _nextSpinTimestamp = Time.time + spinCooldownSeconds;
                }
            }
        }

        private void HandleLeftSwipeSelection()
        {
            if (lobby == null)
            {
                return;
            }

            var leftHand = _handSubsystem.leftHand;
            var leftPinching = IsPinching(leftHand);

            if (leftPinching && !_leftPinchActive && TryGetPinchCenter(leftHand, out var pinchStart))
            {
                _leftPinchActive = true;
                _leftPinchStartPosition = pinchStart;
                return;
            }

            if (!leftPinching)
            {
                _leftPinchActive = false;
                return;
            }

            if (!_leftPinchActive || !TryGetPinchCenter(leftHand, out var currentPinchPosition))
            {
                return;
            }

            var deltaX = currentPinchPosition.x - _leftPinchStartPosition.x;
            if (Mathf.Abs(deltaX) < swipeDistanceThreshold)
            {
                return;
            }

            if (deltaX > 0)
            {
                lobby.SelectNextSlot();
            }
            else
            {
                lobby.SelectPreviousSlot();
            }

            _leftPinchActive = false;
        }

        private bool IsPinching(XRHand hand)
        {
            if (!TryGetJointPose(hand, XRHandJointID.ThumbTip, out var thumbTip))
            {
                return false;
            }

            if (!TryGetJointPose(hand, XRHandJointID.IndexTip, out var indexTip))
            {
                return false;
            }

            return Vector3.Distance(thumbTip.position, indexTip.position) <= pinchDistanceThreshold;
        }

        private bool TryGetPinchCenter(XRHand hand, out Vector3 center)
        {
            center = default;
            if (!TryGetJointPose(hand, XRHandJointID.ThumbTip, out var thumbTip) ||
                !TryGetJointPose(hand, XRHandJointID.IndexTip, out var indexTip))
            {
                return false;
            }

            center = (thumbTip.position + indexTip.position) * 0.5f;
            return true;
        }

        private static bool TryGetJointPose(XRHand hand, XRHandJointID jointId, out Pose pose)
        {
            pose = default;
            if (!hand.isTracked)
            {
                return false;
            }

            var joint = hand.GetJoint(jointId);
            return joint.TryGetPose(out pose);
        }

        private void TryResolveHandSubsystem()
        {
            var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            if (loader == null)
            {
                return;
            }

            _handSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();
            if (_handSubsystem == null)
            {
                return;
            }

            if (!_handSubsystem.running)
            {
                _handSubsystem.Start();
            }
        }
    }
}
