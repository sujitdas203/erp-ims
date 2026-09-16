/* ==========================================================================
   Home Tasks Module — Client-side logic
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

    function validateHomeTaskForm($form) {
        clearFieldErrors($form);
        let isValid = true;

        function required($field, label) {
            const val = ($field.val() || "").toString().trim();
            if (!val) {
                setFieldError($field, label + " is required.");
                isValid = false;
            }
        }

        required($form.find("[name='HT_BatchId']"), "Batch");
        required($form.find("[name='HT_SubjectId']"), "Subject");
        required($form.find("[name='HT_Title']"), "Task Title");
        required($form.find("[name='HT_DueDate']"), "Due Date");

        const $maxMarks = $form.find("[name='HT_MaxMarks']");
        const maxMarksVal = $maxMarks.val();
        if (maxMarksVal !== "" && maxMarksVal !== null && !isNaN(maxMarksVal)) {
            if (parseFloat(maxMarksVal) < 0) {
                setFieldError($maxMarks, "Max marks cannot be negative.");
                isValid = false;
            }
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

            if (!validateHomeTaskForm($form)) return;

            const $submitBtn = $form.find("button[type='submit']");
            const originalText = $submitBtn.html();
            $submitBtn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving...');

            $.ajax({
                url: $form.attr("action"),
                type: "POST",
                data: $form.serialize(),
                success: function (response) {
                    if (response.success) {
                        showSuccess(response.message || "Home task saved successfully.");
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
        $(document).on("change", ".hometask-toggle-active", function () {
            const $switch = $(this);
            const id = $switch.data("id");
            const isActive = $switch.is(":checked");
            const token = $("input[name='__RequestVerificationToken']").val();

            $.ajax({
                url: "/HomeTasks/ToggleActiveAjax",
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
        $(document).on("click", ".confirm-delete-hometask-btn", function () {
            const id = $(this).data("id");
            const $modal = $("#deleteModal-" + id);
            $modal.modal("show");

            $modal.find(".do-delete-hometask-btn").off("click").on("click", function () {
                const $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Deleting...');

                const token = $("input[name='__RequestVerificationToken']").val();

                $.ajax({
                    url: "/HomeTasks/DeleteAjax",
                    type: "POST",
                    data: {
                        id: id,
                        __RequestVerificationToken: token
                    },
                    success: function (response) {
                        $modal.modal("hide");
                        if (response.success) {
                            showSuccess(response.message || "Home task deleted.");
                            $("#task-row-" + id).fadeOut(300, function () { $(this).remove(); });
                        } else {
                            showError(response.message || "Unable to delete home task.");
                        }
                    },
                    error: function () {
                        $modal.modal("hide");
                        showError("Request failed. Please try again.");
                    },
                    complete: function () {
                        $btn.prop("disabled", false).html("Delete Task");
                    }
                });
            });
        });
    }

    // ------------------------------------------------------------------
    // Init
    // ------------------------------------------------------------------

    $(function () {
        wireAjaxFormSubmit("#homeTaskCreateForm", "/HomeTasks/Index");
        wireAjaxFormSubmit("#homeTaskEditForm", "/HomeTasks/Index");
        wireToggleActive();
        wireAjaxDelete();
    });
})();
