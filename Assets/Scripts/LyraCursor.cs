using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LyraCursor : NetworkBehaviour
{
    public ILyraAbilityItem LyraAbilityItem;
    [SerializeField] private float moveSpeed = 5f; // Швидкість руху курсору
    [SerializeField] private float stickSensitivity = 1f; // Чутливість правого стіка

    private Camera mainCamera;
    void Start()
    {
        if (!isOwned)
        {
            gameObject.SetActive(false);
            this.enabled = false;
            return;
        }

        mainCamera = Camera.main; // Отримуємо основну камеру
    }

    public void OnStartAbility()
    {
        LyraAbilityItem = null;
    }

    public void OnStopAbility()
    {
        LyraAbilityItem = null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "LyraAbilityItem")
        {
            LyraAbilityItem = other.gameObject.GetComponent<ILyraAbilityItem>();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (LyraAbilityItem != null && other.gameObject.tag == "LyraAbilityItem")
        {
            LyraAbilityItem = null;
        }
    }

    void Update()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);

        float horizontal = Input.GetAxis("RHorizontal");
        float vertical = Input.GetAxis("RVertical");
        Vector2 stickInput = new Vector2(horizontal, vertical);

        if (stickInput.sqrMagnitude > 0.01f)
        {
            transform.Translate(stickInput * stickSensitivity * Time.deltaTime);
        }
    }
}
