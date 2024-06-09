using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDialog : Dialog
{
    public void Replay()
    {
        Show(false);
        ReloadCurrentScene();
    }

    public void BackHome()
    {
        Show(false);
        ReloadCurrentScene();
    }

    public void ExitGame()
    {
        Show(false);
        Application.Quit();
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
