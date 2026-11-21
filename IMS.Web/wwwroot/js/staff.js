/* Staff Module — client-side logic */
(function () {
    "use strict";

    var IMSStaffForm = {
        init: function (ajaxUrl) {
            var $form = $("#staffCreateForm, #staffEditForm");

            // Clear errors on input/change
            $form.on("input change", "input, select, textarea", function () {
                var $field = $(this);
                $field.removeClass("is-invalid");
                var name = $field.attr("name") || $field.attr("id");
                if (name) {
                    $form.find(".field-error[data-for='" + name + "']").text("");
                }
            });

            // Form submission
            $form.on("submit", function (e) {
                e.preventDefault();
                var $currentForm = $(this);
                if (!validateStaffForm($currentForm)) return;

                var $submitBtn = $currentForm.find("button[type='submit']");
                var originalText = $submitBtn.html();
                $submitBtn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Saving...');

                $.ajax({
                    url: ajaxUrl,
                    type: "POST",
                    data: $currentForm.serialize(),
                    success: function (response) {
                        if (response.success) {
                            if (window.toastr) toastr.success(response.message || "Staff member saved successfully.");
                            setTimeout(function () { window.location.href = "/Staff/Index"; }, 700);
                        } else {
                            if (response.errors) {
                                renderServerErrors($currentForm, response.errors);
                            }
                            if (window.toastr) toastr.error(response.message || "Please fix the validation errors.");
                            $submitBtn.prop("disabled", false).html(originalText);
                        }
                    },
                    error: function (xhr) {
                        var msg = "Request failed. Please try again.";
                        if (xhr.responseJSON && xhr.responseJSON.message) {
                            msg = xhr.responseJSON.message;
                        } else if (xhr.responseText) {
                            try {
                                var parsed = JSON.parse(xhr.responseText);
                                if (parsed.message) msg = parsed.message;
                            } catch (e) { }
                        }
                        if (window.toastr) toastr.error(msg);
                        $submitBtn.prop("disabled", false).html(originalText);
                    }
                });
            });
        }
    };

    function validateStaffForm($form) {
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find(".field-error").text("");
        var valid = true;

        function setFieldError(name, message) {
            var $f = $form.find("[name='" + name + "'], #" + name);
            $f.addClass("is-invalid");
            $form.find(".field-error[data-for='" + name + "']").text(message);
            valid = false;
        }

        function required(name, label) {
            var $f = $form.find("[name='" + name + "'], #" + name);
            var val = $f.val();
            if (!val || !val.toString().trim()) {
                setFieldError(name, label + " is required.");
            }
        }

        required("ST_FirstName", "First Name");
        required("ST_LastName", "Last Name");
        required("ST_BranchId", "Branch");
        required("ST_EmployeeCode", "Employee Code");
        required("ST_Status", "Status");

        // Validate Email format if present
        var $email = $form.find("[name='ST_Email'], #ST_Email");
        var emailVal = $email.val();
        if (emailVal && emailVal.trim()) {
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(emailVal.trim())) {
                setFieldError("ST_Email", "Please enter a valid email address.");
            }
        }

        // Validate Experience if negative
        var $exp = $form.find("[name='ST_ExperienceYears'], #ST_ExperienceYears");
        var expVal = $exp.val();
        if (expVal && parseInt(expVal, 10) < 0) {
            setFieldError("ST_ExperienceYears", "Experience cannot be negative.");
        }

        // Validate Salary if negative
        var $sal = $form.find("[name='ST_BasicSalary'], #ST_BasicSalary");
        var salVal = $sal.val();
        if (salVal && parseFloat(salVal) < 0) {
            setFieldError("ST_BasicSalary", "Salary cannot be negative.");
        }

        if (!valid && window.toastr) {
            toastr.error("Please fill in all required fields properly.");
        }
        return valid;
    }

    function renderServerErrors($form, errors) {
        if (!errors) return;
        $.each(errors, function (field, msgs) {
            var fieldName = field;
            if (field.indexOf(".") > -1) {
                fieldName = field.split(".").pop();
            }
            var $f = $form.find("[name='" + fieldName + "'], #" + fieldName);
            $f.addClass("is-invalid");
            var errorMsg = Array.isArray(msgs) ? msgs.join(" ") : msgs;
            $form.find(".field-error[data-for='" + fieldName + "']").text(errorMsg);
        });
    }

    function wireDelete() {
        $(document).on("click", ".confirm-delete-btn", function () {
            var id = $(this).data("id");
            var $modal = $("#deleteModal-" + id);
            $modal.find(".do-delete-btn").off("click").on("click", function () {
                var $btn = $(this);
                $btn.prop("disabled", true).html('<span class="spinner-border spinner-border-sm me-1"></span> Deleting...');
                $.ajax({
                    url: "/Staff/DeleteStaff/" + id,
                    type: "POST",
                    data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                    success: function (r) {
                        if (r.success) {
                            if (window.toastr) toastr.success(r.message || "Staff member deleted.");
                            setTimeout(function () { $("#staff-row-" + id).fadeOut(300, function () { $(this).remove(); }); }, 500);
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

    window.IMSStaffForm = IMSStaffForm;
    $(function () { wireDelete(); });
})();
