using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.Contracts.Responses;

namespace Chest.Client.Models.Responses
{
    public class GetAllLocalizedValuesResponse : PaginatedResponse<LocalizedValueByKeyContract>
    {
        public GetAllLocalizedValuesResponse([NotNull] IReadOnlyList<LocalizedValueByKeyContract> contents, int start, int size,
            int totalSize) : base(contents, start, size, totalSize)
        {
        }
    }
}