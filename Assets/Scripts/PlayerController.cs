using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;

    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    // 🟡 Double Jump Variablen
    private int jumpCount = 0;
    public int maxJumpCount = 2;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (knockBackCounter <= 0)
        {
            float yStore = moveDirection.y;

            // Bewegung berechnen (X & Z)
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            // Wenn grounded → Reset Jump Count
            if (controller.isGrounded)
            {
                jumpCount = 0;
                moveDirection.y = 0f;
            }

            // Sprunglogik
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
            {
                moveDirection.y = jumpForce;
                jumpCount++;
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }

        // Schwerkraft
        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        // Bewegung anwenden
        controller.Move(moveDirection * Time.deltaTime);

        // Rotation zur Kameraausrichtung
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
    }

    // Knockback-Methode vom HealthManager
    public void Knockback(Vector3 direction)
    {
        knockBackCounter = knockBackTime;

        // Richtung + leichtes Hochstoßen
        moveDirection = direction.normalized * knockBackForce;
        moveDirection.y = knockBackForce * 0.5f;
    }
}
