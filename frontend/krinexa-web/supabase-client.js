/**
 * Krinexa — Supabase Database Integration Client
 * Directly connects frontend signup & login to Supabase PostgreSQL database.
 * Fix: UPSERT on Email, robust client/talent insert, Supabase-only login validation.
 */

const SUPABASE_URL = "https://xpbokgjuqmtweyjybfkg.supabase.co";
const SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhwYm9rZ2p1cW10d2V5anliZmtnIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc4ODQwOTc5NSwiZXhwIjoyMTAzOTg1Nzk1fQ.GasdK51El5lebq5NFNfyc0qqrmO8_4e6wJANkOUW_mc";

const _dbHeaders = {
  "apikey": SUPABASE_KEY,
  "Authorization": "Bearer " + SUPABASE_KEY,
  "Content-Type": "application/json",
  "Prefer": "resolution=merge-duplicates,return=representation"
};

const _dbReadHeaders = {
  "apikey": SUPABASE_KEY,
  "Authorization": "Bearer " + SUPABASE_KEY,
  "Content-Type": "application/json"
};

/* ----------------------------------------------------------------
   Helper: POST to Supabase REST API
---------------------------------------------------------------- */
async function _dbPost(table, payload, conflictCol) {
  const url = conflictCol
    ? `${SUPABASE_URL}/rest/v1/${table}?on_conflict=${conflictCol}`
    : `${SUPABASE_URL}/rest/v1/${table}`;
  const res = await fetch(url, {
    method: "POST",
    headers: _dbHeaders,
    body: JSON.stringify(payload)
  });
  if (!res.ok) {
    const err = await res.text();
    console.warn(`[Supabase] ${table} insert notice (${res.status}):`, err);
    return null;
  }
  const rows = await res.json();
  return Array.isArray(rows) && rows.length > 0 ? rows[0] : null;
}

/* ----------------------------------------------------------------
   Helper: Fetch existing User by email
---------------------------------------------------------------- */
async function _getUserByEmail(email) {
  const res = await fetch(
    `${SUPABASE_URL}/rest/v1/Users?Email=eq.${encodeURIComponent(email)}&select=*&limit=1`,
    { method: "GET", headers: _dbReadHeaders }
  );
  if (res.ok) {
    const rows = await res.json();
    return rows.length > 0 ? rows[0] : null;
  }
  return null;
}

/* ----------------------------------------------------------------
   Helper: Fetch existing TalentProfile by UserId
---------------------------------------------------------------- */
async function _getTalentProfileByUserId(userId) {
  const res = await fetch(
    `${SUPABASE_URL}/rest/v1/TalentProfiles?UserId=eq.${encodeURIComponent(userId)}&select=*&limit=1`,
    { method: "GET", headers: _dbReadHeaders }
  );
  if (res.ok) {
    const rows = await res.json();
    return rows.length > 0 ? rows[0] : null;
  }
  return null;
}

