using System;
using UnityEngine;

namespace Game.UI.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseUIView : MonoBehaviour
    {
        [Header("Base UI Components")]
        [SerializeField] protected CanvasGroup canvasGroup;

        private bool _isInitialized;

        public event Action OnViewOpened;
        public event Action OnViewClosed;

        public bool IsVisible { get; private set; }

        protected virtual void Awake()
        {
            EnsureInitialized();
        }

        private void EnsureInitialized()
        {
            if (_isInitialized) return;

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            IsVisible = gameObject.activeSelf && canvasGroup != null && canvasGroup.alpha > 0f;
            _isInitialized = true;
        }

        public void Show()
        {
            EnsureInitialized();
            if (IsVisible) return;

            IsVisible = true;
            SetBlocksRaycasts(true);
            OnBeforeShow();

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            OnViewOpened?.Invoke();
            PlayShowTransition();
        }

        public void Hide()
        {
            EnsureInitialized();
            if (!IsVisible) return;

            IsVisible = false;
            SetInteractable(false);
            PlayHideTransition(CompleteHide);
        }

        protected virtual void OnBeforeShow()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }

            SetInteractable(true);
        }

        protected virtual void PlayShowTransition()
        {
        }

        protected virtual void PlayHideTransition(Action onComplete)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }

            onComplete?.Invoke();
        }

        private void CompleteHide()
        {
            if (IsVisible) return;

            SetBlocksRaycasts(false);
            OnViewClosed?.Invoke();

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetInteractable(bool interactable)
        {
            EnsureInitialized();
            if (canvasGroup == null) return;
            canvasGroup.interactable = interactable;
        }

        public void SetBlocksRaycasts(bool blocksRaycasts)
        {
            EnsureInitialized();
            if (canvasGroup == null) return;
            canvasGroup.blocksRaycasts = blocksRaycasts;
        }
    }
}