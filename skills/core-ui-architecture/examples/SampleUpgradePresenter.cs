using System;

namespace Game.UI.Core.Examples
{
    public interface IUpgradeService
    {
        event Action StateChanged;
        UpgradeViewData GetCurrentState();
        bool TryUpgrade();
    }


    public class MockUpgradeService : IUpgradeService
    {
        private readonly string _buildingName;
        private int _level = 1;
        private int _gold = 500;

        public event Action StateChanged;

        public MockUpgradeService(string buildingName)
        {
            _buildingName = buildingName;
        }

        public UpgradeViewData GetCurrentState()
        {
            int cost = GetUpgradeCost();
            return new UpgradeViewData(_buildingName, _level, cost, _gold >= cost);
        }

        public bool TryUpgrade()
        {
            int cost = GetUpgradeCost();
            if (_gold < cost) return false;

            _gold -= cost;
            _level++;
            StateChanged?.Invoke();
            return true;
        }

        private int GetUpgradeCost() => _level * 150;
    }

    public class SampleUpgradePresenter : IDisposable
    {
        private readonly IUpgradeView _view;
        private readonly IUpgradeService _service;
        private bool _isDisposed;

        public SampleUpgradePresenter(IUpgradeView view, IUpgradeService service)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _service = service ?? throw new ArgumentNullException(nameof(service));

            _view.OnViewOpened += Refresh;
            _view.OnUpgradeClicked += HandleUpgradeRequest;
            _view.OnCloseClicked += HandleCloseRequest;
            _service.StateChanged += HandleServiceStateChanged;

            if (_view.IsVisible)
            {
                Refresh();
            }
        }

        private void HandleUpgradeRequest()
        {
            // Service tự phát StateChanged khi dữ liệu thay đổi, tránh gọi Refresh() trùng lặp.
            _service.TryUpgrade();
        }

        private void HandleServiceStateChanged()
        {
            if (_view.IsVisible)
            {
                Refresh();
            }
        }

        private void HandleCloseRequest()
        {
            _view.Hide();
        }

        private void Refresh()
        {
            _view.Render(_service.GetCurrentState());
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _view.OnViewOpened -= Refresh;
            _view.OnUpgradeClicked -= HandleUpgradeRequest;
            _view.OnCloseClicked -= HandleCloseRequest;
            _service.StateChanged -= HandleServiceStateChanged;

            _isDisposed = true;
        }
    }
}