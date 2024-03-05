using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Buttons;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.Buttons;

namespace WorldAPI.ButtonAPI.Controls
{
    public class ButtonGrp : Root
    {
        internal List<VRCButton> _buttons = new List<VRCButton>(); // Explicitly specifying List type
        internal List<VRCToggle> _toggles = new List<VRCToggle>(); // Explicitly specifying List type

        public GameObject headerGameObject { get; internal set; }
        public RectMask2D parentMenuMask { get; internal set; }
        public bool WasNoText { get; internal set; }
        public List<VRCButton> Buttons => _buttons.Where(x => x.gameObject != null).ToList();
        public List<VRCToggle> Toggles => _toggles.Where(x => x.gameObject != null).ToList();

        /// <summary>
        ///  Remove Buttons, Toggles, anything that was put on this ButtnGrp
        /// </summary>
        public void RemoveAllChildren()
        {
            gameObject.transform.DestroyChildren();
            _buttons.Clear();
            _toggles.Clear();
        }
    }
}