---
name: core-ui-architecture
description: Quy chuẩn kiến trúc MVP, quản lý vòng đời UI và bố cục UGUI (LayoutGroup, RectTransform, TMP, Raycast) cho Unity. Áp dụng khi tạo mới, sửa lỗi hoặc refactor màn hình, HUD, Popup và script UI.
---

# Core Unity UI & Architecture Standards

## 1. Layout, Positioning & Performance
- **LayoutGroup vs RectTransform:**
  - Danh sách, nhóm nút, cụm thẻ lặp lại -> dùng `VerticalLayoutGroup`, `HorizontalLayoutGroup` hoặc `GridLayoutGroup`.
  - HUD neo góc màn hình, joystick ảo, nút kỹ năng, thanh máu bám theo đối tượng -> dùng `Anchor` và `RectTransform` (`anchoredPosition`).
  - Không thay đổi trực tiếp `anchoredPosition` hoặc `sizeDelta` trên phần tử con đang do `LayoutGroup` cha quản lý.
  - **Animate phần tử trong LayoutGroup:** Không tween trực tiếp `RectTransform` của slot; tạo một wrapper con bên trong slot để tween, hoặc bật tạm `LayoutElement.ignoreLayout = true`.
- **Spacing & Padding:** Chọn theo mật độ UI và kích thước màn hình mục tiêu, đảm bảo vùng bấm giữa các phần tử tương tác không chồng lấn.
- **Quyền điều khiển kích thước (`ContentSizeFitter` & `LayoutElement`):**
  - Chỉ gắn `ContentSizeFitter` cho phần tử cần co giãn theo nội dung (VD: `Content` của `ScrollRect`, tooltip, hộp thoại động).
  - Nút kích thước cố định (nút kỹ năng, icon) -> dùng kích thước cố định hoặc `LayoutElement`, không gắn `ContentSizeFitter`.
  - **Tránh xung đột:** Không để `LayoutGroup` cha (`Control Child Size`) và `ContentSizeFitter` con cùng điều khiển một trục trên cùng một `RectTransform`.
- **Safe Area & Danh sách dài:**
  - Với nền tảng mobile có tai thỏ/camera đục lỗ, bọc vùng HUD tương tác trong container Safe Area (nền/dimmer giữ full-screen).
  - Với danh sách dài hoặc cuộn nhiều phần tử, dùng Object Pooling / tái sử dụng cell thay vì `Instantiate`/`Destroy` liên tục.

## 2. CanvasScaler, TextMeshPro & Raycast
- **CanvasScaler:** Dùng `Scale With Screen Size`. Chọn `Reference Resolution` và `Match` theo thiết kế mục tiêu (hoặc skill định hướng màn hình nếu dự án có cài đặt); chỉ dùng `Match = 0.5` làm mặc định dự phòng.
- **TextMeshPro (`TextMeshProUGUI`):**
  - Dùng `TextMeshProUGUI`, không dùng `UnityEngine.UI.Text`.
  - Giữ cỡ chữ nhất quán theo cấp bậc (Tiêu đề / Nội dung / Nhãn phụ).
  - Văn bản nhiều dòng -> bật Word Wrapping.
  - Nhãn cố định chiều rộng dễ tràn chữ -> dùng `Overflow = Ellipsis` hoặc `Auto Sizing` có giới hạn chặt (`fontSizeMin` / `fontSizeMax`).
- **Raycast Target:** Tắt `raycastTarget` trên mọi `Image` và `TextMeshProUGUI` chỉ để trang trí.

## 3. Kiến trúc MVP & Vòng đời (Lifecycle)
- **Tái sử dụng `BaseUIView`:** Kiểm tra dự án đã có `BaseUIView` chưa. Nếu có -> kế thừa trực tiếp. Nếu chưa -> copy `examples/BaseUIView.cs` **một lần duy nhất** vào thư mục UI Core của dự án. Không tạo class trùng tên.
- **Phân tách 4 vai trò:**
  - **View (`*View.cs`, kế thừa `BaseUIView` và cài đặt `I*View`):** Render dữ liệu qua `Render(...)`, phát `event Action` khi tương tác. Không chứa logic gameplay, không gọi thẳng Singleton/Manager.
  - **Model / Service (`I*Service`):** Lưu trữ trạng thái và xử lý nghiệp vụ.
  - **Presenter (`*Presenter.cs`, cài đặt `IDisposable`):** Phụ thuộc vào interface `I*View` và `I*Service` (thuần C#, dễ unit test).
  - **Installer / Composition Root (`*ScreenInstaller.cs`, file riêng khớp tên `MonoBehaviour`):** Khởi tạo `Presenter` trong `Start()` (sau khi toàn bộ `Awake()` đã chạy xong) và gọi `Dispose()` trong `OnDestroy()`.
- **Quy tắc hiển thị & quản lý sự kiện:**
  - **Luôn bật/tắt UI qua `Show()` / `Hide()`**, không gọi `gameObject.SetActive()` trực tiếp từ bên ngoài để tránh lệch trạng thái `IsVisible`.
  - Khi `View` mở lại (`OnViewOpened`), `Presenter` đọc trạng thái mới nhất từ `Service` và gọi `Render(...)` để chống dữ liệu cũ.
  - Dùng cặp đối xứng `AddListener` / `RemoveListener` và `+=` / `-=`. Không dùng `RemoveAllListeners()` vì sẽ xóa nhầm listener runtime của script khác. Ưu tiên dọn dẹp triệt để subscription tới `Service` và `Presenter` trong `Dispose()`.

## 4. Quy ước đặt tên Hierarchy
- Nút bấm: `Btn_<Tên>` | Văn bản: `Txt_<Tên>` | Ảnh/Icon: `Img_<Tên>` | Thanh tiến trình: `Bar_<Tên>` hoặc `Slider_<Tên>` | Khung chứa: `Panel_<Tên>` hoặc `Group_<Tên>`.

## 5. Tài liệu tham chiếu (Đọc theo nhu cầu)
Chỉ đọc các file trong `examples/` khi lần đầu dựng kiến trúc MVP cho dự án hoặc cần kiểm tra mẫu triển khai:
- `examples/BaseUIView.cs`: Class nền tảng quản lý `Show`/`Hide`, `IsVisible`, tách `interactable`/`blocksRaycasts` và hook animation.
- `examples/SampleUpgradeView.cs`: Mẫu `IUpgradeView` + `SampleUpgradeView`.
- `examples/SampleUpgradePresenter.cs`: Mẫu `IUpgradeService` + `SampleUpgradePresenter` (thuần C#).
- `examples/SampleUpgradeScreenInstaller.cs`: Mẫu `MonoBehaviour` gắn lên Scene để đấu nối và hủy vòng đời.

## 6. Checklist trước khi hoàn tất
1. Mỗi `MonoBehaviour` nằm trong file `.cs` khớp tên class; không trùng tên class/namespace (`CS0101`); đủ `using` và biên dịch sạch.
2. Không xung đột `LayoutGroup` cha với `ContentSizeFitter` con trên cùng trục.
3. Mọi cặp đăng ký sự kiện (`AddListener`/`+=`) đều có hủy đối xứng (`RemoveListener`/`-=`/`Dispose`), không dùng `RemoveAllListeners()`.