/* Batch Module — client-side logic */
(function () {
    "use strict";

    var IMSBatchForm = {
        init: function (ajaxUrl) {
            $(document).on("submit", "#batchCreateForm, #batchEditForm", function (e) {
                e.preventDefault();
                var $form = $(this);
                if (!validateBatchForm($form)) return;

                var $submitBtn = $form.find("button[type='submit']");
                var originalText = $submitBtn.html();
                $submitBtn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving...');

                $.ajax({
                    url: ajaxUrl,
                    type: "POST",
                    data: $form.serialize(),
                    success: function (response) {
                        if (response.success) {
                            if (window.toastr) toastr.success(response.message || "Saved successfully.");
                            setTimeout(function () { window.location.href = "/Batch/Index"; }, 700);
                        } else {
                            if (window.toastr) toastr.error(response.message || "Something went wrong.");
                            $submitBtn.prop("disabled", false).html(originalText);
                        }
                    },
                    error: function (xhr) {
                        if (window.toastr) toastr.error("Request failed: " + (xhr.responseText || xhr.statusText));
                        $submitBtn.prop("disabled", false).html(originalText);
                    }
                });
            });
        }
    };

    function validateBatchForm($form) {
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find(".field-error").text("");
        var valid = true;

        function required(name, label) {
            var $f = $form.find("[name='" + name + "']");
            if (!$f.val() || !$f.val().toString().trim()) {
                $f.addClass("is-invalid");
                $form.find(".field-error[data-for='" + name + "']").text(label + " is required.");
                valid = false;
            }
        }

        required("BT_BranchId", "Branch");
        required("BT_Name", "Batch Name");
        required("BT_Code", "Batch Code");
        required("BT_CourseId", "Course");
        required("BT_AcademicYearId", "Academic Year");
        required("BT_Status", "Status");

        var startDate = $form.find("[name='BT_StartDate']").val();
        var endDate = $form.find("[name='BT_EndDate']").val();
        if (startDate && endDate) {
            var parseDateVal = function (v) {
                if (typeof IMSDatePicker !== "undefined" && IMSDatePicker.parseDate) {
                    var p = IMSDatePicker.parseDate(v);
                    if (p) return p.getTime();
                }
                var t = Date.parse(v);
                if (!isNaN(t)) return t;
                var parts = v.split(/[\/\-]/);
                if (parts.length === 3) {
                    if (parts[0].length === 4) return new Date(parts[0], parts[1] - 1, parts[2]).getTime();
                    return new Date(parts[2], parts[1] - 1, parts[0]).getTime();
                }
                return null;
            };
            var sTime = parseDateVal(startDate);
            var eTime = parseDateVal(endDate);
            if (sTime !== null && eTime !== null && eTime < sTime) {
                $form.find(".field-error[data-for='BT_EndDate']").text("End date cannot be before start date.");
                valid = false;
            }
        }

        if (!valid && window.toastr) toastr.error("Please correct the highlighted fields.");
        return valid;
    }

    function wireDelete() {
        $(document).on("click", ".confirm-delete-btn", function () {
            var id = $(this).data("id");
            var $modal = $("#deleteModal-" + id);
            $modal.find(".do-delete-btn").off("click").on("click", function () {
                var $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Deleting...');
                $.ajax({
                    url: "/Batch/DeleteBatch/" + id,
                    type: "POST",
                    data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                    success: function (response) {
                        $modal.modal("hide");
                        if (response.success) {
                            if (window.toastr) toastr.success(response.message || "Deleted successfully.");
                            $("#batch-row-" + id).fadeOut(300, function () { $(this).remove(); });
                        } else {
                            if (window.toastr) toastr.error(response.message || "Unable to delete.");
                        }
                    },
                    error: function () { $modal.modal("hide"); if (window.toastr) toastr.error("Request failed."); },
                    complete: function () { $btn.prop("disabled", false).html("Delete"); }
                });
            });
        });
    }

    $(function () {
        wireDelete();
    });

    window.IMSBatchForm = IMSBatchForm;
})();
