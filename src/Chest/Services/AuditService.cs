using System;
using System.Threading.Tasks;
using Chest.Data.Repositories;
using Chest.Models.v2;
using Chest.Models.v2.Audit;
using JsonDiffPatchDotNet;
using Lykke.Snow.Common;

namespace Chest.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public Task<PaginatedResponse<IAuditModel>> GetAll(AuditLogsFilterDto filter, int? skip, int? take)
        {
            (skip, take) = PaginationUtils.ValidateSkipAndTake(skip, take);

            return _auditRepository.GetAll(filter, skip, take);
        }

        public async Task<bool> TryAudit(
            string correlationId,
            string userName,
            string referenceId,
            AuditDataType type,
            string newStateJson = null,
            string oldStateJson = null)
        {
            if (string.IsNullOrEmpty(newStateJson) && string.IsNullOrEmpty(oldStateJson))
            {
                return false;
            }

            var auditModel = BuildAuditModel(correlationId, userName, DateTime.UtcNow, referenceId, type, newStateJson, oldStateJson);

            if (auditModel == null)
                return false;

            await _auditRepository.InsertAsync(auditModel);

            return true;
        }

        private static IAuditModel BuildAuditModel(
            string correlationId,
            string userName,
            DateTime timestamp,
            string referenceId,
            AuditDataType dataType,
            string newStateJson,
            string oldStateJson)
        {
            var eventType = AuditEventType.Edition;

            if (string.IsNullOrEmpty(oldStateJson))
            {
                eventType = AuditEventType.Creation;
                oldStateJson = "{}";
            }

            if (string.IsNullOrEmpty(newStateJson))
            {
                eventType = AuditEventType.Deletion;
                newStateJson = "{}";
            }

            var jdp = new JsonDiffPatch();
            var diffResult = jdp.Diff(oldStateJson, newStateJson);

            if (string.IsNullOrEmpty(diffResult))
                return null;

            return new AuditModel
            {
                Timestamp = timestamp,
                CorrelationId = correlationId,
                UserName = userName,
                Type = eventType,
                DataType = dataType,
                DataReference = referenceId,
                DataDiff = diffResult
            };
        }
    }
}