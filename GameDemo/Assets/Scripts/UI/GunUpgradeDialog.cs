using UnityEngine;
using UnityEngine.UI;

public class GunUpgradeDialog : Dialog
{
    [SerializeField] private GunStatUI bulletStatUI;
    [SerializeField] private GunStatUI damageStatUI;
    [SerializeField] private GunStatUI firerateStatUI;
    [SerializeField] private GunStatUI reloadStatUI;
    [SerializeField] private Text upgradeBtnTxt;

    private Weapon weapon;
    private WeaponStats weaponStats;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        Time.timeScale = 0f;

        weapon = GameManager.Ins.Player.weapon;
        weaponStats = weapon.statData;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if(weapon == null || weaponStats == null ) return;

        if(titleTxt != null) titleTxt.text = $"LEVEL {weaponStats.level.ToString("00")}";

        if(upgradeBtnTxt) upgradeBtnTxt.text = $"UP {weaponStats.priceToUp.ToString("n0")}$";

        if(bulletStatUI)
        {
            bulletStatUI.UpdateStat
            (
                "Bullets : ", weaponStats.bullet.ToString("n0"), $"( +{weaponStats.bulletUp.ToString("n0")} )"    
            );
        }

        if(damageStatUI)
        {
            damageStatUI.UpdateStat
            (
                "Damage : ", weaponStats.damage.ToString("F2"), $"( +{weaponStats.damageUp.ToString("F2")} )"
            );
        }

        if(firerateStatUI)
        {
            firerateStatUI.UpdateStat
            (
                "Firerate : ", weaponStats.firerate.ToString("F2"), $"( +{weaponStats.firerateUp.ToString("F2")} )"
            );
        }

        if(reloadStatUI)
        {
            reloadStatUI.UpdateStat
            (
                "Reload : ", weaponStats.reloadTime.ToString("F2"), $"( +{weaponStats.reloadTimeUp.ToString("F2")} )"
            );
        }
    }

    public void UpgradeGun()
    {
        if(weaponStats == null) return;

        weaponStats.Upgrade(OnUpgradeSuccess, OnUpgradeFail);
    }

    private void OnUpgradeSuccess()
    {
        UpdateUI();

        GUIManager.Ins.UpdateCoinCount(Prefs.coins);

        //Souund
        AudioController.Ins.PlaySound(AudioController.Ins.upgradeSuccess);
    }

    private void OnUpgradeFail()
    {
        Debug.Log("Upgrade gun fail!!!");
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }
}
