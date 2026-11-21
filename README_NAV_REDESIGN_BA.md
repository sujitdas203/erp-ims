# ERP-IMS Navigation Architecture — Business Analysis & Redesign Report

**Author:** Business Analyst — 15 Years IMS Domain Experience  
**Document Status:** Analysis & Redesign Recommendation  
**Scope:** Admin Panel · Teacher Panel · Student & Guardian Portal

---

## EXECUTIVE SUMMARY — CRITICAL OBSERVATIONS

After auditing the current navigation structure, three systemic problems are identified:

| Problem | Impact | Severity |
|---|---|---|
| **Cognitive Overload in Admin Menu** | "Academic Management" section has 16 items — a mix of operational (daily-use) and configuration (weekly/monthly-use) items. Operators lose time scrolling to find the right item. | 🔴 Critical |
| **Master Module Boundary Confusion** | `Batch`, `CourseSubject`, `Syllabus`, `Timetable` are day-to-day transactional modules placed inside Master. They should NOT be in Master — Master is for lookup/config tables only. | 🔴 Critical |
| **Teacher Panel Has No Dedicated Area/Layout** | Teachers currently piggyback on the Admin sidebar. They see everything the admin sees. This is a role-isolation gap and a UX problem. | 🟠 High |
| **Student Portal: Section Mixing** | "Leave Apply", "TC Apply" are student requests — they are bundled under Finance & Services which is inaccurate semantically. | 🟡 Medium |
| **No progressive disclosure in Admin** | A flat 16-item list is presented all at once. IMS users (typically non-technical institute clerks) get overwhelmed. | 🟠 High |

---

---

# PART A — ADMIN PANEL REDESIGN

## A1. The Golden Rule: What Goes in Master vs Operational Modules

This is the single most important architectural decision in any IMS.

### ✅ Master Module = Configuration & Lookup Data
> **Rule:** "Does this data change more than once every academic session? Is it created once and then only referenced by other modules?"

If **YES** → it belongs in **Master Management**.

| Should STAY in Master | Reason |
|---|---|
| Academic Year | Session-level config. Created once per year. |
| Branch | Campus setup. Rarely changes. |
| Department | Org structure. Rarely changes. |
| Classroom | Room inventory. Static. |
| Program | Degree/Stream config. Stable. |
| Course | Curriculum definition. Stable. |
| Subject | Subject catalogue. Stable. |
| Designation | HR lookup. Stable. |
| Document Type | Upload config. Stable. |
| Fee Category | Billing type definitions. Stable. |
| Discount | Policy definitions. Stable. |
| Payment Method | Cash / UPI / Card config. Stable. |
| Expense Category | Finance lookup. Stable. |
| Exam Type | Unit test / Annual / Internal config. Stable. |
| Grade Scale | A/B/C/Distinction config. Stable. |
| Vendor | Supplier directory. Changes occasionally. |
| Notification Template | Message templates. Stable. |

### ❌ Should Be REMOVED from Master / NOT belong in Master

| Currently in / Near Master | Correct Home | Reason |
|---|---|---|
| **Batch** | `Academic → Batches` (already there ✓) | Batch is a **transactional entity** — it has enrollment counts, dates, status. It is NOT a lookup/config entry. |
| **Course Subject (Mapping)** | `Academic → Curriculum` sub-section | This is the **curriculum plan** (which subject is taught in which course/batch). It's operational, not configuration. |
| **Syllabus** | `Academic → Curriculum` sub-section | Syllabus is **term-wise content planning** — it changes per session, per teacher. It must NOT be in Master. |
| **Timetable** | `Academic → Scheduling` sub-section | Timetable is a **live operational schedule** — changes weekly. Absolutely not a Master item. |

---

## A2. Proposed Admin Navigation Redesign

