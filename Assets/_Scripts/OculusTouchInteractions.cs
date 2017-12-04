using System;
using UnityEngine;

public class OculusTouchInteractions : MonoBehaviour
{
    // Configurable values

    public float ThrustForceMultiplier;
    public float MaxSpeed;
    public float RotateAngleStep;

    // GameObject references

    public GameObject ThrustSoundGameObject;
    public GameObject SmokeGameObject;
    public GameObject RingManagerGameObject;

    // OVRInput References

    public OVRInput.Controller LeftController;
    public OVRInput.Controller RightController;
    public OVRInput.RawButton RotateLeftButton;
    public OVRInput.RawButton RotateRightButton;

    // Constants / read-only variables
    private static readonly Quaternion TouchRotationFix = Quaternion.Euler(-45, 0, 45);

    // Component references

    private Rigidbody _rb;
    private ParticleSystem _ps;
    private AudioSource _thrustAudioSource;
    private RingManagerController _ringManager;

    // Local properties
    private Boolean _thrusting;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ps = SmokeGameObject.GetComponent<ParticleSystem>();
        _thrustAudioSource = ThrustSoundGameObject.GetComponent<AudioSource>();
        _ringManager = RingManagerGameObject.GetComponent<RingManagerController>();
    }

    private void Update()
    {
        HandleCameraMovement();
    }

    private void FixedUpdate()
    {
        HandleJetpackMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ring"))
        {
            _ringManager.CollectRing();
        }
    }

    private void HandleJetpackMovement()
    {
        var leftTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, LeftController);
        var rightTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, RightController);

        if (leftTriggerInput > 0)
        {
            var leftControllerRotation = OVRInput.GetLocalControllerRotation(LeftController) * TouchRotationFix;
            _rb.AddForce(transform.rotation * leftControllerRotation * Vector3.one * leftTriggerInput *
                         ThrustForceMultiplier);
        }

        if (rightTriggerInput > 0)
        {
            var rightControllerRotation = OVRInput.GetLocalControllerRotation(RightController) * TouchRotationFix;
            _rb.AddForce(transform.rotation * rightControllerRotation * Vector3.one * rightTriggerInput *
                         ThrustForceMultiplier);
        }

        if (leftTriggerInput > 0 || rightTriggerInput > 0)
        {
            _ps.Emit(1);
            if (!_thrusting)
            {
                _thrusting = true;
                _thrustAudioSource.Play();
            }
        }
        else if (_thrusting)
        {
            _thrusting = false;
            _thrustAudioSource.Stop();
        }
    }

    private void HandleCameraMovement()
    {
        if (OVRInput.GetDown(RotateLeftButton))
        {
            transform.Rotate(0, -RotateAngleStep, 0);
        }
        else if (OVRInput.GetDown(RotateRightButton))
        {
            transform.Rotate(0, RotateAngleStep, 0);
        }
    }
}