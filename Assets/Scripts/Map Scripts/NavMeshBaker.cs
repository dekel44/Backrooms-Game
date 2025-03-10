using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshBake : MonoBehaviour
{
    public NavMeshSurface surface; // Reference to the NavMeshSurface component

    // Call this method after spawning your walls
    public void BakeNavMesh()
    {
        if (surface != null)
        {
            surface.BuildNavMesh();
            Debug.Log("NavMesh baked successfully!");
        }
        else
        {
            Debug.LogError("NavMeshSurface reference is missing!");
        }
    }
}