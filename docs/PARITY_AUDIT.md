# Cano parity audit

The old Cano trees were reviewed read-only. Their durable product concepts are
customers and contacts, properties, loans and trust deeds, tasks, notes,
campaigns, documents, pick lists, relationships, activity history, imports,
email integration, reports, and account security.

LendWise already implements the central customer, contact, property, loan,
trust-deed, task, pick-list, relationship, and activity models. This update adds
a loan document checklist with requested/received/verified/rejected/waived
states, blocker counts, receipt workflow, and activity history.

Remaining high-value slices are campaign/referral tracking, notes with an audit
trail, document storage-provider integration, repeatable imports with dry-run
validation, email/calendar adapters, permissions, and pipeline forecasting.
The old code generators, VistaDB-specific layers, duplicated experimental
projects, and embedded third-party binaries are not modernization targets.
