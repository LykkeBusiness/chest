// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Chest.Client.AutorestClient
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Metadata.
    /// </summary>
    public static partial class MetadataExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='key'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<MetadataModel> GetAsync(this IMetadata operations, string category, string collection, string key, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(category, collection, key, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='key'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateAsync(this IMetadata operations, string category, string collection, string key, MetadataModel model = default(MetadataModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateWithHttpMessagesAsync(category, collection, key, model, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='key'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AddAsync(this IMetadata operations, string category, string collection, string key, MetadataModel model = default(MetadataModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.AddWithHttpMessagesAsync(category, collection, key, model, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='key'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task RemoveAsync(this IMetadata operations, string category, string collection, string key, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.RemoveWithHttpMessagesAsync(category, collection, key, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='keyword'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IDictionary<string, string>> GetKeysWithDataAsync(this IMetadata operations, string category, string collection, string keyword = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetKeysWithDataWithHttpMessagesAsync(category, collection, keyword, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BulkAddAsync(this IMetadata operations, string category, string collection, IDictionary<string, MetadataModel> model = default(IDictionary<string, MetadataModel>), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BulkAddWithHttpMessagesAsync(category, collection, model, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='keys'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BulkRemoveAsync(this IMetadata operations, string category, string collection, IList<string> keys = default(IList<string>), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BulkRemoveWithHttpMessagesAsync(category, collection, keys, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BulkUpdateAsync(this IMetadata operations, string category, string collection, IDictionary<string, MetadataModel> model = default(IDictionary<string, MetadataModel>), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BulkUpdateWithHttpMessagesAsync(category, collection, model, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<string>> GetCategoriesAsync(this IMetadata operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetCategoriesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<string>> GetCollectionsAsync(this IMetadata operations, string category, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetCollectionsWithHttpMessagesAsync(category, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='category'>
            /// </param>
            /// <param name='collection'>
            /// </param>
            /// <param name='keys'>
            /// </param>
            /// <param name='keyword'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IDictionary<string, string>> FindByKeysAsync(this IMetadata operations, string category, string collection, IList<string> keys = default(IList<string>), string keyword = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.FindByKeysWithHttpMessagesAsync(category, collection, keys, keyword, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
