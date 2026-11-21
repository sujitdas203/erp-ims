# ERP-IMS Portal Navigation Hierarchy & Menu Flow

This document provides a comprehensive blueprint and hierarchical flow of the left navigation menus and operational modules across the **Admin / Management Panel**, **Teacher / Faculty Operations**, and **Student & Guardian Portal** in the current existing stage of ERP-IMS.

---

## 1. System Overview & Architecture

ERP-IMS is structured into distinct portal interfaces and role-based access domains:

```
                                  ┌───────────────────────────┐
                                  │      ERP-IMS System       │
                                  └─────────────┬─────────────┘
                                                │
         ┌──────────────────────────────────────┼──────────────────────────────────────┐
         │                                      │                                      │
         ▼                                      ▼                                      ▼
┌──────────────────┐                  ┌──────────────────┐                  ┌───────────────────────┐
│   Admin / Main   │                  │  Teacher / Staff │                  │  Student & Guardian   │
│ Management Panel │                  │  Workflows Flow  │                  │     Portal (Area)     │
│ (`/`)            │                  │ (`/`)            │                  │ (`/StudentPortal`)    │
└──────────────────┘                  └──────────────────┘                  └───────────────────────┘
```

---

## 2. Admin / Management Left Menu Hierarchy

Defined in [`IMS.Web/Views/Shared/_PartialSidebar.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/Shared/_PartialSidebar.cshtml).

```
IMS Admin Navigation
├── 📊 Dashboard (/Home/Index)
│
├── 🎓 ACADEMIC MANAGEMENT
│   ├── 📇 Admissions (/AdmissionApplication)
│   ├── 👤+ Enrollments (/Enrollment)
│   ├── 🎓 Students (/Students)
│   ├── 🕒 Student Leaves (/StudentLeave)
│   ├── 🛡️ Transfer Certificates (TC) (/TransferCertificate)
│   ├── 👥 Batches (/Batch)
│   ├── 🔖 Course Subjects (/CourseSubject)
│   ├── 📖 Syllabus (/Syllabus)
│   ├── 📅 Timetable (/Timetable)
│   ├── 📝 Home Tasks (/HomeTasks)
│   ├── 🧪 Mock Tests (/MockTests)
│   ├── 🧑‍🏫 Teachers (/Teachers)
│   ├── 👔 Staff (/Staff)
│   ├── 📋 Teacher Attendance (/TeacherAttendance)
│   ├── 📅❌ Teacher Leaves (/TeacherLeave)
│   └── 📢 Announcements (/Announcements)
│
├── 🏛️ INSTITUTE OPERATIONS
│   ├── 🗓️ Attendance (/Attendance)
│   ├── 💳 Fees & Payments (/Fees)
│   ├── 📑 Examinations (/Exam)
│   └── 🧾 Expenses (/Expense)
│
├── ⚙️ ADMINISTRATION
│   ├── 🗄️ Master Management (/Master) [Collapsible Menu]
│   │   ├── 🔲 Master Overview (/Master)
│   │   │
│   │   ├── [GROUP 1: Academic Structure]
│   │   │   ├── 📅 Academic Year Master (/Master/AcademicYear)
│   │   │   ├── 🏛️ Branch Master (/Master/Branch)
│   │   │   ├── 🗂️ Department Master (/Master/Department)
│   │   │   ├── 🏫 Classroom Master (/Master/Classroom)
│   │   │   ├── 🎓 Program Master (/Master/Program)
│   │   │   ├── 📖 Course Master (/Master/Course)
│   │   │   ├── 🔖 Subject Master (/Master/Subject)
│   │   │   └── 👥 Batch Master (/Master/Batch)
│   │   │
│   │   ├── [GROUP 2: Staff & Administration]
│   │   │   ├── 🪪 Designation Master (/Master/Designation)
│   │   │   └── 📄 Document Type Master (/Master/DocumentType)
│   │   │
│   │   ├── [GROUP 3: Fee & Finance]
│   │   │   ├── 💵 Fee Category Master (/Master/FeeCategory)
│   │   │   ├── 🏷️ Discount Master (/Master/Discount)
│   │   │   ├── 💳 Payment Method Master (/Master/PaymentMethod)
│   │   │   └── 🧾 Expense Category Master (/Master/ExpenseCategory)
│   │   │
│   │   ├── [GROUP 4: Examinations & Grading]
│   │   │   ├── 📝 Exam Type Master (/Master/ExamType)
│   │   │   └── 🏆 Grade Scale Master (/Master/GradeScale)
│   │   │
│   │   └── [GROUP 5: Operations & System]
│   │       ├── 🚚 Vendor Master (/Master/Vendor)
│   │       └── 🔔 Notification Template Master (/Master/NotificationTemplate)
│   │
│   ├── 🖥️ Database Diagnostics (/DbTest)
│   ├── 🎛️ Notification Hub (/NotificationConfig)
│   └── 🛡️ Users & Roles (Status: Soon)
│
└── 📈 INSIGHTS
    └── 📊 Reports (Status: Soon)
```

---

## 3. Teacher / Faculty Operations Flow

Teachers access academic management, teaching duties, and personal self-service through dedicated controllers:

```
Teacher Operations Flow
├── 🧑‍🏫 Classroom & Academic Delivery
│   ├── 👥 Batch & Student Rosters (/Batch, /Students)
│   ├── 📚 Course Subjects & Curriculum Mapping (/CourseSubject)
│   ├── 📖 Syllabus Progress Tracker (/Syllabus)
│   ├── 🗓️ Weekly Routine & Schedule (/Timetable)
│   ├── 📝 Daily Student Attendance (/Attendance)
│   ├── ✏️ Home Tasks (Assign, Review, Grade) (/HomeTasks)
│   └── 🧪 Mock Tests (Create, Evaluate) (/MockTests)
│
├── 📑 Grading & Assessments
│   └── ✍️ Exam Marks Entry & Grading (/Exam)
│
├── 📢 Communications
│   └── 📣 Campus Announcements (/Announcements)
│
└── 👤 Teacher Self-Service
    ├── 📋 Daily Attendance Check-In / History (/TeacherAttendance)
    └── 🏖️ My Leaves (Apply, Status History) (/TeacherLeave/MyLeave)
```

---

## 4. Student & Guardian Portal Left Menu Hierarchy

Defined in [`IMS.Web/Areas/StudentPortal/Views/Shared/_StudentPortalLayout.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Areas/StudentPortal/Views/Shared/_StudentPortalLayout.cshtml).

