using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int poolSize = 20;
    private Queue<Bullet> bullets = new Queue<Bullet>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            bullet.Init(this);
            bullets.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if(bullets.Count > 0)
        {
            return bullets.Dequeue();
        }
        else
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            bullet.Init(this);
            return bullet;
        }
    }

    public void ReturnObject(Bullet bullet)
    {
        bullets.Enqueue(bullet);
    }
}
