using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    private float timeScale;

    public bool IsGameStopped { get; set; }
    public float TimeScale { get { return IsGameStopped ? 0 : timeScale; } set { timeScale = value; } }
    public float PlayTime { get; private set; }

    private void Awake()
    {
        Singleton.Register(this);

        timeScale = 1;
    }

    private void Update()
    {
        Time.timeScale = timeScale;
        PlayTime += Time.deltaTime;
    }
}
