using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cointLabel;
    [SerializeField] TextMeshProUGUI distanceLabel;
    int coint = 0;
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

    public void UpdateCoint(int amount)
    {
        coint += amount;
        cointLabel.text = "Coints\n" + coint;
    }

    public void UpdateDistance(float dis)
    {
        distanceLabel.text = "Distance\n" + dis + " m";
    }
}
