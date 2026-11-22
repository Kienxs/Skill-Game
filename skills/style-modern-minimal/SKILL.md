---
name: style-modern-minimal
description: Quy chuẩn thẩm mỹ và hiệu ứng UI cho game phong cách Hiện đại, Tối giản (Flat UI, bo góc, khoảng thở rộng, độ tương phản cao, animation mượt). Kích hoạt khi thiết kế UI cho game casual hiện đại, sci-fi, thể thao, hoặc mô phỏng.
---

# Modern & Minimalist UI Style Guidelines

Khi thiết kế giao diện cho game mang phong cách Hiện đại / Tối giản, BẮT BUỘC tuân thủ nguyên tắc **Sạch sẽ - Rõ phân cấp - Khoảng thở rộng**:

## 1. Bố cục & Khoảng thở (Whitespace & Hierarchy)
- **Flat & Clean Card:** Sử dụng các khối chữ nhật bo góc (`Sliced` rounded rectangle) với màu nền đơn sắc hoặc bán trong suốt (`Alpha ~ 0.85 - 0.95`) thay vì khung viền trang trí phức tạp.
- **Khoảng cách thoáng (Generous Spacing):**
  - Khoảng cách giữa các thẻ/khối độc lập (`spacing`): tối thiểu **`16px – 24px`**.
  - Khoảng đệm bên trong thẻ (`padding`): tối thiểu **`20px`** đều 4 cạnh.
- **Phân cấp thị giác (Visual Hierarchy):** Trong cùng một khối thông tin, phân tách rõ ràng kích thước chữ giữa Tiêu đề (`28px–36px`, Bold), Nội dung chính (`20px–24px`, Medium) và Chú thích phụ (`16px–18px`, Regular, giảm độ mờ xuống `70%`).

## 2. Hệ màu Hiện đại (Color System)
- **Quy tắc 60 - 30 - 10:**
  - **60% Nền trung tính:** Tối (`#121417`, `#1E222A` cho Dark Mode) hoặc Sáng (`#F4F6F9`, `#FFFFFF` cho Light Mode).
  - **30% Khối phụ / Thẻ chứa:** Tách biệt nhẹ so với nền bằng độ sáng chênh lệch `8%–12%` hoặc viền mỏng `1px–2px` bán trong suốt.
  - **10% Màu nhấn (Accent Color):** Chỉ dùng màu rực (Xanh dương `#3B82F6`, Xanh lá `#10B981`, Cam `#F59E0B`) cho nút hành động chính (`CTA Button`) và thanh tiến trình quan trọng. Không tô màu sặc sỡ tràn lan lên nền.

## 3. Hiệu ứng Chuyển động (Snappy & Smooth DOTween)
- Giao diện hiện đại ưu tiên tốc độ phản hồi nhanh, dứt khoát và mượt mà:
  - **Mở Popup / Panel:** Trượt nhẹ theo trục Y (`anchoredPosition.y` dịch chuyển `30px`) kết hợp Fade Alpha từ `0` lên `1` trong `0.2s` với `Ease.OutCubic` (không dùng bounce/nảy quá đà).
  - **Chuyển Tab / Cập nhật thanh tiến trình:** Dùng `DOFillAmount` hoặc `DOAnchorPos` trong `0.18s – 0.25s` với `Ease.OutQuart`.