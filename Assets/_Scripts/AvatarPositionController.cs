using UnityEngine;

public class AvatarPositionController : MonoBehaviour
{
    public GameObject AnchorObject;

    private CapsuleCollider _cc;

    private void Update()
    {
        // Set position relative to another object
        var anchorPosition = AnchorObject.transform.position;
        transform.position = new Vector3(anchorPosition.x, anchorPosition.y - 1.5f, anchorPosition.z);

        // Set Y rotation relative to another object
        var anchorAngles = AnchorObject.transform.rotation.eulerAngles;
        var eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, anchorAngles.y, eulerAngles.z);
    }
}