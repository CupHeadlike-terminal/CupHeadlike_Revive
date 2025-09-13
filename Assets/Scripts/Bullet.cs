using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private BulletPool pool;
    public float lifeTime = 3f; // 消えるまでの時間
    public float timer;

    void Awake() //startよりも早く呼び出される　引数はない
    {
        rb = GetComponent<Rigidbody2D>(); //rbにRigidbodyを代入
    }
    public void Init(BulletPool bulletPool) //BulletPool型のPoolを引数とする
    {
        pool = bulletPool;　//このクラスの変数poolにBulletPoolクラスのPoolを代入しこのクラスでも使えるように
    }
    public void Shoot(Vector2 velocity)
    {
        gameObject.SetActive(true);
        rb.linearVelocity = velocity;
        timer = 0f;
    }
  
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)　
        {
            ReturnToPool(); //Poolに返す
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            ReturnToPool();
        }
    }
    private void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
        pool.ReturnObject(this);
    }
}
