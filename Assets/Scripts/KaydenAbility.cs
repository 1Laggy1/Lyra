using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mirror;
using UnityEngine;

public class KaydenAbility : PlayerAbility
{
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public bool canDash = true;
    public float dashingCooldown = 1;
    public CharacterController2D playerMovement;

    private Rigidbody2D rb;
    private bool isDashing = false;
    [SerializeField]
    TrailRenderer tr;
    [SerializeField]
    List<AudioClip> useAudio = new List<AudioClip>();
    [SerializeField]
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void UseAbility()
    {
        if (isDashing || !canDash) return;
        DashEffectStart();
        StartCoroutine(DashMovement());
    }
    [Command(requiresAuthority = false)]
    public void DashEffectStart()
    {
        DashEffectStartClientRPC(Random.Range(0, useAudio.Count));
    }
    [Command(requiresAuthority = false)]
    public void DashEffectStop()
    {
        DashEffectStopClientRPC();
    }
    [ClientRpc]
    public void DashEffectStartClientRPC(int audioIndex)
    {
        tr.emitting = true;
        audioSource.PlayOneShot(useAudio[audioIndex]);
    }
    [ClientRpc]
    public void DashEffectStopClientRPC()
    {
        tr.emitting = false;
    }
    private IEnumerator DashMovement()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        float originalXvelocity = rb.velocityX;
        rb.gravityScale = 0f;
        int dashRight = playerMovement.FacingRight ? 1 : -1;
        if (dashRight > 0)
        {
            rb.velocity = new Vector2(rb.velocityX + dashForce, 0f);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocityX - dashForce, 0f);
        }

        yield return new WaitForSeconds(dashDuration);
        DashEffectStop();
        rb.gravityScale = originalGravity;
        rb.velocity = new Vector2(originalXvelocity, rb.velocityY);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
