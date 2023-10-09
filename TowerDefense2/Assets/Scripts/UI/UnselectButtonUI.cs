using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnselectButtonUI : MonoBehaviour
{
    [SerializeField] private Button btn;

    private void Update()
    {
        btn.gameObject.SetActive(Singleton.Get<SelectManager>().SelectedObject != null);
        btn.onClick.AddListener(() =>
        {
            Singleton.Get<SelectManager>().Unselect();
            Singleton.Get<DownMenuUI>().State = DownMenuState.none;
        });
    }
}
