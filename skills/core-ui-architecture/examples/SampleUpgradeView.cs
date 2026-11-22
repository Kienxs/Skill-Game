using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Core.Examples
{
    public readonly struct UpgradeViewData
    {
        public readonly string BuildingName;
        public readonly int Level;
        public readonly int UpgradeCost;
        public readonly bool CanAfford;

        public UpgradeViewData(string buildingName, int level, int upgradeCost, bool canAfford)
        {
            BuildingName = buildingName;
            Level = level;
            UpgradeCost = upgradeCost;
            CanAfford = canAfford;
        }
    }

    public interface IUpgradeView : IUIView
    {
        event Action OnUpgradeClicked;
        event Action OnCloseClicked;
        void Render(UpgradeViewData data);
    }

    public class SampleUpgradeView : BaseUIView, IUpgradeView
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private TextMeshProUGUI txtCost;
        [SerializeField] private Button btnUpgrade;
        [SerializeField] private Button btnClose;

        [Header("Colors")]
        [SerializeField] private Color affordableColor = Color.white;
        [SerializeField] private Color unaffordableColor = Color.red;

        public event Action OnUpgradeClicked;
        public event Action OnCloseClicked;

        protected override void Awake()
        {
            base.Awake();

            if (!ValidateReferences()) return;

            btnUpgrade.onClick.AddListener(HandleUpgradeClick);
            btnClose.onClick.AddListener(HandleCloseClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (btnUpgrade != null)
            {
                btnUpgrade.onClick.RemoveListener(HandleUpgradeClick);
            }

            if (btnClose != null)
            {
                btnClose.onClick.RemoveListener(HandleCloseClick);
            }
        }

        public void Render(UpgradeViewData data)
        {
            if (txtTitle != null) txtTitle.text = data.BuildingName;
            if (txtLevel != null) txtLevel.text = $"Lv.{data.Level}";

            if (txtCost != null)
            {
                txtCost.text = data.UpgradeCost.ToString("N0");
                txtCost.color = data.CanAfford ? affordableColor : unaffordableColor;
            }

            if (btnUpgrade != null)
            {
                btnUpgrade.interactable = data.CanAfford;
            }
        }

        private bool ValidateReferences()
        {
            bool isValid = true;

            if (txtTitle == null) { Debug.LogError($"[{nameof(SampleUpgradeView)}] Thiếu {nameof(txtTitle)}.", this); isValid = false; }
            if (txtLevel == null) { Debug.LogError($"[{nameof(SampleUpgradeView)}] Thiếu {nameof(txtLevel)}.", this); isValid = false; }
            if (txtCost == null) { Debug.LogError($"[{nameof(SampleUpgradeView)}] Thiếu {nameof(txtCost)}.", this); isValid = false; }
            if (btnUpgrade == null) { Debug.LogError($"[{nameof(SampleUpgradeView)}] Thiếu {nameof(btnUpgrade)}.", this); isValid = false; }
            if (btnClose == null) { Debug.LogError($"[{nameof(SampleUpgradeView)}] Thiếu {nameof(btnClose)}.", this); isValid = false; }

            return isValid;
        }

        private void HandleUpgradeClick() => OnUpgradeClicked?.Invoke();
        private void HandleCloseClick() => OnCloseClicked?.Invoke();
    }
}