using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject tutorialUI;
    

    [SerializeField] private Transform lifeGrid;
    [SerializeField] private GameObject lifeIconPrefab;

    [SerializeField] private ImageFilled levelProgressBar;
    [SerializeField] private ImageFilled hpProgressBar;

    [SerializeField] private Text hpCountingTxt;
    [SerializeField] private Text XpCountingTxt;
    [SerializeField] private Text levelCountingTxt;
    [SerializeField] private Text coinCountingTxt;
    [SerializeField] private Text reloadTxt;

    [SerializeField] private Text tutorialTxt;
    [SerializeField] private GameObject joystick;

    [SerializeField] private Dialog gunUpgradeDialog;


    private Dialog activeDialog;

    public Dialog ActiveDialog { get => activeDialog; private set => activeDialog = value; }

    protected override void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            tutorialTxt.text = "Use Joystick keys to move and shoot to fight the zombies.\n\nUpgrade your gun to become stronger. \n \n Survive as long as you can!!!!";
            joystick.SetActive(true);

        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            tutorialTxt.text = "Use AWSD to move and shoot to fight the zombies.\n\nUpgrade your gun to become stronger.\n\n Survive as long as you can!!!!";
            joystick.SetActive(false);

        }
        MakeSingleton(false);
    }
    // public void ShowGameGUI(bool isShow)
    // {
    //     Debug.Log("congthanhhh");
    //     if (gameplayUI != null)
    //     {
    //         gameplayUI.SetActive(isShow);
    //     }
    //     if (mainMenuUI != null)
    //     {
    //         mainMenuUI.SetActive(!isShow);
    //     }
    // }

    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        tutorialUI.SetActive(false);
    }

    public void ShowTutorial()
    {
        tutorialUI.SetActive(true);
        gameplayUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }

    public void PlayGame()
    {
        gameplayUI.SetActive(true);
        tutorialUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }

    public void UpdateLifeInfo(int life)
    {
        if(lifeGrid == null) return;

        ClearLifeGrid();

        DrawLifeGrid(life);
    }

    private void DrawLifeGrid(int life)
    {
        if (lifeGrid == null || lifeIconPrefab == null) return;
        for(int i = 0; i < life; i++)
        {
            var lifeItem = Instantiate(lifeIconPrefab,Vector3.zero, Quaternion.identity);
            lifeItem.transform.SetParent(lifeGrid);
            lifeItem.transform.localScale = Vector3.one;
        }    
    }

    private void ClearLifeGrid()
    {
        if (lifeGrid == null) return;
        int lifeCounting = lifeGrid.childCount;
        for (int i = 0; i < lifeCounting ; i++)
        {
            var lifeItem = lifeGrid.GetChild(i);
            if (lifeItem != null)
            {
                Destroy(lifeItem.gameObject);
            }
        }
    }
    public void UpdateLevelInfo(int currentLevel, float currentXp, float levelUpXpRequire)
    {
        levelProgressBar?.UpdateValue(currentXp, levelUpXpRequire);

        if(levelCountingTxt != null)
        {
            levelCountingTxt.text = "LEVEL " +  currentLevel.ToString("0");
        }
        if(XpCountingTxt != null)
        {
            XpCountingTxt.text = currentXp.ToString("0") + "/" + levelUpXpRequire.ToString("0");
        }
    }
    public void UpdateHpInfo(float currentHp, float maxHp)
    {
        hpProgressBar?.UpdateValue(currentHp, maxHp);

        if(hpCountingTxt != null)
        {
            hpCountingTxt.text = currentHp.ToString("0") + "/" + maxHp.ToString("0");
        }
    }
    public void UpdateCoinCounting(int coin)
    {
        if(coinCountingTxt != null)
        {
            coinCountingTxt.text = coin.ToString("n0");
        }
    }
    public void ShowReloadTxt(bool isShow)
    {
        if(reloadTxt != null)
        {
            reloadTxt.gameObject.SetActive(isShow);
        }
    }
}
