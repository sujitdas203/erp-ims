/* Syllabus — client-side logic. Same toastr/fallback-toast pattern as every other module */
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
        $field.addClass("is-invalid");
        $field.closest(".col-md-4, .col-md-8, .col-12").find(".field-error[data-for='" + $field.attr("name") + "']").text(message);
    }

    function validateForm($form) {
        clearFieldErrors($form);
        let isValid = true;
        const errors = [];

        function required($field, label) {
            const val = ($field.val() || "").toString().trim();
            if (!val) {
                setFieldError($field, label + " is required.");
                errors.push(label + " is required.");
                isValid = false;
            }
        }

        required($form.find("[name='CourseId']"), "Course");
        required($form.find("[name='SubjectId']"), "Subject");
        required($form.find("[name='UnitTitle']"), "Unit title");

        const $unitNumber = $form.find("[name='UnitNumber']");
        const unitNumberVal = parseInt($unitNumber.val(), 10);
        if (!unitNumberVal || unitNumberVal <= 0) {
            setFieldError($unitNumber, "Unit number must be greater than zero.");
            errors.push("Unit number must be greater than zero.");
            isValid = false;
        }

        if (!isValid) showError(errors[0]);
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
                        showSuccess(res.message || "Saved successfully.");
                        setTimeout(() => window.location.href = "/Syllabus/Index", 700);
                    } else {
                        showError(res.message || "Something went wrong.");
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
        wireFormSubmit();
        wireToggle(".toggle-active-switch", "/Syllabus/ToggleActiveAjax", "isActive");
        wireToggle(".toggle-completed-switch", "/Syllabus/ToggleCompletedAjax", "isCompleted");
        wireDelete();
    });
})();
