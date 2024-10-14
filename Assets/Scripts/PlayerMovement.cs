using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    CharacterController2D cC;
    [SerializeField]
    PlayerCombat pc;
    float horizontalMove;

    [SerializeField]
    float runSpeed;
    bool crouch;
    bool jump;

    public float MaxJumpInput = 1f;
    public float JumpInputSpeed = 2f;
    private float currentJumpInput = 0f;

    private IUseable currentUseable;

    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController2D>();
        if (!isLocalPlayer)
        {
            cC.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Use();
        Attack();
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButton("Jump"))
        {
            // Gradually increase the jump input value up to maxJumpInput
            currentJumpInput = Mathf.Lerp(
                currentJumpInput,
                MaxJumpInput,
                JumpInputSpeed * Time.deltaTime);
        }
        else
        {
            // Gradually decrease the jump input value down to 0
            currentJumpInput = Mathf.Lerp(currentJumpInput, 0f, JumpInputSpeed * Time.deltaTime);
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
        cC.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, currentJumpInput);
        jump = false;
    }

    void Use()
    {
        if (Input.GetButtonDown("Submit") && isLocalPlayer && currentUseable != null)
        {
            currentUseable.Use();
            return;
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && isLocalPlayer)
        {
            pc.Attack(fasing: cC.FacingRight ? 1 : -1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Useable")
        {
            currentUseable = other.gameObject.GetComponent<IUseable>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Useable")
        {
            currentUseable = null;
        }
    }
}
