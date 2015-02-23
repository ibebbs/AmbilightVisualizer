using System;
using System.Threading.Tasks;
using System.Windows.Media;
using RestSharp;

namespace Ambilight
{
    public interface IJointSpaceClient
    {
        Task<Processed.IData> GetAmbilightProcessed();
    }

    /// <summary>
    /// Implements requests from http://jointspace.sourceforge.net/
    /// </summary>
    internal class JointSpaceClient : IJointSpaceClient
    {
        private readonly RestClient _client;
        private readonly Processed.IFactory _processFactory;

        public JointSpaceClient(Processed.IFactory processFactory)
        {
            _client = new RestClient("http://192.168.1.62:1925/");
            _processFactory = processFactory;
        }

        public async Task<Processed.IData> GetAmbilightProcessed()
        {
            RestRequest request = new RestRequest("1/ambilight/processed", Method.GET);

            IRestResponse response = await _client.ExecuteGetTaskAsync(request);

            Processed.IData data = _processFactory.FromContent(response.Content);

            return data;
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

        public Task<Processed.IData> GetAmbilightProcessed()
        {
            return Task.FromResult<Processed.IData>(
                new Processed.Data
                {
                    Layers = new [] { 
                        new Processed.Layer
                        {
                            Left = new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor() },
                            Top = new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor(), RandomColor() },
                            Right = new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor() },
                            Bottom = new[] { RandomColor(), RandomColor(), RandomColor(), RandomColor(), RandomColor() }
                        }
                    }
                }
            );
        }
    }
}
