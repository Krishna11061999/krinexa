/**
 * Krinexa Shared Components — Blue & Off-White Glass Header, Footer & Requirement Modal Injector
 */
document.addEventListener('DOMContentLoaded', () => {
  renderHeader();
  renderFooter();
  renderRequirementModal();
  setupNavbarScrollEffect();
});

function renderHeader() {
  const headerContainer = document.getElementById('kr-header');
  if (!headerContainer) return;

  const currentPath = window.location.pathname;

  headerContainer.innerHTML = `
    <nav class="navbar navbar-expand-lg kr-navbar fixed-top">
      <div class="container-fluid px-lg-5">
        <a class="brand-logo me-4" href="01-index.html">
          KRINEXA<span class="dot">.</span>
        </a>
        
        <button class="navbar-toggler border-0 shadow-none" type="button" data-bs-toggle="collapse" data-bs-target="#krNavbarContent">
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="krNavbarContent">
          <ul class="navbar-nav me-auto mb-2 mb-lg-0 gap-1">
            <li class="nav-item">
              <a class="nav-link-custom ${currentPath.includes('01-index') || currentPath.endsWith('/') ? 'active' : ''}" href="01-index.html">Marketplace</a>
            </li>
            <li class="nav-item">
              <a class="nav-link-custom ${currentPath.includes('02-registration') ? 'active' : ''}" href="02-registration.html">Talent Network</a>
            </li>
            <li class="nav-item">
              <a class="nav-link-custom ${currentPath.includes('04-project-interest') ? 'active' : ''}" href="04-project-interest.html">Requirements</a>
            </li>
            <li class="nav-item">
              <a class="nav-link-custom ${currentPath.includes('03-chat') ? 'active' : ''}" href="03-chat.html">Project Chat</a>
            </li>
            <li class="nav-item">
              <a class="nav-link-custom ${currentPath.includes('06-admin') ? 'active' : ''}" href="06-admin-index.html">Admin Console</a>
            </li>
          </ul>

          <div class="d-flex align-items-center gap-2 mt-3 mt-lg-0">
            <a href="05-login.html" class="btn btn-kr-signin btn-sm">Sign In</a>
            <a href="07-signup.html" class="btn btn-kr-signup btn-sm">Sign Up</a>
            <button class="btn btn-kr-brass btn-sm d-flex align-items-center gap-1 ms-1" data-bs-toggle="modal" data-bs-target="#requirementModal">
              <svg width="15" height="15" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"/></svg>
              Post Requirement
            </button>
          </div>
        </div>
      </div>
    </nav>
    <div style="height: 76px;"></div>
  `;
}

function renderFooter() {
  const footerContainer = document.getElementById('kr-footer');
  if (!footerContainer) return;

  footerContainer.innerHTML = `
    <footer class="kr-footer mt-5">
      <div class="container px-lg-5">
        <div class="row g-4 mb-5">
          <div class="col-lg-4 col-md-6">
            <div class="brand-logo mb-3">
              KRINEXA<span class="dot">.</span>
            </div>
            <p class="text-muted small pe-lg-4">
              Direct technology matching platform. Connecting high-growth enterprises with vetted senior, junior, intern, and student developers across .NET, MERN, Azure, and SQL Server.
            </p>
            <div class="eyebrow-rule my-3">
              <span>Noida HQ Office</span>
              <span class="line"></span>
            </div>
            <div class="font-mono text-muted small">
              SECTOR 49, NOIDA, UTTAR PRADESH – 201301
            </div>
          </div>

          <div class="col-lg-2 col-md-6">
            <h6 class="text-navy font-mono mb-3 fw-bold">NAVIGATION</h6>
            <ul class="list-unstyled d-flex flex-column gap-2 small">
              <li><a href="01-index.html">Marketplace Home</a></li>
              <li><a href="02-registration.html">Talent Registration</a></li>
              <li><a href="04-project-interest.html">Project Requirements</a></li>
              <li><a href="03-chat.html">Project Chat Thread</a></li>
              <li><a href="06-admin-index.html">Admin Dashboard</a></li>
            </ul>
          </div>

          <div class="col-lg-3 col-md-6">
            <h6 class="text-navy font-mono mb-3 fw-bold">CORE TECH STACKS</h6>
            <div class="d-flex flex-wrap gap-2">
              <span class="tech-chip">.NET Core</span>
              <span class="tech-chip">ASP.NET</span>
              <span class="tech-chip">React</span>
              <span class="tech-chip">Node.js</span>
              <span class="tech-chip">SQL Server</span>
              <span class="tech-chip">Azure</span>
              <span class="tech-chip">Docker</span>
            </div>
          </div>

          <div class="col-lg-3 col-md-6">
            <h6 class="text-navy font-mono mb-3 fw-bold">PLATFORM STATUS</h6>
            <div class="glass-card p-3 mb-3">
              <div class="d-flex align-items-center gap-2 mb-2">
                <span class="badge bg-success rounded-circle p-1 pulse-badge"></span>
                <span class="font-mono text-navy fw-bold small">Scoring Engine Live</span>
              </div>
              <p class="text-muted mb-0 font-mono" style="font-size: 11.5px;">
                Weighted matching: 40% Tech Stack · 20% Experience Tier
              </p>
            </div>
            <div class="font-mono text-primary fw-bold small">
              15-Day Free Trial Available
            </div>
          </div>
        </div>

        <hr style="border-color: rgba(37, 99, 235, 0.15);" />

        <div class="d-flex flex-column flex-md-row align-items-center justify-content-between pt-2 text-muted small">
          <div>© 2026 Krinexa Technologies. All rights reserved.</div>
          <div class="d-flex gap-4 mt-2 mt-md-0 font-mono" style="font-size: 12px;">
            <a href="#">Privacy Policy</a>
            <a href="#">Terms of Service</a>
            <a href="#">Security & Consent</a>
          </div>
        </div>
      </div>
    </footer>
  `;
}

