using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusTouchInteractions : MonoBehaviour
{
    private const float ThrustSpeed = 5.0f;

    public GameObject CameraGameObject;
    public OVRInput.Controller LeftController;
    public OVRInput.Controller RightController;

    // Debug items
    public Boolean DebugEnabled;

    public GameObject DebugLeftArrow;
    public GameObject DebugRightArrow;
    public GameObject DebugAverageArrow;

    private Rigidbody _rb;
    private Quaternion _touchRotationFix;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Rotate the value retrieved from the touch controller in order to send the player in the right direction
        _touchRotationFix = Quaternion.Euler(-45, 0, 45);
    }

    private void FixedUpdate()
    {
        var leftTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, LeftController);
        var rightTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, RightController);
        Quaternion leftControllerRotation = Quaternion.identity,
            rightControllerRotation = Quaternion.identity,
            averageControllerRotation = Quaternion.identity;

        if (DebugEnabled)
        {
            DebugAverageArrow.SetActive(false);
            DebugLeftArrow.SetActive(false);
            DebugRightArrow.SetActive(false);
        }

        if (leftTriggerInput > 0)
        {
            leftControllerRotation = OVRInput.GetLocalControllerRotation(LeftController) * _touchRotationFix;
            averageControllerRotation = leftControllerRotation;
            _rb.AddForce(leftControllerRotation * Vector3.one * leftTriggerInput * ThrustSpeed);

            if (DebugEnabled)
            {
                DebugLeftArrow.SetActive(true);
                DebugAverageArrow.SetActive(true);
                Debug.DrawLine(Vector3.zero, leftControllerRotation * Vector3.one, Color.red);
                DebugLeftArrow.transform.rotation = leftControllerRotation;
            }
        }

        if (rightTriggerInput > 0)
        {
            rightControllerRotation = OVRInput.GetLocalControllerRotation(RightController) * _touchRotationFix;
            averageControllerRotation = rightControllerRotation;
            _rb.AddForce(rightControllerRotation * Vector3.one * rightTriggerInput * ThrustSpeed);

            if (DebugEnabled)
            {
                DebugRightArrow.SetActive(true);
                DebugAverageArrow.SetActive(true);
                Debug.DrawLine(Vector3.zero, rightControllerRotation * Vector3.one, Color.blue);
                DebugRightArrow.transform.rotation = rightControllerRotation;
            }
        }

        if (leftTriggerInput > 0 && rightTriggerInput > 0)
        {
            averageControllerRotation = Quaternion.Lerp(leftControllerRotation, rightControllerRotation, 0.5f);
        }
        if (DebugEnabled && (leftTriggerInput > 0 || rightTriggerInput > 0))
        {
            DebugAverageArrow.transform.rotation = averageControllerRotation;
            Debug.DrawLine(Vector3.zero, averageControllerRotation * Vector3.one, Color.white);
        }
    }
}