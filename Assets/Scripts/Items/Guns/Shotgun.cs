using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 霰弹枪 一次发射多颗子弹
public class Shotgun : Gun
{
    private const float INTERVAL = 1f;

    public int bulletNum = 3;
    public float bulletAngle = 15;

    public override void Initialize(PlayerMovement owner)
    {
        base.Initialize(owner);

        // 初始化参数 霰弹枪射击速度更慢
        originalInterval = INTERVAL;
    }

    protected override void Fire()
    {
        animator.SetTrigger("Shoot");

        int median = bulletNum / 2;
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePos.position;
            Bullet bul = bullet.GetComponent<Bullet>();
            bul.damage = damage;

            if (bulletNum % 2 == 1)
            {
                bul.SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) * direction);
            }
            else
            {
                bul.SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median) + bulletAngle / 2, Vector3.forward) * direction);
            }
        }

        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
