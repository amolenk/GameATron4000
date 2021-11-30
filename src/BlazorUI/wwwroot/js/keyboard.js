let _onKeyDownRef;

function captureKeyDown(onKeyDownRef) {
    _onKeyDownRef = onKeyDownRef;
    window.addEventListener("keydown", onKeyDown, true);
}

function releaseKeyDown() {
    window.removeEventListener("keydown", onKeyDown);
    _onKeyDownRef = null;
}

async function onKeyDown(event) {    
    if (event.defaultPrevented) {
        return; // Should do nothing if the default action has been cancelled
    }
    var handled = await _onKeyDownRef.invokeMethodAsync('OnKeyDownAsync', event.key);
    if (handled) {
        // Suppress "double action" if event handled
        event.preventDefault();
    }
};
