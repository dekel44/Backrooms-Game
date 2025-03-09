using UnityEngine;
using UnityEngine.AI;
using Unity;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent nm;
    public Transform target;


    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        nm.SetDestination(target.position);

    }
}
