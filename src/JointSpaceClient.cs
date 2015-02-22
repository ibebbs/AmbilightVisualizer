using System;
using System.Threading.Tasks;
using System.Windows.Media;
using RestSharp;

namespace Ambilight
{
    public interface IJointSpaceClient
    {
        Task<Processed> GetAmbilightProcessed();
    }

    /// <summary>
    /// Implements requests from http://jointspace.sourceforge.net/
    /// </summary>
    internal class JointSpaceClient : IJointSpaceClient
    {
        private readonly RestClient _client;

        public JointSpaceClient()
        {
            _client = new RestClient("http://192.168.1.51:1925/");
        }

        public async Task<Processed> GetAmbilightProcessed()
        {
            RestRequest request = new RestRequest("1/ambilight/processed", Method.GET);

            IRestResponse<Processed> response = await _client.ExecuteGetTaskAsync<Processed>(request);

            return response.Data;
        }
    }

    internal class NullJoinSpaceClient : IJointSpaceClient
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        private static Color RandomColor()
        {
            byte[] colors = new byte[3];
            Random.NextBytes(colors);

            return Color.FromRgb(colors[0], colors[1], colors[2]);
        }

        public Task<Processed> GetAmbilightProcessed()
        {
            return Task.FromResult(
                new Processed(
                    new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor(), RandomColor() },
                    new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor() },
                    new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor() },
                    new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor(), RandomColor() }
                )
            );
        }
    }
}
