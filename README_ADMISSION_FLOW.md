# IMS ERP — Complete Admission Module Workflow Guide

> **Document Version:** 1.0  
> **Target Schema:** `database/script_03_10.sql`  
> **Applicable Modules:** Admissions, Academic Registry, Finance/Fee Management, Notifications & Student Portal.

---

## 1. Executive Summary & Architecture Overview

The **IMS ERP Admission Module** provides an end-to-end, multi-channel workflow connecting **public applicants**, **school administrators/reviewers**, **finance billing**, and the **student self-service portal**.

The entire flow operates on a structured **3-Step Lifecycle**:

```mermaid
flowchart TD
    subgraph Step1["Step 1: Application Submission & Live Tracking"]
        S1A[Student/Parent fills Public Form<br/><b>/Home/Admission</b>] --> S1C[Generate Reference No: <b>APP-YY-XXXX</b>]
        S1B[Admin fills Internal Form<br/><b>/AdmissionApplication/Create</b>] --> S1C
        S1C --> S1D[(Insert <b>AdmissionApplications_AA</b><br/>Status = 'Submitted')]
        S1D --> S1E[<b>Email 1:</b> Submission Receipt &<br/>Live Status Tracking Link]
    end

    subgraph Step2["Step 2: Verification, Fee Offer & Approval Window"]
        S2A[Admin reviews Application & Documents<br/><b>/AdmissionApplication/Details/{id}</b>] --> S2B[Select Fee Structure &<br/>Contact Date Window]
        S2B --> S2C[(Update <b>AdmissionApplications_AA</b><br/>Status = 'Approved')]
        S2C --> S2D[<b>Email 2:</b> Approval Notice with<br/>Itemized Fee Table & Contact Window]
        S2E[Public Tracking shows Fee Schedule &<br/>Document Checklist]
    end

    subgraph Step3["Step 3: School Visit, Payment, Enrolment & Portal Access"]
        S3A[Student/Parent visits Campus &<br/>Completes Fee Payment] --> S3B[Admin clicks 'Enroll as Student Now'<br/>in Admin Details View]
        S3B --> S3C[Assign Class/Section/Batch +<br/>Record Payment Method & Receipt]
        S3C --> S3D[(1. Create <b>Students_S</b> Profile)]
        S3C --> S3E[(2. Create <b>Enrollments_E</b> Record)]
        S3C --> S3F[(3. Generate <b>FeeInvoices_FI</b> & Items)]
        S3C --> S3G[(4. Register <b>Payments_PAY</b> Receipt)]
        S3C --> S3H[(5. Update Application Status = 'Enrolled')]
        S3D & S3E & S3F & S3G & S3H --> S3I[<b>Email 3:</b> Enrolment Confirmation,<br/>Payment Receipt & Portal Login Credentials]
        S3I --> S3J[Student logs in at <b>/Student/Login</b><br/>User: Email/Code | Pass: <b>Welcome@123</b>]
    end

    Step1 --> Step2 --> Step3
```

---

## 2. Detailed Step-by-Step Breakdown

### Step 1: Admission Form Fillup & Reference Tracking

#### 1.1 Submission Channels
1. **Public Portal (`/Home/Admission`):**
   - Designed for prospective students and parents.
   - Clean, modern multi-section form capturing Personal Profile, Parent Details, Address, Previous Academic Marks, Program Preferences, and Document Uploads (Photo, Birth Certificate, Marksheets, TC, ID).
2. **Admin Portal (`/AdmissionApplication/Create`):**
   - For front-desk operators and admission counselors entering walk-in applications.

#### 1.2 Automated Actions on Submission
- **Unique Reference Number:** Generates `APP-YY-XXXX` (e.g. `APP-26-4819`).
- **Database Entry:** Saved into table `dbo.AdmissionApplications_AA` with initial status `Submitted`.
- **Step 1 Email Notification:** An instant HTML email is dispatched to the applicant's email containing:
  - Application Reference Number
  - Applied Grade/Course and Campus Branch
  - Direct live tracking link: `/Home/TrackAdmission?appNumber=APP-YY-XXXX`

#### 1.3 Public Application Tracking (`/Home/TrackAdmission`)
- Applicants can enter their Application Reference Number anytime to view their real-time 3-stage progress bar and review status.

---

### Step 2: Admin Verification, Fee Offer & Contact Schedule

