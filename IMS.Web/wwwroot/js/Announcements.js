/* Announcements — client-side logic. Same toastr/fallback-toast pattern as every other module */
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
        $field.closest(".col-md-4, .col-md-6, .col-12").find(".field-error[data-for='" + $field.attr("name") + "']").text(message);
    }

    function validateForm($form) {
        clearFieldErrors($form);
        let isValid = true;
        const errors = [];

        const $title = $form.find("[name='Title']");
        if (!($title.val() || "").trim()) {
            setFieldError($title, "Title is required.");
            errors.push("Title is required.");
            isValid = false;
        }

        const $content = $form.find("[name='Content']");
        if (!($content.val() || "").trim()) {
            setFieldError($content, "Content is required.");
            errors.push("Content is required.");
            isValid = false;
        }

        const publishedVal = $form.find("[name='PublishedAt']").val();
        const $expires = $form.find("[name='ExpiresAt']");
        const expiresVal = $expires.val();
        if (publishedVal && expiresVal && new Date(expiresVal) <= new Date(publishedVal)) {
            setFieldError($expires, "Expiry must be after the publish date.");
            errors.push("Expiry must be after the publish date.");
            isValid = false;
        }

        if (!isValid) showError(errors[0]);
        return isValid;
    }

    function wireFormSubmit() {
        $(document).on("submit", "#announcementForm", function (e) {
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
                        setTimeout(() => window.location.href = "/Announcements/Index", 700);
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

    function wireToggleActive() {
        $(document).on("change", ".toggle-active-switch", function () {
            const $checkbox = $(this);
            const id = $checkbox.data("id");
            const isActive = $checkbox.is(":checked");
            $checkbox.prop("disabled", true);

            $.ajax({
                url: "/Announcements/ToggleActiveAjax",
                type: "POST",
                data: { id: id, isActive: isActive, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                success: function (res) {
                    if (res.success) showSuccess(res.message);
                    else { showError(res.message); $checkbox.prop("checked", !isActive); }
                },
                error: function () {
                    showError("Request failed.");
                    $checkbox.prop("checked", !isActive);
                },
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
                    url: "/Announcements/DeleteAjax/",
                    type: "POST",
                    data: { id: id, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                    success: function (res) {
                        $modal.modal("hide");
                        if (res.success) {
                            showSuccess(res.message || "Deleted.");
                            $("#ann-row-" + id).fadeOut(300, function () { $(this).remove(); });
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
        wireToggleActive();
        wireDelete();
    });
})();