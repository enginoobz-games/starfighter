using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
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
    // Start is called before the first frame update

    // TODO: generic singleton pattern
    // singleton pattern
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
    {
        // if (_instance != null && _instance != this)
        // {
        //     Destroy(this.gameObject);
        // }
        // else
        // {
        //     _instance = this;
        // }
        _instance = this;
    }

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
        bestTravelDistance = Mathf.Max(travelDistance, bestTravelDistance);
        StartCoroutine(Helper.TimeScaleLerp(1f, 0.3f, 4f));
        StartCoroutine(nameof(ShowGameOverPanel));
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverPanel.SetActive(true);
        distanceGameOverLabel.text = "Travel distance: " + travelDistance;
        bestDistanceGameOverLabel.text = "Best distance: " + bestTravelDistance;

        // reset
        travelDistance = 0;
    }

    public void Replay()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
