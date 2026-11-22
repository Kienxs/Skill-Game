---
name: core-ui-architecture
description: Quy chuẩn kiến trúc MVP, quản lý vòng đời UI và bố cục UGUI (LayoutGroup, RectTransform, TMP, Raycast) cho Unity. Áp dụng khi tạo mới, sửa lỗi hoặc refactor màn hình, HUD, Popup và script UI.
---

# Core Unity UI & Architecture Standards

## 1. Layout & Positioning
- **Phạm vi dùng LayoutGroup vs RectTransform:**
  - Danh sách, nhóm nút, cụm thẻ lặp lại -> dùng `VerticalLayoutGroup`, `HorizontalLayoutGroup` hoặc `GridLayoutGroup`.
  - HUD neo góc màn hình, joystick ảo, nút kỹ năng, thanh máu theo đối tượng, hoặc UI có animation di chuyển -> dùng `Anchor` và `RectTransform` (`anchoredPosition`).
  - **Giới hạn:** Không thay đổi thủ công `anchoredPosition` hoặc `sizeDelta` trên phần tử con đang do `LayoutGroup` cha quản lý.
- **Spacing & Padding:** Chọn giá trị theo mật độ hiển thị và kích thước màn hình mục tiêu (không ép cứng một con số cho mọi UI, nhưng đảm bảo các phần tử tương tác không chạm/đè vùng bấm của nhau).
- **Quyền điều khiển kích thước (`ContentSizeFitter` & `LayoutElement`):**
  - Chỉ gắn `ContentSizeFitter` cho thành phần cần co giãn theo nội dung thực tế (VD: `Content` của `ScrollRect`, hộp thoại động, tooltip).
  - Nút kích thước cố định (nút kỹ năng, icon) -> dùng kích thước cố định hoặc `LayoutElement` (`minWidth`, `preferredWidth`), không gắn `ContentSizeFitter`.
  - **Tránh xung đột:** Không để `LayoutGroup` của cha (`Control Child Size`) và `ContentSizeFitter` của con cùng điều khiển cùng một trục kích thước trên một `RectTransform`.

## 2. CanvasScaler, TextMeshPro & Raycast
- **CanvasScaler:** Dùng `Scale With Screen Size`. Chọn `Reference Resolution` và `Match` dựa theo thiết kế mục tiêu hoặc skill định hướng màn hình (`orientation-landscape` / `orientation-portrait`); chỉ lấy `Match = 0.5` làm giá trị dự phòng khi không có quy định riêng.
- **TextMeshPro (`TextMeshProUGUI`):**
  - Dùng `TextMeshProUGUI`, không dùng `UnityEngine.UI.Text`.
  - Ưu tiên giữ cỡ chữ nhất quán theo hệ phân cấp (Tiêu đề / Nội dung / Nhãn phụ).
  - Văn bản nhiều dòng -> bật Word Wrapping.
  - Nhãn cố định chiều rộng dễ tràn chữ (tên dài, đa ngôn ngữ, số lớn) -> dùng `Overflow = Ellipsis` hoặc bật `Auto Sizing` có giới hạn chặt (`fontSizeMin` / `fontSizeMax`) để tránh mất đồng bộ thị giác.
- **Raycast Target:** Tắt `raycastTarget` trên mọi `Image` và `TextMeshProUGUI` chỉ mang tính trang trí.

## 3. Kiến trúc MVP & Vòng đời (Lifecycle)
Tách biệt rõ 4 vai trò:
- **View (`*View.cs`, kế thừa `BaseUIView`):** Giữ tham chiếu UI, render dữ liệu qua phương thức `Render(...)`, và phát `event Action` khi người dùng thao tác. Không chứa logic gameplay, không gọi trực tiếp Singleton/Manager.
- **Model / Service (`I*Service`):** Lưu trữ trạng thái và xử lý nghiệp vụ (trừ tài nguyên, kiểm tra điều kiện, lưu dữ liệu).
- **Presenter (`*Presenter.cs`, cài đặt `IDisposable`):** Nhận `View` và `Service` qua constructor. Đăng ký sự kiện từ `View`, gọi `Service` xử lý, và đẩy dữ liệu mới về `View`.
- **Composition Root / Installer (`*ScreenInstaller.cs` hoặc `UIManager`):** Chịu trách nhiệm khởi tạo `Presenter`, truyền `View` + `Service`, và gọi `presenter.Dispose()` trong `OnDestroy()`.

**Quy tắc xử lý sự kiện & trạng thái mở/đóng:**
- Khi `View` tạm ẩn (`Hide()` / `SetActive(false)`), `Presenter` vẫn giữ subscription để phản hồi lần mở kế tiếp.
- Khi `View` mở lại (`OnViewOpened`), `Presenter` tự động đọc dữ liệu mới nhất từ `Service` và gọi `View.Render(...)` để chống hiển thị dữ liệu cũ.
- Khi `View` hoặc Installer bị hủy (`OnDestroy()`), bắt buộc hủy toàn bộ listener (`RemoveAllListeners()`) và gọi `Presenter.Dispose()` để tránh rò rỉ bộ nhớ.

## 4. Quy ước đặt tên Hierarchy
- Nút bấm: `Btn_<Tên>`
- Văn bản: `Txt_<Tên>`
- Hình ảnh / Icon: `Img_<Tên>`
- Thanh tiến trình: `Bar_<Tên>` hoặc `Slider_<Tên>`
- Khung chứa / Nhóm: `Panel_<Tên>` hoặc `Group_<Tên>`

## 5. Tài liệu tham chiếu
Đọc 3 file trong thư mục `examples/` trước khi tạo mới hệ thống UI:
- `examples/BaseUIView.cs`: Chuẩn lifecycle `Show`/`Hide`, tách `interactable` và `blocksRaycasts`, hỗ trợ animation đóng/mở.
- `examples/SampleUpgradeView.cs`: Mẫu cài đặt View thuần hiển thị.
- `examples/SampleUpgradePresenter.cs`: Mẫu cài đặt Presenter, Service và Installer quản lý vòng đời.

## 6. Kiểm tra sau khi chỉnh sửa (Verification)
Trước khi hoàn tất tác vụ UI, tự kiểm tra:
1. Không trùng tên class/namespace gây lỗi biên dịch (CS0101), đủ `using` và không có lỗi compile.
2. Không có xung đột `LayoutGroup` cha với `ContentSizeFitter` con trên cùng trục.
3. Mọi sự kiện đăng ký (`+=` / `AddListener`) đều có điểm hủy tương ứng (`-=` / `RemoveAllListeners` / `Dispose`).