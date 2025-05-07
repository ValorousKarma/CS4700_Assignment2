using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct
    public int[] damagePoint = { 1, 2, 3, 4, 5 };
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.6f, 3f };

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name != "Player")
            {
                Damage dmg = new Damage { 
                    damageAmount = damagePoint[weaponLevel],
                    origin = transform.position,
                    pushForce = pushForce[weaponLevel],
                };

                coll.SendMessage("ReceiveDamage", dmg);
            }
        }
    }

    private void Swing()
    {
        anim.SetTrigger("swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSpites[weaponLevel];

        // Change stats %%
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSpites[weaponLevel];
    }
}
