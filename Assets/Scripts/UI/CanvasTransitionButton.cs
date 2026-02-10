using BritoWorks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class CanvasTransitionButton : BritoBehavior
    {
        public bool OneShot = false;
        public CanvasScene TargetScene;
        private Button button;
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            
            var currentCanvas = GetComponentInParent<CanvasScene>();
            currentCanvas.SetActive(false);
            TargetScene.SetActive(true);
            if(OneShot) Destroy(gameObject);
        }
    }
}