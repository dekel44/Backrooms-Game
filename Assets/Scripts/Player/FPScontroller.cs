using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FullFPSController : MonoBehaviour
{
    [Header("Camera & Movement")]
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Look Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Footsteps")]
    public AudioSource walkingAudioSource; // Assign a looping footstep clip

    [Header("Health Settings")]
    public float maxHealth = 100f;
    [Tooltip("Current HP of the player; updated at runtime.")]
    public float currentHealth = 100f;
    private bool isDead = false;

    // Reference to the health bar (if you want a UI fill bar).
    public Slider hpSlider;

    // A handle to the regen coroutine so we can stop/restart it.
    private Coroutine regenCoroutine;

    // Internal
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private bool canMove = true;

    void Start()
    {
        // Get or confirm references
        characterController = GetComponent<CharacterController>();

        // Typical FPS: lock & hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Make sure player starts at full HP
        currentHealth = maxHealth;

        // If we assigned an HP slider, set its range
        if (hpSlider != null)
        {
            hpSlider.minValue = 0f;
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }
    }

    void Update()
    {
        if (isDead) return; // No movement or updates if dead

        HandleMovement();
        HandleFootsteps();

        // Continuously update HP slider if assigned
        if (hpSlider != null)
        {
            hpSlider.value = currentHealth;
        }
    }

    /// <summary>
    /// Handles player movement (walk, run, jump, look) via CharacterController.
    /// </summary>
    private void HandleMovement()
    {
        #region Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
        float speedZ = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");
        float oldY = moveDirection.y;

        moveDirection = forward * speedX + right * speedZ;
        #endregion

        #region Jumping
        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = 0f;
            }
        }
        else
        {
            moveDirection.y = oldY - (gravity * Time.deltaTime);
        }
        #endregion

        characterController.Move(moveDirection * Time.deltaTime);

        // Mouse look
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
        }
    }

    /// <summary>
    /// Checks horizontal velocity to play or stop footstep sounds.
    /// </summary>
    private void HandleFootsteps()
    {
        Vector3 horizontalVel = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
        float speed = horizontalVel.magnitude;

        if (speed > 0.1f && characterController.isGrounded && canMove)
        {
            if (!walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
            }
        }
        else
        {
            if (walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Stop();
            }
        }
    }

    /// <summary>
    /// The zombie or any damage source calls this to damage the player.
    /// We reset the regen timer each time we get hit.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
        else
        {
            // Stop any ongoing regen & start fresh
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
            regenCoroutine = StartCoroutine(RegenerateHealthRoutine());
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player is dead. GAME OVER!");
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// Wait 10 seconds after the last hit, then regenerate 10 HP per second until full.
    /// If we get hit again, we stop & reset this process.
    /// </summary>
    private IEnumerator RegenerateHealthRoutine()
    {
        // 1) Wait 10 seconds before starting any regen
        yield return new WaitForSeconds(10f);

        // 2) Then keep adding 10 HP each second until we reach 100 or die or get hit again
        while (currentHealth < maxHealth && !isDead)
        {
            currentHealth += 10f;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            // Each increment takes 1 second
            yield return new WaitForSeconds(1f);
        }

        // Once we reach 100 or the loop ends, the coroutine ends
        regenCoroutine = null;
    }
}
