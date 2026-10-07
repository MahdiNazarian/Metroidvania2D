using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraControls : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [Header("Smoothing")]
    [Tooltip("If you don't want to use just set it to 1 or more")]
    [SerializeField] private Vector2 smoothingDelay;
    [Header("DeadZone")]
    [Tooltip("If you don't want to use just set it to 0")]
    [SerializeField] private Vector2 deadZone;
    [Header("Confiner")]
    [SerializeField] private bool useConfiner;
    [SerializeField] private BoxCollider2D confinerCollider;

    [Header("Static Camera")]
    [SerializeField] private bool stopMoving;

    private PixelPerfectCamera pixelPerfectCamera;
    private Vector3 cameraPos;
    private float cameraBoundXPos;
    private float cameraBoundXNeg;
    private float cameraBoundYPos;
    private float cameraBoundYNeg;
    private float cameraSizeX;
    private float cameraSizeY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z) ;
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();

        cameraBoundXPos = confinerCollider.gameObject.transform.position.x + (confinerCollider.size.x/2.0f) + confinerCollider.offset.x;
        cameraBoundXNeg = -(confinerCollider.gameObject.transform.position.x + (confinerCollider.size.x/2.0f) - confinerCollider.offset.x);
        cameraBoundYPos = confinerCollider.gameObject.transform.position.y + (confinerCollider.size.y / 2.0f) + confinerCollider.offset.y;
        cameraBoundYNeg = -(confinerCollider.gameObject.transform.position.y + (confinerCollider.size.y / 2.0f) - confinerCollider.offset.y);
        cameraSizeX = ((float)pixelPerfectCamera.refResolutionX / (float) pixelPerfectCamera.assetsPPU / 2.0f);
        cameraSizeY = ((float)pixelPerfectCamera.refResolutionY / (float) pixelPerfectCamera.assetsPPU / 2.0f);
    }

    void LateUpdate()
    {
        if (stopMoving)
            return;
        //DeadZone Check
        if(Mathf.Abs(playerTransform.position.x - transform.position.x) > deadZone.x)
        {
            cameraPos.x = Mathf.Lerp(transform.position.x, playerTransform.position.x, smoothingDelay.x);
        }
        else
        {
            cameraPos.x = transform.position.x;
        }
        if(Mathf.Abs(playerTransform.position.y - transform.position.y) > deadZone.y)
        {
            cameraPos.y = Mathf.Lerp(transform.position.y, playerTransform.position.y, smoothingDelay.y);
        }
        else
        {
            cameraPos.y = transform.position.y;
        }
        //End DeadZone Check

        //Confiner Check
        if (useConfiner)
        {
            if(cameraPos.x + cameraSizeX >= cameraBoundXPos)
            {
                cameraPos.x = cameraBoundXPos - cameraSizeX;
            }
            else if(cameraPos.x - cameraSizeX <= cameraBoundXNeg)
            {
                cameraPos.x = cameraBoundXNeg + cameraSizeX;
            }
            if (cameraPos.y + cameraSizeY >= cameraBoundYPos)
            {
                cameraPos.y = cameraBoundYPos - cameraSizeY;
            }
            else if (cameraPos.y - cameraSizeY <= cameraBoundYNeg)
            {
                cameraPos.y = cameraBoundYNeg + cameraSizeY;
            }
        }
        //End Confiner Check

        cameraPos.z = transform.position.z;
        transform.position = cameraPos;
    }
}
