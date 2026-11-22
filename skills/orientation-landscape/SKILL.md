---
name: orientation-landscape
description: Quy chuẩn bố cục giao diện cho game màn hình ngang (Landscape 16:9 / 19.5:9) trên PC và Mobile. Kích hoạt khi thiết kế HUD, Menu, Popup, hoặc màn hình chơi game theo chiều ngang.
---

# Landscape Orientation UI Standards (Màn hình Ngang)

Khi thiết kế hoặc chỉnh sửa giao diện cho game màn hình ngang (PC, Console hoặc Mobile Landscape), BẮT BUỘC tuân thủ các quy tắc phân bổ không gian sau:

## 1. Cấu hình Canvas & Tỷ lệ tham chiếu
- **Reference Resolution:** Thiết lập `CanvasScaler` ở mức chuẩn **`1920 x 1080`** (tỷ lệ 16:9).
- **Screen Match Mode:** `Match Width Or Height` với giá trị **`Match = 0.5`** (cân bằng giữa màn hình 16:9 tiêu chuẩn và màn hình dài 19.5:9 / 21:9).
- **Safe Area (Chống tai thỏ / Camera đục lỗ):** Toàn bộ HUD bám sát mép trái/phải màn hình phải nằm bên trong một container `Panel_SafeArea` có gắn script `UISafeArea.cs` (xem trong `examples/`). Nền (Background/Dimmer) nằm ngoài Safe Area để phủ kín toàn màn hình.

## 2. Phân bổ vùng không gian HUD (4-Corner Rule)
- **Vùng trung tâm (Center 60%):** Giữ thông thoáng tuyệt đối cho không gian gameplay. Không đặt các thanh thông báo hoặc nút bấm thường trực che khuất tầm nhìn nhân vật/bản đồ ở giữa màn hình.
- **Góc Trên - Trái (`Anchor: Top-Left`):** Thông tin trạng thái người chơi (Avatar, Thanh máu/Mana, Cấp độ, Mini-map).
- **Góc Trên - Phải (`Anchor: Top-Right`):** Tài nguyên (Vàng, Kim cương, Gỗ...), đồng hồ đếm ngược/Wave hiện tại, và nút Cài đặt/Tạm dừng (`Btn_Pause`).
- **Cạnh Dưới (`Anchor: Bottom-Left` / `Bottom-Right` / `Bottom-Center`):**
  - *Nếu là game Mobile:* Đặt Joystick ảo ở `Bottom-Left` (cách mép ít nhất `80px`), cụm nút kỹ năng/tương tác ở `Bottom-Right` theo hình vòng cung ngón cái (`Button Size >= 90x90px`).
  - *Nếu là game PC/Chiến thuật/Xây dựng:* Đặt thanh công cụ (Hotbar / Danh sách công trình / Thẻ bài) ở `Bottom-Center` dùng `HorizontalLayoutGroup`.

## 3. Quy chuẩn Popup & Cửa sổ (Modals)
- **Kích thước tối đa:** Popup không được chiếm quá **80% chiều cao** và **75% chiều rộng** màn hình (`maxWidth <= 1440px`, `maxHeight <= 860px`) để người chơi vẫn nhận biết được bối cảnh game phía sau.
- **Bố cục chia cột (Split-View):** Tận dụng chiều ngang để chia Popup thành 2 cột bằng `HorizontalLayoutGroup` (VD: Cột trái 40% hiển thị danh sách vật phẩm cuộn dọc `ScrollRect`; Cột phải 60% hiển thị chi tiết chỉ số và nút hành động) thay vì dồn tất cả theo chiều dọc gây cuộn quá dài.