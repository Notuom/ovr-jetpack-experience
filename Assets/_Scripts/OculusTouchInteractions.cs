using System;
using UnityEngine;

public class OculusTouchInteractions : MonoBehaviour
{
    // Configurable values
    public float ThrustForceMultiplier;
    
    // GameObject references
    public GameObject ThrustSoundGameObject;
    public GameObject SmokeGameObject;
    public GameObject RingManagerGameObject;
    
    // OVRInput References
    public OVRInput.Controller LeftController;
    public OVRInput.Controller RightController;

    // Debug items
    public Boolean DebugEnabled;
    public GameObject DebugLeftArrow;
    public GameObject DebugRightArrow;
    public GameObject DebugAverageArrow;

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
            leftControllerRotation = OVRInput.GetLocalControllerRotation(LeftController) * TouchRotationFix;
            averageControllerRotation = leftControllerRotation;
            _rb.AddForce(leftControllerRotation * Vector3.one * leftTriggerInput * ThrustForceMultiplier);

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
            rightControllerRotation = OVRInput.GetLocalControllerRotation(RightController) * TouchRotationFix;
            averageControllerRotation = rightControllerRotation;
            _rb.AddForce(rightControllerRotation * Vector3.one * rightTriggerInput * ThrustForceMultiplier);

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
        if (leftTriggerInput > 0 || rightTriggerInput > 0)
        {
            _ps.Emit(1);
            if (!_thrusting)
            {
                _thrusting = true;
                _thrustAudioSource.Play();
            }
            if (DebugEnabled)
            {
                DebugAverageArrow.transform.rotation = averageControllerRotation;
                Debug.DrawLine(Vector3.zero, averageControllerRotation * Vector3.one, Color.white);
            }
        }
        else if (_thrusting)
        {
            _thrusting = false;
            _thrustAudioSource.Stop();
        }
    }
}