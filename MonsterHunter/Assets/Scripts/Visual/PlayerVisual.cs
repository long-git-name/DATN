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

        GUIManager.Ins.UpdateHpInfo(actor.CurHp, actor.statData.Hp);
    }

    public void OnLostLife()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.lostLife);

        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);
        GUIManager.Ins.UpdateHpInfo(player.CurHp, playerStats.Hp);
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

        GUIManager.Ins.UpdateLevelInfo(playerStats.Level, playerStats.Exp, playerStats.ExpToUpLevel);
    }

    public void OnLevelUp()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.levelUp);

        GameManager.Ins.Player.CurHp = playerStats.Hp;
        GUIManager.Ins.UpdateHpInfo(playerStats.Hp, playerStats.Hp);
    }
}
