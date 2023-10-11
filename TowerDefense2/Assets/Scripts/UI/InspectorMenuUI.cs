using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectorMenuUI : DownMenuAttachUI
{
    protected override bool OpenCriteria => downMenuUI.State == DownMenuState.inspect;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private TextMeshProUGUI upgradeTxt;

    private ISelectable tempSelectObject;
    private IUpgradeable tempUpgradeObject;

    private void Start()
    {
        upgradeBtn.onClick.AddListener(() =>
        {
            tempUpgradeObject.Upgrade();
            Singleton.Get<GameManager>().Money -= tempUpgradeObject.RequireCost;
        });
    }

    protected override void Update()
    {
        base.Update();

        var sm = Singleton.Get<SelectManager>();

        text.text = sm.SelectedObject?.ExplainContent;

        if (sm.SelectedObject != null)
        {
            if (!ReferenceEquals(tempSelectObject, sm.SelectedObject))
            {
                tempSelectObject = sm.SelectedObject;
                tempUpgradeObject = tempSelectObject.gameObject.GetComponent<IUpgradeable>();
            }
        }
        else
        {
            tempSelectObject = null;
            tempUpgradeObject = null;
        }

        if (tempUpgradeObject != null)
            upgradeTxt.text = $"Upgrade : {tempUpgradeObject.RequireCost:#,###}";

        upgradeBtn.gameObject.SetActive(tempUpgradeObject != null && tempUpgradeObject.RequireCost != -1);
    }
}
