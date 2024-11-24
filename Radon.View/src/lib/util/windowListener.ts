export const setupWindowListener = () => {
  window.onload = () => {
    document.addEventListener("touchstart", event => {
      if (event.touches.length > 1) {
        event.preventDefault();
      }
    });
    document.addEventListener("gesturestart", event => {
      event.preventDefault();
    });
    let lastTouchEnd = 0;
    document.documentElement.addEventListener(
      "touchend",
      event => {
        const now = Date.now();
        if (now - lastTouchEnd <= 300) {
          event.preventDefault();
        }
        lastTouchEnd = now;
      },
      {
        passive: false,
      },
    );
  };

  window.onresize = () => {
    if (document.body.style.zoom != "1") {
      console.debug(`Unexpected resize: ${document.body.style.zoom ?? "?"}`);
      document.body.style.zoom = "1.0";
    }
  };
};
