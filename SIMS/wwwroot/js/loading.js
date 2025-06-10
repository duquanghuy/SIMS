// loading.js
(function () {
    const TEXT = 'LOADING...';
    const STORAGE_KEY = 'sidebarCollapsed';

    // create overlay
    const overlay = document.createElement('div');
    overlay.id = 'loadingOverlay';
    Object.assign(overlay.style, {
        position: 'fixed',
        top: '0',
        left: '0',
        width: '100%',
        height: '100%',
        background: 'rgba(0,0,0,0.8)',
        display: 'none',    // will show it in init
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        zIndex: '10000',
    });

    // spinner icon
    const spinner = document.createElement('i');
    spinner.className = 'fa-solid fa-spinner fa-spin';
    spinner.style.fontSize = '3rem';
    spinner.style.color = '#fff';           // <-- white, not "#white"
    overlay.appendChild(spinner);

    // loading text container
    const textContainer = document.createElement('div');
    textContainer.className = 'loading-text';
    textContainer.style.color = '#fff';     // <-- ensure text is white
    overlay.appendChild(textContainer);


    // append letters as spans, with increasing delay
    for (let i = 0; i < TEXT.length; i++) {
        const span = document.createElement('span');
        span.textContent = TEXT[i];
        span.style.animationDelay = `${i * 0.1}s`;
        textContainer.appendChild(span);
    }

    // track start time for minimum display
    let startTime = 0;

    // show overlay ASAP once body exists
    function initOverlay() {
        document.body.appendChild(overlay);
        overlay.style.display = 'flex';
        startTime = Date.now();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initOverlay);
    } else {
        initOverlay();
    }

    // hide only after full load + minimum 1s
    window.addEventListener('load', () => {
        const elapsed = Date.now() - startTime;
        const remain = Math.max(0, 1000 - elapsed);
        setTimeout(() => overlay.style.display = 'none', remain);
    });

    // re-show on any in-app link click
    document.body.addEventListener('click', e => {
        const a = e.target.closest('a[href]');
        if (!a) return;
        const href = a.getAttribute('href');
        if (
            href.startsWith('#') ||
            href.startsWith('javascript:') ||
            a.target === '_blank' ||
            /^https?:\/\//i.test(href)
        ) return;
        overlay.style.display = 'flex';
    });
})();
