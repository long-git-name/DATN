using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseDialog : Dialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        Time.timeScale = 0f;
    }
    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }

}
