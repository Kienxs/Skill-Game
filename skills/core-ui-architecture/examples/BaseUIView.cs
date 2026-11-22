using System;
using UnityEngine;

namespace Game.UI.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseUIView : MonoBehaviour
    {
        [Header("Base UI Components")]
        [SerializeField] protected CanvasGroup canvasGroup;

        public event Action OnViewOpened;
        public event Action OnViewClosed;

        public bool IsVisible { get; private set; }

        protected virtual void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            SetInteractable(true);
            canvasGroup.alpha = 1f;
            IsVisible = true;
            OnViewOpened?.Invoke();
        }

        public virtual void Hide()
        {
            SetInteractable(false);
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            IsVisible = false;
            OnViewClosed?.Invoke();
        }

        public void SetInteractable(bool interactable)
        {
            if (canvasGroup == null) return;
            canvasGroup.interactable = interactable;
            canvasGroup.blocksRaycasts = interactable;
        }
    }
}