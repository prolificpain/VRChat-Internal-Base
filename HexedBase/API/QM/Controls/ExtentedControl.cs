using System;
using UnityEngine;
using UnityEngine.UI;

namespace WorldAPI.ButtonAPI.Controls
{
    public class ExtentedControl : Root
    {
        public Button ButtonCompnt { get; internal set; }
        public Image ImgCompnt { get; internal set; }
        public Action onClickAction { get; internal set; }
        public string ToolTip { get; internal set; }

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
            ToolTip = tip;
            return tip;
        }

        public void SetSprite(Sprite sprite)
        {
            ImgCompnt.sprite = sprite;
        }

        public Sprite GetSprite()
        {
            return ImgCompnt.sprite;
        }

        public void ShowSubMenuIcon(bool state)
        {
            gameObject.transform.Find("Badge_MMJump").gameObject.SetActive(state);
        }

        public void SetIconColor(Color color)
        {
            ImgCompnt.color = color;
        }

        public void SetAction(Action newAction)
        {
            ButtonCompnt.onClick = new UnityEngine.UI.Button.ButtonClickedEvent(); // Create new UnityEvent
            ButtonCompnt.onClick.AddListener(newAction);
        }

        public void SetBackgroundImage(Sprite newImg)
        {
            gameObject.transform.Find("Background").GetComponent<Image>().sprite = newImg;
            gameObject.transform.Find("Background").GetComponent<Image>().overrideSprite = newImg;
            if (gameObject.transform.Find("Bg") != null)
            {
                gameObject.transform.Find("Bg").GetComponent<Image>().sprite = newImg;
                gameObject.transform.Find("Bg").GetComponent<Image>().overrideSprite = newImg;
            }
            gameObject.SetActive(false); // Deactivate and activate to refresh image
            gameObject.SetActive(true);
        }

        internal void ResetTextPox()
        {
            gameObject.transform.Find("Text_H4").transform.localPosition = Vector3.zero;
        }

        public void TurnHalf(HalfType Type, bool IsGroup)
        {
            ImgCompnt.gameObject.SetActive(false);
            var JmpPoz = gameObject.transform.Find("Badge_MMJump").localPosition;
            gameObject.transform.Find("Badge_MMJump").localPosition = new Vector3(JmpPoz.x, JmpPoz.y - 34, JmpPoz.z);
            if (IsGroup)
                ChangeBoth(new Vector2(0, -115));
            else
                ChangeBoth(new Vector2(0, -80));
            TMProCompnt.fontSize = 22;
            switch (Type)
            {
                case HalfType.Top:
                    ImgCompnt.transform.localPosition = Vector3.zero;
                    gameObject.transform.Find("Text_H4").transform.localPosition = new Vector3(0, 22, 0);
                    ChangeBoth(new Vector3(0, 53, 0));
                    break;

                case HalfType.Normal:
                    ImgCompnt.transform.localPosition = Vector3.zero;
                    gameObject.transform.Find("Text_H4").transform.localPosition = Vector3.zero;
                    break;

                case HalfType.Bottom:
                    ImgCompnt.transform.localPosition = Vector3.zero;
                    gameObject.transform.Find("Text_H4").transform.localPosition = new Vector3(0, -69.9f, 0);
                    ChangeBoth(new Vector3(0, -53, 0));
                    break;
            }
        }

        private void ChangeBoth(Vector2 vec2)
        {
            gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = vec2;
            if (gameObject.transform.Find("Bg") != null)
                gameObject.transform.Find("Bg").GetComponent<RectTransform>().sizeDelta = vec2;
        }

        private void ChangeBoth(Vector3 vec)
        {
            gameObject.transform.Find("Background").localPosition = vec;
            if (gameObject.transform.Find("Bg") != null)
                gameObject.transform.Find("Bg").localPosition = vec;
        }

        public enum HalfType
        {
            Top,
            Normal,
            Bottom
        }
    }
}