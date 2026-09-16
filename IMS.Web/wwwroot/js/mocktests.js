/* ==========================================================================
   Mock Tests Module — Client-side logic
   - Form validation
   - AJAX Create/Edit with toastr notifications
   - AJAX Delete with modal confirmation
   - AJAX Toggle Active switch
   Requires: jQuery, Bootstrap 5 JS, toastr.js
   ========================================================================== */

(function () {
    "use strict";

    // Toastr setup
    if (window.toastr) {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            positionClass: "toast-top-right",
            timeOut: 3500,
            preventDuplicates: true
        };
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
        setTimeout(function () {
            $toast.removeClass("show");
            setTimeout(() => $toast.remove(), 250);
        }, 3500);
    }

    function showSuccess(message) {
        if (window.toastr) toastr.success(message);
        else showFallbackToast("success", message);
    }

    function showError(message) {
        if (window.toastr) toastr.error(message);
        else showFallbackToast("error", message);
    }

    // ------------------------------------------------------------------
    // Validation
    // ------------------------------------------------------------------

    function clearFieldErrors($form) {
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find(".field-error").text("");
    }

    function setFieldError($field, message) {
        $field.addClass("is-invalid");
        const name = $field.attr("name") || $field.attr("id");
        const $errorEl = $field.closest(".col-md-3, .col-md-4, .col-md-6, .col-md-12, .col-12")
            .find(`.field-error[data-for='${name}']`);
        if ($errorEl.length) $errorEl.text(message);
    }

    function validateMockTestForm($form) {
        clearFieldErrors($form);
        let isValid = true;

        function required($field, label) {
            const val = ($field.val() || "").toString().trim();
            if (!val) {
                setFieldError($field, label + " is required.");
                isValid = false;
            }
        }

        required($form.find("[name='MT_BatchId']"), "Batch");
        required($form.find("[name='MT_SubjectId']"), "Subject");
        required($form.find("[name='MT_Title']"), "Test Title");
        required($form.find("[name='MT_TestDate']"), "Test Date");
        required($form.find("[name='MT_Status']"), "Status");

        const $duration = $form.find("[name='MT_DurationMinutes']");
        const durationVal = parseInt($duration.val(), 10);
        if (isNaN(durationVal) || durationVal < 1) {
            setFieldError($duration, "Duration must be at least 1 minute.");
            isValid = false;
        }

        const $totalMarks = $form.find("[name='MT_TotalMarks']");
        const totalMarksVal = parseFloat($totalMarks.val());
        if (isNaN(totalMarksVal) || totalMarksVal <= 0) {
            setFieldError($totalMarks, "Total marks must be greater than 0.");
            isValid = false;
        }

        const $passMarks = $form.find("[name='MT_PassMarks']");
        const passMarksVal = parseFloat($passMarks.val());
        if (isNaN(passMarksVal) || passMarksVal < 0) {
            setFieldError($passMarks, "Pass marks cannot be negative.");
            isValid = false;
        } else if (!isNaN(totalMarksVal) && passMarksVal > totalMarksVal) {
            setFieldError($passMarks, "Pass marks cannot exceed total marks.");
            isValid = false;
        }

        if (!isValid) {
            showError("Please complete all required fields correctly.");
        }

        return isValid;
    }

    // ------------------------------------------------------------------
    // AJAX Form Submit
    // ------------------------------------------------------------------

    function wireAjaxFormSubmit(formSelector, redirectUrl) {
        $(document).on("submit", formSelector, function (e) {
            e.preventDefault();
            const $form = $(this);

            if (!validateMockTestForm($form)) return;

            const $submitBtn = $form.find("button[type='submit']");
            const originalText = $submitBtn.html();
            $submitBtn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving...');

            $.ajax({
                url: $form.attr("action"),
                type: "POST",
                data: $form.serialize(),
                success: function (response) {
                    if (response.success) {
                        showSuccess(response.message || "Mock test saved successfully.");
                        setTimeout(function () {
                            window.location.href = redirectUrl;
                        }, 700);
                    } else {
                        showError(response.message || "Something went wrong.");
                        $submitBtn.prop("disabled", false).html(originalText);
                    }
                },
                error: function (xhr) {
                    showError("Request failed: " + (xhr.responseText || xhr.statusText));
                    $submitBtn.prop("disabled", false).html(originalText);
                }
            });
        });
    }

    // ------------------------------------------------------------------
    // AJAX Toggle Active
    // ------------------------------------------------------------------

    function wireToggleActive() {
        $(document).on("change", ".test-toggle-active", function () {
            const $switch = $(this);
            const id = $switch.data("id");
            const isActive = $switch.is(":checked");
            const token = $("input[name='__RequestVerificationToken']").val();

            $.ajax({
                url: "/MockTests/ToggleActiveAjax",
                type: "POST",
                data: {
                    id: id,
                    isActive: isActive,
                    __RequestVerificationToken: token
                },
                success: function (response) {
                    if (response.success) {
                        showSuccess(response.message || "Status updated.");
                    } else {
                        showError(response.message || "Failed to update status.");
                        $switch.prop("checked", !isActive);
                    }
                },
                error: function () {
                    showError("Request failed. Reverting status.");
                    $switch.prop("checked", !isActive);
                }
            });
        });
    }

    // ------------------------------------------------------------------
    // AJAX Delete
    // ------------------------------------------------------------------

    function wireAjaxDelete() {
        $(document).on("click", ".confirm-delete-mocktest-btn", function () {
            const id = $(this).data("id");
            const $modal = $("#deleteModal-" + id);
            $modal.modal("show");

            $modal.find(".do-delete-mocktest-btn").off("click").on("click", function () {
                const $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Deleting...');

                const token = $("input[name='__RequestVerificationToken']").val();

                $.ajax({
                    url: "/MockTests/DeleteAjax",
                    type: "POST",
                    data: {
                        id: id,
                        __RequestVerificationToken: token
                    },
                    success: function (response) {
                        $modal.modal("hide");
                        if (response.success) {
                            showSuccess(response.message || "Mock test deleted.");
                            $("#test-row-" + id).fadeOut(300, function () { $(this).remove(); });
                        } else {
                            showError(response.message || "Unable to delete mock test.");
                        }
                    },
                    error: function () {
                        $modal.modal("hide");
                        showError("Request failed. Please try again.");
                    },
                    complete: function () {
                        $btn.prop("disabled", false).html("Delete Test");
                    }
                });
            });
        });
    }

    // ------------------------------------------------------------------
    // Init
    // ------------------------------------------------------------------

    $(function () {
        wireAjaxFormSubmit("#mockTestCreateForm", "/MockTests/Index");
        wireAjaxFormSubmit("#mockTestEditForm", "/MockTests/Index");
        wireToggleActive();
        wireAjaxDelete();
    });
})();
