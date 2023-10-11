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
        var gm = Singleton.Get<GameManager>();

        goldText.text = string.Format("{0:#,###}", Singleton.Get<GameManager>().Money);

        int minutes = Mathf.FloorToInt(Singleton.Get<GameTimeManager>().PlayTime / 60F);
        int seconds = Mathf.FloorToInt(Singleton.Get<GameTimeManager>().PlayTime - minutes * 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        scoreText.text = string.Format("{0:#,##0}", DataManager.Instance.Score);

        stageText.text = $"{gm.Stage + 1} Stage";
        waveText.text = $"{gm.Wave + 1} / {gm.MaxWave} Wave";
        waveProgress.fillAmount = Mathf.Lerp(waveProgress.fillAmount, gm.WaveProgress, Time.unscaledDeltaTime * 5f);

        if(gm.EnemySlowTime > 0) enemySlow.fillAmount = gm.EnemySlowTime / 10;
        enemySlow.transform.parent.gameObject.SetActive(gm.EnemySlowTime > 0);

        if(gm.GainAdditiveGoldTime > 0) gainAdditiveGold.fillAmount = gm.GainAdditiveGoldTime / 60;
        gainAdditiveGold.transform.parent.gameObject.SetActive(gm.GainAdditiveGoldTime > 0);

        if(gm.ReduceAttackDelayTime > 0) reduceAttackDelay.fillAmount = gm.ReduceAttackDelayTime / 10;
        reduceAttackDelay.transform.parent.gameObject.SetActive(gm.ReduceAttackDelayTime > 0);

        if(gm.StopEnemyAttackTime > 0) stopEnemyAttack.fillAmount = gm.StopEnemyAttackTime / 10;
        stopEnemyAttack.transform.parent.gameObject.SetActive(gm.StopEnemyAttackTime > 0);
    }
}
