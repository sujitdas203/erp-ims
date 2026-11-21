# Walkthrough: Admin Panel Theme System (Light, Semi-Dark, and Monokai Pro Dark)

## Summary of Changes

### 1. Monokai Pro Dark Theme (`data-theme="dark"`)
- **Palette**:
  - **Canvas / App Background**: `#22252a` (Monokai Pro Dark Charcoal)
  - **Surfaces, Cards, Modals**: `#2d3139` (Monokai Pro Elevated Surface)
  - **Sidebar Background**: `#1a1c23` (Monokai Deep Espresso Sidebar)
  - **Sidebar Active Item & Accent**: `#ffd866` (Monokai Pro Sun Yellow)
  - **Primary Actions & Links**: `#78dce8` (Monokai Pro Cyan)
  - **Main Typography**: `#fcfcfa` (Warm-white high-contrast readable font)
  - **Muted Text**: `#939293`
  - **Borders**: `#3a3f4b`
  - **Status Accents**:
    - **Success / Active**: `#a9dc76` (Monokai Mint Green)
    - **Warning**: `#fc9867` (Monokai Orange)
    - **Danger**: `#ff6188` (Monokai Rose Red)
    - **Cyan / Info**: `#78dce8`

---

### 2. Master Module Full Theme Adaptability (`/Master` & `/Master/{EntityType}`)
- **Location**: [wwwroot/css/master.css](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/wwwroot/css/master.css), [Views/Master/Index.cshtml](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/Master/Index.cshtml), [Views/Master/Menu.cshtml](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/Master/Menu.cshtml)
- **Resolved Issues**:
  - Eliminated hardcoded light backgrounds (`#ffffff`, `#f6f8fb`) and light borders (`#e6eaf0`, `#e2e8f0`).
  - Mapped all master page containers, cards, tables, grid headers, rows, action buttons, filter inputs, and modal dialogs to dynamically inherit `var(--IMS-bg)`, `var(--IMS-surface)`, `var(--IMS-border)`, and `var(--IMS-text)`.
  - Master module now seamlessly transitions between **Light**, **Semi-Dark**, and **Monokai Pro Dark** modes.

---

### 3. Light & Semi-Dark Theme Modes
- **Semi-Dark Theme (`data-theme="semi-dark"`) [Default Classic]**:
  - Executive ERP look with charcoal-obsidian sidebar (`#111827`), amber/indigo active highlights, clean light slate canvas (`#f4f6f9`), white cards, and subtle borders.
- **Light Theme (`data-theme="light"`)**:
  - Bright executive style with soft cool slate-white canvas (`#f8fafc`), crisp pure white surfaces (`#ffffff`), modern royal blue accents (`#3b82f6`), and light sidebar.

---

### 4. Interactive Theme Switcher
- **Header Switcher**: Direct one-click theme switcher dropdown in [`_PartialHeader.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/Shared/_PartialHeader.cshtml) with active checkmark indicators.
- **Zero-Flicker Persistence**: Instant restore script in `<head>` of [`_Layout.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/Shared/_Layout.cshtml) via `localStorage` setting both `data-theme` and `data-bs-theme`.
