using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingWriteUI : MonoBehaviour
{
    [SerializeField] private Button[] keys;
    [SerializeField] private Button del;
    [SerializeField] private TextMeshProUGUI player;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Button submit;
    [SerializeField] private Button cancle;

    private void Awake()
    {
        var keycodes = new KeyCode[]
        {
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.R,
            KeyCode.T,
            KeyCode.Y,
            KeyCode.U,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.Z,
            KeyCode.X,
            KeyCode.C,
            KeyCode.V,
            KeyCode.B,
            KeyCode.N,
            KeyCode.M,
        };

        for (int i = 0; i < keycodes.Length; i++)
        {
            var c = $"{keycodes[i].ToString()[^1]}";
            keys[i].GetComponentInChildren<TextMeshProUGUI>().text = c;
            keys[i].onClick.AddListener(() => player.text += c);
        }
        del.onClick.AddListener(() => { if (player.text.Length > 0) player.text = player.text[..^1]; });
    }

    private void Start()
    {
        StartCoroutine(ScoreAnimation());
    }

    private IEnumerator ScoreAnimation()
    {
        var waitReal = new WaitForSecondsRealtime(0);

        yield return new WaitForSecondsRealtime(ScreenFade.FadeTime + 2f);

        for(float t = 0; t < 2; t += Time.unscaledDeltaTime)
        {
            score.text = $"{Mathf.RoundToInt(Mathf.Lerp(0, DataManager.Instance.Score, t / 2)):#,##0}";
            yield return waitReal;
        }

        submit.onClick.AddListener(() =>
        {
            if (ScreenFade.IsPlayLoading) return;

            var rankings = RankingDataSaveLoad.Load();
            rankings.Add((-1, player.text, DataManager.Instance.Score));
            rankings = rankings.OrderByDescending(item => item.score).Take(10).ToList();

            for(int i = 0; i < rankings.Count; i++)
            {
                var index = i;
                rankings[i] = (index, rankings[i].name, rankings[i].score);
            }
            RankingDataSaveLoad.Save(rankings);
            ScreenFade.LoadScene("Rank", false);
        });
        cancle.onClick.AddListener(() => { ScreenFade.LoadScene("Rank", false); });
    }
}