public class EnemyVisual : ActorVisual
{
    public void OnDead()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.enemyDeath);
    }
}
