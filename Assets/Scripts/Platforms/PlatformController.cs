using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int startIndex;
    [SerializeField] private Transform[] points;
    private int targetIndex;
    private Player player;
    void Start()
    {
        targetIndex = startIndex;
        transform.position = points[targetIndex].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[targetIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, points[targetIndex].position) < 0.05f)
        {
            targetIndex++;
            if(targetIndex >= points.Length)
            {
                targetIndex = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    player = collision.gameObject.GetComponent<Player>();
                    collision.gameObject.transform.parent = transform;
                    player.physicsControl.SetExtrapolate();
                    break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy)
            {
                collision.gameObject.transform.parent = null;
                if (player != null)
                {
                    player.physicsControl.SetInterpolate();
                }
                player = null;
            }
        }
    }
}
