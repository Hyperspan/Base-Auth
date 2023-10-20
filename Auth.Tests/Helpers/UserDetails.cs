using Auth.Shared.Requests;

namespace Auth.Tests.Helpers
{
    internal class RegisterHelpers<T> where T : IEquatable<T>
    {
        internal static readonly RegisterUserRequest UserDetails = new()
        {
            Email = "ayushaher118@gmail.com",
            MobileNumber = "8689952904",
            ConfirmPassword = "ayushaher118@",
            Password = "ayushaher118@",
            UserName = "Ayush Aher",
        };
    }
}
