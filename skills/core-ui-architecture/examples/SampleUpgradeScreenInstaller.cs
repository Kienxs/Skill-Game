using UnityEngine;

namespace Game.UI.Core.Examples
{
    public class SampleUpgradeScreenInstaller : MonoBehaviour
    {
        [SerializeField] private SampleUpgradeView upgradeView;

        private SampleUpgradePresenter _presenter;

        private void Start()
        {
            if (upgradeView == null)
            {
                Debug.LogError($"[{nameof(SampleUpgradeScreenInstaller)}] Thiếu tham chiếu {nameof(upgradeView)}.", this);
                return;
            }

            IUpgradeService service = new MockUpgradeService("Barracks");
            _presenter = new SampleUpgradePresenter(upgradeView, service);
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
            _presenter = null;
        }
    }
}