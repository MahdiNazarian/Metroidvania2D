using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxValueX;
    [SerializeField] private float parallaxValueY;

    private float spriteLength;
    private Camera cameraMain;
    private Vector3 cameraLastPos;
    private Vector3 deltaMovement;
    void Start()
    {
        cameraMain = Camera.main;
        cameraLastPos = cameraMain.transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        deltaMovement = cameraMain.transform.position - cameraLastPos;
        transform.position += new Vector3(deltaMovement.x * parallaxValueX, deltaMovement.y * parallaxValueY);
        cameraLastPos = cameraMain.transform.position;

        if(cameraMain.transform.position.x - transform.position.x >= spriteLength)
        {
            transform.position += new Vector3(cameraMain.transform.position.x + spriteLength, transform.position.y);
        }
        else if(transform.position.x - cameraMain.transform.position.x >= spriteLength)
        {
            transform.position += new Vector3(cameraMain.transform.position.x - spriteLength, transform.position.y);
        }
    }
}
