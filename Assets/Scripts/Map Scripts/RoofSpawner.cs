using UnityEngine;

public class RoofSpawner : MonoBehaviour
{
    // Attach your quad prefab here in the Inspector.
    public GameObject roofPrefab;

    // Set these to match the number of cells in your maze.
    public float mazeWidth = 10f;
    public float mazeHeight = 10f;

    // Size of each cell. Adjust if your maze uses different spacing.
    public float cellSize = 1f;

    void Start()
    {
        if (roofPrefab == null)
        {
            Debug.LogError("Roof prefab is not assigned!");
            return;
        }

        // Loop through each cell coordinate.
        for (float x = -9.5f; x < mazeWidth - 9.5f; x++)
        {
            for (float y = -9.5f; y < mazeHeight - 9.5f; y++)
            {
                // Calculate spawn position for the roof.
                Vector3 spawnPosition = new Vector3(x * cellSize, 0, y * cellSize);

                // Instantiate the roof prefab at the cell position.
                Instantiate(roofPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}