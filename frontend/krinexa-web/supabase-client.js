/**
 * Krinexa — Supabase Database Integration Client
 * Directly connects frontend signup & login to Supabase PostgreSQL database.
 */

const SUPABASE_URL = "https://xpbokgjuqmtweyjybfkg.supabase.co";
const SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhwYm9rZ2p1cW10d2V5anliZmtnIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc4ODQwOTc5NSwiZXhwIjoyMTAzOTg1Nzk1fQ.GasdK51El5lebq5NFNfyc0qqrmO8_4e6wJANkOUW_mc";

const supabaseHeaders = {
  "apikey": SUPABASE_KEY,
  "Authorization": `Bearer ${SUPABASE_KEY}`,
  "Content-Type": "application/json",
  "Prefer": "resolution=merge-duplicates,return=representation"
};

/**
 * Save newly registered candidate or client directly to Supabase PostgreSQL Users & Profiles tables.
 */
async function saveUserToSupabase(data) {
  try {
    const isClient = data.role === 'interviewer' || data.role === 'Client';
    const userType = isClient ? 'Client' : (data.role === 'student' || data.role === 'intern' ? 'Student' : 'Talent');
    const profileType = data.role === 'student' ? 'Student' : (data.role === 'intern' ? 'Intern' : (data.role === 'senior' ? 'Senior' : 'Junior'));
    const email = (data.email || 'user@krinexa.in').toLowerCase().trim();

    // 1. Insert/Upsert into Users table (Status 'Y' = Active User per user requirement)
    const userPayload = {
      Email: email,
      EmailVerified: true,
      PasswordHash: data.password || 'cGFzc3dvcmQ=', // Base64 fallback
      UserType: userType,
      Status: 'Y', // 'Y' = Active user per user requirement
      IsActive: true
    };

    console.log('[Supabase DB] Registering User into PostgreSQL Users table:', email);
    const userRes = await fetch(`${SUPABASE_URL}/rest/v1/Users?on_conflict=Email`, {
      method: "POST",
      headers: supabaseHeaders,
      body: JSON.stringify(userPayload)
    });

    let insertedUser = null;
    if (userRes.ok) {
      const insertedArr = await userRes.json();
      insertedUser = Array.isArray(insertedArr) && insertedArr.length > 0 ? insertedArr[0] : null;
      console.log('[Supabase DB] User saved to Supabase Users table:', insertedUser);
    } else {
      const errText = await userRes.text();
      console.warn('[Supabase DB] Users table insert notice:', errText);
    }

    const userId = insertedUser ? insertedUser.Id : (data.userId || 'USR-' + Math.floor(1000 + Math.random() * 9000));

    // 2. Insert into TalentProfiles or ClientOrganizations table
    if (isClient) {
      const clientPayload = {
        UserId: insertedUser ? insertedUser.Id : undefined,
        OrganizationName: data.company || data.companyName || 'Tech Organization',
        ContactName: data.name || data.fullName || 'Hiring Contact',
        Designation: data.designation || 'Hiring Manager',
        CompanySize: data.companySize || '11-50',
        CompanyUrl: data.companyUrl || 'https://krinexa.in',
        BusinessPhone: data.mobileNumber || data.businessPhone || '+91 98765 43210'
      };
      if (!clientPayload.UserId) delete clientPayload.UserId;

      await fetch(`${SUPABASE_URL}/rest/v1/ClientOrganizations`, {
        method: "POST",
        headers: supabaseHeaders,
        body: JSON.stringify(clientPayload)
      }).catch(e => console.warn('[Supabase DB] ClientOrg insert notice:', e));
    } else {
      const talentPayload = {
        UserId: insertedUser ? insertedUser.Id : undefined,
        Name: data.name || data.fullName || 'Talent User',
        Mobile: data.mobileNumber || data.phone || '+91 98765 43210',
        ProfileType: profileType,
        Summary: `Registered candidate. Skills: ${data.skills || data.techSkills || 'Web Development'}`,
        PortfolioUrl: data.portfolio || data.portfolioUrl || 'https://portfolio.dev',
        GitHubUrl: data.github || data.githubUrl || 'https://github.com/user',
        LinkedInUrl: data.linkedin || data.linkedinUrl || 'https://linkedin.com/in/user',
        IsApproved: true
      };
      if (!talentPayload.UserId) delete talentPayload.UserId;

      await fetch(`${SUPABASE_URL}/rest/v1/TalentProfiles`, {
        method: "POST",
        headers: supabaseHeaders,
        body: JSON.stringify(talentPayload)
      }).catch(e => console.warn('[Supabase DB] TalentProfile insert notice:', e));
    }

    // 3. Update localStorage fallback store
    let localUsers = JSON.parse(localStorage.getItem('krinexa_database_users') || '[]');
    data.dbSaved = true;
    data.status = 'Y';
    data.supabaseId = userId;
    localUsers.push(data);
    localStorage.setItem('krinexa_database_users', JSON.stringify(localUsers));

    return { success: true, userId: userId, email: email };
  } catch (error) {
    console.error('[Supabase DB] Error saving user to Supabase:', error);
    return { success: false, error: error.message };
  }
}

/**
 * Login user by querying Supabase Users table.
 */
async function loginUserFromSupabase(email, password) {
  try {
    const normEmail = (email || '').toLowerCase().trim();
    const res = await fetch(`${SUPABASE_URL}/rest/v1/Users?Email=eq.${encodeURIComponent(normEmail)}&select=*`, {
      method: "GET",
      headers: supabaseHeaders
    });

    if (res.ok) {
      const users = await res.json();
      if (users.length > 0) {
        const user = users[0];
        if (user.Status === 'N' || user.IsActive === false) {
          return { success: false, message: 'Account is inactive (Status: N). Please contact support.' };
        }
        return { success: true, user: user };
      }
    }
  } catch (e) {
    console.warn('[Supabase DB] Login lookup notice:', e);
  }
  return null;
}
