using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform player;
    float cameraDistance = 50.0f;

    public void Awake()
    {
        GetComponent<Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }

    public void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
