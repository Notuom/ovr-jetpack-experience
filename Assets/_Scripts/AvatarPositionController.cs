using UnityEngine;

public class AvatarPositionController : MonoBehaviour
{
    public GameObject AnchorObject;

    private void Update()
    {
        var anchorPosition = AnchorObject.transform.position;
        transform.position = new Vector3(anchorPosition.x, anchorPosition.y - 1.5f, anchorPosition.z - 0.2f);
    }
}