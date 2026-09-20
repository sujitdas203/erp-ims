/* Fees & Payments Module — client-side logic */
(function () {
    "use strict";

    var IMSFeesForm = {
        init: function (ajaxUrl) {
            var $form = $("#feeStructureForm");
            if (!$form.length) return;

            // 1. Initialize main generic dropdowns
            if ($("#FS_AcademicYearId option").length <= 1) {
                Dropdown.load({
                    element: "#FS_AcademicYearId",
                    entityType: "AcademicYear",
                    defaultText: "-- Select Academic Year --",
                    includeDefault: true,
                    selectedValue: $("#FS_AcademicYearId").attr("data-selected-value") || $("#FS_AcademicYearId").data("selected-value")
                });
            }

            if ($("#FS_CourseId option").length <= 1) {
                Dropdown.load({
                    element: "#FS_CourseId",
                    entityType: "Course",
                    defaultText: "-- Select Course (Optional) --",
                    includeDefault: true,
                    selectedValue: $("#FS_CourseId").attr("data-selected-value") || $("#FS_CourseId").data("selected-value"),
                    success: function () {
                        var courseVal = $("#FS_CourseId").val();
                        if ($("#FS_BatchId option").length <= 1) {
                            loadBatchDropdown(courseVal);
                        }
                    }
                });
            }

            $("#FS_CourseId").off("change.fees").on("change.fees", function () {
                var courseVal = $(this).val();
                loadBatchDropdown(courseVal);
            });

            function loadBatchDropdown(courseId) {
                Dropdown.load({
                    element: "#FS_BatchId",
                    entityType: "Batch",
                    parentId: courseId || null,
                    defaultText: "-- Select Batch (Optional) --",
                    includeDefault: true,
                    selectedValue: $("#FS_BatchId").attr("data-selected-value") || $("#FS_BatchId").data("selected-value")
                });
            }

            if ($("#FS_BatchId option").length <= 1) {
                var currentCourse = $("#FS_CourseId").attr("data-selected-value") || $("#FS_CourseId").data("selected-value");
                loadBatchDropdown(currentCourse);
            }

            // 2. Initialize existing Fee Category dropdowns
            initAllFeeCategoryDropdowns();

            // Calculate total amount on init
            calculateTotal();

            // 3. Clear errors on input/change
            $form.on("input change", "input, select, textarea", function () {
                var $field = $(this);
                $field.removeClass("is-invalid");
                var name = $field.attr("name") || $field.attr("id");
                if (name) {
                    $form.find(".field-error[data-for='" + name + "']").text("");
                }
            });

            // 4. Live amount total calculation
            $(document).on("input", ".fee-amount-input", function () {
                calculateTotal();
            });

            // 5. Add Fee Item Row
            $("#addFeeItemBtn").off("click").on("click", function (e) {
                e.preventDefault();
                addFeeItemRow();
            });

            // 6. Remove Fee Item Row
            $(document).on("click", ".remove-fee-item", function (e) {
                e.preventDefault();
                var $row = $(this).closest(".fee-item-row");
                if ($("#feeItemsContainer .fee-item-row").length <= 1) {
                    if (window.toastr) toastr.warning("Fee structure must contain at least one item.");
                    return;
                }
                $row.remove();
                reindexFeeItems();
                calculateTotal();
            });

            // 7. Form Submission
            $form.off("submit").on("submit", function (e) {
                e.preventDefault();
                if (!validateFeeStructureForm($form)) return;

                var $btn = $form.find("button[type='submit']");
                var originalText = $btn.html();
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving...');

                $.ajax({
                    url: ajaxUrl || $form.attr("action"),
                    type: "POST",
                    data: $form.serialize(),
                    success: function (r) {
                        if (r.success) {
                            if (window.toastr) toastr.success(r.message || "Fee structure saved successfully.");
                            setTimeout(function () {
                                window.location.href = "/Fees/Index";
                            }, 700);
                        } else {
                            if (window.toastr) toastr.error(r.message || "Please correct the form errors.");
                            $btn.prop("disabled", false).html(originalText);
                        }
                    },
                    error: function (xhr) {
                        var msg = "Request failed. Please try again.";
                        if (xhr.responseJSON && xhr.responseJSON.message) {
                            msg = xhr.responseJSON.message;
                        }
                        if (window.toastr) toastr.error(msg);
                        $btn.prop("disabled", false).html(originalText);
                    }
                });
            });
        }
    };

    function initAllFeeCategoryDropdowns() {
        $("#feeItemsContainer .fee-category-select").each(function () {
            var $sel = $(this);
            if ($sel.find("option").length <= 1) {
                var selectedVal = $sel.attr("data-selected-value") || $sel.data("selected-value");
                Dropdown.load({
                    element: this,
                    entityType: "FeeCategory",
                    defaultText: "-- Select Category --",
                    includeDefault: true,
                    selectedValue: selectedVal
                });
            }
        });
    }

    function addFeeItemRow() {
        var nextIdx = $("#feeItemsContainer .fee-item-row").length;
        var html = '<tr class="fee-item-row" data-index="' + nextIdx + '">'
            + '<td>'
            + '  <select name="Items[' + nextIdx + '].FSI_FeeCategoryId" class="form-select form-select-sm fee-category-select">'
            + '    <option value="">-- Select Category --</option>'
            + '  </select>'
            + '  <div class="text-danger small field-error" data-for="Items[' + nextIdx + '].FSI_FeeCategoryId"></div>'
            + '</td>'
            + '<td>'
            + '  <div class="input-group input-group-sm">'
            + '    <span class="input-group-text"><i class="fa-solid fa-indian-rupee-sign"></i></span>'
            + '    <input type="number" name="Items[' + nextIdx + '].FSI_Amount" class="form-control form-control-sm fee-amount-input" min="0" step="0.01" value="" placeholder="0.00" />'
            + '  </div>'
            + '  <div class="text-danger small field-error" data-for="Items[' + nextIdx + '].FSI_Amount"></div>'
            + '</td>'
            + '<td>'
            + '  <input type="number" name="Items[' + nextIdx + '].FSI_DueDays" class="form-control form-control-sm" min="0" value="0" placeholder="e.g. 15" />'
            + '</td>'
            + '<td>'
            + '  <select name="Items[' + nextIdx + '].FSI_IsMandatory" class="form-select form-select-sm mandatory-select">'
            + '    <option value="true" selected>Mandatory</option>'
            + '    <option value="false">Optional</option>'
            + '  </select>'
            + '</td>'
            + '<td class="text-center">'
            + '  <button type="button" class="btn btn-sm btn-outline-danger remove-fee-item" title="Remove Item">'
            + '    <i class="fa-solid fa-trash-can"></i>'
            + '  </button>'
            + '</td>'
            + '</tr>';

        $("#feeItemsContainer").append(html);
        var $newRow = $("#feeItemsContainer .fee-item-row:last");
        Dropdown.load({
            element: $newRow.find(".fee-category-select")[0],
            entityType: "FeeCategory",
            defaultText: "-- Select Category --",
            includeDefault: true
        });
        calculateTotal();
    }

    function reindexFeeItems() {
        $("#feeItemsContainer .fee-item-row").each(function (idx) {
            var $row = $(this);
            $row.attr("data-index", idx);

            $row.find("input[name*='.FSI_Id']").attr("name", "Items[" + idx + "].FSI_Id");

            var $cat = $row.find(".fee-category-select");
            $cat.attr("name", "Items[" + idx + "].FSI_FeeCategoryId");
            $row.find(".field-error[data-for*='FSI_FeeCategoryId']").attr("data-for", "Items[" + idx + "].FSI_FeeCategoryId");

            var $amt = $row.find(".fee-amount-input");
            $amt.attr("name", "Items[" + idx + "].FSI_Amount");
            $row.find(".field-error[data-for*='FSI_Amount']").attr("data-for", "Items[" + idx + "].FSI_Amount");

            var $due = $row.find("input[name*='.FSI_DueDays']");
            $due.attr("name", "Items[" + idx + "].FSI_DueDays");

            var $mand = $row.find(".mandatory-select");
            $mand.attr("name", "Items[" + idx + "].FSI_IsMandatory");
        });
    }

    function calculateTotal() {
        var total = 0;
        $(".fee-amount-input").each(function () {
            var val = parseFloat($(this).val());
            if (!isNaN(val) && val > 0) {
                total += val;
            }
        });
        $("#totalFeeAmount").text(total.toLocaleString("en-IN", { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    }

    function validateFeeStructureForm($form) {
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find(".field-error").text("");
        var valid = true;

        function setFieldError(name, msg) {
            var $f = $form.find("[name='" + name + "'], #" + name);
            $f.addClass("is-invalid");
            $form.find(".field-error[data-for='" + name + "']").text(msg);
            valid = false;
        }

        var nameVal = $("#FS_Name").val();
        if (!nameVal || !nameVal.trim()) {
            setFieldError("FS_Name", "Fee Structure Name is required.");
        }

        var codeVal = $("#FS_Code").val();
        if (!codeVal || !codeVal.trim()) {
            setFieldError("FS_Code", "Fee Structure Code is required.");
        }

        var ayVal = $("#FS_AcademicYearId").val();
        if (!ayVal || !ayVal.trim()) {
            setFieldError("FS_AcademicYearId", "Academic Year is required.");
        }

        var $rows = $("#feeItemsContainer .fee-item-row");
        if ($rows.length === 0) {
            if (window.toastr) toastr.error("Please add at least one fee item.");
            valid = false;
        } else {
            $rows.each(function (idx) {
                var $r = $(this);
                var $cat = $r.find(".fee-category-select");
                var catVal = $cat.val();
                if (!catVal || !catVal.trim()) {
                    $cat.addClass("is-invalid");
                    $r.find(".field-error[data-for='Items[" + idx + "].FSI_FeeCategoryId']").text("Category is required.");
                    valid = false;
                }

                var $amt = $r.find(".fee-amount-input");
                var amtVal = parseFloat($amt.val());
                if (isNaN(amtVal) || amtVal < 0) {
                    $amt.addClass("is-invalid");
                    $r.find(".field-error[data-for='Items[" + idx + "].FSI_Amount']").text("Valid amount is required.");
                    valid = false;
                }
            });
        }

        if (!valid && window.toastr) {
            toastr.error("Please fill in all required fields.");
        }
        return valid;
    }

    function wireDelete() {
        $(document).on("click", ".confirm-delete-btn", function () {
            var id = $(this).data("id");
            var $modal = $("#deleteModal-" + id);
            $modal.find(".do-delete-btn").off("click").on("click", function () {
                var $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Deleting...');
                var deleteUrl = "/Fees/DeleteFeeStructure/" + id;
                if (window.location.pathname.indexOf("/Invoices") > -1) deleteUrl = "/Fees/DeleteInvoice/" + id;
                else if (window.location.pathname.indexOf("/Payments") > -1) deleteUrl = "/Fees/DeletePayment/" + id;
                $.ajax({
                    url: deleteUrl,
                    type: "POST",
                    data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                    success: function (r) {
                        if (r.success) {
                            if (window.toastr) toastr.success(r.message || "Deleted.");
                            setTimeout(function () {
                                $("[id$='row-" + id + "']").fadeOut(300, function () { $(this).remove(); });
                            }, 500);
                        } else {
                            if (window.toastr) toastr.error(r.message || "Delete failed.");
                        }
                        $btn.prop("disabled", false).html("Delete");
                        $modal.modal("hide");
                    },
                    error: function () {
                        if (window.toastr) toastr.error("Request failed.");
                        $btn.prop("disabled", false).html("Delete");
                        $modal.modal("hide");
                    }
                });
            });
        });
    }

    window.IMSFeesForm = IMSFeesForm;
    $(function () {
        wireDelete();
    });
})();
