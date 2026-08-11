using AxioVital.Domain.Entities;
using AxioVital.Domain.ValueObjects;
using AxioVital.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

Console.WriteLine("==================================================");
Console.WriteLine(" AxioVital Native — Running Foundation Smoke Tests");
Console.WriteLine("==================================================");

int passed = 0;
int total = 0;

void RunTest(string testName, Action testAction)
{
    total++;
    try
    {
        testAction();
        Console.WriteLine($"[PASS] {testName}");
        passed++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[FAIL] {testName}: {ex.Message}");
    }
}

// Test 1: Domain entity initialization
RunTest("Domain Entity & Tenant Initialization", () =>
{
    var tenant = new Tenant { Name = "General Hospital", Identifier = "gen-hosp" };
    if (tenant.Id == Guid.Empty) throw new Exception("Tenant ID was empty");
    if (tenant.IsDeleted) throw new Exception("Tenant should not be deleted by default");
});

// Test 2: Value Object validation
RunTest("Email Value Object Validation", () =>
{
    var email = new Email("doctor@axiovital.com");
    if (email.Value != "doctor@axiovital.com") throw new Exception("Email value mismatch");

    try
    {
        _ = new Email("invalid-email");
        throw new Exception("Failed to reject invalid email");
    }
    catch (ArgumentException)
    {
        // Expected
    }
});

// Test 3: Argon2 Password Hasher
RunTest("Argon2id Password Hasher", () =>
{
    var hasher = new Argon2PasswordHasher();
    var password = "SecurePassword123!";
    var hash = hasher.HashPassword(password);

    if (string.IsNullOrEmpty(hash) || !hash.Contains(':'))
        throw new Exception("Invalid Argon2 hash format");

    if (!hasher.VerifyPassword(password, hash))
        throw new Exception("Password verification failed");

    if (hasher.VerifyPassword("WrongPassword", hash))
        throw new Exception("Password verification false positive");
});

// Test 4: JWT Token Service
RunTest("JWT Token Generation & Claim Validation", () =>
{
    var settings = Options.Create(new JwtSettings
    {
        Secret = "VERY_SECURE_SECRET_KEY_FOR_TESTING_PURPOSES_ONLY_32_CHARS",
        Issuer = "AxioVitalTest",
        Audience = "AxioVitalDesktopTest",
        AccessTokenExpirationMinutes = 15
    });

    var jwtService = new JwtTokenService(settings);
    var userId = Guid.NewGuid();
    var tenantId = Guid.NewGuid();
    var email = "admin@axiovital.com";
    var roles = new[] { "Admin", "Physician" };

    var token = jwtService.GenerateAccessToken(userId, tenantId, email, roles);
    if (string.IsNullOrEmpty(token)) throw new Exception("JWT token was empty");

    var principal = jwtService.ValidateToken(token);
    if (principal == null) throw new Exception("JWT token validation failed");

    var tenantClaim = principal.FindFirst("tenant_id")?.Value;
    if (tenantClaim != tenantId.ToString()) throw new Exception("Tenant claim mismatch");
});

Console.WriteLine("==================================================");
Console.WriteLine($" Summary: {passed}/{total} tests passed successfully.");
Console.WriteLine("==================================================");

if (passed != total)
{
    Environment.Exit(1);
}
