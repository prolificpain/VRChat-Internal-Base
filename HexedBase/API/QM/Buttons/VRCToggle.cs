using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.Buttons;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.Buttons
{
    public class VRCToggle : ToggleControl
    {
        private static Transform ToggleTemplate;

        public VRCToggle(Transform menu, string text, Action<bool> listener, bool DefaultState = false,
            string OffTooltip = null, string OnToolTip = null,
            Sprite onimage = null, Sprite offimage = null, bool half = false)
        {
            if (!APIBase.IsReady()) throw new Exception();

            if (menu != null)
                APIBase.LastButtonParent = menu;
            else if (menu == null && APIBase.LastButtonParent == null)
                menu = APIBase.Button.parent;
            else if (menu == null && APIBase.LastButtonParent != null)
                menu = APIBase.LastButtonParent;
            OffTooltip = OffTooltip != null ? OffTooltip : $"Turn On {text.Replace("\n", string.Empty)}";
            OnToolTip = OnToolTip != null ? OnToolTip : $"Turn Off {text.Replace("\n", string.Empty)}";

            if (ToggleTemplate == null)
            {
                ToggleTemplate = new VRCButton(APIBase.ButtonGrp.transform, text, string.Empty, null).transform;

                Object.DestroyImmediate(ToggleTemplate.GetComponent<Button>());
                Object.DestroyImmediate(ToggleTemplate.Find("Icon_Secondary"));
                Object.DestroyImmediate(ToggleTemplate.Find("Badge_Close"));
                Object.DestroyImmediate(ToggleTemplate.Find("Badge_MMJump"));
                ToggleTemplate.gameObject.AddComponent<Toggle>();
                var defaultImageObj = ToggleTemplate.Find("Icon");
                defaultImageObj.name = "Icon_Off";
                var onImge = Object.Instantiate(defaultImageObj, defaultImageObj.parent);
                onImge.name = "Icon_On";
                defaultImageObj.gameObject.active = true;
                onImge.gameObject.active = true;
                defaultImageObj.transform.localScale = new Vector3(.7f, .7f, .7f);
                ToggleTemplate.gameObject.SetActive(false);
            }

            Transform transform = Object.Instantiate(ToggleTemplate, menu);
            gameObject = transform.gameObject;
            transform.gameObject.SetActive(true);

            ToggleCompnt = transform.GetOrAddComponent<Toggle>();
            transform.GetComponentInChildren<TextMeshProUGUIEx>().prop_String_0 = text;
            TMProCompnt = transform.GetComponentInChildren<TextMeshProUGUI>();
            Text = text;
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            ToggleCompnt.onValueChanged = new Toggle.ToggleEvent();
            State = DefaultState;
            Listener = listener;
            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) =>
            {
                APIBase.SafelyInvolk(val, Listener, Text);
                OnImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, val ? 1 : 0.17f);
                OffImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, val ? 0.17f : 1);
                APIBase.Events.onVRCToggleValChange?.Invoke(this, val);
                SetToolTip(val ? OnToolTip : OffTooltip);
            }));

            OnImage = gameObject.transform.Find("Icon_On").GetComponent<Image>();
            OffImage = gameObject.transform.Find("Icon_Off").GetComponent<Image>();
            OnImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, DefaultState ? 1 : 0.17f);
            OffImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, DefaultState ? 0.17f : 1);
            OffImage.transform.localPosition = new Vector3(-46f, 43, 0);
            OnImage.transform.localPosition = new Vector3(49, 55, 0);

            inst = this;

            SetImages(true, onimage, offimage);
            SetToolTip(DefaultState ? OnToolTip : OffTooltip);
            if (half) TurnHalf();
            transform.name = text;
        }

        public VRCToggle RSetActive(bool val)
        {
            SetActive(val);
            return this;
        }

        public VRCToggle(ButtonGroup buttonGroup, string text, Action<bool> stateChanged, bool DefaultState = false, string OffTooltip = "Off", string OnToolTip = "On")
            : this(buttonGroup.transform, text, stateChanged, DefaultState, OffTooltip, OnToolTip) => buttonGroup._toggles.Add(this);

        public VRCToggle(CollapsibleButtonGroup buttonGroup, string text, Action<bool> stateChanged, bool DefaultState = false, string OffTooltip = "Off", string OnToolTip = "On")
            : this(buttonGroup.buttonGroup.transform, text, stateChanged, DefaultState, OffTooltip, OnToolTip) => buttonGroup.buttonGroup._toggles.Add(this);
    }
}