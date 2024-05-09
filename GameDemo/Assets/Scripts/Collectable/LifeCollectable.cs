public class LifeCollectable : Collectable
{
    public override void Trigger()
    {
        GameManager.Ins.CurLife += bonus;

        //Update UI
        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);

        //Souund
        AudioController.Ins.PlaySound(AudioController.Ins.lifePickup);
    }
}
