using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;

public class CollapsibleButtonGroup : Root
{
    public bool IsOpen;
    public GameObject headerObj;
    public ButtonGroup buttonGroup;

    public CollapsibleButtonGroup(Transform parent, string text, bool openByDefault = true)
    {
        if (!APIBase.IsReady()) throw new Exception();

        headerObj = Object.Instantiate(APIBase.ColpButtonGrp, parent);
        headerObj.name = $"{text}_CollapsibleButtonGroup";

        headerObj.transform.Find("QM_Settings_Panel/VerticalLayoutGroup").DestroyChildren();

        TMProCompnt = headerObj.transform.Find("QM_Foldout/Label").GetComponent<TMPro.TextMeshProUGUI>();
        TMProCompnt.richText = true;
        TMProCompnt.text = text;

        buttonGroup = new ButtonGroup(parent, string.Empty, true);
        gameObject = buttonGroup.gameObject;

        var foldout = headerObj.transform.Find("QM_Foldout/Background_Button").GetComponent<Toggle>();
        foldout.isOn = openByDefault;
        foldout.onValueChanged.AddListener(new Action<bool>(val =>
        {
            buttonGroup.gameObject.SetActive(val);
            IsOpen = val;
        }));
    }

    /// <summary>
    ///  Remove Buttons, Toggles, anything that was put on this ButtnGrp
    /// </summary>
    public void RemoveAllChildren() =>
        buttonGroup.gameObject.transform.DestroyChildren();

    public CollapsibleButtonGroup(VRCPage page, string text, bool openByDefault = false) : this(page.menuContents, text, openByDefault)
    {
    }
}