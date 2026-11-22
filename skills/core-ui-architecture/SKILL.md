---
name: core-ui-architecture
description: Quy chuẩn nền tảng về kiến trúc UI (MVP) và bố cục tự động (LayoutGroup, ContentSizeFitter, TextMeshPro) trong Unity. Kích hoạt bắt buộc khi tạo hoặc chỉnh sửa bất kỳ UI, HUD, Popup, Menu hoặc script giao diện nào.
---

# Core Unity UI & Architecture Rules

Mọi tác vụ tạo mới hoặc chỉnh sửa giao diện (UGUI) và mã nguồn UI trong Unity BẮT BUỘC tuân thủ nghiêm ngặt các nguyên tắc dưới đây.

## 1. Quy tắc Layout & Chống đè chữ (Zero-Overlap Policy)
- **Cấm đặt tọa độ thủ công:** Tuyệt đối không sắp xếp các phần tử UI cùng cấp bằng cách hardcode `anchoredPosition` chồng chéo. Mọi danh sách, nhóm nút, hoặc cụm thông tin phải dùng `VerticalLayoutGroup`, `HorizontalLayoutGroup`, hoặc `GridLayoutGroup` với thông số `padding` và `spacing` rõ ràng (tối thiểu `spacing >= 8`).
- **Tự động co giãn khung chứa:** Các hộp thoại, nút bấm có chữ dài, hoặc khung mô tả động phải gắn `ContentSizeFitter` (`Horizontal Fit` / `Vertical Fit` = `Preferred Size`) kết hợp `LayoutElement` (`minWidth`, `minHeight`, `preferredWidth`) để khung tự nở theo nội dung, không bao giờ cắt chữ hoặc đè lên phần tử bên cạnh.
- **Chuẩn TextMeshPro (TMP):**
  - 100% sử dụng `TextMeshProUGUI` (cấm dùng `UnityEngine.UI.Text` cũ).
  - Bật `enableWordWrapping = true` cho đoạn văn bản mô tả.
  - Với nhãn (Label) cố định chiều rộng (như tên nhân vật, số vàng), bật `enableAutoSizing = true` (thiết lập khoảng `fontSizeMin` và `fontSizeMax` hợp lý, VD: min 14, max 28) hoặc đặt `Overflow = Ellipsis` để chữ không bao giờ tràn ra ngoài khung.
- **Cấu hình Canvas gốc:** Mọi Canvas chính phải gắn `CanvasScaler` với `UI Scale Mode = Scale With Screen Size` và `Screen Match Mode = Match Width Or Height` (`Match = 0.5`).
- **Tối ưu Raycast:** Tắt `Raycast Target` trên toàn bộ `Image` và `TextMeshProUGUI` chỉ dùng để trang trí (không tương tác) để tránh chặn nhầm thao tác bấm nút.

## 2. Kiến trúc Logic UI (Strict MVP Pattern)
- **Tách bạch View và Logic:**
  - Script gắn trên Prefab UI (`*View.cs`, kế thừa từ `BaseUIView`) **chỉ làm nhiệm vụ hiển thị** (gán text, cập nhật `Slider`/`Image.fillAmount`, bật/tắt trạng thái `interactable` của nút) và phát ra `event Action` khi người chơi tương tác.
  - **Cấm tuyệt đối:** Không viết logic trừ tài nguyên, kiểm tra điều kiện nâng cấp, tính toán chỉ số, lưu file, hay gọi thẳng Singleton/Manager bên trong sự kiện `onClick` của `Button`.
- **Lớp điều phối (`*Presenter.cs` hoặc `*Controller.cs`):**
  - Lắng nghe `Action` từ `View`, giao tiếp với hệ thống dữ liệu/gameplay (Model/Service), sau đó gọi hàm cập nhật ngược lại cho `View`.
  - Bắt buộc hủy đăng ký sự kiện (`-=`) trong `OnDestroy()` hoặc `Dispose()` để chống rò rỉ bộ nhớ (Memory Leak).

## 3. Quy ước đặt tên Hierarchy & Biến Serialize
Đặt tên GameObject trong Hierarchy theo đúng tiền tố để dễ đọc và bind reference:
- Nút bấm: `Btn_<Tên>` (VD: `Btn_Confirm`, `Btn_Upgrade`)
- Văn bản: `Txt_<Tên>` (VD: `Txt_Title`, `Txt_GoldCost`)
- Hình ảnh / Icon: `Img_<Tên>` (VD: `Img_Background`, `Img_Icon`)
- Thanh tiến trình: `Slider_<Tên>` hoặc `Bar_<Tên>` (VD: `Bar_Health`)
- Khung chứa / Nhóm: `Panel_<Tên>` hoặc `Group_<Tên>` (VD: `Panel_Header`, `Group_Stats`)

## 4. Tham chiếu Code Mẫu
Trước khi viết script UI mới, hãy đọc các file trong thư mục `examples/` (`BaseUIView.cs` và `SampleUpgradePresenter.cs`) để tuân thủ đúng cấu trúc kế thừa, lifecycle và quản lý sự kiện.