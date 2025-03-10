using UnityEngine;
using UnityEngine.AI;
using Unity;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent nm;
    public Transform target;

    // Minimal addition: reference to an AudioSource for footsteps
    public AudioSource walkingSound;

    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nm.SetDestination(target.position);

        // --- Footstep logic (added) ---
        if (nm.velocity.magnitude > 0.1f)
        {
            if (!walkingSound.isPlaying) walkingSound.Play();
        }
        else
        {
            if (walkingSound.isPlaying) walkingSound.Stop();
        }
    }
}
