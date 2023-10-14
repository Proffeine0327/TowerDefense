using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private Button exit;
    [SerializeField] private RankingDisplay[] ranks;

    private void Start()
    {
        var data = RankingDataSaveLoad.Load();
        int i;
        for(i = 0; i < Mathf.Min(data.Count, ranks.Length); i++) ranks[i].Set(i, data[i].name, data[i].score);
        for(; i < ranks.Length; i++) ranks[i].Set(-1, "", 0);

        exit.onClick.AddListener(() => ScreenFade.LoadScene("Lobby", false));
    }
}
