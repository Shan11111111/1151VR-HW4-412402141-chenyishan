
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    [Header("Existing fields")]
    public FlightFeedback airplane;
    public TMP_Text scoreText;
    public GameObject resultPanel;
    public TMP_Text resultTitle;
    public Button restartButton;

    [Header("Start Screen")]
    public GameObject startPanel;
    public Button startButton;

    [Header("Mobile Flight Controls")]
    public GameObject flightControls;

    private bool resultShown = false;
    private bool gameStarted = false;

    void Start()
    {
        resultShown = false;
        gameStarted = false;

        // 開始時只顯示開始介面
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (startPanel != null)
            startPanel.SetActive(true);

        // 飛行前隱藏方向按鈕
        ShowFlightControls(false);

        // 綁定開始與重新挑戰按鈕
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (startButton != null)
            startButton.onClick.AddListener(BeginGame);

        UpdateScore();
    }

    public void BeginGame()
    {
        if (gameStarted || resultShown)
            return;

        if (airplane == null)
        {
            Debug.LogError("尚未指定飛機物件！");
            return;
        }

        // 啟動飛行
        airplane.StartFlight();
        gameStarted = true;

        // 隱藏開始畫面
        if (startPanel != null)
            startPanel.SetActive(false);

        // 顯示手機方向按鈕
        ShowFlightControls(true);
    }

    void Update()
    {
        if (airplane == null)
            return;

        UpdateScore();

        if (!gameStarted || resultShown)
            return;

        if (airplane.IsCrashed)
        {
            ShowResult("飛行失敗！");
        }
        else if (airplane.IsCompleted)
        {
            ShowResult("挑戰成功！");
        }
    }

    void UpdateScore()
    {
        if (scoreText == null || airplane == null)
            return;

        scoreText.text =
            "通過：" +
            airplane.PassedCount +
            " / " +
            airplane.targetCheckpoints;
    }

    void ShowResult(string message)
    {
        if (resultShown)
            return;

        resultShown = true;
        gameStarted = false;

        // 結束時關閉方向按鈕
        ShowFlightControls(false);

        // 確保開始畫面已隱藏
        if (startPanel != null)
            startPanel.SetActive(false);

        // 顯示結果畫面
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultTitle != null)
            resultTitle.text = message;
    }

    void ShowFlightControls(bool visible)
    {
        if (flightControls != null)
            flightControls.SetActive(visible);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);

        if (startButton != null)
            startButton.onClick.RemoveListener(BeginGame);
    }
}
