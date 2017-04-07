using System;
using System.Security.Claims;

namespace ParrotWings.Services.Core.Extensions
{
    public static class IdentityExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal identity)
        {
            return new Guid(
                identity.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
