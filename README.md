# 🎮 Unity Game UI & Architecture Skills (for Antigravity)

Bộ sưu tập các **AI Agent Skills** được thiết kế chuyên biệt cho phát triển giao diện (UI/UX) và kiến trúc hệ thống trong **Unity**, tối ưu hóa cho **Antigravity**.

Dự án này giải quyết triệt để hai vấn đề lớn nhất khi dùng AI lập trình giao diện game:
1. **Lỗi thị giác (Visual Bugs):** Chữ đè lên nhau, tràn khung, sai tỷ lệ màn hình do AI tự đoán tọa độ pixel.
2. **Lỗi kiến trúc (Logic Coupling):** Code UI rối rắm, nhét trực tiếp logic gameplay vào sự kiện nút bấm (`onClick`).

---

## 📂 Cấu trúc thư mục Skills

Hệ thống được chia thành các module độc lập để bạn có thể kết hợp linh hoạt tùy theo từng dự án game:

| Nhóm Skill | Tên thư mục | Mô tả chức năng |
| :--- | :--- | :--- |
| **Nền tảng (Bắt buộc)** | `core-ui-architecture` | Ép buộc dùng `LayoutGroup`, `ContentSizeFitter`, chuẩn `TextMeshPro` chống đè chữ và tách lớp logic theo mô hình **MVP (Model-View-Presenter)**. |
| **Khung hình (Orientation)** | `orientation-landscape` | Tối ưu bố cục màn hình ngang (`1920x1080`), phân bổ HUD 4 góc và quản lý vùng thông tin trung tâm. |
| **Khung hình (Orientation)** | `orientation-portrait` | Tối ưu bố cục màn hình dọc (`1080x1920`), thiết kế theo **Thumb-Zone** (vùng thao tác ngón cái phía dưới) và xử lý tai thỏ (`SafeArea`). |
| **Phong cách (Art Style)** | `style-classic-medieval` | Quy chuẩn UI cho game cổ điển / trung cổ / fantasy (khung viền dày, bảng gỗ/giấy da, hiệu ứng DOTween đầm chắc). |
| **Phong cách (Art Style)** | `style-modern-minimal` | Quy chuẩn UI cho game hiện đại / casual / sci-fi (thiết kế phẳng, bo góc, độ tương phản cao, animation mượt). |
| **Phong cách (Art Style)** | `style-pixel-retro` | Quy chuẩn UI cho game Pixel Art (khóa `Pixel Perfect`, font chữ không làm mờ, kích thước bội số chẵn). |

---

## 🚀 Hướng dẫn cài đặt & Sử dụng

### Cách 1: Cài đặt cho từng dự án Unity cụ thể (Khuyên dùng)
Copy thư mục skill bạn cần vào thư mục `.agent/skills/` nằm ở gốc dự án Unity của bạn.

Ví dụ: Bạn đang làm game **màn hình ngang** theo phong cách **cổ điển**, hãy copy 3 thư mục sau vào project:
```text
YourUnityProject/
└── .agent/
    └── skills/
        ├── core-ui-architecture/
        ├── orientation-landscape/
        └── style-classic-medieval/

Cách 2: Cài đặt dùng chung toàn máy (Global)
Copy các thư mục skill vào đường dẫn cấu hình global của Antigravity:

Windows: C:\Users\<Tên_User>\.gemini\antigravity\skills\

macOS / Linux: ~/.gemini/antigravity/skills/

Kích hoạt trong Antigravity
Mở (hoặc reload) cửa sổ làm việc trên Antigravity.

Khi yêu cầu tạo giao diện, agent sẽ tự động đọc và kết hợp các quy tắc từ những skill đã cài đặt.

Bạn cũng có thể gọi đích danh skill trong câu lệnh:

"Dùng @core-ui-architecture và @style-classic-medieval để tạo màn hình Nâng cấp Nhà lính."