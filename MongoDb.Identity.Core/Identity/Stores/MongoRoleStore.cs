using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDb.Identity.Core.Identity.Stores
{
    public class MongoRoleStore<TRole, TKey> : IRoleStore<TRole>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoCollection<TRole> _roles;

        public MongoRoleStore(MongoTablesFactory proxyTables)
        {
            _roles = proxyTables.GetCollection<TRole>(MongoTablesFactory.TABLE_ROLES);
        }

        public virtual void Dispose()
        {
            // no need to dispose of anything, mongodb handles connection pooling automatically
        }

        public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken token)
        {
            await _roles.InsertOneAsync(role, cancellationToken: token);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
        {
            var result = await _roles.ReplaceOneAsync(r => r.Id.Equals(role.Id), role, cancellationToken: token);

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
        {
            var result = await _roles.DeleteOneAsync(r => r.Id.Equals(role.Id), token);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Id.ToString());

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Name);

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.NormalizedName);

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public virtual Task<TRole> FindByIdAsync(string roleId, CancellationToken token)
            => _roles.Find(r => r.Id.Equals(roleId))
                .FirstOrDefaultAsync(token);

        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
            => _roles.Find(r => r.NormalizedName == normalizedName)
                .FirstOrDefaultAsync(token);
    }
}
