using BritoWorks;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasScene : BritoBehavior
    {
        public bool IsActive;
        private CanvasGroup canvasGroup;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Start()
        {
            SetActive(approx(canvasGroup.alpha, 1));
        }

        // Update is called once per frame
        void Update()
        {
            float desiredAlpha = IsActive ? 1 : 0;

            if (approx(canvasGroup.alpha, desiredAlpha))
            {
                canvasGroup.alpha = desiredAlpha;
                return;
            }

            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, desiredAlpha,
                Time.deltaTime * CanvasSceneManager.Instance.TransitionTime);
            
        }

        public void SetActive(bool active, bool instant = false)
        {
            IsActive = active;
            canvasGroup.interactable = active;
            canvasGroup.blocksRaycasts = active;
            if (instant)
            {
                canvasGroup.alpha = active ? 1 : 0;
            }
        }
    }
}