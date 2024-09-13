using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    CharacterController2D CC;
    float horizontalMove;

    [SerializeField]
    float runSpeed;
    bool crouch;
    bool jump;

    public float maxJumpInput = 1f;
    public float jumpInputSpeed = 2f;
    private float currentJumpInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController2D>();
        if (!isLocalPlayer)
        {
            CC.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButton("Jump"))
        {
            // Gradually increase the jump input value up to maxJumpInput
            currentJumpInput = Mathf.Lerp(
                currentJumpInput,
                maxJumpInput,
                jumpInputSpeed * Time.deltaTime
            );
        }
        else
        {
            // Gradually decrease the jump input value down to 0
            currentJumpInput = Mathf.Lerp(currentJumpInput, 0f, jumpInputSpeed * Time.deltaTime);
        }

        // Debug or use the jump input value to control jumping force
        Debug.Log("Current Jump Input: " + currentJumpInput);
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else
        {
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(Input.GetAxisRaw("Jump"));
        CC.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, currentJumpInput);
        jump = false;
    }
}
