using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum CameraMode { Static, Dynamic, None }
public class CameraMovement : MonoBehaviour
{

    public CameraMode curretMode = CameraMode.Static;

    private Vector3 staticPosition;

    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [SerializeField] private float minZoom = 7f;

    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float maxOffset = 7f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float targetZoom = 0f;
    [SerializeField] private float distance = 0f;
    public bool ChangedMode;
    float defaultOSize = 7;
    private Camera cam;
    private Vector2 inputOffset;
    bool playersConnected;
    void Start()
    {
        cam = GetComponent<Camera>();
        staticPosition = transform.position;
        StartCoroutine(WaitPlayers());

    }
    IEnumerator WaitPlayers()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == 2);
        player1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        player2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Transform>();
        this.enabled = false;
        this.enabled = true;
        playersConnected = true;
    }
    void Update()
    {
        if (!playersConnected) return;
        switch (curretMode)
        {
            case CameraMode.Static:
                transform.position = Vector3.Lerp(transform.position, staticPosition, followSpeed * Time.deltaTime);
                break;
            case CameraMode.Dynamic:
                HandleDynamicMode();
                break;
        }
    }
    public void SetStaticPosition(Vector3 position)
    {
        staticPosition = position;
    }
    private void HandleDynamicMode()
    {
        Vector3 midPoint = (player1.position + player2.position) / 2f;
        midPoint = new Vector3(midPoint.x, midPoint.y, midPoint.z);
        distance = Vector2.Distance(player1.position, player2.position);
        distance = distance / 2;
        targetZoom = Mathf.Clamp(distance, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
        Vector2 mouseoffset = GetMouseOffset();
        Vector2 joystickOffset = Vector2.zero;//GetJoystickOffset();
        inputOffset = Vector2.Lerp(inputOffset, mouseoffset + joystickOffset, followSpeed * Time.deltaTime);
        inputOffset = Vector2.ClampMagnitude(inputOffset, maxOffset);
        Vector3 targetPosition = midPoint + new Vector3(inputOffset.x, inputOffset.y, -10f);
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
    private Vector2 GetJoystickOffset()
    {
        float joystickX = Input.GetAxis("HorizontalRightStick");
        float joystickY = Input.GetAxis("VerticalRightStick");
        return new Vector2(joystickX, joystickY) * maxOffset;
    }
    private Vector2 GetMouseOffset()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 normalizedOffset = (mousePosition - screenCenter) / screenCenter;
        return normalizedOffset * maxOffset;
    }
    public void SetDynamicMode(int changedMode = 0)
    {
        Debug.Log("Changed to Dynamic");
        ChangedMode = changedMode == 1 ? true : false;
        curretMode = CameraMode.Dynamic;
    }
    public void SetStaticMode(int changedMode = 0)
    {
        Debug.Log("Changed to static");
        ChangedMode = changedMode == 1 ? true : false;
        curretMode = CameraMode.Static;
    }
    public void SetNoneMode(int changedMode = 0)
    {
        Debug.Log("Changed to none");
        ChangedMode = changedMode == 1 ? true : false;
        curretMode = CameraMode.None;
    }
    public void SetFov(int fov)
    {
        Camera.main.orthographicSize = fov;
        maxZoom += fov;
    }
}