### 4.1 Desktop Left Navigation

```
Student / Guardian Portal (`/StudentPortal`)
├── 🏠 MAIN
│   ├── 📊 Dashboard (`/StudentPortal/Dashboard/Index`)
│   ├── 🎓 Student Info (`/StudentPortal/Dashboard/Profile`)
│   ├── 🪪 Student I-Card (`/StudentPortal/Dashboard/IdCard`)
│   ├── 🪪 Guardian I-Card (`/StudentPortal/Dashboard/GuardianIdCard`)
│   └── 📢 Notice Board (`/StudentPortal/Dashboard/Notices`)
│
├── 📚 ACADEMICS
│   ├── 🗓️ Attendance (`/StudentPortal/Academics/Attendance`)
│   ├── ⏰ Class Schedule (`/StudentPortal/Academics/Timetable`)
│   ├── 📖 Syllabus (`/StudentPortal/Academics/Syllabus`)
│   └── 🏫 Class Details (`/StudentPortal/Academics/ClassDetails`)
│
├── ✏️ LEARNING & TESTS
│   ├── 📋 Home Task (`/StudentPortal/Tasks/HomeTasks`)
│   └── 💻 Mock Test (`/StudentPortal/Tasks/MockTests`)
│
├── 📑 EXAMINATIONS
│   ├── 🎟️ Admit Card (`/StudentPortal/Exams/AdmitCard`)
│   └── 📊 Mark Sheet (`/StudentPortal/Exams/MarkSheet`)
│
└── 💰 FINANCE & SERVICES
    ├── 🧾 Fees & Receipts (`/StudentPortal/Finance/Transactions`)
    ├── 🚶 Leave Apply (`/StudentPortal/Requests/LeaveApply`)
    ├── 🚌 Bus Portal (`/StudentPortal/Requests/Transport`)
    └── 📜 TC Apply (`/StudentPortal/Requests/TCApply`)
```

### 4.2 Top Bar Features
* **Multi-Ward Switcher (for Guardians):** Allows guardians with multiple enrolled children to switch between wards dynamically via dropdown (`switchActiveStudent`).
* **Profile / Change Password Modal:** Quick password reset modal with CSRF token verification.
* **Sign Out:** Secure logout form post.

### 4.3 Mobile Bottom Navigation (<768px)
* 🏠 **Home** (`/StudentPortal/Dashboard/Index`)
* 🗓️ **Attendance** (`/StudentPortal/Academics/Attendance`)
* 📋 **Tasks** (`/StudentPortal/Tasks/HomeTasks`)
* 🧾 **Fees** (`/StudentPortal/Finance/Transactions`)
* 👤 **Profile** (`/StudentPortal/Dashboard/Profile`)

---

## 5. Route & Controller Mapping Table

