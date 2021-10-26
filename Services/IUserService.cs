using System;

namespace EstateServiceApp.Services
{
    public interface IUserService
    {
        string GetUserId();
        Boolean IsAuthenticated();
    }
}