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
            btnUpgrade.onClick.AddListener(HandleUpgradeClick);
            btnClose.onClick.AddListener(HandleCloseClick);
        }

        private void OnDestroy()
        {
            btnUpgrade.onClick.RemoveListener(HandleUpgradeClick);
            btnClose.onClick.RemoveListener(HandleCloseClick);
        }

        public void Render(UpgradeViewData data)
        {
            txtTitle.text = data.BuildingName;
            txtLevel.text = $"Lv.{data.Level}";
            txtCost.text = data.UpgradeCost.ToString("N0");
            txtCost.color = data.CanAfford ? affordableColor : unaffordableColor;

            btnUpgrade.interactable = data.CanAfford;
        }

        private void HandleUpgradeClick() => OnUpgradeClicked?.Invoke();
        private void HandleCloseClick() => OnCloseClicked?.Invoke();
    }
}