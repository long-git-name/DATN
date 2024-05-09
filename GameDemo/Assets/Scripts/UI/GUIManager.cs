using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] private GameObject homeGUI;
    [SerializeField] private GameObject gameGUI;

    [SerializeField] private Transform lifeGrid;
    [SerializeField] private GameObject lifeIconPrefab;

    [SerializeField] private ImageFilled levelProgressBar;
    [SerializeField] private ImageFilled hpProgressBar;

    [SerializeField] private Text levelCountTxt;
    [SerializeField] private Text expCountTxt;
    [SerializeField] private Text hpCountTxt;
    [SerializeField] private Text coinCountTxt;
    [SerializeField] private Text reloadStateTxt;

    [SerializeField] private Dialog gunUpgradeDialog;
    [SerializeField] private Dialog gameOverDialog;

    private Dialog activeDialog;

    public Dialog ActiveDialog { get => activeDialog; private set => activeDialog = value; }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGameGUI(bool isShow)
    {
        if(gameGUI != null)
        {
            gameGUI.SetActive(isShow);
        }

        if(homeGUI != null)
        {
            homeGUI.SetActive(!isShow);
        }
    }

    private void ShowDialog(Dialog dialog)
    {
        if(dialog == null) return;
        activeDialog = dialog;
        activeDialog.Show(true);
    }

    public void ShowGunUpgradeDialog()
    {
        ShowDialog(gunUpgradeDialog);
    }

    public void ShowGameOverDialog()
    {
        ShowDialog(gameOverDialog);
    }

    public void UpdateLifeInfo(int life)
    {
        ClearLifeGrid();
        DrawLifeGrid(life);
    }

    private void DrawLifeGrid(int life)
    {
        if(lifeGrid == null || lifeIconPrefab == null) return;

        for(int i = 0; i < life; i++)
        {   
            var lifeIconClone = Instantiate(lifeIconPrefab, Vector3.zero, Quaternion.identity);
            
            lifeIconClone.transform.SetParent(lifeGrid);
            lifeIconClone.transform.localPosition = Vector3.zero;
            lifeIconClone.transform.localScale = Vector3.one;
        }
    }

    private void ClearLifeGrid()
    {
        if(lifeGrid == null) return;

        int lifeItemCount = lifeGrid.childCount;

        for(int i = 0; i < lifeItemCount; i++)
        {   
            var lifeItem = lifeGrid.GetChild(i);
            if(lifeItem == null) continue;
            Destroy(lifeItem.gameObject);
        }
    }

    public void UpdateLevelInfo(int curLevel, float curExp, float expToUpLevel)
    {
        levelProgressBar?.UpdateValue(curExp, expToUpLevel);

        if(levelCountTxt != null) levelCountTxt.text = $"LEVEL {curLevel.ToString("00")}";

        if(expCountTxt != null) expCountTxt.text = $"{curExp.ToString("F2")} / {expToUpLevel.ToString("F2")}";
    }

    public void UpdateHpInfo(float curHp, float maxHp)
    {
        hpProgressBar?.UpdateValue(curHp, maxHp);

        if(hpCountTxt != null) hpCountTxt.text = $"{curHp.ToString("F2")} / {maxHp.ToString("F2")}";
    }

    public void UpdateCoinCount(int coins)
    {
        if(coinCountTxt != null) coinCountTxt.text = $"{coins.ToString("n0")}";
    }

    public void ShowReloadTxt(bool isShow)
    {
        if(reloadStateTxt != null) reloadStateTxt.gameObject.SetActive(isShow);
    }
}