function renderRequirementModal() {
  if (document.getElementById('requirementModal')) return;

  const modalDiv = document.createElement('div');
  modalDiv.className = 'modal fade modal-glass';
  modalDiv.id = 'requirementModal';
  modalDiv.tabIndex = -1;
  modalDiv.setAttribute('aria-hidden', 'true');

  modalDiv.innerHTML = `
    <div class="modal-dialog modal-dialog-centered modal-lg">
      <div class="modal-content tick-card p-2">
        <div class="modal-header border-0 pb-0">
          <div>
            <div class="eyebrow-rule mb-1">
              <span>Client Portal</span>
              <span class="line"></span>
            </div>
            <h4 class="modal-title text-navy font-mono fw-bold">Post a Requirement</h4>
          </div>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body pt-3">
          <form id="reqForm" onsubmit="handleReqSubmit(event)">
            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label text-muted small font-mono">PROJECT / REQUIREMENT TITLE *</label>
                <input type="text" class="form-control" placeholder="e.g. Senior .NET Core Backend Migration" required>
              </div>
              <div class="col-md-6">
                <label class="form-label text-muted small font-mono">EXPERIENCE TIER NEEDED *</label>
                <select class="form-select" required>
                  <option value="">Select Tier...</option>
                  <option>Senior Developer (5+ yrs)</option>
                  <option>Junior Developer (1-3 yrs)</option>
                  <option>Intern / Graduate</option>
                  <option>Student (Part-time)</option>
                  <option>Dedicated Project Team</option>
                </select>
              </div>
            </div>

            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label text-muted small font-mono">PRIMARY TECH STACK *</label>
                <input type="text" class="form-control" placeholder="e.g. ASP.NET Core, C#, Azure, SQL" required>
              </div>
              <div class="col-md-6">
                <label class="form-label text-muted small font-mono">DURATION & ENGAGEMENT</label>
                <input type="text" class="form-control" placeholder="e.g. 3 Months, Full-time">
              </div>
            </div>

            <div class="mb-3">
              <div class="form-label text-muted small font-mono">REQUIREMENT DETAILS</div>
              <textarea class="form-control" rows="3" placeholder="Describe project scope, key deliverables, and specific skills..."></textarea>
            </div>

            <div class="d-flex justify-content-end gap-2 pt-2">
              <button type="button" class="btn btn-kr-ghost" data-bs-dismiss="modal">Cancel</button>
              <button type="submit" class="btn btn-kr-brass">Submit & Match Talent →</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `;

  document.body.appendChild(modalDiv);
}

function setupNavbarScrollEffect() {
  window.addEventListener('scroll', () => {
    const navbar = document.querySelector('.kr-navbar');
    if (navbar) {
      if (window.scrollY > 40) {
        navbar.classList.add('scrolled');
      } else {
        navbar.classList.remove('scrolled');
      }
    }
  });
}

function handleReqSubmit(e) {
  e.preventDefault();
  alert('Requirement submitted successfully! Krinexa matching engine will now evaluate candidate profiles.');
  const modalEl = document.getElementById('requirementModal');
  const modal = bootstrap.Modal.getInstance(modalEl);
  if (modal) modal.hide();
}
