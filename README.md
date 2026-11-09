# BulkyWeb

Public registration creates Customer accounts only. To seed a demo Admin account when running in Development, set credentials with .NET user secrets from the repository root:

```powershell
dotnet user-secrets set "AdminAccount:Email" "admin@example.com" --project BulkyWeb\BulkyWeb.csproj
dotnet user-secrets set "AdminAccount:Password" "<choose-a-strong-password>" --project BulkyWeb\BulkyWeb.csproj
```

The configured account is created and assigned the Admin role at startup after the database is migrated. The credentials are stored outside the repository, and this Admin seeding is disabled outside Development. Leave these secrets unset to run without seeding an Admin account.

For hosted email confirmation, configure the SMTP values as private application environment variables in the hosting dashboard (use double underscores for nested settings):

- `Smtp__Host`
- `Smtp__Port`
- `Smtp__Username`
- `Smtp__Password`
- `Smtp__FromEmail`
- `Smtp__FromName`
- `Smtp__EnableSsl`

Do not put SMTP credentials in `wwwroot` or commit them to application settings. If SMTP sending fails, registration remains saved as an unconfirmed Customer and the confirmation page offers a resend link.
