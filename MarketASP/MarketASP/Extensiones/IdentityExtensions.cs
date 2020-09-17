using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MarketASP.Extensiones
{
    public static class IdentityExtensions
    {
        public static string GetOrganizationId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("OrganizationId");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetLocal(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Local");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetNombre(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Nombre");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetApellido(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Apellido");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}