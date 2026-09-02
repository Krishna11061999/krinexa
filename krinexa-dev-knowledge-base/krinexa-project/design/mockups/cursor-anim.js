/**
 * Krinexa Custom Mouse Cursor Animation
 * Interactive glass glowing pointer with spring trailing physics.
 */
document.addEventListener('DOMContentLoaded', () => {
  let dot = document.querySelector('.cursor-dot');
  let follower = document.querySelector('.cursor-follower');

  if (!dot) {
    dot = document.createElement('div');
    dot.className = 'cursor-dot';
    document.body.appendChild(dot);
  }

  if (!follower) {
    follower = document.createElement('div');
    follower.className = 'cursor-follower';
    document.body.appendChild(follower);
  }

  let mouseX = window.innerWidth / 2;
  let mouseY = window.innerHeight / 2;
  let followerX = mouseX;
  let followerY = mouseY;

  window.addEventListener('mousemove', (e) => {
    mouseX = e.clientX;
    mouseY = e.clientY;

    dot.style.left = `${mouseX}px`;
    dot.style.top = `${mouseY}px`;
  });

  function animate() {
    followerX += (mouseX - followerX) * 0.15;
    followerY += (mouseY - followerY) * 0.15;

    follower.style.left = `${followerX}px`;
    follower.style.top = `${followerY}px`;

    requestAnimationFrame(animate);
  }
  animate();

  const interactiveSelectors = 'a, button, .btn, .btn-kr-primary, .btn-kr-brass, .btn-kr-ghost, .glass-card, .tick-card, .tech-chip, input, select, textarea, .nav-link-custom';
  
  function attachHoverListeners() {
    const targets = document.querySelectorAll(interactiveSelectors);
    targets.forEach((target) => {
      target.addEventListener('mouseenter', () => {
        follower.classList.add('hovering');
        dot.style.transform = 'translate(-50%, -50%) scale(1.5)';
      });

      target.addEventListener('mouseleave', () => {
        follower.classList.remove('hovering');
        dot.style.transform = 'translate(-50%, -50%) scale(1)';
      });
    });
  }

  attachHoverListeners();

  window.addEventListener('mousedown', () => {
    follower.style.transform = 'translate(-50%, -50%) scale(0.7)';
    dot.style.transform = 'translate(-50%, -50%) scale(0.5)';
  });

  window.addEventListener('mouseup', () => {
    follower.style.transform = 'translate(-50%, -50%) scale(1)';
    dot.style.transform = 'translate(-50%, -50%) scale(1)';
  });

  document.addEventListener('mouseleave', () => {
    dot.style.opacity = '0';
    follower.style.opacity = '0';
  });

  document.addEventListener('mouseenter', () => {
    dot.style.opacity = '1';
    follower.style.opacity = '1';
  });

  const observer = new MutationObserver(() => {
    attachHoverListeners();
  });
  observer.observe(document.body, { childList: true, subtree: true });
});
