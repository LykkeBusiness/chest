﻿using System.Threading.Tasks;
using Chest.Client.Models;
using Chest.Client.Models.Requests;
using Chest.Client.Models.Responses;
using Lykke.Contracts.Responses;
using Refit;

namespace Chest.Client.Api
{
    /// <summary>
    /// Manages localized values
    /// </summary>
    public interface ILocalizedValuesApi
    {
        /// <summary>
        /// Adds new localized value
        /// </summary>
        [Post("/api/v2/localized-values")]
        Task<ErrorCodeResponse<LocalizedValuesErrorCodesContract>> Add([Body] AddLocalizedValueRequest value);
        
        /// <summary>
        /// Updates existing localized value by locale and key
        /// </summary>
        [Put("/api/v2/localized-values/{locale}/{key}")]
        Task<ErrorCodeResponse<LocalizedValuesErrorCodesContract>> Update(string locale, string key, [Body] UpdateLocalizedValueRequest value);
        
        /// <summary>
        /// Deletes existing localized value
        /// </summary>
        [Delete("/api/v2/localized-values/{locale}/{key}")]
        Task<ErrorCodeResponse<LocalizedValuesErrorCodesContract>> Delete(string locale, string key, [Body] DeleteLocalizedValueRequest request);
        
        /// <summary>
        /// Gets localized value by locale and key
        /// </summary>
        [Get("/api/v2/localized-values/{locale}/{key}")]
        Task<GetLocalizedValueResponse> Get(string locale, string key);
        
        /// <summary>
        /// Gets all localized values by locale
        /// </summary>
        [Get("/api/v2/localized-values/{locale}")]
        Task<GetLocalizedValuesByLocaleResponse> Get(string locale);

        /// <summary>
        /// Gets all localized values as a flat object. Sorts by key in [asc] order
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [Get("/api/v2/localized-values")]
        Task<GetAllLocalizedValuesResponse> Get(int skip = default, int take = 0);

        /// <summary>
        /// Upserts all localized values that match a specified key
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Put("/api/v2/localized-values")]
        Task<UpsertLocalizedValuesByKeyErrorCodeResponse> UpsertByKey(
            UpsertLocalizedValuesByKeyRequest request);
    }
}