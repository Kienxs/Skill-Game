---
name: core-ui-architecture
description: Quy chuẩn kiến trúc MVP, quản lý vòng đời UI và bố cục UGUI (LayoutGroup, RectTransform, TMP, Raycast) cho Unity. Áp dụng khi tạo mới, sửa lỗi hoặc refactor màn hình, HUD, Popup và script UI.
---

# Core Unity UI & Architecture Standards

## 1. Layout, Positioning & Performance
- **LayoutGroup vs RectTransform:**
  - Danh sách, nhóm nút, cụm thẻ lặp lại -> dùng `VerticalLayoutGroup`, `HorizontalLayoutGroup` hoặc `GridLayoutGroup`.
  - HUD neo góc màn hình, joystick ảo, nút kỹ năng, thanh máu bám đối tượng -> dùng `Anchor` và `RectTransform` (`anchoredPosition`).
  - Không thay đổi trực tiếp `anchoredPosition` hoặc `sizeDelta` trên phần tử con đang do `LayoutGroup` cha quản lý.
  - **Animate phần tử trong LayoutGroup:** Không tween trực tiếp `RectTransform` của slot; tạo wrapper con bên trong slot để tween, hoặc bật tạm `LayoutElement.ignoreLayout = true`.
- **Spacing & Padding:** Chọn theo mật độ UI và kích thước màn hình mục tiêu, đảm bảo vùng bấm giữa các phần tử tương tác không chồng lấn.
- **Quyền điều khiển kích thước (`ContentSizeFitter` & `LayoutElement`):**
  - Chỉ gắn `ContentSizeFitter` cho phần tử cần co giãn theo nội dung (`Content` của `ScrollRect`, tooltip, hộp thoại động).
  - Nút kích thước cố định (nút kỹ năng, icon) -> dùng kích thước cố định hoặc `LayoutElement`, không gắn `ContentSizeFitter`.
  - **Tránh xung đột:** Không để `LayoutGroup` cha (`Control Child Size`) và `ContentSizeFitter` con cùng điều khiển một trục trên cùng một `RectTransform`.
- **Safe Area & Danh sách/Cell:**
  - **Safe Area:** Dùng component Safe Area sẵn có của dự án; nếu chưa có thì tạo một lần duy nhất. Nền/dimmer giữ full-screen bên ngoài Safe Area.
  - **List/Cell:** Với danh sách dài, dùng Object Pooling thay vì `Instantiate`/`Destroy` liên tục. Mỗi cell chỉ là view thụ động (`Bind(data)` + event), do `Presenter` của màn hình quản lý — không tạo `Presenter` riêng cho từng cell.

## 2. CanvasScaler, TextMeshPro & Raycast
- **CanvasScaler:** Dùng `Scale With Screen Size`. Chọn `Reference Resolution` và `Match` theo thiết kế mục tiêu (hoặc skill định hướng màn hình nếu dự án có cài đặt); chỉ dùng `Match = 0.5` làm mặc định dự phòng.
- **TextMeshPro (`TextMeshProUGUI`):**
  - Dùng `TextMeshProUGUI`, không dùng `UnityEngine.UI.Text`.
  - Giữ cỡ chữ nhất quán theo cấp bậc (Tiêu đề / Nội dung / Nhãn phụ).
  - Văn bản nhiều dòng -> bật Word Wrapping.
  - Nhãn cố định chiều rộng dễ tràn chữ -> dùng `Overflow = Ellipsis` hoặc `Auto Sizing` có giới hạn chặt (`fontSizeMin` / `fontSizeMax`).
- **Raycast Target:** Tắt `raycastTarget` trên mọi `Image` và `TextMeshProUGUI` chỉ để trang trí.

## 3. Kiến trúc MVP & Vòng đời (Lifecycle)
- **Khi nào KHÔNG cần MVP đầy đủ:** UI thuần trình bày, không chứa logic nghiệp vụ (toast, tooltip tĩnh, bảng chữ đơn giản) -> chỉ cần `View`, không tạo `Presenter`/`Service`/`Installer`.
- **Tái sử dụng `BaseUIView`:**
  - Kiểm tra dự án đã có `BaseUIView` chưa. Nếu đã có -> đối chiếu contract (`IsVisible`, `OnViewOpened`, `OnViewClosed`, `virtual Awake`, `virtual OnDestroy`, `KillTransitions`); nếu thiếu thành phần nào thì bổ sung vào bản sẵn có, **không tạo class trùng**.
  - Nếu chưa có -> copy `examples/BaseUIView.cs` **một lần duy nhất** vào thư mục UI Core, đổi namespace `Game.UI.Core` cho khớp quy ước dự án, và kiểm tra file `.asmdef` (nếu dự án dùng Assembly Definition, đảm bảo có tham chiếu `Unity.TextMeshPro`).
