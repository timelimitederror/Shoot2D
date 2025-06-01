using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 子弹
public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject explosionPrefab;
    new private Rigidbody2D rigidbody;
    public int damage = 10;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ignore")) // 原版的没有这个内容，然后霰弹枪这种发射密集子弹的，就可能两个子弹对对碰消失，效果是霰弹枪的喷射距离几乎是近战枪。加了这个之后，霰弹枪的子弹可以一直飞了。
        {
            return;
        }

        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;

        // 造成伤害
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Damage(damage);
        }

        // Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}
