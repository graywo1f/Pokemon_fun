using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pokemon_Api.Interface;
using Pokemon_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon_Api
{
    /// <summary>
    /// Gets data from the PokeAPI service
    /// </summary>
    public class PokeApiClient : IDisposable, IPokeApiClient
    {
        /// <summary>
        /// The default `User-Agent` header value used by instances of <see cref="PokeApiClient"/>.
        /// </summary>
        public static readonly ProductHeaderValue DefaultUserAgent = GetDefaultUserAgent();
        private readonly HttpClient _client;
        private readonly Uri _baseUri = new Uri("https://pokeapi.co/api/v2/");
        readonly ILogger _logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PokeApiClient(ILogger logger) : this(logger, DefaultUserAgent) {}


        /// <summary>
        /// Constructor with message handler
        /// </summary>
        /// <param name="messageHandler">Message handler implementation</param>
        public PokeApiClient(HttpMessageHandler messageHandler, ILogger logger)
            : this(messageHandler, DefaultUserAgent, logger)
        { }

        /// <summary>
        /// Constructor with message handler and `User-Agent` header value
        /// </summary>
        /// <param name="messageHandler">Message handler implementation</param>
        /// <param name="userAgent">The value for the default `User-Agent` header.</param>
        public PokeApiClient(HttpMessageHandler messageHandler, ProductHeaderValue userAgent, ILogger logger)
        {
            if (userAgent == null)
            {
                throw new ArgumentNullException(nameof(userAgent));
            }
            _logger = logger;
            _client = new HttpClient(messageHandler) { BaseAddress = _baseUri };
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(userAgent));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokeApiClient"/> with 
        /// a given value for the `User-Agent` header
        /// </summary>
        /// <param name="userAgent">The value for the default `User-Agent` header.</param>
        public PokeApiClient(ILogger logger,ProductHeaderValue userAgent)
        {
            if (userAgent == null)
            {
                throw new ArgumentNullException(nameof(userAgent));
            }
            _logger = logger;
            _client = new HttpClient() { BaseAddress = _baseUri };
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(userAgent));
        }

        
        
        /// <summary>
        /// Close resources
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();         
        }

        private static ProductHeaderValue GetDefaultUserAgent()
        {
            var version = typeof(PokeApiClient).Assembly.GetName().Version;
            return new ProductHeaderValue("PokeApiNet", $"{version.Major}.{version.Minor}");
        }

        /// <summary>
        /// Send a request to the api and serialize the response into the specified type
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="apiParam">The name or id of the resource</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <exception cref="HttpRequestException">Something went wrong with your request</exception>
        /// <returns>An instance of the specified type with data from the request</returns>
        private async Task<T> GetResourcesWithParamsAsync<T>(string apiParam, CancellationToken cancellationToken)
            where T : ResourceBase
        {
           
            // lowercase the resource name as the API doesn't recognize upper case and lower case as the same
            string sanitizedApiParam = apiParam.ToLowerInvariant();
            string apiEndpoint = GetApiEndpointString<T>();

            return await GetAsync<T>($"{apiEndpoint}/{sanitizedApiParam}/", cancellationToken);
        }

        /// <summary>
        /// Gets a resource from a navigation url; resource is retrieved from cache if possible
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="url">Navigation url</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <exception cref="NotSupportedException">Navigation url doesn't contain the resource id</exception>
        /// <returns>The object of the resource</returns>
        private async Task<T> GetResourceByUrlAsync<T>(string url, CancellationToken cancellationToken)
            where T : ResourceBase
        {
            // need to parse out the id in order to check if it's cached.
            // navigation urls always use the id of the resource
            string trimmedUrl = url.TrimEnd('/');
            string resourceId = trimmedUrl.Substring(trimmedUrl.LastIndexOf('/') + 1);

            if (!int.TryParse(resourceId, out int id))
            {
                // not sure what to do here...
                throw new NotSupportedException($"Navigation url '{url}' is in an unexpected format");
            }

          
             T   resource = await GetResourcesWithParamsAsync<T>(resourceId, cancellationToken);
           

            return resource;
        }

        /// <summary>
        /// Gets a resource by id; resource is retrieved from cache if possible
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="id">Id of resource</param>
        /// <returns>The object of the resource</returns>
        public async Task<T> GetResourceAsync<T>(int id) where T : ResourceBase
        {
            _logger.LogInformation("Starting GetResourceAsync for {0} param {1}", typeof(T).ToString(), id);
            return await GetResourceAsync<T>(id, CancellationToken.None);
        }

        /// <summary>
        /// Gets a resource by id; resource is retrieved from cache if possible
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="id">Id of resource</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>The object of the resource</returns>
        public async Task<T> GetResourceAsync<T>(int id, CancellationToken cancellationToken)
            where T : ResourceBase
        {
            _logger.LogInformation("Starting GetResourceAsync with CancellationToken for {0} param {1}", typeof(T).ToString(), id);
            T    resource = await GetResourcesWithParamsAsync<T>(id.ToString(), cancellationToken);
            

            return resource;
        }

        /// <summary>
        /// Gets a resource by name; resource is retrieved from cache if possible. This lookup
        /// is case insensitive.
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="name">Name of resource</param>
        /// <returns>The object of the resource</returns>
        public async Task<T> GetResourceAsync<T>(string name)
            where T : NamedApiResource
        {
            _logger.LogInformation("Starting GetResourceAsync for {0} param {1}", typeof(T).ToString(), name);
            return await GetResourceAsync<T>(name, CancellationToken.None);
        }

        /// <summary>
        /// Gets a resource by name; resource is retrieved from cache if possible. This lookup
        /// is case insensitive.
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="name">Name of resource</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>The object of the resource</returns>
        public async Task<T> GetResourceAsync<T>(string name, CancellationToken cancellationToken)
            where T : NamedApiResource
        {
            _logger.LogInformation("Starting GetResourceAsync with CancellationToken for {0} param {1}", typeof(T).ToString(), name);
            string sanitizedName = name
                .Replace(" ", "-")      // no resource can have a space in the name; API uses -'s in their place
                .Replace("'", "")       // looking at you, Farfetch'd
                .Replace(".", "");      // looking at you, Mime Jr. and Mr. Mime

            // Nidoran is interesting as the API wants 'nidoran-f' or 'nidoran-m'

            T resource = await GetResourcesWithParamsAsync<T>(sanitizedName, cancellationToken);
            

            return resource;
        }

        /// <summary>
        /// Resolves all navigation properties in a collection
        /// </summary>
        /// <typeparam name="T">Navigation type</typeparam>
        /// <param name="collection">The collection of navigation objects</param>
        /// <returns>A list of resolved objects</returns>
        public async Task<List<T>> GetResourceAsync<T>(IEnumerable<UrlNavigation<T>> collection)
            where T : ResourceBase
        {
            _logger.LogInformation("Starting GetResourceAsync with CancellationToken for {0} ", typeof(T).ToString());
            return await GetResourceAsync<T>(collection, CancellationToken.None);
        }

        /// <summary>
        /// Resolves all navigation properties in a collection
        /// </summary>
        /// <typeparam name="T">Navigation type</typeparam>
        /// <param name="collection">The collection of navigation objects</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>A list of resolved objects</returns>
        public async Task<List<T>> GetResourceAsync<T>(IEnumerable<UrlNavigation<T>> collection, CancellationToken cancellationToken)
            where T : ResourceBase
        {
            _logger.LogInformation("Starting GetResourceAsync for {0}", typeof(T).ToString());
            return (await Task.WhenAll(collection.Select(m => GetResourceAsync(m, cancellationToken)))).ToList();
        }

        /// <summary>
        /// Resolves a single navigation property
        /// </summary>
        /// <typeparam name="T">Navigation type</typeparam>
        /// <param name="urlResource">The single navigation object to resolve</param>
        /// <returns>A resolved object</returns>
        public async Task<T> GetResourceAsync<T>(UrlNavigation<T> urlResource)
            where T : ResourceBase
        {
            _logger.LogInformation("Starting GetResourceAsync for {0}", typeof(T).ToString());
            return await GetResourceByUrlAsync<T>(urlResource.Url, CancellationToken.None);
        }

        /// <summary>
        /// Resolves a single navigation property
        /// </summary>
        /// <typeparam name="T">Navigation type</typeparam>
        /// <param name="urlResource">The single navigation object to resolve</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>A resolved object</returns>
        public async Task<T> GetResourceAsync<T>(UrlNavigation<T> urlResource, CancellationToken cancellationToken)
            where T : ResourceBase
        {
            return await GetResourceByUrlAsync<T>(urlResource.Url, cancellationToken);
        }

        


        /// <summary>
        /// Gets a single page of named resource data
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>The paged resource object</returns>
        public Task<NamedApiResourceList<T>> GetNamedResourcePageAsync<T>(int? limit =null,int? offset=null, CancellationToken cancellationToken = default)
            where T : NamedApiResource
        {
            _logger.LogInformation("Starting GetNamedResourcePageAsync for {0} param limit {1}, offset {2}", typeof(T).ToString(), limit, offset);
            string url = GetApiEndpointString<T>();
            return InternalGetNamedResourcePageAsync<T>(AddPaginationParamsToUrl(url, limit, offset), cancellationToken);
        }

        /// <summary>
        /// Gets the specified page of named resource data
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="limit">The number of resources in a list page</param>
        /// <param name="offset">Page offset</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>The paged resource object</returns>
        public Task<NamedApiResourceList<T>> GetNamedResourcePageAsync<T>(int limit, int offset, CancellationToken cancellationToken = default)
            where T : NamedApiResource
        {
            _logger.LogInformation("Starting GetNamedResourcePageAsync for {0} param limit {1}, offset {2}", typeof(T).ToString(), limit, offset);

            string url = GetApiEndpointString<T>();
                return InternalGetNamedResourcePageAsync<T>(AddPaginationParamsToUrl(url, limit, offset), cancellationToken);
        }

        private async Task<NamedApiResourceList<T>> InternalGetNamedResourcePageAsync<T>(string url, CancellationToken cancellationToken)
            where T : NamedApiResource
        {

            var resources = await GetAsync<NamedApiResourceList<T>>(url, cancellationToken);
                

            return resources;
        }

        /// <summary>
        /// Gets a single page of unnamed resource data
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>The paged resource object</returns>
        public Task<ApiResourceList<T>> GetApiResourcePageAsync<T>(CancellationToken cancellationToken = default)
            where T : ApiResource
        {
            _logger.LogInformation("Starting GetApiResourcePageAsync for {0} ", typeof(T).ToString());

            string url = GetApiEndpointString<T>();
            return InternalGetApiResourcePageAsync<T>(AddPaginationParamsToUrl(url), cancellationToken);
        }

        /// <summary>
        /// Gets the specified page of unnamed resource data
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="limit">The number of resources in a list page</param>
        /// <param name="offset">Page offset</param>
        /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
        /// <returns>The paged resource object</returns>
        public Task<ApiResourceList<T>> GetApiResourcePageAsync<T>(int limit, int offset, CancellationToken cancellationToken = default)
            where T : ApiResource
        {
            _logger.LogInformation("Starting GetNamedResourcePageAsync for {0} param limit {1}, offset {2}", typeof(T).ToString(), limit, offset);

            string url = GetApiEndpointString<T>();
            return InternalGetApiResourcePageAsync<T>(AddPaginationParamsToUrl(url, limit, offset), cancellationToken);
        }

        private async Task<ApiResourceList<T>> InternalGetApiResourcePageAsync<T>(string url, CancellationToken cancellationToken)
            where T : ApiResource
        {
            var resources = await GetAsync<ApiResourceList<T>>(url, cancellationToken);
                

            return resources;
        }

        /// <summary>
        /// Handles all outbound API requests to the PokeAPI server and deserializes the response
        /// </summary>
        private async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();
            return DeserializeStream<T>(await response.Content.ReadAsStreamAsync());
        }

        /// <summary>
        /// Handles deserialization of a given stream to a given type
        /// </summary>
        private T DeserializeStream<T>(System.IO.Stream stream)
        {
            using var sr = new System.IO.StreamReader(stream);
            using JsonReader reader = new JsonTextReader(sr);
            var serializer = JsonSerializer.Create();
            return serializer.Deserialize<T>(reader);
        }

        private static string AddPaginationParamsToUrl(string uri, int? limit = null, int? offset = null)
        {
            var queryParameters = new Dictionary<string, string>();

            // TODO consider to always set the limit parameter when not present to the default "20"
            // in order to have a single cached resource list for requests with explicit or implicit default limit
            if (limit.HasValue)
            {
                queryParameters.Add(nameof(limit), limit.Value.ToString());
            }
            
            if (offset.HasValue)
            {
                queryParameters.Add(nameof(offset), offset.Value.ToString());
            }

            return QueryHelpers.AddQueryString(uri, queryParameters);
        }

        private static string GetApiEndpointString<T>()
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty("ApiEndpoint", BindingFlags.Static | BindingFlags.NonPublic);
            return propertyInfo.GetValue(null).ToString();
        }
    }
}
