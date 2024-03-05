using Il2CppInterop.Runtime;
using System;
using UnityEngine;
using VRC.UI.Core.Styles;
using WorldAPI.ButtonAPI.Buttons;

namespace WorldAPI
{
    internal static class Logs
    {
        internal static void Log(string message, ConsoleColor color = ConsoleColor.White)
        {
            //console.writeLine(message);
        }

        internal static void Debug(string message) => Log("[Debug] " + message, ConsoleColor.DarkGray);

        internal static void Error(Exception e, string message) => Error(message, e);

        internal static void Error(string message, Exception e = null)
        {
            //console.writeLine($"[Error] {message}");
        }
    }

    public class APIBase
    {
        public class Events
        {
            public static Action<VRCToggle, bool> onVRCToggleValChange = new Action<VRCToggle, bool>((er, str) => { });
            //public static Action<CToggle, bool> onCToggleValChange = new Action<CToggle, bool>((er, str) => { });
            //public static Action<WToggle, bool> onWToggleValChange = new Action<WToggle, bool>((er, str) => { });
        }

        /// <summary>
        ///  Set this if u want to override what happens when a button/ tgl throws an error
        /// </summary>
        public static Action<Exception, string> ErrorCallBack { get; set; } = new Action<Exception, string>((er, str) =>
        {
            Logs.Error($"The ButtonAPI had an Error At {str}", er);
        });

        public static Transform LastButtonParent;
        private static bool HasChecked;

        public static Sprite DefaultButtonSprite; // Override these if u want custom ones
        public static Sprite OffSprite, OnSprite; // Override these if u want custom ones
        public static GameObject QuickMenu, ColpButtonGrp, ButtonGrp, ButtonGrpText;
        public static Transform Button, Tab, MenuTab, Slider;

        public static bool IsReady()
        {
            if (HasChecked) return true;
            if ((QuickMenu = GameObject.Find("Canvas_QuickMenu(Clone)")) == null)
            {
                Logs.Error("QuickMenu Is Null!");
                return false;
            }
            if ((Button = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn")) == null)
            {
                Logs.Error("Button Is Null!");
                return false;
            }
            if ((Slider = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_QM_AudioSettings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/AudioVolume/QM_Settings_Panel/VerticalLayoutGroup/Master")) == null)
            {
                Logs.Error("Slider Is Null!");
                return false;
            }
            if ((MenuTab = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard")) == null)
            {
                Logs.Error("MenuTab Is Null!");
                return false;
            }

            if ((Tab = QuickMenu.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_DevTools")) == null)
            {
                Logs.Error("Tab Is Null!");
                return false;
            }
            if ((ButtonGrp = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").gameObject) == null)
            {
                Logs.Error("ButtonGrp Is Null!");
                return false;
            }
            if ((ButtonGrpText = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").gameObject) == null)
            {
                Logs.Error("ButtonGrpText Is Null!");
                return false;
            }
            if ((ColpButtonGrp = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_QM_GeneralSettings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/YourAvatar").gameObject) == null)
            {
                Logs.Error("ColpButtonGrp Is Null!");
                return false;
            }

            if (!GetToglSprites()) return false;
            HasChecked = true;
            return true;
        }

        private static bool GetToglSprites()
        {
            StyleEngine styleEngine = QuickMenu.GetComponent<StyleEngine>();
            var resources = styleEngine.field_Public_StyleResource_0.resources;
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].obj == null) continue;
                if (resources[i].obj.GetIl2CppType() == null) continue;
                if (resources[i].obj.GetIl2CppType() != Il2CppType.Of<Sprite>()) continue;

                if (resources[i].obj.name.Equals("Decline")) OffSprite = resources[i].obj.Cast<Sprite>();
                if (resources[i].obj.name.Equals("Checkmark")) OnSprite = resources[i].obj.Cast<Sprite>();
                if (OffSprite != null && OnSprite != null) break;
            }

            if (OffSprite == null)
            {
                Logs.Error("OffSprite Is Null!");
                return false;
            }
            if (OnSprite == null)
            {
                Logs.Error("OnSprite Is Null!");
                return false;
            }
            return true;
        }

        internal static void SafelyInvolk(Action action, string name)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                ErrorCallBack.Invoke(e, name);
            }
        }

        internal static void SafelyInvolk(bool state, Action<bool> action, string name)
        {
            try
            {
                action.Invoke(state);
            }
            catch (Exception e)
            {
                ErrorCallBack.Invoke(e, name);
            }
        }
    }
}