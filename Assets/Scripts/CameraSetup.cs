using UnityEngine;
public class CameraSetup : MonoBehaviour
{
    public Camera mainCamera;
    public FluidSimulator simulator; // Referencia al script del simulador

    void Start()
    {
        if (mainCamera == null || simulator == null) return;

        float gridWidth = simulator.gridSizeX * simulator.cellSize;
        float gridHeight = simulator.gridSizeY * simulator.cellSize;

        // Centrar la cámara sobre la grilla y ajustar la altura
        mainCamera.transform.position = new Vector3(gridWidth / 2, 20, gridHeight / 2);
        mainCamera.transform.rotation = Quaternion.Euler(60, 0, 0); // Vista oblicua desde arriba
    }
}
