using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 30;             // 30x30 grid
    public float cellSize = 1f;          // Each cell is 1 unit wide
    public GameObject zombiePrefab;      // Drag your Zombie prefab here

    void Start()
    {
        // Randomly choose how many zombies to spawn (1 to 3)
        int zombieCount = Random.Range(1, 4); // min inclusive, max exclusive => 1..3

        for (int i = 0; i < zombieCount; i++)
        {
            // Pick a random cell in the 30x30
            int x = Random.Range(0, gridSize); // 0..29
            int z = Random.Range(0, gridSize); // 0..29

            // Center of the chosen cell => ( x+0.5, z+0.5 ) if each cell is 1x1
            // If your planes are scaled differently, adjust 'cellSize' or offset.
            Vector3 spawnPos = new Vector3(
                x * cellSize + cellSize / 2f,
                0f,
                z * cellSize + cellSize / 2f
            );

            // Instantiate the zombie
            Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        }
    }
}
