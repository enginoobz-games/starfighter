using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Michsky.UI.ModernUIPack;

// TODO: make GameMananger persistent & remains references
public class CustomGameManager : MonoBehaviourSingleton<CustomGameManager>
{
    [Header("SPAWNERS")]
    [SerializeField] ObjectSpawner enemySpawner;
    [SerializeField] ObjectSpawner mineSpawner;
    [SerializeField] BossSpawner bossSpawner;

    [Header("GAME UI")]
    [SerializeField] TextMeshProUGUI coinLabel;
    [SerializeField] TextMeshProUGUI distanceLabel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI distanceGameOverLabel;
    [SerializeField] TextMeshProUGUI bestDistanceGameOverLabel;

    [Header("GAME PLAY")]
    [Tooltip("Boss will appear after this number of tiles since last boss defeat")]
    public int bossOccurrence = 3;
    int coint = 0;
    int travelDistance = 0;
    int bestTravelDistance = 0;
    bool isPaused = false;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateDistance), 0f, 1f);
    }

    public void UpdateCoint(int amount)
    {
        coint += amount;
        coinLabel.text = "Coints\n" + coint;
    }

    public void UpdateDistance()
    {
        travelDistance = (int)CameraRig.Instance.transform.position.x;
        distanceLabel.text = "Distance\n" + travelDistance + " m";
    }

    public void OnBossAppear()
    {
        enemySpawner.enabled = false;
        mineSpawner.enabled = false;
        bossSpawner.Spawn();
    }

    public void AfterBossDefeat()
    {
        enemySpawner.enabled = true;
        mineSpawner.enabled = true;
        CameraRig.Instance.AfterBossDefeat();
    }

    public void OnGameOver()
    {
        StartCoroutine(Helper.TimeScaleLerp(1f, 0.3f, 3f));
        StartCoroutine(nameof(ShowGameOverPanel));
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverPanel.SetActive(true);
        distanceGameOverLabel.text = "Travel distance: " + travelDistance;
        bestTravelDistance = Mathf.Max(travelDistance, bestTravelDistance);
        bestDistanceGameOverLabel.text = "Best distance: " + bestTravelDistance;

        // reset
        travelDistance = 0;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
        }
    }
}
