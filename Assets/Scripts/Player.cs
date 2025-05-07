using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Mover
{

    private SpriteRenderer spriteRenderer;
    // Update is called once per frame

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector2(x, y));
    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level)
    {
        for ( int i = 0; i < level; i++ )
        {
            OnLevelUp();
        }
    }

    protected override void Death()
    {
        base.Death();
        GameManager.instance.coins = 0;
        GameManager.instance.experience = GameManager.instance.GetXpToLevel(GameManager.instance.GetCurrentLevel() - 1);
        GameManager.instance.SaveState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
