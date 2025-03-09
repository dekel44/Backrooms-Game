using UnityEngine;
using Unity.AI.Navigation; 

public class NavMesh : MonoBehaviour
{
    [SerializeField] NavMeshSurface navSurface;

    void Start()
    {
        Invoke(nameof(BuildNav), 1f);
    }

    void BuildNav()
    {
        navSurface.BuildNavMesh();
        Debug.Log("Code did run");
    }
}
