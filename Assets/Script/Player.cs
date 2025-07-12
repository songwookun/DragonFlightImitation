using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    private Camera mainCam;
    private float halfWidth;

    void Start()
    {
        mainCam = Camera.main;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.size.x / 2f;
        }
    }

    void Update()
    {
        float distaceX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        transform.Translate(distaceX, 0, 0);
        ClampToScreen();
    }

    void ClampToScreen()
    {
        Vector3 pos = transform.position;

        float zDist = transform.position.z - mainCam.transform.position.z;
        Vector3 min = mainCam.ViewportToWorldPoint(new Vector3(0, 0, zDist));
        Vector3 max = mainCam.ViewportToWorldPoint(new Vector3(1, 1, zDist));

        pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject); 
        }
    }

    void OnDestroy()
    {
        GameOverManager goManager = FindAnyObjectByType<GameOverManager>();
        if (goManager != null)
        {
            goManager.ShowGameOver();
        }
    }
}
