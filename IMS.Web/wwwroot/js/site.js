

// Theme Management for IMS Admin Shell
window.IMSTheme = (function () {
    var STORAGE_KEY = 'ims_admin_theme';
    var DEFAULT_THEME = 'semi-dark';

    function getSavedTheme() {
        try {
            return localStorage.getItem(STORAGE_KEY) || DEFAULT_THEME;
        } catch (e) {
            return DEFAULT_THEME;
        }
    }

    function updateSwitcherUI(theme) {
        var iconElem = document.getElementById('themeSwitcherIcon');
        if (iconElem) {
            iconElem.className = 'fa-solid ' + (
                theme === 'light' ? 'fa-sun text-warning' :
                theme === 'dark' ? 'fa-moon text-info' :
                'fa-circle-half-stroke text-primary'
            );
        }

        var checkIcons = document.querySelectorAll('.theme-check-icon');
        for (var i = 0; i < checkIcons.length; i++) {
            var val = checkIcons[i].getAttribute('data-theme-check');
            if (val === theme) {
                checkIcons[i].classList.remove('d-none');
            } else {
                checkIcons[i].classList.add('d-none');
            }
        }

        var btns = document.querySelectorAll('.theme-option-btn');
        for (var j = 0; j < btns.length; j++) {
            var bVal = btns[j].getAttribute('data-theme-value');
            if (bVal === theme) {
                btns[j].classList.add('active');
            } else {
                btns[j].classList.remove('active');
            }
        }
    }

    function applyTheme(theme) {
        if (!theme || (theme !== 'light' && theme !== 'dark' && theme !== 'semi-dark')) {
            theme = DEFAULT_THEME;
        }
        document.documentElement.setAttribute('data-theme', theme);
        document.documentElement.setAttribute('data-bs-theme', theme === 'dark' ? 'dark' : 'light');
        if (document.body) {
            document.body.setAttribute('data-theme', theme);
            document.body.setAttribute('data-bs-theme', theme === 'dark' ? 'dark' : 'light');
        }
        try {
            localStorage.setItem(STORAGE_KEY, theme);
        } catch (e) { }
        updateSwitcherUI(theme);

        window.dispatchEvent(new CustomEvent('ims:themechange', { detail: { theme: theme } }));
    }

    function init() {
        var currentTheme = getSavedTheme();
        applyTheme(currentTheme);

        document.querySelectorAll('.theme-option-btn').forEach(function (btn) {
            btn.onclick = function (e) {
                e.preventDefault();
                var selectedTheme = this.getAttribute('data-theme-value');
                applyTheme(selectedTheme);
            };
        });
    }

    return {
        init: init,
        getTheme: getSavedTheme,
        setTheme: applyTheme
    };
})();

document.addEventListener('DOMContentLoaded', function () {
    IMSTheme.init();

    var app = document.getElementById('IMSApp');
    var toggleBtn = document.getElementById('sidebarToggle');
    var backdrop = document.getElementById('IMSBackdrop');

    if (!app || !toggleBtn) {
        return;
    }

    var isMobile = function () {
        return window.innerWidth < 992;
    };

    // Restore the desktop collapsed preference
    if (!isMobile() && localStorage.getItem('IMS-sidebar-collapsed') === 'true') {
        app.classList.add('sidebar-collapsed');
    }

    toggleBtn.addEventListener('click', function () {
        if (isMobile()) {
            app.classList.toggle('sidebar-open');
        } else {
            app.classList.toggle('sidebar-collapsed');
            localStorage.setItem('IMS-sidebar-collapsed', app.classList.contains('sidebar-collapsed'));
        }
    });

    if (backdrop) {
        backdrop.addEventListener('click', function () {
            app.classList.remove('sidebar-open');
        });
    }

    // If the window is resized past the mobile breakpoint, close the drawer state
    window.addEventListener('resize', function () {
        if (!isMobile()) {
            app.classList.remove('sidebar-open');
        }
    });
});

// Sidebar Scroll Position and Active Item Preservation
(function () {
    var SIDEBAR_SCROLL_KEY = 'ims_sidebar_scroll_pos';

    function getSidebar() {
        return document.getElementById('IMSSidebar');
    }

    function saveScroll() {
        var sidebar = getSidebar();
        if (sidebar) {
            try {
                sessionStorage.setItem(SIDEBAR_SCROLL_KEY, sidebar.scrollTop);
            } catch (e) { }
        }
    }

    function restoreScroll() {
        var sidebar = getSidebar();
        if (!sidebar) return;

        var savedScroll = sessionStorage.getItem(SIDEBAR_SCROLL_KEY);
        var activeItem = sidebar.querySelector('.nav-link.active, .sub-link.active');

        if (savedScroll !== null && !isNaN(parseInt(savedScroll, 10))) {
            sidebar.scrollTop = parseInt(savedScroll, 10);
        } else if (activeItem) {
            activeItem.scrollIntoView({ block: 'nearest', inline: 'nearest' });
        }

        // Secondary check after layout and accordion expansion finish
        setTimeout(function () {
            if (savedScroll !== null && !isNaN(parseInt(savedScroll, 10))) {
                sidebar.scrollTop = parseInt(savedScroll, 10);
            } else if (activeItem) {
                var itemRect = activeItem.getBoundingClientRect();
                var sidebarRect = sidebar.getBoundingClientRect();
                if (itemRect.top < sidebarRect.top || itemRect.bottom > sidebarRect.bottom) {
                    activeItem.scrollIntoView({ block: 'nearest', inline: 'nearest' });
                }
            }
        }, 60);
    }

    function initSidebarScroll() {
        var sidebar = getSidebar();
        if (!sidebar) return;

        restoreScroll();

        // Save scroll position when user scrolls sidebar
        var scrollTimeout;
        sidebar.addEventListener('scroll', function () {
            clearTimeout(scrollTimeout);
            scrollTimeout = setTimeout(saveScroll, 50);
        }, { passive: true });

        // Save immediately when clicking navigation links inside sidebar
        sidebar.addEventListener('click', function (e) {
            var target = e.target.closest('a');
            if (target && target.getAttribute('href') && target.getAttribute('href') !== 'javascript:void(0);' && target.getAttribute('href') !== '#') {
                saveScroll();
            }
        });

        // Save on unload / pagehide
        window.addEventListener('beforeunload', saveScroll);
        window.addEventListener('pagehide', saveScroll);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initSidebarScroll);
    } else {
        initSidebarScroll();
    }
})();
