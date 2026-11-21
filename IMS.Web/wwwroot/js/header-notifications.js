/**
 * Active / Unread Only Header Notifications Manager for IMS ERP
 */

(function () {
    let unreadCount = 0;
    let unreadList = [];

    document.addEventListener("DOMContentLoaded", function () {
        loadHeaderNotifications();
        setInterval(loadHeaderNotifications, 60000);
    });

    window.refreshHeaderNotifications = function () {
        loadHeaderNotifications();
    };

    async function loadHeaderNotifications() {
        try {
            const response = await fetch('/NotificationConfig/GetHeaderNotifications');
            if (!response.ok) return;

            const data = await response.json();
            if (data && data.success) {
                unreadCount = data.unreadCount || 0;
                // Only keep unread / active notifications
                const allItems = data.items || [];
                unreadList = allItems.filter(item => !item.isRead);
                renderNotifications();
            }
        } catch (e) {
            console.warn('Unable to fetch header notifications', e);
        }
    }

    function renderNotifications() {
        const badge = document.getElementById('headerNotificationBadge');
        const unreadPill = document.getElementById('headerUnreadBadge');
        const container = document.getElementById('notificationList');

        if (!badge || !container) return;

        // Update badge visibility & counts
        if (unreadCount > 0) {
            badge.classList.remove('d-none');
            if (unreadPill) {
                unreadPill.textContent = unreadCount;
                unreadPill.classList.remove('d-none');
            }
        } else {
            badge.classList.add('d-none');
            if (unreadPill) {
                unreadPill.classList.add('d-none');
            }
        }

        // Render Only Active / Unread Notifications
        if (!unreadList || unreadList.length === 0) {
            container.innerHTML = `
                <div class="text-center py-4 text-muted">
                    <span style="font-size: 0.8rem;">No active notifications</span>
                </div>`;
            return;
        }

        let html = '';
        unreadList.forEach(item => {
            const link = item.linkUrl || '#';

            html += `
                <div class="p-2 border-bottom bg-light notification-item-row" 
                     style="cursor: pointer; transition: background 0.15s ease;"
                     onmouseover="this.style.backgroundColor='#f1f5f9'"
                     onmouseout="this.style.backgroundColor='#f8fafc'"
                     onclick="handleNotificationClick('${item.id}', '${link}', event)">
                    
                    <div class="d-flex align-items-center justify-content-between mb-1">
                        <span class="fw-semibold text-dark text-truncate" style="font-size: 0.82rem;">${escapeHtml(item.title)}</span>
                        <span class="text-muted flex-shrink-0 ms-2" style="font-size: 0.7rem;">${item.timeAgo || ''}</span>
                    </div>
                    <div class="text-muted" style="font-size: 0.75rem; line-height: 1.3;">
                        ${escapeHtml(item.message)}
                    </div>
                </div>
            `;
        });

        container.innerHTML = html;
    }

    window.handleNotificationClick = async function (id, linkUrl, event) {
        if (event) event.preventDefault();
        try {
            await fetch('/NotificationConfig/MarkRead?id=' + id, { method: 'POST' });
            
            // Remove from local list immediately
            unreadList = unreadList.filter(n => n.id !== id);
            if (unreadCount > 0) unreadCount--;
            renderNotifications();

            if (linkUrl && linkUrl !== '#' && linkUrl !== '') {
                window.location.href = linkUrl;
            }
        } catch (e) {
            if (linkUrl && linkUrl !== '#' && linkUrl !== '') {
                window.location.href = linkUrl;
            }
        }
    };

    window.markAllNotificationsRead = async function (event) {
        if (event) event.stopPropagation();
        try {
            await fetch('/NotificationConfig/MarkAllRead', { method: 'POST' });
            unreadCount = 0;
            unreadList = [];
            renderNotifications();
            if (window.toastr) {
                toastr.success("All notifications cleared");
            }
        } catch (e) {
            console.error('Failed to mark all as read', e);
        }
    };

    function escapeHtml(str) {
        if (!str) return '';
        return str
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
})();
