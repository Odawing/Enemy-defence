using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [SerializeField]
    private Slider healthBar, ultimateBar;
    [SerializeField]
    private TextMeshProUGUI enemyCounterText, totalEnemyCounterText, ultTooltipText;
    [SerializeField]
    private Animator enemyCounterAnim;
    [SerializeField]
    private GameObject endGameMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshHPBar();
        RefreshUltimateBar();
    }

    public void RefreshEnemyCounter()
    {
        enemyCounterText.text = GameManagerScr.Instance.enemiesKilled.ToString();
        enemyCounterAnim.Play("AddKill");
    }

    public void RefreshHPBar()
    {
        healthBar.maxValue = GameManagerScr.Instance.player.maxHealth;
        healthBar.value = GameManagerScr.Instance.player.curHealth;
    }

    public void RefreshUltimateBar()
    {
        ultimateBar.maxValue = GameManagerScr.Instance.player.maxUltimateCharge;
        ultimateBar.value = GameManagerScr.Instance.player.ultimateCharge;

        // Show tooltip when ult is ready
        if (ultimateBar.value == ultimateBar.maxValue) ultTooltipText.gameObject.SetActive(true);
        else ultTooltipText.gameObject.SetActive(false);
    }

    public void ShowEndGameMenu()
    {
        totalEnemyCounterText.text = GameManagerScr.Instance.enemiesKilled.ToString();
        endGameMenu.SetActive(true);
    }

    public void RestartBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}