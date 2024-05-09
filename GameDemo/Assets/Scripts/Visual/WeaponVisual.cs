using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip reloadSound;

    public void OnShoot()
    {
        AudioController.Ins.PlaySound(shootingSound);
    }

    public void OnReload()
    {
        GUIManager.Ins.ShowReloadTxt(true);
    }

    public void OnReloadDone()
    {
        AudioController.Ins.PlaySound(reloadSound);
        GUIManager.Ins.ShowReloadTxt(false);
    }
}
