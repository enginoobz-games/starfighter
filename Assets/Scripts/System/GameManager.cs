using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("GAME SPAWNERS")]
    [SerializeField] ObjectSpawner enemySpawner;
    [SerializeField] ObjectSpawner mineSpawner;

    [Header("GAME STATUS")]
    [SerializeField] TextMeshProUGUI cointLabel;
    [SerializeField] TextMeshProUGUI distanceLabel;

    [Header("GAME PLAY")]
    [Tooltip("Boss will appear after this number of tiles since last boss defeat")]
    public int bossOccurrence = 3;
    [SerializeField] GiantWorm boss;

    int coint = 0;
    int travelDistance = 0;
    // Start is called before the first frame update

    // TODO: generic singleton pattern
    // singleton pattern
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateDistance), 0f, 1f);
    }

    public void UpdateCoint(int amount)
    {
        coint += amount;
        cointLabel.text = "Coints\n" + coint;
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
        boss.gameObject.SetActive(true);
        boss.Appear(new Vector3(CameraRig.Instance.transform.position.x + boss.appearDistance, 10, -20));
    }

    public void AfterBossDefeat()
    {
        enemySpawner.enabled = true;
        mineSpawner.enabled = true;
        CameraRig.Instance.AfterBossDefeat();
    }
}
