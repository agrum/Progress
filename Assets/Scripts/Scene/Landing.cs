using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP;
using SimpleJSON;
using UnityEngine.UI;

namespace West
{
    namespace Scene
    {
        class Landing : MonoBehaviour
        {
            public View.TextButton buttonPlayV = null;
            public View.TextButton buttonSpecializeV = null;
            public View.TextButton buttonGearV = null;
            public View.TextButton buttonTradeV = null;

            private ViewModel.ButtonPlay buttonPlayVM = null;
            private ViewModel.ButtonSpecialize buttonSpecializeVM = null;
            private ViewModel.ButtonGear buttonGearVM = null;
            private ViewModel.ButtonTrade buttonTradeVM = null;

            void Start()
            {
                if (buttonPlayV == null
                    || buttonSpecializeV == null
                    || buttonGearV == null
                    || buttonTradeV == null)
                {
                    Debug.Log("View buttons are null in Landing");
                    return;
                }

                App.Content.Account.Load(() =>
                {
                    Setup();
                });
            }

            private void Setup()
            {
                if (this == null)
                    return;

                buttonPlayVM = new ViewModel.ButtonPlay(buttonPlayV);
                buttonSpecializeVM = new ViewModel.ButtonSpecialize(buttonSpecializeV);
                buttonGearVM = new ViewModel.ButtonGear(buttonGearV);
                buttonTradeVM = new ViewModel.ButtonTrade(buttonTradeV);
            }
        }
    }
}