#### 2.1 Admin Review Desk (`/AdmissionApplication/Details/{id}`)
- Admin inspects the applicant's complete profile, uploaded documents with the embedded universal document previewer (Images/PDFs).
- In the **Review & Decision Card**, Admin chooses:
  - **Decision Status:** `Approved`, `UnderReview`, `Waitlisted`, or `Rejected`.
  - **Assigned Fee Structure:** Dropdown dynamically loaded from `dbo.FeeStructures_FS`.
  - **Dynamic Fee Breakdown:** Selecting a fee plan triggers an AJAX call to `/AdmissionApplication/GetFeeStructureItems` which renders an itemized summary (Tuition, Admission, Exam, Lab, etc.) and total payable fee amount.
  - **Contact Window:** Sets `ContactStartDate` to `ContactEndDate` (default +14 days) representing the official window for the applicant to visit the campus.
  - **Contact & Payment Instructions:** Custom counter notes (e.g., "Please bring original marksheets & TC for physical verification").

#### 2.2 Step 2 Email Notification Dispatch
When approved, the system sends the **Step 2 Approval Email** containing:
- Official Congratulatory & Approval Notice.
- **Itemized Fee Schedule Breakdown Table** (component name, mandatory/optional flag, component amounts, and total payable amount).
- **Official Contact & Payment Window Dates** (e.g., `10-Oct-2026 to 24-Oct-2026`).
- **Required Document Checklist** (Birth certificate, TC/Migration, Marksheets, Photographs, ID Proof).

---

### Step 3: Fee Payment, Enrolment & Student Portal Access

#### 3.1 Enrolment & Payment Collection Modal
Once the student contacts the school and completes payment at the accounts desk, Admin clicks **"Enroll as Student Now"** on [`Details.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/AdmissionApplication/Details.cshtml).

The modal provides a streamlined interface:
1. **Academic Allocation:**
   - Admission Number (defaults to `APP-YY-XXXX` or customizable)
   - Roll Number
   - Class / Grade, Section, and Batch
2. **Fee Structure & Payment Collection:**
   - Fee Plan breakdown table and total calculation
   - **"Record Initial Fee Payment Now"** toggle
   - Payment Method: `Cash`, `Bank Transfer`, `UPI / QR Code`, `Online Gateway`, `Cheque / DD`, `POS / Card`
   - Paid Amount (pre-filled with total fee amount)
   - Receipt Number (e.g., `REC-26-8291`) and Transaction Reference

#### 3.2 Automated Multi-Entity Registration in Database
Executing the enrolment performs an atomic operation via [`AdmissionApplicationService.EnrollStudentAsync`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Services/AdmissionApplicationService.cs):

| Entity Created / Updated | Table | Description |
| :--- | :--- | :--- |
| **Student Profile** | `dbo.Students_S` | Permanent student entity with generated Student Code (e.g. `STU-26-0012`), personal info, and guardian linkage. |
| **Academic Enrollment** | `dbo.Enrollments_E` | Annual enrollment mapping student to Academic Year, Class, Section, Batch, and Roll Number. |
| **Fee Invoice** | `dbo.FeeInvoices_FI` | Official fee invoice with Subtotal, Paid Amount, Balance Amount, and Status (`Paid` / `Partially Paid`). |
| **Payment Receipt** | `dbo.Payments_PAY` | Financial transaction record with Payment Method, Transaction Reference, and Receipt Number. |
| **Application Link** | `dbo.AdmissionApplications_AA` | Updated to status `Enrolled` with `AA_AdmittedStudentId` and `AA_EnrollmentId` linked. |

#### 3.3 Step 3 Email Notification & Portal Credentials
The system automatically delivers the **Step 3 Enrolment & Portal Credentials Email** to the student/parent email containing:
- Welcome message to the institution
- Student Code, Admission Number, Enrolled Class, and Section
- Fee Payment Receipt details (Receipt Number, Amount Paid, Payment Method, Date, Balance)
- **Student Portal Login Credentials:**
  - **Login URL:** `/Student/Login`
  - **Username:** Student Email or Student Code
  - **Default Password:** `Welcome@123`
  - Security tip prompting password reset upon first login

---

## 3. Database Schema Mapping (`script_03_10.sql`)

The entire flow leverages the standard database tables:

```
+---------------------------+        +---------------------------+
| AdmissionApplications_AA  |        |    FeeStructures_FS       |
+---------------------------+        +---------------------------+
| AA_Id (PK)                |------->| FS_Id (PK)                |
| AA_ApplicationNumber      |        | FS_Name, FS_Code          |
| AA_Status                 |        +-------------+-------------+
| AA_FeeStructureId         |                      | 1..*
| AA_AdmittedStudentId      |--+                   v
| AA_EnrollmentId           |  |     +---------------------------+
+---------------------------+  |     |   FeeStructureItems_FSI   |
                               |     +---------------------------+
                               |     | FSI_FeeCategoryId         |
                               |     | FSI_Amount, FSI_DueDays   |
                               |     +---------------------------+
                               |
                               +-------------------+
                                                   |
                                                   v
+---------------------------+        +---------------------------+
|       Students_S          |<-------|       Enrollments_E       |
+---------------------------+        +---------------------------+
| S_Id (PK)                 |        | E_Id (PK)                 |
| S_StudentCode             |        | E_StudentId, E_ClassId    |
| S_AdmissionNumber         |        | E_SectionId, E_BatchId    |
| S_FirstName, S_LastName   |        | E_FeeStructureId          |
+-------------+-------------+        +---------------------------+
              |
              +----------------------+
              |                      |
              v                      v
+---------------------------+  +---------------------------+
|     FeeInvoices_FI        |  |       Payments_PAY        |
+---------------------------+  +---------------------------+
| FI_Id (PK), FI_StudentId  |  | PAY_Id (PK), PAY_StudentId|
| FI_InvoiceNumber          |  | PAY_PaymentNumber         |
| FI_TotalAmount            |  | PAY_Amount                |
| FI_PaidAmount, FI_Status  |  | PAY_TransactionReference  |
+---------------------------+  +---------------------------+
```

---

## 4. Key Source Code References

| Layer | File Path | Key Responsibilities |
| :--- | :--- | :--- |
| **Models** | [`IMS.Models/Academic/AdmissionApplicationViewModels.cs`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Models/Academic/AdmissionApplicationViewModels.cs) | `AdmissionApplicationDetailsViewModel`, `AdmissionReviewViewModel`, `AdmissionEnrollViewModel`, `FeeStructureItemSummaryViewModel`. |
| **DAL** | [`IMS.DAL/AdmissionApplicationDAL.cs`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.DAL/AdmissionApplicationDAL.cs) | CRUD operations, parameterized queries, priority sorting, status updates, and fee structure linkage. |
| **Service** | [`IMS.Services/AdmissionApplicationService.cs`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Services/AdmissionApplicationService.cs) | Business logic for Review/Approval, Fee Item loading, multi-entity Enrolment & automatic HTML email dispatch for Steps 1, 2, and 3. |
| **Controllers** | [`IMS.Web/Controllers/AdmissionApplicationController.cs`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Controllers/AdmissionApplicationController.cs)<br/>[`IMS.Web/Controllers/HomeController.cs`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Controllers/HomeController.cs) | Admin endpoints (`Review`, `Enroll`, `GetFeeStructureItems`) and public tracking endpoints (`ApplyAdmission`, `TrackAdmission`). |
| **Views** | [`IMS.Web/Views/AdmissionApplication/Details.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/AdmissionApplication/Details.cshtml)<br/>[`IMS.Web/Views/Home/TrackAdmission.cshtml`](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Views/Home/TrackAdmission.cshtml) | Interactive Admin Review card, dynamic fee preview, Quick Enroll modal, and Public Live Status page. |

