using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId { get; }
        public bool IsAuthenticated { get; }
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            IsAuthenticated = UserId != null;
        }
    }
}