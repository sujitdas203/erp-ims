/*!
 * ===========================================================
 * Generic Dropdown Framework
 * Project : IMS
 * Author  : IMS
 * ===========================================================
 */

var Dropdown = (function () {

    /**
     * Helper to resolve property value with multiple casing fallbacks
     */
    function getProp(obj, props) {
        if (!obj) return undefined;
        for (var i = 0; i < props.length; i++) {
            if (obj[props[i]] !== undefined && obj[props[i]] !== null) {
                return obj[props[i]];
            }
        }
        return undefined;
    }

    /**
     * Load Generic Dropdown
     */
    function load(options) {

        var settings = $.extend({
            element: null,
            entityType: null,
            parentId: null,
            search: null,
            activeOnly: true,
            page: 1,
            pageSize: 100,
            selectedValue: null,
            includeDefault: true,
            defaultText: "-- Select --",
            async: true,
            success: null,
            error: null
        }, options);

        if (!settings.element)
            return;

        var ddl = $(settings.element);
        if (ddl.length === 0)
            return;

        // Auto-detect selectedValue from data-selected-value if not explicitly passed
        if (settings.selectedValue === null || settings.selectedValue === undefined || settings.selectedValue === "") {
            var dataVal = ddl.attr("data-selected-value") || ddl.data("selected-value");
            if (dataVal !== undefined && dataVal !== null && dataVal !== "") {
                settings.selectedValue = dataVal;
            }
        }

        $.ajax({
            url: "/Dropdown/GetDropdown",
            type: "GET",
            dataType: "json",
            async: settings.async,
            data: {
                entityType: settings.entityType,
                parentId: settings.parentId,
                search: settings.search,
                activeOnly: settings.activeOnly,
                page: settings.page,
                pageSize: settings.pageSize
            },
            success: function (response) {

                ddl.empty();

                if (settings.includeDefault) {
                    ddl.append(
                        $("<option>")
                            .val("")
                            .text(settings.defaultText)
                    );
                }

                // Handle both PascalCase and camelCase response structures
                var isSuccess = false;
                var rawData = null;

                if (response) {
                    if (response.Success === true || response.success === true) {
                        isSuccess = true;
                        rawData = response.Data !== undefined ? response.Data : response.data;
                    } else if (response.Data !== undefined || response.data !== undefined) {
                        isSuccess = true;
                        rawData = response.Data !== undefined ? response.Data : response.data;
                    } else if (Array.isArray(response)) {
                        isSuccess = true;
                        rawData = response;
                    }
                }

                if (isSuccess && Array.isArray(rawData)) {

                    $.each(rawData, function (i, item) {

                        var val = getProp(item, ["Value", "value", "Id", "id", "Key", "key"]);
                        var txt = getProp(item, ["Text", "text", "Name", "name", "Title", "title"]);
                        var code = getProp(item, ["Code", "code"]);
                        var parentId = getProp(item, ["ParentId", "parentId"]);

                        val = val !== undefined ? String(val) : "";
                        txt = txt !== undefined ? String(txt) : "";

                        var opt = $("<option>")
                            .val(val)
                            .text(txt);

                        if (code) {
                            opt.attr("data-code", code);
                        }
                        if (parentId) {
                            opt.attr("data-parent", parentId);
                        }

                        ddl.append(opt);

                    });

                    // Handle selected value (case-insensitive and type-tolerant)
                    if (settings.selectedValue !== null && settings.selectedValue !== undefined && settings.selectedValue !== "") {
                        var targetVal = String(settings.selectedValue).trim();
                        var matched = false;

                        // Try direct match first
                        ddl.val(targetVal);

                        if (ddl.val() === targetVal) {
                            matched = true;
                        }

                        // Try case-insensitive / GUID match
                        if (!matched) {
                            var lowerTarget = targetVal.toLowerCase();
                            ddl.find("option").each(function () {
                                var optVal = $(this).val();
                                if (optVal && String(optVal).trim().toLowerCase() === lowerTarget) {
                                    ddl.val(optVal);
                                    matched = true;
                                    return false;
                                }
                            });
                        }
                    }

                    // Sync Select2 if present
                    if (ddl.hasClass("select2-hidden-accessible")) {
                        ddl.trigger("change.select2");
                    }

                    // Trigger standard change event
                    ddl.trigger("change");

                    if ($.isFunction(settings.success)) {
                        settings.success(rawData);
                    }

                } else {
                    if (response && (response.Message || response.message)) {
                        console.warn("Dropdown load warning: " + (response.Message || response.message));
                    }
                }

            },
            error: function (xhr, status, error) {

                console.error("Dropdown load error for entity: " + settings.entityType, xhr.responseText || error);

                if ($.isFunction(settings.error)) {
                    settings.error(xhr, status, error);
                }

            }
        });

    }

    /**
     * Cascading Dropdown
     */
    function cascade(options) {

        var parent = $(options.parent);

        parent.off("change.dropdown");

        parent.on("change.dropdown", function () {

            var parentVal = $(this).val();

            if (parentVal && parentVal !== "") {

                load({
                    element: options.child,
                    entityType: options.entityType,
                    parentId: parentVal,
                    defaultText: options.defaultText || "-- Select --",
                    includeDefault: true,
                    selectedValue: options.selectedValue || null,
                    success: options.success
                });

            } else {

                clear(options.child, options.defaultText);

                if (options.grandchild) {
                    clear(options.grandchild, options.grandchildDefaultText);
                }

            }

        });

    }

    /**
     * Clear Dropdown
     */
    function clear(element, defaultText) {

        var ddl = $(element);

        ddl.empty();

        ddl.append(
            $("<option>")
                .val("")
                .text(defaultText || "-- Select --")
        );

        if (ddl.hasClass("select2-hidden-accessible")) {
            ddl.trigger("change.select2");
        }

        ddl.trigger("change");

    }

    /**
     * Reload Dropdown
     */
    function reload(options) {

        load(options);

    }

    /**
     * Select2 Support
     */
    function loadSelect2(options) {

        var originalSuccess = options.success;

        load($.extend({}, options, {

            success: function (data) {

                var $el = $(options.element);
                var modalParent = $el.closest('.modal');
                var defaultOpts = {
                    theme: "bootstrap-5",
                    width: "100%",
                    placeholder: options.defaultText || "-- Select --",
                    allowClear: true
                };

                if (modalParent.length > 0) {
                    defaultOpts.dropdownParent = modalParent;
                }

                var finalOpts = $.extend(defaultOpts, options.select2Options || {});

                if ($el.hasClass("select2-hidden-accessible")) {
                    $el.trigger("change.select2");
                } else {
                    $el.select2(finalOpts);
                }

                if ($.isFunction(originalSuccess)) {
                    originalSuccess(data);
                }

            }

        }));

    }

    // Auto-focus search input inside Select2 dropdown when opened (especially inside modals)
    $(document).on('select2:open', function () {
        window.setTimeout(function () {
            var searchField = document.querySelector('.select2-container--open .select2-search__field');
            if (searchField) {
                searchField.focus();
            }
        }, 50);
    });

    return {
        load: load,
        reload: reload,
        clear: clear,
        cascade: cascade,
        loadSelect2: loadSelect2
    };

})();