---

## 5. Testing & Verification Walkthrough

To test the entire workflow end-to-end:

1. **Step 1 (Submission):**
   - Navigate to `/Home/Admission`.
   - Submit a form with student name and email.
   - Note the generated Application Reference Number (e.g. `APP-26-XXXX`).
   - Open `/Home/TrackAdmission?appNumber=APP-26-XXXX` to verify live status shows **Step 1: Application Received**.

2. **Step 2 (Admin Review & Approval):**
   - Log in to Admin portal and go to `/AdmissionApplication`.
   - Click on the newly submitted application to view details.
   - In the **Review & Decision** card, set status to `Approved`, select a **Fee Plan**, and adjust the Contact Window dates if desired.
   - Click **Save Review Decision & Notify**.
   - Notice the Step 2 Approval Email is triggered, and refreshing `/Home/TrackAdmission` now shows the itemized fee table and contact window instructions.

3. **Step 3 (Enrolment & Fee Collection):**
   - Click **Enroll as Student Now** in the Admin Details view.
   - Choose Class and Section.
   - Verify fee calculation and payment fields (Payment Method: Cash/UPI, Paid Amount, Receipt Number).
   - Click **Confirm Admission & Complete Enrolment**.
   - Application status updates to `Enrolled`.
   - Student Master profile, Annual Enrollment, Fee Invoice, and Payment Receipt are created.
   - Step 3 Email is sent with student credentials (`/Student/Login`, Username, Password: `Welcome@123`).
