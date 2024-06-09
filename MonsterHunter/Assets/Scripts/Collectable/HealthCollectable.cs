using UnityEngine;

public class HealthCollectable : Collectable
{
    public override void Trigger()
    {
        if(player == null) return;

        player.CurHp += bonus;
        player.CurHp = Mathf.Clamp(player.CurHp, 0, player.PlayerStats.Hp);

        //Update UI
        GUIManager.Ins.UpdateHpInfo(player.CurHp, player.PlayerStats.Hp);

        //Souund
        AudioController.Ins.PlaySound(AudioController.Ins.healthPickup);
    }
}
