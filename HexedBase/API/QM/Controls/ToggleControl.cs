using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Buttons;
using static WorldAPI.APIBase;

namespace WorldAPI.ButtonAPI.Controls
{
    public class ToggleControl : Root
    {
        public Toggle ToggleCompnt { get; internal set; }
        internal Action<bool> Listener { get; set; }
        public Image OnImage { get; internal set; }
        public Image OffImage { get; internal set; }
        internal VRCToggle inst { get; set; }
        public VRC.UI.Elements.Tooltips.UiToggleTooltip TipComp { get; internal set; }
        public bool IsHalf { get; internal set; }

        public bool State
        {
            get => ToggleCompnt.isOn;
            set => ToggleCompnt.isOn = value;
        }

        public void SetAction(Action<bool> newAction) => Listener = newAction;

        public void SoftSetState(bool value)
        {
            ToggleCompnt.onValueChanged = new Toggle.ToggleEvent();
            ToggleCompnt.isOn = value;
            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) =>
            {
                APIBase.SafelyInvolk(val, Listener, Text);
                Events.onVRCToggleValChange?.Invoke(inst, val);
                OnImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, val ? 1 : 0.17f);
                OffImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, val ? 0.17f : 1);
                if (IsHalf)
                {
                    OffImage.gameObject.active = !val;
                    OnImage.gameObject.active = val;
                }
            }));
            OnImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, value ? 1 : 0.17f);
            OffImage.color = new Color(OnImage.color.r, OnImage.color.g, OnImage.color.b, value ? 0.17f : 1);
        }

        public (Sprite, Sprite) SetImages(Sprite onSprite = null, Sprite offSprite = null)
        {
            OffImage.sprite = offSprite;
            OnImage.sprite = onSprite;
            return (onSprite, offSprite);
        }

        public void SetInteractable(bool val) => ToggleCompnt.interactable = val;

        public (Sprite, Sprite) SetImages(bool checkForNull, Sprite onSprite = null, Sprite offSprite = null)
        {
            if (checkForNull)
            {
                if (onSprite == null) onSprite = APIBase.OnSprite;
                if (offSprite == null) offSprite = APIBase.OffSprite;
            }
            if (offSprite != null)
                OffImage.sprite = offSprite;
            if (onSprite != null)
                OnImage.sprite = onSprite;
            return (onSprite, offSprite);
        }

        public void TurnHalf(Vector3 TogglePoz, float FontSize = 24f)
        {
            gameObject.transform.localPosition = TogglePoz;
            TurnHalf(FontSize);
        }

        public void TurnHalf(float FontSize = 24f)
        {
            OnImage.transform.localScale = new Vector3(0.86f, 0.86f, 0.86f);
            OnImage.transform.localPosition = new Vector3(-52.22f, 30.18f, 0f);
            OnImage.gameObject.SetActive(ToggleCompnt.isOn);
            OffImage.transform.localScale = new Vector3(0.86f, 0.86f, 0.86f);
            OffImage.transform.localPosition = new Vector3(-52.22f, 30.18f, 0f);
            OffImage.gameObject.SetActive(!ToggleCompnt.isOn);

            TMProCompnt.fontSize = FontSize;
            TMProCompnt.transform.localPosition = new Vector3(34.42f, -22, 0);
            TMProCompnt.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 50f);
            gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -80);
            if (gameObject.transform.Find("Bg") != null)
                gameObject.transform.Find("Bg").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -80);
            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) =>
            { // Adding Listener, so we dont have to reset it
                OffImage.gameObject.active = !val;
                OnImage.gameObject.active = val;
            }));
        }

        public string SetToolTip(string tip)
        {
            bool Fi = false;
            foreach (var s in gameObject.GetComponentsInChildren<VRC.UI.Elements.Tooltips.UiTooltip>())
            {
                if (!Fi)
                {
                    Fi = true;
                    s.Method_Public_Void_String_0(tip);
                    s.enabled = !string.IsNullOrEmpty(tip);
                }
                else s.enabled = false;
            }

            return tip;
        }
    }
}