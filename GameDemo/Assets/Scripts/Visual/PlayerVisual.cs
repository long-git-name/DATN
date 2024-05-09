using UnityEngine;

public class PlayerVisual : ActorVisual
{
    [SerializeField] private GameObject deathVfx;

    private Player player;
    private PlayerStats playerStats;

    private void Start()
    {
        player = (Player)actor;
        playerStats = player.PlayerStats;
    }

    public override void OnTakeDamage()
    {
        base.OnTakeDamage();

        GUIManager.Ins.UpdateHpInfo(actor.CurHp, actor.statData.hp);
    }

    public void OnLostLife()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.lostLife);

        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);
        GUIManager.Ins.UpdateHpInfo(player.CurHp, playerStats.hp);
    }

    public void OnDead()
    {
        if(deathVfx)
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
        }

        AudioController.Ins.PlaySound(AudioController.Ins.playerDeath);

        GUIManager.Ins.ShowGameOverDialog();
    }

    public void OnAddExp()
    {
        if(playerStats == null) return;

        GUIManager.Ins.UpdateLevelInfo(playerStats.level, playerStats.exp, playerStats.expToUpLevel);
    }

    public void OnLevelUp()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.levelUp);

        GUIManager.Ins.UpdateHpInfo(player.CurHp, playerStats.hp);
    }
}
