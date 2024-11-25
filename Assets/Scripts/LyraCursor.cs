using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LyraCursor : NetworkBehaviour
{
    public ILyraAbilityItem lyraAbilityItem;
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
        lyraAbilityItem = null;
    }
    public void OnStopAbility()
    {
        lyraAbilityItem = null;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "LyraAbilityItem")
        {
            lyraAbilityItem = other.gameObject.GetComponent<ILyraAbilityItem>();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (lyraAbilityItem != null && other.gameObject.tag == "LyraAbilityItem")
        {
            lyraAbilityItem = null;
        }

    }
    void Update()
    {
        // Переміщення курсору за допомогою миші
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);

        // Переміщення курсору за допомогою правого стіка (для геймпадів)
        float horizontal = Input.GetAxis("RHorizontal"); // Ось X правого стіка
        float vertical = Input.GetAxis("RVertical"); // Ось Y правого стіка
        Vector2 stickInput = new Vector2(horizontal, vertical);

        if (stickInput.sqrMagnitude > 0.01f) // Якщо є рух
        {
            transform.Translate(stickInput * stickSensitivity * Time.deltaTime);
        }
    }
}
