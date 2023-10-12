using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingDisplay : MonoBehaviour
{
    [SerializeField] private bool isCombine;

    private TextMeshProUGUI index;
    private TextMeshProUGUI player;
    private TextMeshProUGUI score;

    public void Set(int index, string name, int score)
    {
        if(this.index != null) this.index.gameObject.SetActive(index != -1);
        if(this.player != null) this.index.gameObject.SetActive(index != -1);
        if(this.index != null) this.index.gameObject.SetActive(index != -1);

        if(index == -1) return;

        var indexStr = index switch
        {
            0 => "1st",
            1 => "2nd",
            2 => "3rd",
            _ => $"{index + 1}."
        };

        if(isCombine)
        {
            this.index.text = $"{indexStr} {name}   {score:#,##0}";
            return;
        }

        this.index.text = indexStr;
        this.player.text = name;
        this.score.text = $"{score:#,##0}";
    }

    private void Awake()
    {
        index = transform.GetChild(0)?.GetComponent<TextMeshProUGUI>();
        player = transform.GetChild(1)?.GetComponent<TextMeshProUGUI>();
        score = transform.GetChild(2)?.GetComponent<TextMeshProUGUI>();
    }
}
