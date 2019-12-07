using IdentityModel.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PrismSampleApp
{
    public class AbsRefreshTokenHandler : DelegatingHandler
    {

        private readonly IApiTokenProvider _loginManager;

        public AbsRefreshTokenHandler(IApiTokenProvider loginManager)
        {
            _loginManager = loginManager;
        }

        public AbsRefreshTokenHandler(HttpMessageHandler innerHandler, IApiTokenProvider loginManager) : base(
            innerHandler)
        {
            _loginManager = loginManager;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _loginManager.GetTokenAsync();

            request.SetBearerToken(token);

           // request.RequestUri=new System.Uri($"{request.RequestUri.Scheme}://{request.RequestUri.Authority}/grpc{request.RequestUri.AbsolutePath}");

            //request.RequestUri =new System.Uri("");

            return await base.SendAsync(request, cancellationToken);
        }


    }
}
