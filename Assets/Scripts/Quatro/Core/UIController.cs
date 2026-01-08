using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quatro.Core
{
    public class UIController : Singleton<UIController>
    {

        [SerializeField] private TMP_Text DebugText;
        [SerializeField] private Button ViewBoardButton; 
        
        public static void SetDebugText(string text) => Instance.DebugText.text = text;
        void Start()
        {
            
        }
    }
}