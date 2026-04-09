using Api.Model.Common;

namespace Api.Interface.Foundation
{
    public interface IVaultFoundationService
    {
        ValueTask AddLoginAsync(CancellationToken cancellationToken = default);
        ValueTask<AppSettings> RetrieveVaultRootSecretAsync(CancellationToken cancellationToken = default);
        ValueTask<AppSettings> RetrieveVaultPostgresCredentialAsync(string databaseEngineName, string databaseEngineRole, CancellationToken cancellationToken = default);
    }
}