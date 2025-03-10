using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;

    public NavMeshBake navMeshBaker;

    private void Start()
    {
        BeginGame();
    }

    private void BeginGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;

        yield return StartCoroutine(mazeInstance.Generate());

        navMeshBaker.BakeNavMesh();
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}