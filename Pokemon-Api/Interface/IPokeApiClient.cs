using Pokemon_Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon_Api.Interface
{
    public interface IPokeApiClient
    {
       // Task<T> GetResourceAsync<T>(int id) where T : ResourceBase;

       // Task<T> GetResourceAsync<T>(int id, CancellationToken cancellationToken);

       // Task<T> GetResourceAsync<T>(string name);

       // Task<T> GetResourceAsync<T>(string name, CancellationToken cancellationToken);
       // Task<List<T>> GetResourceAsync<T>(IEnumerable<UrlNavigation<T>> collection) where T : ResourceBase;

       //Task<List<T>> GetResourceAsync<T>(IEnumerable<UrlNavigation<T>> collection, CancellationToken cancellationToken)            where T : ResourceBase;
       // Task<T> GetResourceAsync<T>(UrlNavigation<T> urlResource)            where T : ResourceBase;
       // Task<T> GetResourceAsync<T>(UrlNavigation<T> urlResource, CancellationToken cancellationToken)            where T : ResourceBase;
       
    }
}
