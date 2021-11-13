
window.CaptureKeyDown = (callbackRef) =>
{
    window.addEventListener("keydown", async event => {
        if (event.defaultPrevented) {
          return; // Should do nothing if the default action has been cancelled
        }
        var handled = await callbackRef.invokeMethodAsync('OnKeyDownAsync', event.key);
        if (handled) {
          // Suppress "double action" if event handled
          event.preventDefault();
        }
      }, true);
}