- **Phân tách 4 vai trò:**
  - **View (`*View.cs`, kế thừa `BaseUIView` & `I*View`):** Render dữ liệu qua `Render(...)`, phát `event Action` khi tương tác, kiểm tra null các `SerializeField` bắt buộc. Không chứa logic gameplay, không gọi Singleton/Manager.
  - **Model / Service (`I*Service`):** Lưu trữ trạng thái, xử lý nghiệp vụ và phát event (`StateChanged`) khi dữ liệu thay đổi.
  - **Presenter (`*Presenter.cs`, thuần C#, cài đặt `IDisposable`):** Phụ thuộc interface `I*View` và `I*Service`. Lắng nghe event từ cả `View` lẫn `Service`; chỉ gọi `Render(...)` khi `_view.IsVisible == true`. Không giả định `View.Awake()` đã chạy trước (chỉ dựa vào `OnViewOpened` và `IsVisible`).
  - **Installer (`*ScreenInstaller.cs`, file riêng khớp tên `MonoBehaviour`):** Là **nơi duy nhất** được phép lấy `Service` từ Singleton / ServiceLocator / DI container, khởi tạo `Presenter` trong `Start()` và gọi `Dispose()` trong `OnDestroy()`. Các class `Mock*` chỉ dùng trong mẫu/test, không copy vào production.
- **Quy tắc hiển thị, Animation & Sự kiện:**
  - Luôn bật/tắt UI qua `Show()` / `Hide()`, không gọi `SetActive()` từ bên ngoài. Không đặt `View` dưới một GameObject cha có thể bị tắt trực tiếp bằng `SetActive(false)`.
  - Subclass có animation (DOTween/Animator) phải override `KillTransitions()` để hủy tween đang chạy (tự động được gọi ở `Show()`, `Hide()` và `OnDestroy()`), và nếu override `OnDestroy()` phải gọi `base.OnDestroy()`.
  - Dùng cặp đối xứng `AddListener`/`RemoveListener` và `+=`/`-=`. Không dùng `RemoveAllListeners()`.

## 4. Quy ước đặt tên Hierarchy
- `Btn_<Tên>` | `Txt_<Tên>` | `Img_<Tên>` | `Bar_<Tên>` / `Slider_<Tên>` | `Panel_<Tên>` / `Group_<Tên>`

## 5. Tài liệu tham chiếu (Đọc khi cần)
Chỉ đọc `examples/` khi lần đầu dựng kiến trúc MVP cho dự án hoặc cần đối chiếu mẫu:
- `examples/BaseUIView.cs`: Class nền tảng (`Show`/`Hide`, `IsVisible`, `KillTransitions`, tách `interactable`/`blocksRaycasts`).
- `examples/SampleUpgradeView.cs`: Mẫu `IUpgradeView` + `SampleUpgradeView` có null-check và `override OnDestroy`.
- `examples/SampleUpgradePresenter.cs`: Mẫu `IUpgradeService` (có `StateChanged`) + `SampleUpgradePresenter` thuần C#.
- `examples/SampleUpgradeScreenInstaller.cs`: Mẫu `MonoBehaviour` đấu nối `Service` -> `Presenter` và hủy vòng đời.

## 6. Checklist trước khi hoàn tất
1. Mỗi `MonoBehaviour` nằm đúng file `.cs` khớp tên class; không trùng class/namespace (`CS0101`), không có warning `CS0114`, biên dịch sạch.
2. Không xung đột `LayoutGroup` cha với `ContentSizeFitter` con trên cùng trục.
3. Mọi đăng ký sự kiện (`View` lẫn `Service`) đều có hủy đối xứng (`RemoveListener`/`-=`/`Dispose`), không dùng `RemoveAllListeners()`.
4. UI chỉ bật/tắt qua `Show()`/`Hide()`, không `SetActive()` từ bên ngoài.
5. `Presenter` tạo ở `Start()` của `Installer`, `Dispose()` ở `OnDestroy()`; `Service` chỉ lấy ở `Installer` (không dùng `Mock*` cho production).
6. `Image`/`TMP` trang trí đã tắt `raycastTarget`; không dùng `UnityEngine.UI.Text`.
7. Tên Hierarchy đúng quy ước; không tạo trùng `BaseUIView`.