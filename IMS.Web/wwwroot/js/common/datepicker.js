/*!
 * ===========================================================
 * Generic Date Picker Framework
 * Project : IMS
 * Author  : IMS
 *
 * Usage:
 *   IMSDatePicker.init({
 *       input: '#field_AY_StartDate',
 *       format: 'dd/MM/yyyy',       // display format
 *       valueFormat: 'yyyy-MM-dd',   // value sent to server
 *       placeholder: 'DD/MM/YYYY',
 *       allowFutureDates: true,
 *       allowPastDates: true,
 *       minDate: null,
 *       maxDate: null,
 *       required: true,
 *       onChange: null
 *   });
 *
 *   IMSDatePicker.getValue('#field_AY_StartDate')  -> '2026-08-29'
 *   IMSDatePicker.setValue('#field_AY_StartDate', '2026-08-29')
 *   IMSDatePicker.clear('#field_AY_StartDate')
 * ===========================================================
 */

var IMSDatePicker = (function () {

    var _instances = {};

    var MONTHS = [
        'January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'
    ];

    var DAYS_SHORT = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];

    // ---- Helpers ----

    function parseDate(str) {
        if (!str) return null;
        if (str instanceof Date) return isNaN(str.getTime()) ? null : str;

        // Try native Date parser first (handles ISO formats like "2026-08-29T10:30:00", "2026-08-29", etc.)
        if (typeof str === 'string') {
            var trimmed = str.trim();
            var native = new Date(trimmed);
            if (!isNaN(native.getTime())) {
                return native;
            }
        }

        // Manual parsing for dd/MM/yyyy HH:mm or dd/MM/yyyy format
        var parts = str.trim().split(' ');
        var dateParts = parts[0].split('/');
        var timeParts = parts.length > 1 ? parts[1].split(':') : null;
        if (dateParts.length === 3) {
            var d = parseInt(dateParts[0], 10);
            var m = parseInt(dateParts[1], 10) - 1;
            var y = parseInt(dateParts[2], 10);
            var hh = timeParts && timeParts.length >= 1 ? parseInt(timeParts[0], 10) : 0;
            var mm = timeParts && timeParts.length >= 2 ? parseInt(timeParts[1], 10) : 0;
            if (!isNaN(d) && !isNaN(m) && !isNaN(y)) {
                return new Date(y, m, d, isNaN(hh) ? 0 : hh, isNaN(mm) ? 0 : mm, 0);
            }
        }

        return null;
    }

    function formatDate(date, fmt) {
        if (!date) return '';
        var dd = String(date.getDate()).padStart(2, '0');
        var mm = String(date.getMonth() + 1).padStart(2, '0');
        var yyyy = date.getFullYear();
        var hh = String(date.getHours()).padStart(2, '0');
        var min = String(date.getMinutes()).padStart(2, '0');

        if (fmt === 'yyyy-MM-ddTHH:mm') return yyyy + '-' + mm + '-' + dd + 'T' + hh + ':' + min;
        if (fmt === 'yyyy-MM-dd HH:mm' || fmt === 'yyyy-MM-dd HH:mm:ss') return yyyy + '-' + mm + '-' + dd + ' ' + hh + ':' + min;
        if (fmt === 'yyyy-MM-dd') return yyyy + '-' + mm + '-' + dd;
        if (fmt === 'dd/MM/yyyy HH:mm') return dd + '/' + mm + '/' + yyyy + ' ' + hh + ':' + min;
        return dd + '/' + mm + '/' + yyyy;
    }

    function today() {
        var d = new Date();
        return new Date(d.getFullYear(), d.getMonth(), d.getDate());
    }

    function isSameDay(a, b) {
        return a && b &&
            a.getFullYear() === b.getFullYear() &&
            a.getMonth() === b.getMonth() &&
            a.getDate() === b.getDate();
    }

    function getDaysInMonth(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    function getFirstDayOfMonth(year, month) {
        var day = new Date(year, month, 1).getDay();
        return day === 0 ? 6 : day - 1; // Monday=0
    }

    // ---- Calendar Rendering ----

    function renderCalendar(inst) {
        var cal = inst.calendarEl;
        var year = inst.viewYear;
        var month = inst.viewMonth;

        var html = '';

        // Header: month/year + nav
        html += '<div class="ims-dp-header">';
        html += '<button type="button" class="ims-dp-nav ims-dp-prev-month" aria-label="Previous month">&lsaquo;</button>';
        html += '<span class="ims-dp-month-year">';
        html += '<select class="ims-dp-month-select" aria-label="Month">';
        for (var m = 0; m < 12; m++) {
            html += '<option value="' + m + '"' + (m === month ? ' selected' : '') + '>' + MONTHS[m] + '</option>';
        }
        html += '</select> ';
        html += '<select class="ims-dp-year-select" aria-label="Year">';
        var minYear = inst.minDate ? inst.minDate.getFullYear() : year - 100;
        var maxYear = inst.maxDate ? inst.maxDate.getFullYear() : year + 50;
        for (var y = minYear; y <= maxYear; y++) {
            html += '<option value="' + y + '"' + (y === year ? ' selected' : '') + '>' + y + '</option>';
        }
        html += '</select>';
        html += '</span>';
        html += '<button type="button" class="ims-dp-nav ims-dp-next-month" aria-label="Next month">&rsaquo;</button>';
        html += '</div>';

        // Day headers
        html += '<div class="ims-dp-weekdays">';
        for (var d = 0; d < 7; d++) {
            html += '<span class="ims-dp-weekday">' + DAYS_SHORT[d] + '</span>';
        }
        html += '</div>';

        // Days grid
        var daysInMonth = getDaysInMonth(year, month);
        var firstDay = getFirstDayOfMonth(year, month);
        var selectedDate = inst.selectedDate;
        var todayDate = today();

        html += '<div class="ims-dp-days">';

        // Empty cells before first day
        for (var e = 0; e < firstDay; e++) {
            html += '<span class="ims-dp-day ims-dp-empty"></span>';
        }

        for (var day = 1; day <= daysInMonth; day++) {
            var dayDate = new Date(year, month, day);
            var classes = ['ims-dp-day'];

            // Check if day is disabled
            var disabled = false;
            if (!inst.allowFutureDates && dayDate > todayDate) disabled = true;
            if (!inst.allowPastDates && dayDate < todayDate) disabled = true;
            if (inst.minDate && dayDate < inst.minDate) disabled = true;
            if (inst.maxDate && dayDate > inst.maxDate) disabled = true;

            if (disabled) classes.push('ims-dp-disabled');
            if (isSameDay(dayDate, todayDate)) classes.push('ims-dp-today');
            if (isSameDay(dayDate, selectedDate)) classes.push('ims-dp-selected');

            html += '<button type="button" class="' + classes.join(' ') + '"'
                + ' data-day="' + day + '"'
                + (disabled ? ' tabindex="-1" aria-disabled="true"' : '')
                + ' aria-label="' + day + ' ' + MONTHS[month] + ' ' + year + '"'
                + '>' + day + '</button>';
        }

        html += '</div>';

        if (inst.showTime) {
            var curHour = inst.selectedDate ? inst.selectedDate.getHours() : 0;
            var curMin = inst.selectedDate ? inst.selectedDate.getMinutes() : 0;

            html += '<div class="ims-dp-time">';
            html += '<label class="ims-dp-time-label"><i class="fa fa-clock-o me-1"></i>Time:</label>';
            html += '<select class="ims-dp-time-hour" aria-label="Hour">';
            for (var h = 0; h < 24; h++) {
                var hStr = String(h).padStart(2, '0');
                html += '<option value="' + h + '"' + (h === curHour ? ' selected' : '') + '>' + hStr + '</option>';
            }
            html += '</select>';
            html += '<span class="ims-dp-time-sep">:</span>';
            html += '<select class="ims-dp-time-min" aria-label="Minute">';
            for (var min = 0; min < 60; min += 5) {
                var minStr = String(min).padStart(2, '0');
                html += '<option value="' + min + '"' + (min === Math.floor(curMin / 5) * 5 ? ' selected' : '') + '>' + minStr + '</option>';
            }
            html += '</select>';
            html += '</div>';
        }

        // Footer: Today/Now + Apply + Clear
        html += '<div class="ims-dp-footer">';
        html += '<button type="button" class="ims-dp-btn ims-dp-today-btn">' + (inst.showTime ? 'Now' : 'Today') + '</button>';
        if (inst.showTime) {
            html += '<button type="button" class="ims-dp-btn ims-dp-apply-btn">Apply</button>';
        }
        html += '<button type="button" class="ims-dp-btn ims-dp-clear-btn">Clear</button>';
        html += '</div>';

        cal.html(html);

        // Bind calendar events
        cal.find('.ims-dp-day:not(.ims-dp-disabled):not(.ims-dp-empty)').on('click', function () {
            var day = parseInt($(this).data('day'));
            selectDate(inst, new Date(year, month, day), !inst.showTime);
            if (inst.showTime) renderCalendar(inst);
        });

        cal.find('.ims-dp-prev-month').on('click', function () {
            navigateMonth(inst, -1);
        });

        cal.find('.ims-dp-next-month').on('click', function () {
            navigateMonth(inst, 1);
        });

        cal.find('.ims-dp-month-select').on('change', function () {
            inst.viewMonth = parseInt($(this).val());
            renderCalendar(inst);
        });

        cal.find('.ims-dp-year-select').on('change', function () {
            inst.viewYear = parseInt($(this).val());
            renderCalendar(inst);
        });

        cal.find('.ims-dp-time-hour, .ims-dp-time-min').on('change', function () {
            var hour = parseInt(cal.find('.ims-dp-time-hour').val(), 10) || 0;
            var minute = parseInt(cal.find('.ims-dp-time-min').val(), 10) || 0;
            var baseDate = inst.selectedDate || today();
            var updated = new Date(baseDate.getFullYear(), baseDate.getMonth(), baseDate.getDate(), hour, minute, 0);
            updateSelection(inst, updated, false);
        });

        cal.find('.ims-dp-apply-btn').on('click', function () {
            if (!inst.selectedDate) {
                var hour = parseInt(cal.find('.ims-dp-time-hour').val(), 10) || 0;
                var minute = parseInt(cal.find('.ims-dp-time-min').val(), 10) || 0;
                var t = today();
                selectDate(inst, new Date(t.getFullYear(), t.getMonth(), t.getDate(), hour, minute, 0), true);
            } else {
                hideCalendar(inst);
            }
        });

        cal.find('.ims-dp-today-btn').on('click', function () {
            var now = new Date();
            selectDate(inst, now, !inst.showTime);
            if (inst.showTime) renderCalendar(inst);
        });

        cal.find('.ims-dp-clear-btn').on('click', function () {
            clearValue(inst);
        });
    }

    function navigateMonth(inst, delta) {
        inst.viewMonth += delta;
        if (inst.viewMonth > 11) {
            inst.viewMonth = 0;
            inst.viewYear++;
        } else if (inst.viewMonth < 0) {
            inst.viewMonth = 11;
            inst.viewYear--;
        }
        renderCalendar(inst);
    }

    function selectDate(inst, date, shouldHide) {
        if (shouldHide === undefined) shouldHide = !inst.showTime;
        if (inst.showTime) {
            var hour = inst.calendarEl.find('.ims-dp-time-hour').length
                ? (parseInt(inst.calendarEl.find('.ims-dp-time-hour').val(), 10) || 0)
                : (inst.selectedDate ? inst.selectedDate.getHours() : date.getHours());
            var minute = inst.calendarEl.find('.ims-dp-time-min').length
                ? (parseInt(inst.calendarEl.find('.ims-dp-time-min').val(), 10) || 0)
                : (inst.selectedDate ? inst.selectedDate.getMinutes() : date.getMinutes());
            date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), hour, minute, 0);
        }
        updateSelection(inst, date, shouldHide);
    }

    function updateSelection(inst, date, shouldHide) {
        inst.selectedDate = date;
        inst.inputEl.val(formatDate(date, inst.valueFormat));
        inst.displayEl.text(formatDate(date, inst.format));
        inst.displayEl.removeClass('ims-dp-placeholder');
        if (shouldHide) {
            hideCalendar(inst);
        }
        inst.inputEl.trigger('change');
        if (typeof inst.onChange === 'function') {
            inst.onChange(formatDate(date, inst.valueFormat), date);
        }
    }

    function clearValue(inst) {
        inst.selectedDate = null;
        inst.inputEl.val('');
        inst.displayEl.text(inst.placeholder);
        inst.displayEl.addClass('ims-dp-placeholder');
        hideCalendar(inst);
        inst.inputEl.trigger('change');
        if (typeof inst.onChange === 'function') {
            inst.onChange('', null);
        }
    }

    function showCalendar(inst) {
        // Close any other open calendars
        Object.keys(_instances).forEach(function (key) {
            if (key !== inst.inputId) hideCalendar(_instances[key]);
        });

        inst.calendarEl.addClass('ims-dp-open');
        inst.isOpen = true;

        // Set initial view to selected date or today
        if (inst.selectedDate) {
            inst.viewYear = inst.selectedDate.getFullYear();
            inst.viewMonth = inst.selectedDate.getMonth();
        } else {
            var t = today();
            inst.viewYear = t.getFullYear();
            inst.viewMonth = t.getMonth();
        }

        renderCalendar(inst);

        // Smart position calendar
        var inputRect = inst.wrapperEl[0].getBoundingClientRect();
        var calHeight = 330;
        var calWidth = 280;
        var spaceBelow = window.innerHeight - inputRect.bottom;
        var spaceRight = window.innerWidth - inputRect.left;

        var cssPos = {
            'z-index': 1070
        };

        // If not enough space below, flip upwards
        if (spaceBelow < calHeight && inputRect.top > calHeight) {
            cssPos.top = 'auto';
            cssPos.bottom = '100%';
            cssPos.marginTop = '0';
            cssPos.marginBottom = '4px';
        } else {
            cssPos.top = '100%';
            cssPos.bottom = 'auto';
            cssPos.marginTop = '4px';
            cssPos.marginBottom = '0';
        }

        // If right side of screen overflows, align right
        if (spaceRight < calWidth) {
            cssPos.left = 'auto';
            cssPos.right = '0';
        } else {
            cssPos.left = '0';
            cssPos.right = 'auto';
        }

        inst.calendarEl.css(cssPos);
    }

    function hideCalendar(inst) {
        inst.calendarEl.removeClass('ims-dp-open');
        inst.isOpen = false;
    }

    function toggleCalendar(inst) {
        if (inst.isOpen) {
            hideCalendar(inst);
        } else {
            showCalendar(inst);
        }
    }

    // ---- Public API ----

    function init(options) {
        var settings = $.extend({
            input: null,
            format: 'dd/MM/yyyy',
            valueFormat: 'yyyy-MM-dd',
            placeholder: 'DD/MM/YYYY',
            allowFutureDates: true,
            allowPastDates: true,
            minDate: null,
            maxDate: null,
            required: false,
            showTime: false,
            onChange: null
        }, options);

        var $input = $(settings.input);
        if (!$input.length) return;

        var inputId = $input.attr('id') || $input.data('column') || 'dp_' + Math.random().toString(36).substr(2, 9);

        // Build DOM wrapper
        var $wrapper = $('<div class="ims-datepicker-wrapper"></div>');
        var $display = $('<span class="ims-dp-display"></span>');
        var $icon = $('<button type="button" class="ims-dp-icon" aria-label="Open date picker"><i class="fa fa-calendar"></i></button>');
        var $calendar = $('<div class="ims-dp-calendar" role="dialog" aria-label="Date picker"></div>');
        var $error = $('<div class="ims-dp-error" role="alert"></div>');

        $input.attr('type', 'hidden');
        $input.addClass('master-field');
        $input.before($wrapper);
        $wrapper.append($input).append($display).append($icon).append($calendar).append($error);

        // Parse initial value
        var initialDate = null;
        var initialValue = $input.val();
        if (initialValue) {
            initialDate = parseDate(initialValue);
            if (!initialDate) {
                // Try parsing yyyy-MM-dd format
                initialDate = parseDate(initialValue);
            }
        }

        // Parse min/max dates
        var minDate = settings.minDate ? parseDate(settings.minDate) : null;
        var maxDate = settings.maxDate ? parseDate(settings.maxDate) : null;

        var inst = {
            inputId: inputId,
            inputEl: $input,
            wrapperEl: $wrapper,
            displayEl: $display,
            calendarEl: $calendar,
            errorEl: $error,
            iconEl: $icon,
            format: settings.format,
            valueFormat: settings.valueFormat,
            placeholder: settings.placeholder,
            allowFutureDates: settings.allowFutureDates,
            allowPastDates: settings.allowPastDates,
            minDate: minDate,
            maxDate: maxDate,
            required: settings.required,
            showTime: settings.showTime,
            onChange: settings.onChange,
            selectedDate: initialDate,
            viewYear: initialDate ? initialDate.getFullYear() : today().getFullYear(),
            viewMonth: initialDate ? initialDate.getMonth() : today().getMonth(),
            isOpen: false
        };

        _instances[inputId] = inst;

        // Set initial display
        if (initialDate) {
            $display.text(formatDate(initialDate, settings.format));
            $display.removeClass('ims-dp-placeholder');
        } else {
            $display.text(settings.placeholder);
            $display.addClass('ims-dp-placeholder');
        }

        // Events
        $display.on('click', function () {
            toggleCalendar(inst);
        });

        $icon.on('click', function (e) {
            e.stopPropagation();
            toggleCalendar(inst);
        });

        // Keyboard support on display
        $display.on('keydown', function (e) {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                toggleCalendar(inst);
            } else if (e.key === 'Escape') {
                hideCalendar(inst);
            } else if (e.key === 'ArrowLeft') {
                e.preventDefault();
                if (!inst.isOpen) showCalendar(inst);
                navigateMonth(inst, -1);
            } else if (e.key === 'ArrowRight') {
                e.preventDefault();
                if (!inst.isOpen) showCalendar(inst);
                navigateMonth(inst, 1);
            }
        });

        // Close on outside click
        $(document).on('click.imsdp_' + inputId, function (e) {
            if (inst.isOpen && !$wrapper[0].contains(e.target)) {
                hideCalendar(inst);
            }
        });

        // Prevent calendar clicks from closing
        $calendar.on('click', function (e) {
            e.stopPropagation();
        });

        // Set aria attributes
        $display.attr({
            'role': 'combobox',
            'aria-expanded': 'false',
            'aria-haspopup': 'dialog',
            'tabindex': '0'
        });
    }

    function getValue(inputSelector) {
        var inst = _instances[typeof inputSelector === 'string'
            ? inputSelector.replace('#', '')
            : inputSelector];
        return inst ? inst.inputEl.val() : '';
    }

    function setValue(inputSelector, dateStr) {
        var key = typeof inputSelector === 'string'
            ? inputSelector.replace('#', '')
            : inputSelector;
        var inst = _instances[key];
        if (!inst) return;

        var date = parseDate(dateStr);
        if (date) {
            selectDate(inst, date);
        } else {
            clearValue(inst);
        }
    }

    function clear(inputSelector) {
        var key = typeof inputSelector === 'string'
            ? inputSelector.replace('#', '')
            : inputSelector;
        var inst = _instances[key];
        if (inst) clearValue(inst);
    }

    function showError(inputSelector, message) {
        var key = typeof inputSelector === 'string'
            ? inputSelector.replace('#', '')
            : inputSelector;
        var inst = _instances[key];
        if (inst) {
            inst.errorEl.text(message).show();
            inst.wrapperEl.addClass('ims-dp-has-error');
        }
    }

    function clearError(inputSelector) {
        var key = typeof inputSelector === 'string'
            ? inputSelector.replace('#', '')
            : inputSelector;
        var inst = _instances[key];
        if (inst) {
            inst.errorEl.text('').hide();
            inst.wrapperEl.removeClass('ims-dp-has-error');
        }
    }

    function destroy(inputSelector) {
        var key = typeof inputSelector === 'string'
            ? inputSelector.replace('#', '')
            : inputSelector;
        var inst = _instances[key];
        if (inst) {
            hideCalendar(inst);
            $(document).off('click.imsdp_' + key);
            inst.wrapperEl.find('.ims-dp-display').off();
            inst.wrapperEl.find('.ims-dp-icon').off();
            inst.inputEl.removeClass('master-field').attr('type', 'hidden');
            inst.wrapperEl.find('.ims-dp-display, .ims-dp-icon, .ims-dp-calendar, .ims-dp-error').remove();
            delete _instances[key];
        }
    }

    function initAll() {
        $('.ims-datepicker-input').each(function () {
            var $el = $(this);
            if (_instances[$el.attr('id') || $el.data('column')]) return;

            var isTime = $el.data('show-time') === true || $el.data('show-time') === 'true';

            init({
                input: '#' + ($el.attr('id') || $el.data('column')),
                format: $el.data('format') || (isTime ? 'dd/MM/yyyy HH:mm' : 'dd/MM/yyyy'),
                valueFormat: $el.data('value-format') || (isTime ? 'yyyy-MM-ddTHH:mm' : 'yyyy-MM-dd'),
                placeholder: $el.data('placeholder') || (isTime ? 'DD/MM/YYYY HH:mm' : 'DD/MM/YYYY'),
                allowFutureDates: $el.data('allow-future') !== false,
                allowPastDates: $el.data('allow-past') !== false,
                minDate: $el.data('min-date') || null,
                maxDate: $el.data('max-date') || null,
                showTime: isTime,
                required: $el.data('required') === true || $el.data('required') === 'true'
            });
        });
    }

    return {
        init: init,
        getValue: getValue,
        setValue: setValue,
        clear: clear,
        showError: showError,
        clearError: clearError,
        destroy: destroy,
        initAll: initAll,
        formatDate: formatDate,
        parseDate: parseDate
    };

})();
