using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.Buttons
{
    public class VRCButton : ExtentedControl
    {
        public Transform transform;

        public VRCButton(Transform menu, string text, string tooltip, Action listener, bool Half = false, bool SubMenuIcon = false, Sprite Icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
        {
            if (!APIBase.IsReady()) { Logs.Error("Error, Something Was Missing!"); return; }

            Icon = Icon != null ? Icon : APIBase.DefaultButtonSprite; // Assigning value with ternary operator
            if (menu != null) APIBase.LastButtonParent = menu;
            else if (menu == null && APIBase.LastButtonParent == null) menu = APIBase.Button.parent;
            else if (menu == null && APIBase.LastButtonParent != null) menu = APIBase.LastButtonParent;

            transform = Object.Instantiate(APIBase.Button, menu);
            gameObject = transform.gameObject;

            transform.gameObject.SetActive(true);
            TMProCompnt = transform.GetComponentInChildren<TextMeshProUGUI>();
            transform.GetComponentInChildren<TextMeshProUGUIEx>().prop_String_0 = text;
            Text = text;
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            ButtonCompnt = transform.GetComponent<Button>();
            ButtonCompnt.onClick = new Button.ButtonClickedEvent();
            if (listener != null)
            {
                onClickAction = listener;
                ButtonCompnt.onClick.AddListener(new Action(() => APIBase.SafelyInvolk(onClickAction, Text)));
            }
            else ButtonCompnt.interactable = false;

            ImgCompnt = transform.transform.Find("Icon").GetComponent<Image>();
            if (ImgCompnt.gameObject.GetComponent<StyleElement>() != null)
                ImgCompnt.gameObject.GetComponent<StyleElement>().enabled = false; // Fix the Images from going back to the default

            if (Icon != null)
                ImgCompnt.sprite = Icon;
            else
            {
                transform.transform.Find("Icon").gameObject.active = false;
                ResetTextPox();
            }
            Object.Destroy(transform.transform.Find("Icon_Secondary").gameObject);
            gameObject.transform.Find("Badge_MMJump").gameObject.SetActive(SubMenuIcon);
            this.SetToolTip(tooltip);
            if (Half) TurnHalf(Type, IsGroup);
            transform.name = "Button_" + text;
        }

        public VRCButton RSetActive(bool val)
        {
            SetActive(val);
            return this;
        }

        public VRCButton(ButtonGroup buttonGroup, string text, string tooltip, Action click, bool Half = false, bool subMenuIcon = false, Sprite icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
            : this(buttonGroup.transform, text, tooltip, click, Half, subMenuIcon, icon, Type, IsGroup) => buttonGroup._buttons.Add(this);

        public VRCButton(CollapsibleButtonGroup buttonGroup, string text, string tooltip, Action click, bool Half = false, bool subMenuIcon = false, Sprite icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
            : this(buttonGroup.buttonGroup.transform, text, tooltip, click, Half, subMenuIcon, icon, Type, IsGroup) => buttonGroup.buttonGroup._buttons.Add(this);
    }
}