using System.Threading.Tasks;
using Chest.Client.Models;
using Chest.Client.Models.Requests;
using Lykke.Contracts.Responses;
using Refit;

namespace Chest.Client.Api
{
    public interface IAuditApi
    {
        /// <summary>
        /// Get audit logs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [Get("/api/v2/audit")]
        Task<PaginatedResponse<AuditContract>> GetAuditTrailAsync([Query] GetAuditLogsRequest request,
            int? skip = null, int? take = null);
    }
}