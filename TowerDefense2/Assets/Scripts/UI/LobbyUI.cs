using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button gameStart;
    [SerializeField] private Button introduce;
    [SerializeField] private Button ranking;
    [SerializeField] private Button exit;

    private void Awake()
    {
        gameStart.onClick.AddListener(() => ScreenFade.LoadScene("Stage1"));
        exit.onClick.AddListener(() => Application.Quit());
        ranking.onClick.AddListener(() => ScreenFade.LoadScene("Rank", false));
    }
}
