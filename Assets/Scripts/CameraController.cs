using UnityEngine;
public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 50f;

    void Update()
    {
        // Movimiento de la cámara
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * panSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * panSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * panSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * panSpeed * Time.deltaTime);

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * zoomSpeed, Space.Self);

        // Rotación
        if (Input.GetMouseButton(1)) // Click derecho
        {
            float rotateX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotateY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotateX, Space.World);
            transform.Rotate(Vector3.left, rotateY, Space.Self);
        }
    }
}
