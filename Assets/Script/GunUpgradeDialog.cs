using System;
using UnityEngine;
using UnityEngine.UI;

public class GunUpgradeDialog : Dialog
{
    [SerializeField] private GunStatsUI bulletStatsUI;
    [SerializeField] private GunStatsUI damageStatsUI;
    [SerializeField] private GunStatsUI firerateStatsUI;
    [SerializeField] private GunStatsUI reloadStatsUI;

    [SerializeField] private Text upgradeButtonTxt;

    public Weapon weapon;
    public override void Show(bool isShow)
    {
        Time.timeScale = 0f;
        UpdateUI();
    }
    private void UpdateUI()
    {
        if(weapon == null) return;

        if (titleTxt) titleTxt.text = "Gun Level:" + weapon.weaponCurrentLv.ToString("00");
        if (upgradeButtonTxt) upgradeButtonTxt.text = "$" + weapon.currentPrice.ToString("n0");

        if(bulletStatsUI)
        {
            bulletStatsUI.UpdateStat(
                "Bullets:",
                weapon.maxBullet.ToString(),
                "+" + weapon.bulletUpInfo
            );
        }
        if (damageStatsUI)
        {
            damageStatsUI.UpdateStat(
                "Damage:",
                weapon.currentDamage.ToString(),
                "+" + weapon.damageUpInfo
            );
        }
        if (firerateStatsUI)
        {
            firerateStatsUI.UpdateStat(
                " Fire Rate:",
                weapon.currentFirerate.ToString("n1"),
                "-" + weapon.fireRateUpInfo.ToString("n1")
            );
        }
        if (reloadStatsUI)
        {
            reloadStatsUI.UpdateStat(
                "Reload Time:",
                weapon.currentReloadTime.ToString("n1"),
                "-" + weapon.reloadTimeUpInfo.ToString("n1")
            );
        }
    }
    public void UpgradeClick()
    {
        if(weapon == null) return;
        Upgrade(OnUpgradeSuccess,OnUpgradeFail);
    }
    private void OnUpgradeSuccess()
    {
        UpdateUI();
        GUIManager.Ins.UpdateCoinCounting((int)GameManager.Ins.player.currentCoin);
        AudioController.Ins.PlaySound(AudioController.Ins.upgradeSuccess);
    }
    private void OnUpgradeFail()
    {
    }
    public override void Close()
    {
        Time.timeScale = 1f;
    }
    public void Upgrade(Action onSuccess = null, Action onFailed = null)
    {
        if((int)GameManager.Ins.player.currentCoin >= weapon.currentPrice && !weapon.IsMaxLevel())
        {
            GameManager.Ins.player.currentCoin -= weapon.currentPrice;
            weapon.maxBullet += weapon.bulletUpInfo;
            weapon.currentDamage += weapon.damageUpInfo;
            weapon.currentFirerate -= weapon.fireRateUpInfo;
            weapon.currentPrice += weapon.priceInfo;
            Debug.Log("fireRate " +  weapon.currentFirerate );
            weapon.currentReloadTime -= weapon.reloadTimeUpInfo;

            weapon.weaponCurrentLv++;
            onSuccess?.Invoke();

            return;
        }
        onFailed?.Invoke();
    }

    
}