| Panel | Section / Group | Menu Title | Route / URL | Controller / Action |
|---|---|---|---|---|
| **Admin** | Dashboard | Dashboard | `/Home/Index` | `HomeController.Index` |
| **Admin** | Academic Management | Admissions | `/AdmissionApplication` | `AdmissionApplicationController.Index` |
| **Admin** | Academic Management | Enrollments | `/Enrollment` | `EnrollmentController.Index` |
| **Admin** | Academic Management | Students | `/Students` | `StudentsController.Index` |
| **Admin** | Academic Management | Student Leaves | `/StudentLeave` | `StudentLeaveController.Index` |
| **Admin** | Academic Management | Transfer Certificates | `/TransferCertificate` | `TransferCertificateController.Index` |
| **Admin** | Academic Management | Batches | `/Batch` | `BatchController.Index` |
| **Admin** | Academic Management | Course Subjects | `/CourseSubject` | `CourseSubjectController.Index` |
| **Admin** | Academic Management | Syllabus | `/Syllabus` | `SyllabusController.Index` |
| **Admin** | Academic Management | Timetable | `/Timetable` | `TimetableController.Index` |
| **Admin** | Academic Management | Home Tasks | `/HomeTasks` | `HomeTasksController.Index` |
| **Admin** | Academic Management | Mock Tests | `/MockTests` | `MockTestsController.Index` |
| **Admin** | Academic Management | Teachers | `/Teachers` | `TeachersController.Index` |
| **Admin** | Academic Management | Staff | `/Staff` | `StaffController.Index` |
| **Admin** | Academic Management | Teacher Attendance | `/TeacherAttendance` | `TeacherAttendanceController.Index` |
| **Admin** | Academic Management | Teacher Leaves | `/TeacherLeave` | `TeacherLeaveController.Index` |
| **Admin** | Academic Management | Announcements | `/Announcements` | `AnnouncementsController.Index` |
| **Admin** | Operations | Attendance | `/Attendance` | `AttendanceController.Index` |
| **Admin** | Operations | Fees & Payments | `/Fees` | `FeesController.Index` |
| **Admin** | Operations | Examinations | `/Exam` | `ExamController.Index` |
| **Admin** | Operations | Expenses | `/Expense` | `ExpenseController.Index` |
| **Admin** | Administration | Master Overview | `/Master` | `MasterController.Index` |
| **Admin** | Administration | Master Entities (18) | `/Master/{entityType}` | `MasterController.EntityList` |
| **Admin** | Administration | Database Diagnostics | `/DbTest` | `DbTestController.Index` |
| **Admin** | Administration | Notification Hub | `/NotificationConfig` | `NotificationConfigController.Index` |
| **Teacher** | Self-Service | My Leaves | `/TeacherLeave/MyLeave` | `TeacherLeaveController.MyLeave` |
| **Teacher** | Academic Delivery | Attendance & Tasks | `/Attendance`, `/HomeTasks` | Multiple Controllers |
| **Student** | Main | Dashboard | `/StudentPortal/Dashboard` | `DashboardController.Index` |
| **Student** | Main | Student Info | `/StudentPortal/Dashboard/Profile` | `DashboardController.Profile` |
| **Student** | Main | Student I-Card | `/StudentPortal/Dashboard/IdCard` | `DashboardController.IdCard` |
| **Student** | Main | Guardian I-Card | `/StudentPortal/Dashboard/GuardianIdCard` | `DashboardController.GuardianIdCard` |
| **Student** | Main | Notice Board | `/StudentPortal/Dashboard/Notices` | `DashboardController.Notices` |
| **Student** | Academics | Attendance | `/StudentPortal/Academics/Attendance` | `AcademicsController.Attendance` |
| **Student** | Academics | Class Schedule | `/StudentPortal/Academics/Timetable` | `AcademicsController.Timetable` |
| **Student** | Academics | Syllabus | `/StudentPortal/Academics/Syllabus` | `AcademicsController.Syllabus` |
| **Student** | Academics | Class Details | `/StudentPortal/Academics/ClassDetails` | `AcademicsController.ClassDetails` |
| **Student** | Learning & Tests | Home Task | `/StudentPortal/Tasks/HomeTasks` | `TasksController.HomeTasks` |
| **Student** | Learning & Tests | Mock Test | `/StudentPortal/Tasks/MockTests` | `TasksController.MockTests` |
| **Student** | Examinations | Admit Card | `/StudentPortal/Exams/AdmitCard` | `ExamsController.AdmitCard` |
| **Student** | Examinations | Mark Sheet | `/StudentPortal/Exams/MarkSheet` | `ExamsController.MarkSheet` |
| **Student** | Finance & Services | Fees & Receipts | `/StudentPortal/Finance/Transactions` | `FinanceController.Transactions` |
| **Student** | Finance & Services | Leave Apply | `/StudentPortal/Requests/LeaveApply` | `RequestsController.LeaveApply` |
| **Student** | Finance & Services | Bus Portal | `/StudentPortal/Requests/Transport` | `RequestsController.Transport` |
| **Student** | Finance & Services | TC Apply | `/StudentPortal/Requests/TCApply` | `RequestsController.TCApply` |
