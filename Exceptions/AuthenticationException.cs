namespace DotNetCardsServer.Exceptions
{
    public class AuthenticationException:Exception
    {
        public AuthenticationException() :base("Email or Password are wrong") { }
    }
}
