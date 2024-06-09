public class MoneyCollectable : Collectable
{
    public override void Trigger()
    {
        Prefs.coins += bonus; 

        //Update UI
        GUIManager.Ins.UpdateCoinCount(Prefs.coins);

        //Souund
        AudioController.Ins.PlaySound(AudioController.Ins.coinPickup);
    }
}