### Design Principles Applied
1. **Max 7 items per section** (Miller's Law — cognitive load management)
2. **Role-frequency ordering** — most-used items first
3. **Clear semantic sections** — a clerk's daily workflow should map to exactly one section
4. **Master = config only** — no transactional data

```
PROPOSED ADMIN NAVIGATION
═══════════════════════════════════════════════════════

📊 Dashboard
   └─ /Home/Index   [Always visible — zero clicks to reach]

───────────────────────────────────────────────────────
👥  STUDENTS & ADMISSIONS
    Purpose: Student lifecycle — from inquiry to graduation
───────────────────────────────────────────────────────
    ├── 📇 Admission Applications    /AdmissionApplication
    │         ↳ Inquiry → Application → Approval flow
    ├── 👤+ Enrollments              /Enrollment
    │         ↳ Approved applicants formally enrolled
    ├── 🎓 Students                  /Students
    │         ↳ Full student register + profiles
    ├── 🕒 Student Leaves            /StudentLeave
    │         ↳ Leave applications + approvals
    └── 🛡️ Transfer Certificates    /TransferCertificate
              ↳ TC generation + tracking

───────────────────────────────────────────────────────
📚  ACADEMIC DELIVERY
    Purpose: What is taught, when, by whom — daily/weekly ops
───────────────────────────────────────────────────────
    ├── 👥 Batches                   /Batch
    │         ↳ Class groups + enrollment counts
    ├── 🗺️ Curriculum
    │   ├── 📚 Course Subjects       /CourseSubject
    │   │         ↳ Which subjects map to which course
    │   └── 📖 Syllabus              /Syllabus
    │             ↳ Topic/chapter planner per subject+batch
    ├── 📅 Timetable                 /Timetable
    │         ↳ Weekly period schedule
    ├── 📝 Home Tasks                /HomeTasks
    │         ↳ Assign + monitor homework
    └── 🧪 Mock Tests                /MockTests
              ↳ Practice test management

───────────────────────────────────────────────────────
🗓️  ATTENDANCE CENTRE
    Purpose: Daily mark-taking — student + teacher
───────────────────────────────────────────────────────
    ├── 🗓️ Student Attendance        /Attendance
    └── 📋 Teacher Attendance        /TeacherAttendance

───────────────────────────────────────────────────────
👔  HR & FACULTY
    Purpose: People management — teachers and non-teaching staff
───────────────────────────────────────────────────────
    ├── 🧑‍🏫 Teachers                /Teachers
    │         ↳ Faculty register + assignments
    ├── 👔 Non-Teaching Staff        /Staff
    │         ↳ Admin, support staff register
    └── 📅❌ Teacher Leaves          /TeacherLeave
              ↳ Leave applications + calendar

───────────────────────────────────────────────────────
📑  EXAMINATIONS
    Purpose: Exam planning → marks → results
───────────────────────────────────────────────────────
    └── ✍️ Examinations              /Exam
              ↳ Schedule, hall allocations, marks entry, results

───────────────────────────────────────────────────────
💰  FINANCE
    Purpose: All money matters — fee collection + expenditure
───────────────────────────────────────────────────────
    ├── 💳 Fees & Payments           /Fees
    │         ↳ Fee plans, invoices, receipts, dues
    └── 🧾 Expenses                  /Expense
              ↳ Institute expense recording

───────────────────────────────────────────────────────
📢  COMMUNICATION
    Purpose: Information broadcast + alerts
───────────────────────────────────────────────────────
    ├── 📣 Announcements             /Announcements
    └── 🎛️ Notification Hub          /NotificationConfig
              ↳ Configure event-based SMS/Email/WhatsApp alerts

───────────────────────────────────────────────────────
⚙️  SYSTEM SETUP (Master Management)
    Purpose: One-time or rare config — 🔒 Admin/Principal only
───────────────────────────────────────────────────────
    ├── 🗄️ Master Management         /Master  [Collapsible]
    │   │
    │   ├── [Academic Structure]
    │   │   ├── 📅 Academic Year     /Master/AcademicYear
    │   │   ├── 🏛️ Branch            /Master/Branch
    │   │   ├── 🗂️ Department        /Master/Department
    │   │   ├── 🏫 Classroom         /Master/Classroom
    │   │   ├── 🎓 Program           /Master/Program
    │   │   ├── 📖 Course            /Master/Course
    │   │   └── 🔖 Subject           /Master/Subject
    │   │   [NOTE: Batch REMOVED — moved to Academic Delivery]
    │   │
    │   ├── [HR & Staff Setup]
    │   │   ├── 🪪 Designation       /Master/Designation
    │   │   └── 📄 Document Types    /Master/DocumentType
    │   │
    │   ├── [Finance Setup]
    │   │   ├── 💵 Fee Categories    /Master/FeeCategory
    │   │   ├── 🏷️ Discounts         /Master/Discount
    │   │   ├── 💳 Payment Methods   /Master/PaymentMethod
    │   │   └── 🧾 Expense Category  /Master/ExpenseCategory
    │   │
    │   ├── [Exam & Grading Setup]
    │   │   ├── 📝 Exam Types        /Master/ExamType
    │   │   └── 🏆 Grade Scales      /Master/GradeScale
    │   │
    │   └── [Operations & System]
    │       ├── 🚚 Vendors           /Master/Vendor
    │       └── 🔔 Notification Templates  /Master/NotificationTemplate
    │
    ├── 🛡️ Users & Roles             (Soon)
    └── 🖥️ DB Diagnostics            /DbTest

───────────────────────────────────────────────────────
📈  INSIGHTS
    Purpose: Decision-support — for principals & management
───────────────────────────────────────────────────────
    └── 📊 Reports & Analytics       (Soon)
```

---

## A3. Item Count Comparison (Before vs After)

| Section (Current) | Items | Section (Proposed) | Items |
|---|---|---|---|
| Academic Management | **16** ← overloaded | Students & Admissions | 5 |
| Institute Operations | 4 | Academic Delivery | 6 (+ 1 sub-group) |
| Administration | 4 + 18 in master | Attendance Centre | 2 |
| — | — | HR & Faculty | 3 |
| — | — | Examinations | 1 |
| — | — | Finance | 2 |
| — | — | Communication | 2 |
| — | — | System Setup (Master) | 17 entries |
| — | — | Insights | 1 |

**Result:** Admin clerks go from scanning **16-item mega-list** to **5-item focused sections**.

---

---

# PART B — TEACHER PANEL REDESIGN

## B1. Current Problems in Teacher Access

> **Core Issue:** Teachers currently use the same sidebar as Admin. This means a teacher sees "Fees & Payments", "Expenses", "Transfer Certificates", "Master Management" — items entirely irrelevant to them.

### Teacher's Actual Daily Workflow (Frequency Order)
1. Mark student attendance → **daily**
2. View today's timetable → **daily**
3. Assign / update homework → **3–5 times/week**
4. Enter mock test marks → **weekly**
5. View my class roster → **weekly**
6. Update syllabus progress → **weekly**
7. Apply / view my leave → **monthly**
8. Submit exam marks → **per exam cycle**
9. Post announcement → **occasional**

### B2. Proposed Teacher Panel Navigation

> **Recommendation:** Create a dedicated Teacher Area (`/TeacherPortal`) similar to StudentPortal. Until then, use role-based menu filter on the existing sidebar.

```
PROPOSED TEACHER NAVIGATION
═══════════════════════════════════════════════════════

📊 Dashboard
   └─ /Home/Index
        ↳ Summary cards: Today's periods, pending tasks,
          attendance summary, leave balance

───────────────────────────────────────────────────────
🗓️  MY TODAY (Daily Actions — shown prominently)
───────────────────────────────────────────────────────
    ├── 🗓️ Today's Timetable         /Timetable [filtered by teacher]
    └── ✅ Mark Attendance           /Attendance [filtered by assigned class]

───────────────────────────────────────────────────────
📚  MY CLASSES
    Purpose: Everything about the teacher's assigned batches
───────────────────────────────────────────────────────
    ├── 👥 My Batches / Students     /Batch, /Students [filtered]
    ├── 📚 Course Subjects           /CourseSubject [view-only]
    ├── 📖 Syllabus Progress         /Syllabus [edit own assigned]
    └── 📅 Full Timetable View       /Timetable [read-only]

───────────────────────────────────────────────────────
✏️  ASSIGNMENTS & TESTS
───────────────────────────────────────────────────────
    ├── 📝 Home Tasks                /HomeTasks [own assigned]
    └── 🧪 Mock Tests                /MockTests [own created]

───────────────────────────────────────────────────────
📑  EXAMINATIONS
───────────────────────────────────────────────────────
    └── ✍️ Enter Exam Marks          /Exam [own subjects only]

───────────────────────────────────────────────────────
📢  COMMUNICATION
───────────────────────────────────────────────────────
    └── 📣 Post Announcement         /Announcements [own batch/dept]

───────────────────────────────────────────────────────
👤  MY ACCOUNT (Self-Service)
───────────────────────────────────────────────────────
    ├── 📋 My Attendance History     /TeacherAttendance/MyHistory
    ├── 🏖️ Apply Leave               /TeacherLeave/Apply
    └── 🏖️ My Leave History          /TeacherLeave/MyLeave
```

### B3. Key Teacher Panel Recommendations

| Recommendation | Reason |
|---|---|
| **Dedicated `/TeacherPortal` Area** | Stops teachers from accidentally accessing admin-only modules |
| **Filter all lists by teacher's assigned batches** | A teacher with 3 classes should ONLY see their 3 classes in Attendance / Timetable / HomeTasks — not all 50 batches |
| **Today's Schedule widget on Dashboard** | #1 daily need — should be zero-click access |
| **Syllabus progress editable by teacher** | Teacher updates their chapter completion; Admin/Principal views all progress |
| **Announcements: scope to department/batch** | Teacher should only post to their own class, not institute-wide |

---

---

# PART C — STUDENT & GUARDIAN PORTAL REDESIGN

## C1. Current Problems

| Issue | Details |
|---|---|
| **"Finance & Services" is semantically wrong** | Leave Apply, TC Apply, and Bus Portal have nothing to do with Finance. These are **student service requests**, not financial transactions. |
| **"Learning & Tests" is undersized** | Only 2 items under a section header — wastes cognitive space. Merge with Academics. |
| **No concept of "My Requests" / Status Tracking** | Student applies for leave / TC but has no unified place to track all pending requests. |
| **Guardian-specific items not separated** | Guardian I-Card lives under "Main" alongside academic items. Guardian has different information needs from a student. |
| **Exam section has only 2 items** | Admit Card and Mark Sheet are correct, but Result History and Grade Card are missing (future). |

## C2. Proposed Student & Guardian Portal Navigation

```
PROPOSED STUDENT / GUARDIAN PORTAL NAVIGATION
═══════════════════════════════════════════════════════

📊 Dashboard
   └─ /StudentPortal/Dashboard
        ↳ Attendance %, upcoming exams, latest notice,
          fee due alert, next class widget

───────────────────────────────────────────────────────
👤  MY PROFILE
    Purpose: Identity & documentation
───────────────────────────────────────────────────────
    ├── 🎓 Student Information       /Dashboard/Profile
    │         ↳ Personal details, guardian info, documents
    ├── 🪪 Student ID Card           /Dashboard/IdCard
    └── 🪪 Guardian ID Card          /Dashboard/GuardianIdCard
    [NOTE: Separated cleanly from academic & notice items]

───────────────────────────────────────────────────────
📚  MY ACADEMICS
    Purpose: Day-to-day academic life — merged with Learning
───────────────────────────────────────────────────────
    ├── 🗓️ Attendance                /Academics/Attendance
    │         ↳ Month-wise calendar view + % summary
    ├── ⏰ Class Timetable           /Academics/Timetable
    ├── 📖 Syllabus                  /Academics/Syllabus
    │         ↳ Chapter-wise syllabus + completion status
    ├── 🏫 Class / Batch Details     /Academics/ClassDetails
    │         ↳ Batch info, subjects, teacher list
    ├── 📝 Home Tasks                /Tasks/HomeTasks
    │         ↳ Pending & submitted assignments
    └── 💻 Mock Tests                /Tasks/MockTests
              ↳ Past scores + upcoming scheduled tests

───────────────────────────────────────────────────────
📑  EXAMINATIONS & RESULTS
───────────────────────────────────────────────────────
    ├── 🎟️ Admit Card                /Exams/AdmitCard
    ├── 📊 Mark Sheet / Result       /Exams/MarkSheet
    └── 📋 Grade Card / Report Card  (Future)

───────────────────────────────────────────────────────
💰  FEES & PAYMENTS
    Purpose: ONLY financial items here
───────────────────────────────────────────────────────
    └── 🧾 Fee Statement & Receipts  /Finance/Transactions
              ↳ Due → Pay → Receipt download

───────────────────────────────────────────────────────
📮  MY REQUESTS   ← THIS IS THE KEY NEW SECTION
    Purpose: All student self-service requests + status tracking
───────────────────────────────────────────────────────
    ├── 🚶 Apply Leave               /Requests/LeaveApply
    │         ↳ Apply + track approval status
    ├── 📜 Transfer Certificate      /Requests/TCApply
    │         ↳ Apply + download issued TC
    └── 🚌 Bus / Transport           /Requests/Transport
              ↳ Bus route info + opt-in request

───────────────────────────────────────────────────────
📢  NOTICE BOARD
───────────────────────────────────────────────────────
    └── 📣 Notices & Announcements   /Dashboard/Notices
    [NOTE: Moved out of "Main" — it's not a profile item]
```

### C3. Mobile Bottom Bar — Revised Priority

```
Current:  Home | Attendance | Tasks | Fees | Profile
Proposed: Home | Attendance | Exams | My Requests | Profile

Reason: Students check exam results and request status far
        more than they check Fees on mobile. Fee checks happen
        1–2x/month; Exam/Requests checked weekly.
```

---

---

# PART D — CONSOLIDATED DECISIONS TABLE

## D1. Master Module — Keep / Move Decision

| Entity | Current Location | Decision | Move To |
|---|---|---|---|
| Academic Year | Master ✓ | ✅ KEEP | Master > Academic Structure |
| Branch | Master ✓ | ✅ KEEP | Master > Academic Structure |
| Department | Master ✓ | ✅ KEEP | Master > Academic Structure |
| Classroom | Master ✓ | ✅ KEEP | Master > Academic Structure |
| Program | Master ✓ | ✅ KEEP | Master > Academic Structure |
| Course | Master ✓ | ✅ KEEP | Master > Academic Structure |
| Subject | Master ✓ | ✅ KEEP | Master > Academic Structure |
| **Batch** | Master ✓ | ⛔ MOVE | Academic Delivery → Batches |
| Designation | Master ✓ | ✅ KEEP | Master > HR Setup |
| Document Type | Master ✓ | ✅ KEEP | Master > HR Setup |
| Fee Category | Master ✓ | ✅ KEEP | Master > Finance Setup |
| Discount | Master ✓ | ✅ KEEP | Master > Finance Setup |
| Payment Method | Master ✓ | ✅ KEEP | Master > Finance Setup |
| Expense Category | Master ✓ | ✅ KEEP | Master > Finance Setup |
| Exam Type | Master ✓ | ✅ KEEP | Master > Exam Setup |
| Grade Scale | Master ✓ | ✅ KEEP | Master > Exam Setup |
| Vendor | Master ✓ | ✅ KEEP | Master > Operations |
| Notification Template | Master ✓ | ✅ KEEP | Master > Operations |
| **Course Subjects** | Academic Mgmt | ✅ CORRECT | Academic Delivery → Curriculum |
| **Syllabus** | Academic Mgmt | ✅ CORRECT | Academic Delivery → Curriculum |
| **Timetable** | Academic Mgmt | ✅ CORRECT | Academic Delivery → Scheduling |

> **Verdict:** `Batch` is currently in `MasterConfigRegistry.cs` as a Master entity — this is a design debt. It should remain in the Master registry only for lookup/dropdown purposes, but its **management UI** (create, edit, view list with enrollment counts) must live in the `Academic Delivery` section of the sidebar, NOT in Master Management.

---

## D2. Misplaced Item Summary

| Item | Current Section | Problem | Correct Section |
|---|---|---|---|
| Student Leaves | Academic Management | Leaves = student services, not academic curriculum | Students & Admissions |
| Transfer Certificates | Academic Management | TC = lifecycle event, not academic delivery | Students & Admissions |
| Teachers | Academic Management | HR entry buried inside curriculum items | HR & Faculty |
| Staff | Academic Management | Same as above | HR & Faculty |
| Teacher Attendance | Academic Management | Attendance ops ≠ academic delivery | Attendance Centre |
| Teacher Leaves | Academic Management | HR/leave ops ≠ academic delivery | HR & Faculty |
| Announcements | Academic Management | Communication ≠ academic management | Communication |
| Leave Apply (Student) | Finance & Services | Not a finance item | My Requests |
| TC Apply (Student) | Finance & Services | Not a finance item | My Requests |
| Bus Portal (Student) | Finance & Services | Transport request ≠ finance | My Requests |
| Notice Board (Student) | Main (with Profile) | Notice board is NOT a profile item | Standalone section |

---

## D3. Priority Implementation Roadmap

```
PHASE 1 — Quick Wins (1–2 days, sidebar-only changes)
─────────────────────────────────────────────────────
□ Split Admin "Academic Management" into 5 focused sections
□ Move Teacher/Staff/Leaves out to HR & Faculty section
□ Move Attendance out to standalone "Attendance Centre"
□ Move Announcements to "Communication"
□ Rename "Institute Operations" → "Finance" (remove Attendance from here)

PHASE 2 — Student Portal UX Fix (1 day)
─────────────────────────────────────────
□ Create "My Requests" section from Leave/TC/Transport
□ Separate "Notice Board" as standalone section
□ Merge "Learning & Tests" INTO "My Academics"
□ Update mobile bottom bar priority order

PHASE 3 — Structural (3–5 days)
─────────────────────────────────
□ Create dedicated /TeacherPortal Area with filtered data views
□ Remove Batch management from MasterController rendering
  (keep in MasterConfigRegistry for dropdown lookups only)
□ Add "Curriculum" sub-section under Academic Delivery

PHASE 4 — Future Features
──────────────────────────
□ Admin: Reports & Analytics
□ Admin: Users & Roles management
□ Student: Result History / Grade Card
□ Teacher: Lesson plan management under Syllabus
□ Teacher: Parent communication log
```

---

## D4. Section Access Matrix (Role-Based)

| Section / Module | Admin | Principal | Teacher | Student | Guardian |
|---|:---:|:---:|:---:|:---:|:---:|
| Dashboard | ✅ | ✅ | ✅ | ✅ | ✅ |
| Students & Admissions | ✅ | ✅ | 👁️ readonly | ❌ | ❌ |
| Academic Delivery | ✅ | ✅ | ✅ own | ❌ | ❌ |
| Attendance Centre | ✅ | ✅ | ✅ own class | 👁️ own | 👁️ ward |
| HR & Faculty | ✅ | ✅ | 👁️ own profile | ❌ | ❌ |
| Examinations | ✅ | ✅ | ✅ own subj | 👁️ own | 👁️ ward |
| Finance | ✅ | ✅ | ❌ | 👁️ own | 👁️ ward |
| Communication | ✅ | ✅ | ✅ own dept | 👁️ | 👁️ |
| System Setup (Master) | ✅ | 👁️ | ❌ | ❌ | ❌ |
| Reports & Insights | ✅ | ✅ | ❌ | ❌ | ❌ |

✅ = Full Access · 👁️ = View/Read Only · ❌ = No Access

---

*Document generated from: `_PartialSidebar.cshtml`, `_StudentPortalLayout.cshtml`, `MasterConfigRegistry.cs`*  
*Last reviewed against: ERP-IMS build — September 2026*
