using UnityEngine;

public class FluidSimulator : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public float cellSize = 1f;
    public float maxFluidLevel = 1f; // Máximo nivel de fluido por celda
    public float fluidDecay = 0.005f; // Pérdida de fluido por frame

    public GameObject fluidPrefab; // Prefab para representar visualmente el fluido
    public GameObject obstaclePrefab;

    private float[,] grid;
    private GameObject[,] fluidVisuals; // Almacena los objetos visuales del fluido
    private BoxCollider boxCollider;

    private void Start()
    {
        grid = new float[gridSizeX, gridSizeY];
        fluidVisuals = new GameObject[gridSizeX, gridSizeY];
        boxCollider = GetComponent<BoxCollider>();

        InitializeFluidVisuals();
        AdjustBoxCollider(); // Ajustar el BoxCollider según el tamaño de la grilla
    }

    private void Update()
    {
        SimulateFlow();
        UpdateFluidVisuals();
        HandleUserInput();
    }

    private void SimulateFlow()
    {
        float[,] newGrid = (float[,])grid.Clone();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y] > 0)
                {
                    float amount = grid[x, y] * 0.25f;
                    SpreadFluid(x, y, newGrid, amount);
                }

                // Aplicar pérdida de fluido
                newGrid[x, y] = Mathf.Max(newGrid[x, y] - fluidDecay, 0);
            }
        }

        grid = newGrid;
    }

    private void SpreadFluid(int x, int y, float[,] newGrid, float amount)
    {
        TransferFluid(x, y, x - 1, y, newGrid, amount); // Izquierda
        TransferFluid(x, y, x + 1, y, newGrid, amount); // Derecha
        TransferFluid(x, y, x, y - 1, newGrid, amount); // Abajo
        TransferFluid(x, y, x, y + 1, newGrid, amount); // Arriba

        newGrid[x, y] -= amount * 4;
    }

    private void TransferFluid(int fromX, int fromY, int toX, int toY, float[,] newGrid, float amount)
    {
        if (toX >= 0 && toX < gridSizeX && toY >= 0 && toY < gridSizeY && grid[toX, toY] >= 0)
        {
            float spaceAvailable = maxFluidLevel - newGrid[toX, toY];
            float transferAmount = Mathf.Min(amount, spaceAvailable);
            newGrid[toX, toY] += transferAmount;
        }
    }

    private void InitializeFluidVisuals()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                fluidVisuals[x, y] = Instantiate(fluidPrefab, position, Quaternion.identity);
                fluidVisuals[x, y].transform.localScale = Vector3.zero; // Inicialmente sin agua visible
            }
        }
    }

    private void UpdateFluidVisuals()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y] > 0)
                {
                    Vector3 scale = new Vector3(cellSize, grid[x, y], cellSize);
                    fluidVisuals[x, y].transform.localScale = scale;
                }
                else
                {
                    fluidVisuals[x, y].transform.localScale = Vector3.zero;
                }
            }
        }
    }

    private void HandleUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 worldPoint = hit.point;
                int x = Mathf.FloorToInt(worldPoint.x / cellSize);
                int y = Mathf.FloorToInt(worldPoint.z / cellSize);

                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && grid[x, y] >= 0)
                {
                    Instantiate(obstaclePrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
                    grid[x, y] = -1; // Marcar como obstáculo
                }
            }
        }
    }

    private void AdjustBoxCollider()
    {
        if (boxCollider == null)
            return;

        // Ajustar el tamaño y posición del BoxCollider
        boxCollider.size = new Vector3(gridSizeX * cellSize, 0.1f, gridSizeY * cellSize);
        boxCollider.center = new Vector3((gridSizeX * cellSize) / 2 - cellSize / 2, 0, (gridSizeY * cellSize) / 2 - cellSize / 2);
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
            return;

        Gizmos.color = Color.white;
        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int y = 0; y <= gridSizeY; y++)
            {
                Vector3 start = new Vector3(x * cellSize, 0, 0);
                Vector3 end = new Vector3(x * cellSize, 0, gridSizeY * cellSize);
                Gizmos.DrawLine(start, end);

                start = new Vector3(0, 0, y * cellSize);
                end = new Vector3(gridSizeX * cellSize, 0, y * cellSize);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
