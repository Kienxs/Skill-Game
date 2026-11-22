---
name: style-pixel-retro
description: Quy chuẩn kỹ thuật và thẩm mỹ UI cho game Pixel Art / Retro (khóa Pixel Perfect, chống mờ chữ, kích thước bội số chẵn, hiệu ứng giật cục theo nhịp retro). Kích hoạt khi thiết kế UI cho game 2D Pixel Art.
---

# Pixel Art & Retro UI Style Guidelines

Khi thiết kế giao diện cho game 2D Pixel Art, lỗi phổ biến nhất là làm mờ sprite hoặc vỡ hạt chữ do sai tỷ lệ. BẮT BUỘC tuân thủ các quy tắc kỹ thuật sau:

## 1. Quy tắc Chống mờ & Vỡ hạt (Pixel Integrity)
- **Bội số kích thước chẵn:** Mọi kích thước của khung (`Width`, `Height`), `padding`, và `spacing` trong `LayoutGroup` BẮT BUỘC phải là **số nguyên chẵn** (ưu tiên chia hết cho `2` hoặc `4`, VD: `8, 12, 16, 24, 32`) để tránh lỗi làm tròn sub-pixel gây nhòe viền sprite.
- **Tắt tính năng làm mờ:** Đảm bảo các Sprite UI sử dụng `Filter Mode = Point (no filter)` và `Compression = None`.
- **Canvas Pixel Perfect:** Khuyến nghị bật `Pixel Perfect = true` trên component `Canvas` gốc (nếu dùng `Screen Space - Overlay`) để mọi cạnh pixel luôn sắc nét thẳng hàng.

## 2. Chuẩn Font chữ Pixel (TextMeshPro Bitmap)
- **Kích thước Font chuẩn:** Chỉ đặt `Font Size` ở các bội số chuẩn của font pixel gốc (thường là bội số của `8` hoặc `16`, VD: `16, 24, 32, 48`). Tránh để số lẻ như `19` hay `23` vì sẽ làm các nét chữ pixel dày mỏng không đều nhau.
- **Tắt Auto-Sizing tùy tiện:** Nếu bật `enableAutoSizing` cho font pixel, chỉ giới hạn trong khoảng hẹp hoặc dùng font pixel có độ phân giải cao để tránh hiện tượng biến dạng hạt pixel.
- **Đổ bóng cứng (Hard Shadow):** Dùng `Underlay` trong `TextMeshPro` với `Softness = 0` và `Offset X = 1, Offset Y = -1` để tạo bóng đổ sắc cạnh chuẩn retro thay vì bóng mờ hiện đại.

## 3. Hiệu ứng Chuyển động (Crisp Retro Motion)
- Giữ chuyển động gọn, dứt khoát và có nhịp điệu retro:
  - **Mở Popup:** Scale nhanh từ `0.9f` lên `1.0f` trong `0.12s – 0.15s` với `Ease.OutQuad` (hạn chế dùng `Ease.OutElastic` kéo dài vì dễ làm lệch lưới pixel trong lúc animation chạy).
  - **Hiệu ứng chữ chạy (Typewriter):** Với hộp thoại hội thoại/mô tả, ưu tiên hiển thị từng ký tự (`maxVisibleCharacters`) kết hợp âm thanh bíp ngắn thay vì fade mờ từng dòng.