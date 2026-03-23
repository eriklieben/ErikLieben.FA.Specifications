# ErikLieben.FA.Specifications

.NET library providing specification pattern implementations for the ErikLieben.FA (Functional Architecture) ecosystem.

## Heartbeat

Before doing anything else in any session:

1. Read `context/SOUL.md` — who you are, how you behave
2. Read `context/USER.md` — who you're helping and their preferences
3. Read `context/MEMORY.md` — long-term curated knowledge
4. Read `context/memory/{today}.md` + `context/memory/{yesterday}.md` — recent session context
5. **Create or open today's memory file** — if `context/memory/{YYYY-MM-DD}.md` doesn't exist, create it with a session start timestamp. If it already exists (second session today), append a new session header.
6. Read `workflow.json` — know where docs repo and templates live
7. Scan `context/` — flag anything older than 30 days
8. Greet the user briefly. Mention what you remember from recent sessions if relevant.

Don't ask permission for steps 1-6. Just do it.

## Workflow Config

This project uses a shared documentation repository. See `workflow.json` for paths.

- **Templates**: Document templates live in the docs repo
- **Output**: Generated docs (ADRs, RFCs, etc.) go to the docs repo
- **Skills**: Document generation skills read `workflow.json` for paths

## Memory

You wake up fresh each session. These files are your continuity:

- **Daily notes:** `context/memory/YYYY-MM-DD.md` — raw logs of what happened
- **Long-term:** `context/MEMORY.md` — curated wisdom, distilled from daily notes

### Memory Security
- **MEMORY.md only loads in main sessions** (direct chat with your human)
- **DO NOT load in shared contexts** (group chats, sub-agent sessions, CI)

### Write It Down — No "Mental Notes"
- When someone says "remember this" → update the daily file or MEMORY.md
- When you learn a lesson → update SOUL.md or MEMORY.md
- When you make a mistake → document it so future-you doesn't repeat it

## Commits

- Keep commit messages clean — no AI tool mentions or commercial branding
- No "Co-Authored-By" lines
- Verify before declaring done
