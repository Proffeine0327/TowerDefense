using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI stageText;

    private void Update()
    {
        goldText.text = Singleton.Get<GameManager>().Money.ToString("C");

        int minutes = Mathf.FloorToInt(Singleton.Get<GameTimeManager>().PlayTime / 60F);
        int seconds = Mathf.FloorToInt(Singleton.Get<GameTimeManager>().PlayTime - minutes * 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        scoreText.text = DataManager.Instance.Score.ToString("C");
    }
}
