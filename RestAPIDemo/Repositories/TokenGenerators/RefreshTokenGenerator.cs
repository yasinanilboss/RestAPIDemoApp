using RestAPIDemo.Models;

namespace RestAPIDemo.Repositories.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration authenticationConfiguration,
            TokenGenerator tokenGenerator)
        {
            _configuration = authenticationConfiguration;
            _tokenGenerator = tokenGenerator;

        }

        public string GenerateToken()
        {

            return _tokenGenerator.GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.RefreshTokenExpirationMinutes);
        }
    }
}
