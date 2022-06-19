using UnityEngine;

public class ActorCameraOrbit : MonoBehaviour
{
    [SerializeField, Header("左右に回転するか")]
    private bool isRotatableHorizontal;

    [SerializeField, Header("上下に回転するか")]
    private bool isRotatableVertical;

    [SerializeField, Header("左右回転用の向きのオフセット")]
    private int horizontalOffset = 1;

    [SerializeField, Header("上下回転用の向きの」オフセット")]
    private int verticalOffset = -1;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float axisX  = Input.GetAxis("Mouse X");
            float axisY  = Input.GetAxis("Mouse Y");
            float angleX = isRotatableHorizontal ? axisX * horizontalOffset : 0;
            float angleY = isRotatableVertical ? axisY   * verticalOffset : 0;

            transform.eulerAngles += new Vector3(angleY, angleX);
        }
    }
}
