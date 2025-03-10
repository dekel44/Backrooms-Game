using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent nm;             // Should be on the same GameObject as this script
    public AudioSource walkingSound;    // Footstep or shuffle sound
    public Animator animator;           // Zombie's Animator
    public Transform target;            // Will auto-find the real player if not assigned

    [Header("Zombie Stats")]
    public float health = 100f;

    [Header("Animations")]
    public string walkAnimationName = "ZombieWalk";
    public string attackAnimationName = "ZombieAttack";
    public string[] deathAnimations;  // multiple possible death states

    [Header("Attack Settings")]
    public float attackRange = 2f;  // distance at which zombie attacks

    private bool isDead = false;

    void Start()
    {
        // If no NavMeshAgent assigned in Inspector, try auto-getting it
        if (!nm) nm = GetComponent<NavMeshAgent>();

        // If no target assigned, find the player by tag at runtime
        if (!target)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("No GameObject tagged 'Player' found in scene!");
            }
        }
    }

    void Update()
    {
        if (isDead) return; // No behavior once dead

        // If there's no target at all, do nothing
        if (!target)
        {
            nm.isStopped = true;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Attack if close enough
        if (distanceToTarget <= attackRange)
        {
            nm.isStopped = true; // freeze agent
            animator.Play(attackAnimationName);

            // Stop walking sound
            if (walkingSound && walkingSound.isPlaying)
                walkingSound.Stop();
        }
        else
        {
            // Otherwise, chase the target
            nm.isStopped = false;
            nm.SetDestination(target.position);

            // If actively moving, play footsteps & walk anim
            if (nm.velocity.magnitude > 0.1f)
            {
                if (walkingSound && !walkingSound.isPlaying)
                    walkingSound.Play();

                animator.Play(walkAnimationName);
            }
            else
            {
                if (walkingSound && walkingSound.isPlaying)
                    walkingSound.Stop();
            }
        }
    }

    /// <summary>
    /// Called by external sources to damage the zombie
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; // Already dead, ignore

        health -= damageAmount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        nm.isStopped = true; // stop moving
        if (walkingSound && walkingSound.isPlaying)
            walkingSound.Stop();

        // Random death anim from your list
        if (deathAnimations != null && deathAnimations.Length > 0)
        {
            int index = Random.Range(0, deathAnimations.Length);
            animator.Play(deathAnimations[index]);
        }
        else
        {
            // Fallback animation name
            animator.Play("ZombieDeath");
        }
        // Optionally destroy object after a delay
        // Destroy(gameObject, 5f);
    }

    /// <summary>
    /// Animation Event method for the Attack Animation.
    /// Ensure you add an event in the "ZombieAttack" clip that calls OnAttackHit().
    /// </summary>
    public void OnAttackHit()
    {
        // If the zombie is close enough at the moment of impact,
        // the player takes damage
        if (!target) return; // No target, no damage
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            // The player's script must have a TakeDamage method
            // (e.g., FullFPSController.TakeDamage(10f)).
            FullFPSController playerScript = target.GetComponent<FullFPSController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(10f);
            }
        }
    }
}
