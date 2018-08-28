using UnityEngine;

public class Player2_Cam : MonoBehaviour
{
    [SerializeField]  float Y_ANGLE_MIN = -30.0f;
    [SerializeField]  float Y_ANGLE_MAX = 40.0f;

    [SerializeField] float sensitivityX = 40.0f;
    [SerializeField] float sensitivityY = 40.0f;

    Transform camTransform;

    public Transform lookAt;
    public float distance = 2.0f;

    private float currX ;
    private float currY ;

    private void Start()
    {
        camTransform = transform;
    }

    public void moveCamera(float x, float y)
    {
        currX += x * sensitivityX;
        currY += y * sensitivityY;

        currY = Mathf.Clamp(currY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        Vector3 dir = new Vector3(0, 0, distance);
        Quaternion rotation = Quaternion.Euler(-currY, currX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
