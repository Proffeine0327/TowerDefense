using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DownMenuState { none, item, tower, inspect }

public class DownMenuUI : MonoBehaviour
{
    [SerializeField] private Button itemBtn;
    [SerializeField] private Button towerBtn;

    public DownMenuState State { get; set; }

    private void Awake()
    {
        Singleton.Register(this);

        itemBtn.onClick.AddListener(() =>
        {
            State = State == DownMenuState.item ? DownMenuState.none : DownMenuState.item;
            Singleton.Get<InGameCameraManager>().CameraState = CameraState.normal;
            Singleton.Get<SelectManager>().Unselect();
        });
        towerBtn.onClick.AddListener(() =>
        {
            State = State == DownMenuState.tower ? DownMenuState.none : DownMenuState.tower;
            Singleton.Get<InGameCameraManager>().CameraState = CameraState.normal;
            Singleton.Get<SelectManager>().Unselect();
        });
    }
}