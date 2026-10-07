using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    private Camera minimapCamera;

    private Player player;

    [Header("Move Camera Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float drag;

    [Header("Zoom Camera Settings")]
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomSpeed;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        minimapCamera = GetComponent<Camera>();
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y , transform.position.z);
        player.gatherInput.SetMinimapActivatorActionPerformedInstructions(SetStartingPosition);
    }
    private void OnDisable()
    {
        player.gatherInput.RemoveMinimapActivatorActionPerformedInstructions(SetStartingPosition);
    }

    private void LateUpdate()
    {
        if(player.gatherInput.minimapMoveValue != Vector2.zero)
        {
            Vector3 newPosition = transform.position + new Vector3(player.gatherInput.minimapMoveValue.x * moveSpeed * Time.deltaTime, player.gatherInput.minimapMoveValue.y * moveSpeed * Time.deltaTime, 0);
            transform.position = Vector3.Lerp(transform.position, newPosition, drag);
        }
        if(player.gatherInput.minimapZoomValue != 0)
        {
            float newSize = Mathf.Clamp(minimapCamera.orthographicSize - (player.gatherInput.minimapZoomValue * zoomSpeed * Time.deltaTime), minZoom, maxZoom);
            minimapCamera.orthographicSize = Mathf.Lerp(minimapCamera.orthographicSize, newSize, drag);
        }
    }

    private void SetStartingPosition()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}