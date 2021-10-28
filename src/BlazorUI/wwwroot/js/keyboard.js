
window.CaptureKeyDown = (callbackRef) =>
{
    window.addEventListener("keydown", function (event) {
        if (event.defaultPrevented) {
          return; // Should do nothing if the default action has been cancelled
        }
      
        var handled = callbackRef.invokeMethod('OnKeyDown', event.key);
        if (handled) {
          // Suppress "double action" if event handled
          event.preventDefault();
        }
      }, true);
}
