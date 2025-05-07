using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += UpdateManager;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    // Resources
    public List<Sprite> playerSprites, weaponSpites;
    public List<int> weaponPrices, xpTable;
    public int currentSpriteId;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;

    // Logic
    public int coins;
    public int experience;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        weapon = GameObject.Find("sword_0").GetComponent<Weapon>();
        floatingTextManager = GameObject.Find("FloatingTextManager").GetComponent<FloatingTextManager>();
    }

    public void UpdateManager(Scene s, LoadSceneMode mode)
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        weapon = GameObject.Find("sword_0").GetComponent<Weapon>();
        floatingTextManager = GameObject.Find("FloatingTextManager").GetComponent<FloatingTextManager>();
    }


    public void ShowText(string msg, int fontSize, Color color, Transform target, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, target, motion, duration);
    }

    // Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }
        if (coins >= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Experience System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;


        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // max level
                return r;
        }

        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while(r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        player.OnLevelUp();
        Debug.Log("level up");
    }

    /*
     * INT preferedSkin
     * INT coins
     * INT experience
     * INT weaponLevel
     */
    public void SaveState()
    {
        string s = "";

        s += currentSpriteId + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();


        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split("|");
        // [preferedSkinID]|[coins]|[xp]|[weaponLevel]

        // Set player skin
        currentSpriteId = int.Parse(data[0]);
        player.SwapSprite(currentSpriteId);

        // Set coin amount
        coins = int.Parse(data[1]);

        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        // Set weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        Debug.Log("LoadState");
    }
}
