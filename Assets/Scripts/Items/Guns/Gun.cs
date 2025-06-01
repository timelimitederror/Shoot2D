using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    private const float INTERVAL = 0.5f;
    private const int DAMAGE = 10;

    public float interval = INTERVAL;
    protected float originalInterval = INTERVAL;
    public int damage = DAMAGE;
    protected int originalDamage = DAMAGE;
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    protected Transform muzzlePos;
    protected Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    protected float flipY;
    protected Animator animator;

    protected BoxCollider2D thisCollider;
    protected bool isUse = false;// 正在使用

    protected virtual void Awake()
    {
        stackable = true;
        maxStackCount = 5;
    }

    public override void Initialize(PlayerMovement owner)
    {
        base.Initialize(owner);
        isUse = true;
        thisCollider.enabled = false;

        // 初始化参数
        stackCount = 1;
        originalInterval = INTERVAL;
        originalDamage = DAMAGE;
    }

    public override void UnInitialize()
    {
        base .UnInitialize();
        isUse = false;
        thisCollider.enabled = true;
    }

    public override void Recovery()
    {
        interval = originalInterval;
        damage = originalDamage;
    }

    public override void ApplyEffect()
    {
        // do nothing
    }

    protected override void UpdateStackEffect() // 升级武器，射击间隔缩短，伤害提高
    {
        originalDamage = originalDamage * 2;
        originalInterval = originalInterval * 0.8f;
    }

    protected virtual void Start()
    {
        thisCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        flipY = transform.localScale.y;
    }

    protected virtual void Update()
    {
        if (!TimeManager.Instance.TimeFlow() || !isUse)
        {
            return;
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);

        Shoot();
    }

    protected virtual void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }

        if (Input.GetButton("Fire1"))
        {
            if (timer == 0)
            {
                timer = interval;
                Fire();
            }
        }
    }

    protected virtual void Fire()
    {
        animator.SetTrigger("Shoot");

        // GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = muzzlePos.position;

        float angel = Random.Range(-5f, 5f);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.damage = damage;
        bul.SetSpeed(Quaternion.AngleAxis(angel, Vector3.forward) * direction);

        // Instantiate(shellPrefab, shellPos.position, shellPos.rotation);
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