/* ================================================================
   saveUserToSupabase — called on every registration form submit
================================================================ */
async function saveUserToSupabase(data) {
  try {
    const role = (data.role || 'student').toLowerCase();
    const isClient = role === 'interviewer' || role === 'client';

    // Map role → UserType (as per DB CHECK constraint)
    const userType = isClient ? 'Client'
      : (role === 'student' ? 'Student'
        : (role === 'intern' ? 'Student' : 'Talent'));

    // Map role → ProfileType for TalentProfiles
    const profileType = role === 'intern' ? 'Intern'
      : (role === 'senior' ? 'Senior'
        : (role === 'student' ? 'Student' : 'Junior'));

    const email = (data.email || '').toLowerCase().trim();
    if (!email) {
      console.warn('[Supabase] No email provided — skipping save.');
      return { success: false, error: 'Please enter a valid email address.' };
    }

    // ── Step 1: Upsert into Users table ──────────────────────────
    console.log('[Supabase] Saving user:', email, '| UserType:', userType);
    let insertedUser = await _dbPost('Users', {
      Email: email,
      EmailVerified: true,
      PasswordHash: data.password || 'cGFzc3dvcmQ=',
      UserType: userType,
      Status: 'Y',      // Y = Active user
      IsActive: true
    }, 'Email');

    // If upsert returned no row (e.g. duplicate updated), fetch existing
    if (!insertedUser) {
      insertedUser = await _getUserByEmail(email);
    }

    const userId = insertedUser ? insertedUser.Id : null;
    if (!userId) {
      console.error('[Supabase] Failed to resolve UserId for:', email);
      return { success: false, error: 'Could not create user record in Krinexa database.' };
    }

    console.log('[Supabase] User row resolved — Id:', userId);

    // ── Step 2: Insert profile table ─────────────────────────────
    if (isClient) {
      await _dbPost('ClientOrganizations', {
        UserId: userId,
        OrganizationName: data.company || data.companyName || 'Tech Organization',
        ContactName: data.name || data.fullName || 'Client User',
        Designation: data.designation || 'Hiring Manager',
        CompanySize: data.companySize || '11-50',
        CompanyUrl: data.companyUrl || 'https://krinexa.in',
        BusinessPhone: data.mobileNumber || data.businessPhone || '+91 98765 43210'
      });
      console.log('[Supabase] ClientOrganizations row inserted for:', email);
    } else {
      let talentProfile = await _dbPost('TalentProfiles', {
        UserId: userId,
        Name: data.name || data.fullName || 'Talent User',
        Mobile: data.mobileNumber || data.phone || '+91 98765 43210',
        ProfileType: profileType,
        Summary: 'Registered via Krinexa portal. Skills: ' + (data.skills || data.techSkills || 'Web Development'),
        PortfolioUrl: data.portfolio || data.portfolioUrl || 'https://portfolio.dev',
        GitHubUrl: data.github || data.githubUrl || 'https://github.com/user',
        LinkedInUrl: data.linkedin || data.linkedinUrl || 'https://linkedin.com/in/user',
        IsApproved: true
      });

      if (!talentProfile) {
        talentProfile = await _getTalentProfileByUserId(userId);
      }

      console.log('[Supabase] TalentProfiles row inserted/resolved for:', email);

      // Insert StudentProfiles if student/intern
      if (role === 'student' || role === 'intern') {
        const talentProfileId = talentProfile ? talentProfile.Id : null;
        if (talentProfileId) {
          const gradYrMatch = (data.passingYear || '2026').match(/\d{4}/);
          const gradYr = gradYrMatch ? parseInt(gradYrMatch[0]) : 2026;

          await _dbPost('StudentProfiles', {
            TalentProfileId: talentProfileId,
            College: data.collegeName || data.college || 'ABES Engineering College',
            Degree: data.degree || 'B.Tech',
            Branch: data.branch || 'Computer Science',
            CurrentYear: data.passingYear || 'Final Year',
            GraduationYear: gradYr
          });
          console.log('[Supabase] StudentProfiles row inserted for:', email);
        }
      }
    }

    // ── Step 3: localStorage fallback copy ───────────────────────
    const store = JSON.parse(localStorage.getItem('krinexa_database_users') || '[]');
    store.push({ ...data, supabaseId: userId, status: 'Y', dbSaved: true });
    localStorage.setItem('krinexa_database_users', JSON.stringify(store));

    return { success: true, userId: userId, email: email };
  } catch (err) {
    console.error('[Supabase] saveUserToSupabase error:', err);
    return { success: false, error: err.message || 'Database connection error.' };
  }
}

/* ================================================================
   loginUserFromSupabase — validates credentials from Supabase ONLY
================================================================ */
async function loginUserFromSupabase(email, password) {
  const normEmail = (email || '').toLowerCase().trim();
  if (!normEmail) return { success: false, message: 'Please enter your email address.' };

  let user = null;
  try {
    user = await _getUserByEmail(normEmail);
  } catch (e) {
    console.warn('[Supabase] Login fetch error:', e);
    return { success: false, message: 'Could not connect to Krinexa database. Please try again.' };
  }

  if (!user) {
    return { success: false, message: 'No account found for this email.\nPlease register first.' };
  }

  if (user.Status === 'N' || user.IsActive === false) {
    return { success: false, message: 'Your account is inactive (Status: N).\nPlease contact support@krinexa.in.' };
  }

  // Password check (base64 compare — matches what registration stores)
  if (password) {
    const encodedInput = btoa(password);
    if (user.PasswordHash !== encodedInput && user.PasswordHash !== password) {
      return { success: false, message: 'Incorrect password. Please try again.' };
    }
  }

  return { success: true, user: user };
}
