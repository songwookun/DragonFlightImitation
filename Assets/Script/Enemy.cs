
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float EnemySpeed = 1.3f;
    private int hp;
    private int maxHp;

    public bool isBoss = false; 

    void Update()
    {
        float distanceY = EnemySpeed * Time.deltaTime;
        transform.Translate(0, -distanceY, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Init(int hp, float speed, bool boss = false)
    {
        this.hp = hp;
        this.maxHp = hp;
        this.EnemySpeed = speed;
        this.isBoss = boss;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameHUD hud = FindObjectOfType<GameHUD>();
            if (hud != null)
                hud.AddKillScore();

            if (isBoss && GameManager.Instance != null)
                GameManager.Instance.EndGame();

            Destroy(gameObject);
        }
    }
}
