using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreLabel;
    int score = 0;
    // Start is called before the first frame update

    // singleton pattern
    private static GameUI _instance;
    public static GameUI Instance { get { return _instance; } }
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

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreLabel.text = "Score\n" + score;
    }
}
