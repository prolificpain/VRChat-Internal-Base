using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using System;
using System.Collections;
using UnityEngine;





namespace HexedBase
{
    public class Entry : HexedCheat // Define the Main Class for the Loader
    {
        public override void OnLoad()
        {
            // Entry thats getting called by HexedLoader 
            Console.WriteLine("Hexed Base Cheat successfully loaded!");

            // Specify our main function hooks to let the loader know about the games base functions, it takes any method that matches the original unity function struct
            MonoManager.PatchUpdate(typeof(VRCApplication).GetMethod(nameof(VRCApplication.Update))); // Update is needed to work with IEnumerators, hooking it will enable the CoroutineManager
            MonoManager.PatchOnApplicationQuit(typeof(VRCApplication).GetMethod(nameof(VRCApplication.OnApplicationQuit))); // Optional Hook to enable the OnApplicationQuit callback

            // Apply our custom Hooked function
            PatchManager.ApplyStorePatch();

            // Start a delayed function
            CoroutineManager.RunCoroutine(waitForQM());
        }

        public override void OnApplicationQuit()
        {
            Console.WriteLine("Game Closed! Bye!");
        }

        public override void OnUpdate()
        {
            // Methods that need to run every frame, added a simple hotkey zoom module
            KeybindManager.Update();
        }

        public override void OnFixedUpdate()
        {
            // Function is not hooked, won't get called
        }

        public override void OnLateUpdate()
        {
            // Function is not hooked, won't get called
        }

        public override void OnGUI()
        {
            // Function is not hooked, won't get called
        }

        private static IEnumerator waitForQM()
        {
            while (GameObject.Find("Canvas_QuickMenu(Clone)") == null) yield return null; // Trys to find the Qm, if the Find Return null, it will wait for the next frame and check
            yield return null; // wait one more frame, this isn't needed, but shut up

            MakeMenu.MakeMainMenu();

            yield break; // This stops the IEnum
        }

    }
}
