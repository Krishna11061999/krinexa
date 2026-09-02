# Code Change Policy

Mandatory for any agent or developer editing code in this repository.
Read this before touching an existing file.

## 1. Comment every change

Any change to existing code must be marked with a comment at the point
of change, so the history is readable directly in the file, not just
in git blame.

```csharp
// [CHANGED 2026-09-02] Added AsNoTracking for read-only query — perf fix
var projects = await _dbContext.Projects.AsNoTracking().ToListAsync();
```

```csharp
// [ADDED 2026-09-02] New method — see reuse note below
public Task<bool> IsSlugAvailableAsync(string slug) { ... }
```

Keep the comment short: what changed and why, not a full changelog
entry. If the project later adopts a `CODE_CHANGE_LOG.md` or relies on
git history instead, this rule can be relaxed by explicit team
decision — until then, it applies to every edit.

## 2. Don't touch what already works

If a function is correct — it does what it's supposed to do, passes
its tests, and isn't the direct target of the current task — **do not
modify it**, even to "clean it up" or restyle it. Unrelated refactors
inside a task that's about something else create risk and noise.

Exceptions: the function is the actual subject of the current task, it
has a real bug, or the current task's requirements genuinely require
its signature/behavior to change.

## 3. Reuse before you write

Before writing a new function, search the codebase for one that
already does what you need (or nearly does). Prefer:

1. **Reuse as-is** — call the existing function.
2. **Reuse with a small, comment-marked extension** — e.g. add an
   optional parameter, following rule 1.
3. **Write a new function** — only when reuse would distort the
   existing function's purpose or contract.

This keeps the codebase (and any AI agent's working context) smaller —
fewer near-duplicate functions means less code to read, explain, and
carry through future sessions. When a new function genuinely is
needed, name it precisely and add a one-line comment on why an
existing function wasn't reused, so the next agent doesn't repeat the
search.

## 4. Quick checklist before submitting any change

- [ ] Did I change only what the task required?
- [ ] Is every changed or added line marked with a `[CHANGED]` /
      `[ADDED]` comment?
- [ ] Did I check for an existing function to reuse before adding a
      new one?
- [ ] If I added a new function, is there a one-line comment
      explaining why reuse wasn't possible?
- [ ] Does this still match the conventions in
      `knowledge-index/SKILL_tech_stack.md`?
