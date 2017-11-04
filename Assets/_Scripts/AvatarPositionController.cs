using UnityEngine;

public class AvatarPositionController : MonoBehaviour
{
    public GameObject AnchorObject;

    private void Update()
    {
        var anchorPosition = AnchorObject.transform.position;
        transform.position = new Vector3(anchorPosition.x, anchorPosition.y - 1.5f, anchorPosition.z);
        var anchorAngles = AnchorObject.transform.rotation.eulerAngles;
        var eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, anchorAngles.y, eulerAngles.z);
    }
}