/* Syllabus — client-side logic with cascading dropdowns, live validation & toast feedback */
(function () {
    "use strict";

    if (window.toastr) {
        toastr.options = { closeButton: true, progressBar: true, positionClass: "toast-top-right", timeOut: 3500, preventDuplicates: true };
    }

    function ensureFallbackToastContainer() {
        if ($("#imsToastContainer").length) return;
        $("body").append('<div id="imsToastContainer" class="ims-toast-container"></div>');
    }
    function showFallbackToast(type, message) {
        ensureFallbackToastContainer();
        const $toast = $(`<div class="ims-toast ims-toast-${type}">${message}</div>`);
        $("#imsToastContainer").append($toast);
        requestAnimationFrame(() => $toast.addClass("show"));
        setTimeout(() => { $toast.removeClass("show"); setTimeout(() => $toast.remove(), 250); }, 3500);
    }
    function showSuccess(msg) { window.toastr ? toastr.success(msg) : showFallbackToast("success", msg); }
    function showError(msg) { window.toastr ? toastr.error(msg) : showFallbackToast("error", msg); }

    function clearFieldErrors($form) {
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find(".field-error").text("");
    }

    function setFieldError($field, message) {
        if (!$field || !$field.length) return;
        $field.addClass("is-invalid");
        const name = $field.attr("name") || $field.attr("id");
        const $err = $field.closest(".col-md-4, .col-md-8, .col-12, .col-sm-6, .mb-3").find(".field-error[data-for='" + name + "']");
        if ($err.length) {
            $err.text(message);
        }
    }

    function wireLiveValidation($form) {
        $form.on("input change", "input, select, textarea", function () {
            const $field = $(this);
            if ($field.hasClass("is-invalid")) {
                $field.removeClass("is-invalid");
                const name = $field.attr("name") || $field.attr("id");
                $field.closest(".col-md-4, .col-md-8, .col-12, .col-sm-6, .mb-3").find(".field-error[data-for='" + name + "']").text("");
            }
        });
    }

    function wireCourseCascading() {
        const $courseSelect = $("#CourseId");
        const $subjectSelect = $("#SubjectId");

        if (!$courseSelect.length || !$subjectSelect.length) return;

        $courseSelect.on("change", function () {
            const courseId = $(this).val();
            const currentSubjectId = $subjectSelect.val();

            if (!courseId || courseId === "00000000-0000-0000-0000-000000000000") {
                // Fetch all subjects when no course selected
                loadSubjects(null, currentSubjectId);
                return;
            }

            loadSubjects(courseId, currentSubjectId);
        });

        function loadSubjects(courseId, selectedSubjectId) {
            const url = "/Syllabus/GetSubjectsByCourse" + (courseId ? ("?courseId=" + encodeURIComponent(courseId)) : "");
            $.ajax({
                url: url,
                type: "GET",
                success: function (res) {
                    if (res && res.success && Array.isArray(res.data)) {
                        $subjectSelect.empty();
                        $subjectSelect.append('<option value="">-- Select Subject --</option>');
                        let matched = false;
                        res.data.forEach(function (s) {
                            const isSel = selectedSubjectId && String(s.value).toLowerCase() === String(selectedSubjectId).toLowerCase();
                            if (isSel) matched = true;
                            $subjectSelect.append(
                                $('<option></option>').val(s.value).text(s.text).prop('selected', isSel)
                            );
                        });
                        if (!matched && selectedSubjectId) {
                            $subjectSelect.val("");
                        }
                    }
                }
            });
        }
    }

    function validateForm($form) {
        clearFieldErrors($form);
        let isValid = true;
        let firstInvalid = null;

        function checkRequired($field, fieldName, label) {
            const val = ($field.val() || "").toString().trim();
            if (!val || val === "00000000-0000-0000-0000-000000000000") {
                setFieldError($field, label + " is required.");
                if (!firstInvalid) firstInvalid = $field;
                isValid = false;
            }
        }

        const $course = $form.find("#CourseId, [name='CourseId']");
        const $subject = $form.find("#SubjectId, [name='SubjectId']");
        const $unitTitle = $form.find("#UnitTitle, [name='UnitTitle']");
        const $unitNumber = $form.find("#UnitNumber, [name='UnitNumber']");

        checkRequired($course, "CourseId", "Course");
        checkRequired($subject, "SubjectId", "Subject");
        checkRequired($unitTitle, "UnitTitle", "Unit Title");

        const unitNumberVal = parseInt($unitNumber.val(), 10);
        if (isNaN(unitNumberVal) || unitNumberVal <= 0) {
            setFieldError($unitNumber, "Unit Number must be greater than zero.");
            if (!firstInvalid) firstInvalid = $unitNumber;
            isValid = false;
        }

        if (!isValid) {
            showError("Please fill in all required fields correctly.");
            if (firstInvalid && firstInvalid.length) {
                firstInvalid.focus();
            }
        }
        return isValid;
    }

    function wireFormSubmit() {
        $(document).on("submit", "#syllabusForm", function (e) {
            e.preventDefault();
            const $form = $(this);
            if (!validateForm($form)) return;

            const $btn = $form.find("button[type='submit']");
            const original = $btn.html();
            $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving...');

            $.ajax({
                url: $form.attr("action"),
                type: "POST",
                data: $form.serialize(),
                success: function (res) {
                    if (res.success) {
                        showSuccess(res.message || "Syllabus unit saved successfully.");
                        setTimeout(() => window.location.href = "/Syllabus/Index", 800);
                    } else {
                        showError(res.message || "Failed to save syllabus unit.");
                        if (res.errors) {
                            let firstErrField = null;
                            $.each(res.errors, function (key, val) {
                                const $f = $form.find("[name='" + key + "'], #" + key);
                                if ($f.length) {
                                    setFieldError($f, val);
                                    if (!firstErrField) firstErrField = $f;
                                }
                            });
                            if (firstErrField) firstErrField.focus();
                        }
                        $btn.prop("disabled", false).html(original);
                    }
                },
                error: function (xhr) {
                    showError("Request failed: " + (xhr.responseText || xhr.statusText));
                    $btn.prop("disabled", false).html(original);
                }
            });
        });
    }

    function wireToggle(selector, url, paramName) {
        $(document).on("change", selector, function () {
            const $checkbox = $(this);
            const id = $checkbox.data("id");
            const value = $checkbox.is(":checked");
            $checkbox.prop("disabled", true);

            const data = { id: id, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() };
            data[paramName] = value;

            $.ajax({
                url: url,
                type: "POST",
                data: data,
                success: function (res) {
                    if (res.success) showSuccess(res.message);
                    else { showError(res.message); $checkbox.prop("checked", !value); }
                },
                error: function () { showError("Request failed."); $checkbox.prop("checked", !value); },
                complete: function () { $checkbox.prop("disabled", false); }
            });
        });
    }

    function wireDelete() {
        $(document).on("click", ".confirm-delete-btn", function () {
            const id = $(this).data("id");
            const $modal = $("#deleteModal-" + id);

            $modal.find(".do-delete-btn").off("click").on("click", function () {
                const $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Deleting...');

                $.ajax({
                    url: "/Syllabus/DeleteAjax/",
                    type: "POST",
                    data: { id: id, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                    success: function (res) {
                        $modal.modal("hide");
                        if (res.success) {
                            showSuccess(res.message || "Deleted.");
                            $("#syllabus-row-" + id).fadeOut(300, function () { $(this).remove(); });
                        } else {
                            showError(res.message || "Unable to delete.");
                        }
                    },
                    error: function () { $modal.modal("hide"); showError("Request failed."); },
                    complete: function () { $btn.prop("disabled", false).html("Delete"); }
                });
            });
        });
    }

    $(function () {
        const $form = $("#syllabusForm");
        if ($form.length) {
            wireLiveValidation($form);
            wireCourseCascading();
        }
        wireFormSubmit();
        wireToggle(".toggle-active-switch", "/Syllabus/ToggleActiveAjax", "isActive");
        wireToggle(".toggle-completed-switch", "/Syllabus/ToggleCompletedAjax", "isCompleted");
        wireDelete();
    });
})();
