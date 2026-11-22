---
name: orientation-portrait
description: Quy chuẩn bố cục giao diện cho game màn hình dọc (Portrait 9:16 / 9:19.5) tối ưu thao tác một tay (Thumb-Zone). Kích hoạt khi thiết kế UI cho game mobile dọc, casual, idle, puzzle hoặc vertical shooter.
---

# Portrait Orientation UI Standards

Khi thiết kế hoặc chỉnh sửa giao diện cho game màn hình dọc (Mobile Portrait), BẮT BUỘC tuân thủ nguyên tắc **Thao tác một tay (Thumb-Zone)** và **Phân tầng theo chiều dọc**:

## 1. Cấu hình Canvas & Tỷ lệ tham chiếu
- **Reference Resolution:** Thiết lập `CanvasScaler` ở mức chuẩn **`1080 x 1920`** (tỷ lệ 9:16).
- **Screen Match Mode:** `Match Width Or Height` với giá trị **`Match = 0.0`** (ưu tiên khớp theo chiều rộng `1080px` để các nút bấm hàng ngang không bao giờ bị tràn mép trái/phải trên các máy màn hình hẹp).
- **Safe Area:** Bắt buộc bọc toàn bộ UI tương tác trong `Panel_SafeArea` có gắn `UISafeArea.cs` để tránh phần tai thỏ/Dynamic Island phía trên (`Top`) và thanh vuốt Home phía dưới (`Bottom`).

## 2. Phân tầng theo vùng ngón cái (Vertical Ergonomics)
Chia chiều cao màn hình thành 3 vùng chức năng rõ rệt:
- **Vùng Trên (Top 15% - Vùng chỉ đọc):**
  - Chỉ dùng để hiển thị thông tin (Thanh tài nguyên Vàng/Kim cương, Cấp độ, Điểm số, Thanh tiến trình màn chơi).
  - **Hạn chế đặt nút bấm quan trọng ở sát mép trên** vì ngón cái người chơi khó với tới khi cầm một tay. Nút `Btn_Pause` hoặc `Btn_Settings` nếu đặt ở trên phải có kích thước tối thiểu `80 x 80px`.
- **Vùng Giữa (Middle 55% - Vùng trình diễn):**
  - Dành cho nhân vật, bàn chơi, hiệu ứng hình ảnh hoặc danh sách thẻ bài cuộn dọc (`ScrollRect`).
- **Vùng Dưới (Bottom 30% - Vùng thao tác chính / Thumb-Zone):**
  - Đặt toàn bộ các nút hành động chính (`Btn_Play`, `Btn_Upgrade`, `Btn_Summon`), thanh điều hướng (Bottom Navigation Bar 3–5 tab), hoặc Joystick điều khiển ở khu vực này.
  - **Kích thước nút bấm:** Chiều cao tối thiểu của nút hành động chính là **`100px – 130px`**, khoảng cách giữa các nút trong `HorizontalLayoutGroup` tối thiểu **`16px`** để tránh bấm nhầm.

## 3. Quy chuẩn Popup & Văn bản trên màn hình hẹp
- **Chiều rộng Popup:** Chiếm **`88% – 92%` chiều rộng màn hình** (`~950px – 1000px`), căn giữa màn hình.
- **Xếp chồng dọc thay vì chia cột:** Với chiều rộng `1080px`, tuyệt đối **không chia quá 2 cột** cho các thẻ chứa nhiều chữ. Ưu tiên xếp các phần tử từ trên xuống dưới bằng `VerticalLayoutGroup` (Tiêu đề -> Hình ảnh minh họa -> Mô tả chỉ số -> Nút xác nhận ở đáy Popup).
- **Chống tràn chữ theo chiều ngang:** Mọi `TextMeshProUGUI` nằm trong thanh tài nguyên hoặc nút bấm hàng ngang phải bật `enableAutoSizing = true` hoặc giới hạn số ký tự hiển thị ngắn gọn (dùng định dạng rút gọn số lớn: `1.5K`, `2.4M`).