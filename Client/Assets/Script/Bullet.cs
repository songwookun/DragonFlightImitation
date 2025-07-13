
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 0.45f;
    public int Damage = 1;

    void Update()
    {
        transform.Translate(0, BulletSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
