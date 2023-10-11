using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomOutButtonUI : MonoBehaviour
{
    [SerializeField] private Button btn;

    private void Update()
    {
        btn.gameObject.SetActive(Singleton.Get<InGameCameraManager>().CameraState != CameraState.normal);
        btn.onClick.AddListener(() =>
        {
            Singleton.Get<SelectManager>().Unselect();
            Singleton.Get<InGameCameraManager>().CameraState = CameraState.normal;
            Singleton.Get<DownMenuUI>().State = DownMenuState.none;
        });
    }
}
