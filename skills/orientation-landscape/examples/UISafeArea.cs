using UnityEngine;

namespace Game.UI.Layout
{
    [RequireComponent(typeof(RectTransform))]
    public class UISafeArea : MonoBehaviour
    {
        [SerializeField] private bool conformX = true;
        [SerializeField] private bool conformY = true;

        private RectTransform _panel;
        private Rect _lastSafeArea = Rect.zero;
        private Vector2Int _lastScreenSize = Vector2Int.zero;
        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            Refresh();
        }

        private void Update()
        {
            if (_lastSafeArea != Screen.safeArea 
                || _lastScreenSize.x != Screen.width 
                || _lastScreenSize.y != Screen.height 
                || _lastOrientation != Screen.orientation)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            Rect safeArea = Screen.safeArea;

            if (Screen.width <= 0 || Screen.height <= 0) return;

            _lastSafeArea = safeArea;
            _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            _lastOrientation = Screen.orientation;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x = conformX ? anchorMin.x / Screen.width : 0f;
            anchorMin.y = conformY ? anchorMin.y / Screen.height : 0f;
            anchorMax.x = conformX ? anchorMax.x / Screen.width : 1f;
            anchorMax.y = conformY ? anchorMax.y / Screen.height : 1f;

            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
            _panel.offsetMin = Vector2.zero;
            _panel.offsetMax = Vector2.zero;
        }
    }
}