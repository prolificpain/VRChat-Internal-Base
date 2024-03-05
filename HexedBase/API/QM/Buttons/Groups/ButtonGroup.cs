using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;
using WorldAPI;
using WorldAPI.ButtonAPI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;

public class ButtonGroup : ButtonGrp
{
    public Transform transform;
    private GridLayoutGroup Layout;

    public ButtonGroup(Transform parent, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter)
    {
        if (!APIBase.IsReady()) throw new Exception();

        WasNoText = NoText;

        if (!NoText)
        {
            headerGameObject = Object.Instantiate(APIBase.ButtonGrpText, parent);
            TMProCompnt = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            headerGameObject.GetComponentInChildren<TextMeshProUGUIEx>().prop_String_0 = text;
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            Text = text;
        }

        gameObject = Object.Instantiate(APIBase.ButtonGrp, parent);
        gameObject.name = text;
        gameObject.transform.DestroyChildren();
        transform = gameObject.transform;

        Layout = gameObject.GetOrAddComponent<GridLayoutGroup>();
        Layout.childAlignment = ButtonAlignment;

        parentMenuMask = parent.parent.GetOrAddComponent<RectMask2D>();
    }

    public void ChangeChildAlignment(TextAnchor ButtonAlignment = TextAnchor.UpperCenter) => Layout.childAlignment = ButtonAlignment;

    public ButtonGroup(VRCPage page, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter) : this(page.menuContents, text, NoText, ButtonAlignment)
    {
    }
}