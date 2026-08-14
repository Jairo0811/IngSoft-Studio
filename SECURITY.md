# Security Policy

## Supported version

| Version | Supported |
|---|---|
| 1.x | ✅ |
| < 1.0 | ❌ |

## Reporting vulnerabilities

Do not publish exploitable security details in a public issue. Report the vulnerability privately to the repository owner with a concise reproduction, affected component, impact and proposed mitigation when available.

## Security baseline

IngSoft Studio applies JWT validation, ASP.NET Core Identity password and lockout policies, authorization, CORS allowlisting, HTTPS/HSTS outside Development, rate limiting, structured logging, exception handling, secure response headers and secret configuration through environment variables.

Secrets, passwords and production connection strings must never be committed to the repository.
