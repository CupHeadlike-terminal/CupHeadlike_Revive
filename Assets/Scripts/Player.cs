 using UnityEngine;
 public class Player : MonoBehaviour
 {
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePos;       // 発射位置
    [SerializeField] private float bulletSpeed = 10f; // 弾速
    [SerializeField] private float fireRate = 0.2f;


    public float groundSpeed = 5.0f;
    public float airSpeed = 3.0f;
    public float jumpForce = 5.0f;
    private float nextFireTime = 0f;


    private Rigidbody2D rb; //Rigidbodyへの参照
    private bool isGrounded = false; //地面にいるがどうか判定
    private bool touchingwall = false;
    private int facingDirection = 1; //  右向き=1, 左向き=-1

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//RIgidBodyを取得
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = isGrounded ? groundSpeed : airSpeed;
        float move = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = -currentSpeed; //左入力で左に移動
            facingDirection = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            move = currentSpeed; //右入力で右に移動
            facingDirection = 1;
        }
        //壁に触れている場合壁方向への入力を0にする
        if (touchingwall)
        {
            if ((move > 0 && rb.linearVelocity.x > 0) || (move < 0 && rb.linearVelocity.x < 0))
            {
                move = 0f;
            }
        }
        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; //ジャンプしたらすぐにfalseにする
        }
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)  
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // 次に発射可能になる時刻を更新
        }
    }
    void Shoot()
    {
        Bullet bullet = bulletPool.GetBullet();
        bullet.transform.position = firePos.position;
        bullet.Shoot(new Vector2(bulletSpeed * facingDirection, 0));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            touchingwall = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag ==("Wall"))
        {
            touchingwall = false;
        }
    }
}
