---
name: style-classic-medieval
description: Quy chuẩn thẩm mỹ và hiệu ứng UI cho game phong cách Cổ điển, Trung cổ, Fantasy, Chiến thuật hoặc Sinh tồn làng mạc (khung gỗ/đá, giấy da, viền dày, hiệu ứng đầm chắc). Kích hoạt khi thiết kế UI cho game medieval, fantasy, RPG cổ điển.
---

# Classic & Medieval UI Style Guidelines

Khi thiết kế giao diện cho game mang phong cách Cổ điển / Trung cổ / Fantasy, BẮT BUỘC tuân thủ các quy tắc thị giác và chuyển động sau:

## 1. Cấu trúc Khung & Tỷ lệ (9-Slice & Padding)
- **9-Slice Sprite:** Mọi khung nền (`Panel_`), nút bấm (`Btn_`) và thanh tiến trình (`Bar_`) có viền chạm khắc gỗ, đá hoặc kim loại phải thiết lập `Image.Type = Sliced` với `Pixels Per Unit Multiplier` phù hợp để góc viền không bao giờ bị kéo dãn méo mó.
- **Vùng đệm an toàn cho viền dày (Inner Padding):** Vì khung cổ điển thường có viền trang trí dày ở 4 cạnh, `padding` của `VerticalLayoutGroup` hoặc `HorizontalLayoutGroup` bên trong khung phải tối thiểu **`24px – 36px`** để chữ không đè lên họa tiết viền khung.

## 2. Bảng màu & Chữ (Typography & Contrast)
- **Hệ màu ấm & tương phản:**
  - Nền giấy da sáng (Parchment): Dùng chữ màu nâu đậm (`#2B1B12`) hoặc đen than (`#1A1412`), **cấm dùng chữ trắng** trên nền giấy da.
  - Nền gỗ tối / đá phiến (`#231A16` / `#1E2229`): Dùng chữ màu vàng kem (`#F5E6C8`) hoặc vàng kim (`#FFD166`) cho tiêu đề.
- **Tạo chiều sâu cho chữ:** Bật `Outline` (màu tối dày `0.15 - 0.2`) hoặc `Underlay` (Drop Shadow) trong `TextMeshPro` để chữ nổi bật rõ ràng trên các bề mặt có vân (texture gỗ/đá).
- **Font chữ:** Ưu tiên font có chân (Serif) hoặc font mang hơi hướng cổ điển cho tiêu đề (`Txt_Title`), nhưng giữ font rõ nét, dễ đọc cho các con số và mô tả chỉ số dài.

## 3. Hiệu ứng Chuyển động (Tactile & Weighted DOTween)
- Giao diện trung cổ mang cảm giác vật lý nặng, đầm chắc (gỗ, sắt, đá), tránh dùng hiệu ứng nảy quá dẻo kiểu hoạt hình hiện đại:
  - **Mở Popup:** Scale từ `0.85f` lên `1.0f` kết hợp Fade Alpha từ `0` lên `1` trong `0.25s` với `Ease.OutBack` (overshoot nhẹ `1.1f`).
  - **Bấm nút (Button Punch):** Khi nhấn xuống (`OnPointerDown`), scale nút về `0.94f` trong `0.08s` (`Ease.OutQuad`); khi thả ra (`OnPointerUp`), trả về `1.0f` trong `0.12s`.