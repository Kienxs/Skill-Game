using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Core.Examples
{
    public class SampleUpgradeView : BaseUIView
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private TextMeshProUGUI txtCost;
        [SerializeField] private Button btnUpgrade;
        [SerializeField] private Button btnClose;

        public event Action OnUpgradeClicked;
        public event Action OnCloseClicked;

        protected override void Awake()
        {
            base.Awake();
            btnUpgrade.onClick.AddListener(() => OnUpgradeClicked?.Invoke());
            btnClose.onClick.AddListener(() => OnCloseClicked?.Invoke());
        }

        private void OnDestroy()
        {
            btnUpgrade.onClick.RemoveAllListeners();
            btnClose.onClick.RemoveAllListeners();
        }

        public void Render(string title, int currentLevel, int upgradeCost, bool canAfford)
        {
            txtTitle.text = title;
            txtLevel.text = $"Lv.{currentLevel}";
            txtCost.text = upgradeCost.ToString("N0");
            
            btnUpgrade.interactable = canAfford;
            txtCost.color = canAfford ? Color.white : Color.red;
        }
    }

    public class SampleUpgradePresenter : IDisposable
    {
        private readonly SampleUpgradeView _view;
        private int _currentLevel = 1;
        private int _playerGold = 500;

        public SampleUpgradePresenter(SampleUpgradeView view)
        {
            _view = view;
            _view.OnUpgradeClicked += HandleUpgradeRequest;
            _view.OnCloseClicked += HandleCloseRequest;

            RefreshUI();
        }

        private void HandleUpgradeRequest()
        {
            int cost = CalculateUpgradeCost(_currentLevel);
            if (_playerGold < cost) return;

            _playerGold -= cost;
            _currentLevel++;

            RefreshUI();
        }

        private void HandleCloseRequest()
        {
            _view.Hide();
        }

        private void RefreshUI()
        {
            int nextCost = CalculateUpgradeCost(_currentLevel);
            bool canAfford = _playerGold >= nextCost;
            _view.Render("Barracks", _currentLevel, nextCost, canAfford);
        }

        private int CalculateUpgradeCost(int level) => level * 150;

        public void Dispose()
        {
            _view.OnUpgradeClicked -= HandleUpgradeRequest;
            _view.OnCloseClicked -= HandleCloseRequest;
        }
    }
}