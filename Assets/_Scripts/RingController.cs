using UnityEngine;

public class RingController : MonoBehaviour
{
    public float SpinSpeed;
    
    // Update is called once per frame
    void Update()
    {
        // Spin at given speed
        transform.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime);
    }
}