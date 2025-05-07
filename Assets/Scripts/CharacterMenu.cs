using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public TextMeshProUGUI levelText, hitpointText, coinsText, upgradeCostText, xpText;

    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Character Selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
            {
                currentCharacterSelection = 0;
            }

            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            if (currentCharacterSelection < 0)
            {
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }

            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
        GameManager.instance.currentSpriteId = currentCharacterSelection;
    }

    public void onUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    public void UpdateMenu()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSpites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        // Meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        coinsText.text = GameManager.instance.coins.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        // xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total xp";
            xpBar.localScale = Vector3.one;
        } else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;

            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + "/" + diff;
        }
    }
}
