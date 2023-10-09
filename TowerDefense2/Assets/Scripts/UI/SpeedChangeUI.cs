using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedChangeUI : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI text;

    private int index;

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            index++;
            if(index == 3) index = 0;
            
            switch(index)
            {
                case 0:
                    text.text = "1x";
                    Singleton.Get<GameTimeManager>().TimeScale = 1;
                    break;
                case 1:
                    text.text = "2x";
                    Singleton.Get<GameTimeManager>().TimeScale = 2;
                    break;
                case 2:
                    text.text = "4x";
                    Singleton.Get<GameTimeManager>().TimeScale = 4;
                    break;
            }
        });
    }
}
