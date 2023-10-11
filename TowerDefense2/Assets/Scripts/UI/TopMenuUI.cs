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
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Image waveProgress;
    [Header("EffectIcon")]
    [SerializeField] private Image enemySlow;
    [SerializeField] private Image gainAdditiveGold;
    [SerializeField] private Image reduceAttackDelay;
    [SerializeField] private Image stopEnemyAttack;

    private void Update()
    {
        goldText.text = string.Format("{0:#,###}", Singleton.Get<GameManager>().Money);

        int minutes = Mathf.FloorToInt(Singleton.Get<GameTimeManager>().PlayTime / 60F);
        int seconds = Mathf.FloorToInt(Singleton.Get<GameTimeManager>().PlayTime - minutes * 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        scoreText.text = string.Format("{0:#,###}", DataManager.Instance.Score);

        var gm = Singleton.Get<GameManager>();
        stageText.text = $"{gm.Stage + 1} Stage";
        waveText.text = $"{gm.Wave + 1} / {gm.MaxWave} Wave";

        waveProgress.fillAmount = Mathf.Lerp(waveProgress.fillAmount, gm.WaveProgress, Time.unscaledDeltaTime * 5f);
    }
}
