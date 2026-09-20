/* ==========================================================================
   Student Module — Frontend Logic & Validation
   - Comprehensive real-time & on-submit validation
   - Dynamic add/remove guardian rows with sequential index renumbering
   - Guardian search & linking to existing master records
   - AJAX create/edit/delete with Toastr notifications & loading states
   ========================================================================== */

var StudentModule = (function () {
    "use strict";

    // Toastr notification configuration
    if (window.toastr) {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            positionClass: "toast-top-right",
            timeOut: 4000,
            preventDuplicates: true
        };
    }

    function ensureFallbackToastContainer() {
        if ($("#imsToastContainer").length) return;
        $("body").append('<div id="imsToastContainer" class="ims-toast-container position-fixed top-0 end-0 p-3" style="z-index:9999;"></div>');
    }

    function showFallbackToast(type, message) {
        ensureFallbackToastContainer();
        const alertClass = type === "success" ? "alert-success" : (type === "warning" ? "alert-warning" : "alert-danger");
        const $toast = $(`
            <div class="alert ${alertClass} alert-dismissible fade show shadow-sm" role="alert">
                <div>${message}</div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `);
        $("#imsToastContainer").append($toast);
        setTimeout(function () {
            $toast.alert("close");
        }, 4000);
    }

    function showSuccess(message) {
        if (window.toastr) toastr.success(message);
        else showFallbackToast("success", message);
    }

    function showError(message) {
        if (window.toastr) toastr.error(message);
        else showFallbackToast("error", message);
    }

    function showWarning(message) {
        if (window.toastr) toastr.warning(message);
        else showFallbackToast("warning", message);
    }

    // ------------------------------------------------------------------
    // Validation Patterns & Helpers
    // ------------------------------------------------------------------

    const EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const PHONE_REGEX = /^[0-9+\-\s()]{7,25}$/;

    function clearFieldError($field) {
        $field.removeClass("is-invalid");
        const fieldName = $field.attr("name");
        if (fieldName) {
            $field.closest(".col-md-3, .col-md-4, .col-md-6, .col-md-8, .col-md-12, .col-12, .form-group, .card-body")
                .find(`.field-error[data-for='${fieldName}']`).text("");
        }
    }

    function clearFieldErrors($form) {
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find(".field-error").text("");
    }

    function setFieldError($field, message) {
        $field.addClass("is-invalid");
        const fieldName = $field.attr("name");
        if (fieldName) {
            let $errorEl = $field.closest(".col-md-3, .col-md-4, .col-md-6, .col-md-8, .col-md-12, .col-12, .form-group, .card-body")
                .find(`.field-error[data-for='${fieldName}']`);
            if ($errorEl.length) {
                $errorEl.text(message);
            } else {
                $field.siblings(".field-error").text(message);
            }
        }
    }

    // ------------------------------------------------------------------
    // Form Validation Logic
    // ------------------------------------------------------------------

    function validateField($field) {
        const name = $field.attr("name") || "";
        const val = ($field.val() || "").toString().trim();

        clearFieldError($field);

        // Branch
        if (name === "S_BranchId") {
            if (!val || val === "00000000-0000-0000-0000-000000000000") {
                setFieldError($field, "Please select a branch.");
                return false;
            }
        }

        // First Name
        if (name === "S_FirstName") {
            if (!val) {
                setFieldError($field, "First Name is required.");
                return false;
            }
            if (val.length < 2) {
                setFieldError($field, "First Name must be at least 2 characters.");
                return false;
            }
            if (val.length > 100) {
                setFieldError($field, "First Name cannot exceed 100 characters.");
                return false;
            }
        }

        // Last Name
        if (name === "S_LastName") {
            if (!val) {
                setFieldError($field, "Last Name is required.");
                return false;
            }
            if (val.length < 1) {
                setFieldError($field, "Last Name is required.");
                return false;
            }
            if (val.length > 100) {
                setFieldError($field, "Last Name cannot exceed 100 characters.");
                return false;
            }
        }

        // Status
        if (name === "S_Status") {
            if (!val) {
                setFieldError($field, "Status is required.");
                return false;
            }
        }

        // Email
        if (name === "S_Email") {
            if (val && !EMAIL_REGEX.test(val)) {
                setFieldError($field, "Please enter a valid email address.");
                return false;
            }
            if (val && val.length > 255) {
                setFieldError($field, "Email cannot exceed 255 characters.");
                return false;
            }
        }

        // Phone
        if (name === "S_Phone") {
            if (val && !PHONE_REGEX.test(val)) {
                setFieldError($field, "Please enter a valid phone number (7-25 digits/symbols).");
                return false;
            }
            if (val && val.length > 30) {
                setFieldError($field, "Phone cannot exceed 30 characters.");
                return false;
            }
        }

        // Date of Birth
        if (name === "S_DateOfBirth") {
            if (val) {
                const dob = new Date(val);
                const now = new Date();
                if (isNaN(dob.getTime())) {
                    setFieldError($field, "Please enter a valid date.");
                    return false;
                }
                if (dob > now) {
                    setFieldError($field, "Date of Birth cannot be in the future.");
                    return false;
                }
            }
        }

        // Guardian fields
        if (name.includes("Guardians[")) {
            const $row = $field.closest(".guardian-row");
            const hasExisting = !!$row.find(".existing-guardian-id").val();

            if (name.endsWith(".FirstName")) {
                if (!hasExisting && !val) {
                    setFieldError($field, "Guardian First Name is required.");
                    return false;
                }
                if (val && val.length > 100) {
                    setFieldError($field, "First Name cannot exceed 100 characters.");
                    return false;
                }
            }

            if (name.endsWith(".LastName")) {
                if (!hasExisting && !val) {
                    setFieldError($field, "Guardian Last Name is required.");
                    return false;
                }
                if (val && val.length > 100) {
                    setFieldError($field, "Last Name cannot exceed 100 characters.");
                    return false;
                }
            }

            if (name.endsWith(".Phone")) {
                if (!hasExisting && !val) {
                    setFieldError($field, "Guardian Phone is required.");
                    return false;
                }
                if (val && !PHONE_REGEX.test(val)) {
                    setFieldError($field, "Please enter a valid phone number.");
                    return false;
                }
                if (val && val.length > 30) {
                    setFieldError($field, "Phone cannot exceed 30 characters.");
                    return false;
                }
            }

            if (name.endsWith(".Email")) {
                if (val && !EMAIL_REGEX.test(val)) {
                    setFieldError($field, "Please enter a valid email address.");
                    return false;
                }
                if (val && val.length > 255) {
                    setFieldError($field, "Email cannot exceed 255 characters.");
                    return false;
                }
            }

            if (name.endsWith(".Relation")) {
                if (!val) {
                    setFieldError($field, "Please select guardian relation.");
                    return false;
                }
            }
        }

        return true;
    }

    function validateStudentForm($form) {
        clearFieldErrors($form);
        let isValid = true;
        let firstInvalidEl = null;
        const errorMessages = [];

        // Required student fields
        const $branch = $form.find("[name='S_BranchId']");
        const branchVal = ($branch.val() || "").toString().trim();
        if (!branchVal || branchVal === "00000000-0000-0000-0000-000000000000") {
            setFieldError($branch, "Please select a branch.");
            errorMessages.push("Branch is required.");
            if (!firstInvalidEl) firstInvalidEl = $branch;
            isValid = false;
        }

        const $firstName = $form.find("[name='S_FirstName']");
        const firstNameVal = ($firstName.val() || "").toString().trim();
        if (!firstNameVal) {
            setFieldError($firstName, "First Name is required.");
            errorMessages.push("Student First Name is required.");
            if (!firstInvalidEl) firstInvalidEl = $firstName;
            isValid = false;
        } else if (firstNameVal.length < 2) {
            setFieldError($firstName, "First Name must be at least 2 characters.");
            errorMessages.push("First Name must be at least 2 characters.");
            if (!firstInvalidEl) firstInvalidEl = $firstName;
            isValid = false;
        }

        const $lastName = $form.find("[name='S_LastName']");
        const lastNameVal = ($lastName.val() || "").toString().trim();
        if (!lastNameVal) {
            setFieldError($lastName, "Last Name is required.");
            errorMessages.push("Student Last Name is required.");
            if (!firstInvalidEl) firstInvalidEl = $lastName;
            isValid = false;
        }

        const $status = $form.find("[name='S_Status']");
        const statusVal = ($status.val() || "").toString().trim();
        if (!statusVal) {
            setFieldError($status, "Status is required.");
            errorMessages.push("Status is required.");
            if (!firstInvalidEl) firstInvalidEl = $status;
            isValid = false;
        }

        // Optional email
        const $email = $form.find("[name='S_Email']");
        const emailVal = ($email.val() || "").toString().trim();
        if (emailVal && !EMAIL_REGEX.test(emailVal)) {
            setFieldError($email, "Please enter a valid email address.");
            errorMessages.push("Invalid student email address.");
            if (!firstInvalidEl) firstInvalidEl = $email;
            isValid = false;
        }

        // Optional phone
        const $phone = $form.find("[name='S_Phone']");
        const phoneVal = ($phone.val() || "").toString().trim();
        if (phoneVal && !PHONE_REGEX.test(phoneVal)) {
            setFieldError($phone, "Please enter a valid phone number (7-25 digits/symbols).");
            errorMessages.push("Invalid student phone number.");
            if (!firstInvalidEl) firstInvalidEl = $phone;
            isValid = false;
        }

        // Date of birth
        const $dob = $form.find("[name='S_DateOfBirth']");
        const dobVal = ($dob.val() || "").toString().trim();
        if (dobVal) {
            const dob = new Date(dobVal);
            const now = new Date();
            if (isNaN(dob.getTime())) {
                setFieldError($dob, "Please enter a valid date of birth.");
                errorMessages.push("Invalid date of birth.");
                if (!firstInvalidEl) firstInvalidEl = $dob;
                isValid = false;
            } else if (dob > now) {
                setFieldError($dob, "Date of birth cannot be in the future.");
                errorMessages.push("Date of birth cannot be in the future.");
                if (!firstInvalidEl) firstInvalidEl = $dob;
                isValid = false;
            }
        }

        // Validate Guardian rows
        const $guardianRows = $form.find(".guardian-row");
        let hasPrimary = false;

        $guardianRows.each(function (idx) {
            const $row = $(this);
            const hasExisting = !!$row.find(".existing-guardian-id").val();

            const $gFirst = $row.find("[name$='.FirstName']");
            const gFirstVal = ($gFirst.val() || "").toString().trim();

            const $gLast = $row.find("[name$='.LastName']");
            const gLastVal = ($gLast.val() || "").toString().trim();

            const $gPhone = $row.find("[name$='.Phone']");
            const gPhoneVal = ($gPhone.val() || "").toString().trim();

            const $gEmail = $row.find("[name$='.Email']");
            const gEmailVal = ($gEmail.val() || "").toString().trim();

            const $gRelation = $row.find("[name$='.Relation']");
            const gRelationVal = ($gRelation.val() || "").toString().trim();

            const isPrimary = $row.find(".primary-contact-check").is(":checked");
            if (isPrimary) hasPrimary = true;

            const rowHasAnyData = hasExisting || gFirstVal || gLastVal || gPhoneVal || gEmailVal || gRelationVal;

            if (rowHasAnyData) {
                if (!hasExisting) {
                    if (!gFirstVal) {
                        setFieldError($gFirst, "Guardian First Name is required.");
                        errorMessages.push(`Guardian ${idx + 1} First Name is required.`);
                        if (!firstInvalidEl) firstInvalidEl = $gFirst;
                        isValid = false;
                    }
                    if (!gLastVal) {
                        setFieldError($gLast, "Guardian Last Name is required.");
                        errorMessages.push(`Guardian ${idx + 1} Last Name is required.`);
                        if (!firstInvalidEl) firstInvalidEl = $gLast;
                        isValid = false;
                    }
                    if (!gPhoneVal) {
                        setFieldError($gPhone, "Guardian Phone is required.");
                        errorMessages.push(`Guardian ${idx + 1} Phone is required.`);
                        if (!firstInvalidEl) firstInvalidEl = $gPhone;
                        isValid = false;
                    } else if (!PHONE_REGEX.test(gPhoneVal)) {
                        setFieldError($gPhone, "Please enter a valid phone number.");
                        errorMessages.push(`Guardian ${idx + 1} Phone number is invalid.`);
                        if (!firstInvalidEl) firstInvalidEl = $gPhone;
                        isValid = false;
                    }
                }

                if (!gRelationVal) {
                    setFieldError($gRelation, "Guardian Relation is required.");
                    errorMessages.push(`Guardian ${idx + 1} Relation is required.`);
                    if (!firstInvalidEl) firstInvalidEl = $gRelation;
                    isValid = false;
                }

                if (gEmailVal && !EMAIL_REGEX.test(gEmailVal)) {
                    setFieldError($gEmail, "Please enter a valid email address.");
                    errorMessages.push(`Guardian ${idx + 1} Email address is invalid.`);
                    if (!firstInvalidEl) firstInvalidEl = $gEmail;
                    isValid = false;
                }
            }
        });

        // Ensure at least one primary guardian is marked if guardians exist
        if ($guardianRows.length > 0 && !hasPrimary) {
            $guardianRows.first().find(".primary-contact-check").prop("checked", true);
        }

        if (!isValid) {
            showError(errorMessages[0] || "Please correct the highlighted errors.");
            if (firstInvalidEl && firstInvalidEl.length) {
                firstInvalidEl[0].scrollIntoView({ behavior: "smooth", block: "center" });
                firstInvalidEl.focus();
            }
        }

        return isValid;
    }

    // ------------------------------------------------------------------
    // Dynamic Guardian Management & Index Renumbering
    // ------------------------------------------------------------------

    function reindexGuardianRows() {
        const $rows = $("#guardiansContainer .guardian-row");

        $rows.each(function (idx) {
            const $row = $(this);
            $row.attr("data-index", idx);
            $row.find(".guardian-title").html(`<i class="bi bi-person-heart me-1 text-danger"></i>Guardian ${idx + 1}`);

            $row.find(".existing-guardian-id").attr("name", `Guardians[${idx}].ExistingGuardianId`);

            $row.find(".guardian-first-name")
                .attr("name", `Guardians[${idx}].FirstName`);
            $row.find(".field-error[data-for*='FirstName']")
                .attr("data-for", `Guardians[${idx}].FirstName`);

            $row.find(".guardian-last-name")
                .attr("name", `Guardians[${idx}].LastName`);
            $row.find(".field-error[data-for*='LastName']")
                .attr("data-for", `Guardians[${idx}].LastName`);

            $row.find(".guardian-phone")
                .attr("name", `Guardians[${idx}].Phone`);
            $row.find(".field-error[data-for*='Phone']")
                .attr("data-for", `Guardians[${idx}].Phone`);

            $row.find(".guardian-email")
                .attr("name", `Guardians[${idx}].Email`);
            $row.find(".field-error[data-for*='Email']")
                .attr("data-for", `Guardians[${idx}].Email`);

            $row.find(".guardian-occupation")
                .attr("name", `Guardians[${idx}].Occupation`);

            $row.find(".relation-select")
                .attr("name", `Guardians[${idx}].Relation`);
            $row.find(".field-error[data-for*='Relation']")
                .attr("data-for", `Guardians[${idx}].Relation`);

            const checkId = `guardian_primary_${idx}`;
            $row.find(".primary-contact-check")
                .attr("name", `Guardians[${idx}].IsPrimary`)
                .attr("id", checkId);
            $row.find(".primary-contact-check").next("label").attr("for", checkId);
        });

        if ($rows.length === 0) {
            $("#noGuardiansNotice").removeClass("d-none");
        } else {
            $("#noGuardiansNotice").addClass("d-none");
        }
    }

    function createGuardianRowHtml(index) {
        return `
        <div class="card mb-3 guardian-row border shadow-none bg-light" data-index="${index}" data-existing="false">
            <div class="card-body">
                <div class="d-flex justify-content-between align-items-center mb-3 pb-2 border-bottom">
                    <h6 class="mb-0 fw-bold text-secondary guardian-title">
                        <i class="bi bi-person-heart me-1 text-danger"></i>Guardian ${index + 1}
                    </h6>
                    <button type="button" class="btn btn-sm btn-outline-danger remove-guardian-btn" title="Remove Guardian">
                        <i class="bi bi-trash3 me-1"></i>Remove
                    </button>
                </div>

                <input type="hidden" name="Guardians[${index}].ExistingGuardianId" class="existing-guardian-id" value="" />

                <div class="row g-2 mb-3">
                    <div class="col-md-8 position-relative">
                        <label class="form-label small fw-semibold">Search Existing Guardian (optional)</label>
                        <div class="input-group input-group-sm">
                            <span class="input-group-text"><i class="bi bi-search"></i></span>
                            <input type="text" class="form-control guardian-search-input" autocomplete="off"
                                   placeholder="Type name, phone or email to find and link existing guardian..." />
                        </div>
                        <div class="list-group guardian-search-results position-absolute w-100 shadow-lg" style="z-index:1050; display:none;"></div>
                    </div>
                    <div class="col-md-4 d-flex align-items-end">
                        <button type="button" class="btn btn-sm btn-outline-secondary clear-guardian-link-btn w-100">
                            <i class="bi bi-x-circle me-1"></i>Clear / New Guardian
                        </button>
                    </div>
                </div>

                <div class="row g-3">
                    <div class="col-md-3">
                        <label class="form-label small fw-semibold">First Name <span class="text-danger">*</span></label>
                        <input type="text" name="Guardians[${index}].FirstName" class="form-control form-control-sm guardian-first-name" maxlength="100" />
                        <div class="text-danger small field-error" data-for="Guardians[${index}].FirstName"></div>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label small fw-semibold">Last Name <span class="text-danger">*</span></label>
                        <input type="text" name="Guardians[${index}].LastName" class="form-control form-control-sm guardian-last-name" maxlength="100" />
                        <div class="text-danger small field-error" data-for="Guardians[${index}].LastName"></div>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label small fw-semibold">Phone <span class="text-danger">*</span></label>
                        <input type="text" name="Guardians[${index}].Phone" class="form-control form-control-sm guardian-phone" maxlength="30" />
                        <div class="text-danger small field-error" data-for="Guardians[${index}].Phone"></div>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label small fw-semibold">Email</label>
                        <input type="email" name="Guardians[${index}].Email" class="form-control form-control-sm guardian-email" maxlength="255" />
                        <div class="text-danger small field-error" data-for="Guardians[${index}].Email"></div>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label small fw-semibold">Occupation</label>
                        <input type="text" name="Guardians[${index}].Occupation" class="form-control form-control-sm guardian-occupation" maxlength="150" />
                    </div>
                    <div class="col-md-4">
                        <label class="form-label small fw-semibold">Relation <span class="text-danger">*</span></label>
                        <select name="Guardians[${index}].Relation" class="form-select form-select-sm relation-select"></select>
                        <div class="text-danger small field-error" data-for="Guardians[${index}].Relation"></div>
                    </div>
                    <div class="col-md-4 d-flex align-items-center">
                        <div class="form-check mt-3">
                            <input type="checkbox" class="form-check-input primary-contact-check" name="Guardians[${index}].IsPrimary" value="true" id="guardian_primary_${index}" />
                            <label class="form-check-label small fw-semibold" for="guardian_primary_${index}">Primary Contact</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;
    }

    function addGuardianRow() {
        const $container = $("#guardiansContainer");
        const nextIdx = $container.find(".guardian-row").length;
        const $row = $(createGuardianRowHtml(nextIdx));
        $container.append($row);

        reindexGuardianRows();

        // Load relation dropdown
        if (typeof Dropdown !== "undefined" && Dropdown.load) {
            Dropdown.load({
                element: $row.find(".relation-select"),
                entityType: "Relation",
                defaultText: "Select Relation",
                includeDefault: true
            });
        }

        wireGuardianSearch($row);

        // If it's the only guardian row, mark as primary by default
        if ($container.find(".guardian-row").length === 1) {
            $row.find(".primary-contact-check").prop("checked", true);
        }

        $row.find(".guardian-first-name").focus();
    }

    function removeGuardianRow($row) {
        const wasPrimary = $row.find(".primary-contact-check").is(":checked");
        $row.fadeOut(200, function () {
            $(this).remove();
            reindexGuardianRows();
            if (wasPrimary) {
                $("#guardiansContainer .guardian-row").first().find(".primary-contact-check").prop("checked", true);
            }
        });
    }

    // ------------------------------------------------------------------
    // Guardian Autocomplete Search
    // ------------------------------------------------------------------

    let searchDebounceTimer = null;

    function wireGuardianSearch($row) {
        const $input = $row.find(".guardian-search-input");
        const $results = $row.find(".guardian-search-results");

        $input.off("input").on("input", function () {
            const term = $(this).val().trim();
            clearTimeout(searchDebounceTimer);

            if (term.length < 2) {
                $results.hide().empty();
                return;
            }

            searchDebounceTimer = setTimeout(function () {
                $.get("/Students/SearchGuardians", { term: term })
                    .done(function (data) {
                        $results.empty();
                        if (!data || data.length === 0) {
                            $results.append('<div class="list-group-item text-muted small py-2"><i class="bi bi-info-circle me-1"></i>No matching guardians found</div>');
                        } else {
                            data.forEach(function (g) {
                                const guardianId = g.G_Id || g.g_Id || g.id || g.Id;
                                const firstName = g.G_FirstName || g.g_FirstName || g.firstName || g.FirstName || "";
                                const lastName = g.G_LastName || g.g_LastName || g.lastName || g.LastName || "";
                                const fullName = g.fullName || `${firstName} ${lastName}`.trim();
                                const phone = g.G_Phone || g.g_Phone || g.phone || g.Phone || "";
                                const email = g.G_Email || g.g_Email || g.email || g.Email || "";
                                const occupation = g.G_Occupation || g.g_Occupation || g.occupation || g.Occupation || "";

                                const $item = $(`
                                    <button type="button" class="list-group-item list-group-item-action small py-2">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <strong><i class="bi bi-person me-1"></i>${fullName}</strong>
                                            <span class="badge bg-light text-dark">${occupation || "Guardian"}</span>
                                        </div>
                                        <div class="text-muted small">${phone ? '<i class="bi bi-telephone me-1"></i>' + phone : ''} ${email ? '· <i class="bi bi-envelope me-1"></i>' + email : ''}</div>
                                    </button>
                                `);

                                $item.on("click", function (e) {
                                    e.preventDefault();
                                    applyGuardianSelection($row, {
                                        id: guardianId,
                                        firstName: firstName,
                                        lastName: lastName,
                                        fullName: fullName,
                                        phone: phone,
                                        email: email,
                                        occupation: occupation
                                    });
                                    $results.hide().empty();
                                });
                                $results.append($item);
                            });
                        }
                        $results.show();
                    })
                    .fail(function () {
                        $results.hide().empty();
                    });
            }, 300);
        });

        // Close dropdown when clicking outside
        $(document).on("click", function (e) {
            if (!$(e.target).closest($row.find(".position-relative")).length) {
                $results.hide();
            }
        });

        // Clear button
        $row.find(".clear-guardian-link-btn").off("click").on("click", function () {
            $row.find(".existing-guardian-id").val("");
            $row.attr("data-existing", "false");
            $row.find(".guardian-first-name").val("").prop("readonly", false).removeClass("bg-white text-muted is-invalid");
            $row.find(".guardian-last-name").val("").prop("readonly", false).removeClass("bg-white text-muted is-invalid");
            $row.find(".guardian-phone").val("").prop("readonly", false).removeClass("bg-white text-muted is-invalid");
            $row.find(".guardian-email").val("").prop("readonly", false).removeClass("bg-white text-muted is-invalid");
            $row.find(".guardian-occupation").val("").prop("readonly", false).removeClass("bg-white text-muted is-invalid");
            $row.find(".field-error").text("");
            $input.val("");
            $results.hide().empty();
        });
    }

    function applyGuardianSelection($row, guardian) {
        $row.find(".existing-guardian-id").val(guardian.id);
        $row.attr("data-existing", "true");

        let first = guardian.firstName;
        let last = guardian.lastName;
        if (!first && guardian.fullName) {
            const parts = guardian.fullName.split(" ");
            first = parts[0] || "";
            last = parts.slice(1).join(" ") || "";
        }

        $row.find(".guardian-first-name").val(first).prop("readonly", true).addClass("bg-white text-muted");
        $row.find(".guardian-last-name").val(last).prop("readonly", true).addClass("bg-white text-muted");
        $row.find(".guardian-phone").val(guardian.phone || "").prop("readonly", true).addClass("bg-white text-muted");
        $row.find(".guardian-email").val(guardian.email || "").prop("readonly", true).addClass("bg-white text-muted");
        $row.find(".guardian-occupation").val(guardian.occupation || "").prop("readonly", true).addClass("bg-white text-muted");
        $row.find(".guardian-search-input").val(guardian.fullName || `${first} ${last}`.trim());

        clearFieldError($row.find(".guardian-first-name"));
        clearFieldError($row.find(".guardian-last-name"));
        clearFieldError($row.find(".guardian-phone"));
        clearFieldError($row.find(".guardian-email"));
    }

    // ------------------------------------------------------------------
    // AJAX Form Submissions
    // ------------------------------------------------------------------

    function wireAjaxFormSubmit(formSelector, ajaxUrl) {
        $(document).off("submit", formSelector).on("submit", formSelector, function (e) {
            e.preventDefault();
            const $form = $(this);

            if (!validateStudentForm($form)) {
                return false;
            }

            const $submitBtn = $form.find("button[type='submit']");
            const originalBtnHtml = $submitBtn.html();
            $submitBtn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving Student...');

            $.ajax({
                url: ajaxUrl,
                type: "POST",
                data: $form.serialize(),
                headers: {
                    "RequestVerificationToken": $form.find("input[name='__RequestVerificationToken']").val()
                },
                success: function (response) {
                    if (response && response.success) {
                        showSuccess(response.message || "Student saved successfully.");
                        setTimeout(function () {
                            window.location.href = "/Students/Index";
                        }, 700);
                    } else {
                        showError(response ? (response.message || "Failed to save student.") : "Something went wrong.");
                        $submitBtn.prop("disabled", false).html(originalBtnHtml);
                    }
                },
                error: function (xhr) {
                    let errMsg = "Something went wrong while saving the student.";
                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        errMsg = xhr.responseJSON.message;
                    } else if (xhr.responseText) {
                        try {
                            const parsed = JSON.parse(xhr.responseText);
                            if (parsed && parsed.message) errMsg = parsed.message;
                        } catch (e) {
                            errMsg = "Server returned an error (" + xhr.status + "). Please check your inputs.";
                        }
                    }
                    showError(errMsg);
                    $submitBtn.prop("disabled", false).html(originalBtnHtml);
                }
            });
        });
    }

    // ------------------------------------------------------------------
    // AJAX Delete Student
    // ------------------------------------------------------------------

    function wireAjaxDelete() {
        $(document).off("click", ".confirm-delete-btn").on("click", ".confirm-delete-btn", function () {
            const id = $(this).data("id");
            const $modal = $("#deleteModal-" + id);

            $modal.find(".do-delete-btn").off("click").on("click", function () {
                const $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Removing...');

                $.ajax({
                    url: "/Students/DeleteStudent/" + id,
                    type: "POST",
                    data: {
                        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").first().val()
                    },
                    success: function (response) {
                        $modal.modal("hide");
                        if (response && response.success) {
                            showSuccess(response.message || "Student removed successfully.");
                            $("#student-row-" + id).fadeOut(350, function () { $(this).remove(); });
                        } else {
                            showError(response ? (response.message || "Unable to remove student.") : "Failed to remove student.");
                        }
                    },
                    error: function () {
                        $modal.modal("hide");
                        showError("Request failed. Please try again.");
                    },
                    complete: function () {
                        $btn.prop("disabled", false).html("Remove");
                    }
                });
            });
        });
    }

    // ------------------------------------------------------------------
    // Realtime Input Event Bindings
    // ------------------------------------------------------------------

    function wireLiveValidation() {
        $(document).on("blur change", "#studentCreateForm input, #studentCreateForm select, #studentEditForm input, #studentEditForm select", function () {
            validateField($(this));
        });

        $(document).on("input", "#studentCreateForm input, #studentEditForm input", function () {
            if ($(this).hasClass("is-invalid")) {
                validateField($(this));
            }
        });

        // Enforce single primary contact checkbox
        $(document).on("change", ".primary-contact-check", function () {
            if ($(this).is(":checked")) {
                $("#guardiansContainer .primary-contact-check").not(this).prop("checked", false);
            }
        });

        // Add Guardian button
        $(document).on("click", "#addGuardianBtn", function (e) {
            e.preventDefault();
            addGuardianRow();
        });

        // Remove Guardian button
        $(document).on("click", ".remove-guardian-btn", function (e) {
            e.preventDefault();
            removeGuardianRow($(this).closest(".guardian-row"));
        });
    }

    // ------------------------------------------------------------------
    // Initialization
    // ------------------------------------------------------------------

    function init() {
        wireLiveValidation();
        reindexGuardianRows();

        $("#guardiansContainer .guardian-row").each(function () {
            wireGuardianSearch($(this));
        });

        wireAjaxFormSubmit("#studentCreateForm", "/Students/AddStudent");
        wireAjaxFormSubmit("#studentEditForm", "/Students/EditStudent");
        wireAjaxDelete();
    }

    $(function () {
        init();
    });

    return {
        init: init,
        addGuardianRow: addGuardianRow,
        reindexGuardianRows: reindexGuardianRows,
        validateStudentForm: validateStudentForm
    };
})();