using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDb.Identity.Core.Identity.Stores
{
    public class MongoUserStore<TUser, TKey> : IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserLockoutStore<TUser>,
        IProtectedUserStore<TUser>
        where TUser : IdentityUser<TKey>, IIdentityUserRole
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoCollection<TUser> _users;
        private readonly ILookupNormalizer _normalizer;
        public IQueryable<TUser> Users => throw new NotImplementedException();

        public MongoUserStore(MongoTablesFactory proxyTables, ILookupNormalizer normalizer)
        {
            _users = proxyTables.GetCollection<TUser>(MongoTablesFactory.TABLE_USERS);
            _normalizer = normalizer;
        }

        public virtual void Dispose()
        {
            //do nothing here
        }

        public virtual async Task<IdentityResult> CreateAsync(TUser user, CancellationToken token)
        {
            await _users.InsertOneAsync(user, cancellationToken: token);
            return IdentityResult.Success;
        }
        private Task<TUser> ByIdAsync(TKey id, CancellationToken cancellationToken)
        {
            return _users.AsQueryable().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }
        public virtual async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken token)
        {
            await _users.ReplaceOneAsync(u => u.Id.Equals(user.Id), user, cancellationToken: token);
            return IdentityResult.Success;
        }
        private async Task UpdateAsync<TFieldValue>(TUser user, Expression<Func<TUser, TFieldValue>> expression, TFieldValue value, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var updateDefinition = Builders<TUser>.Update.Set(expression, value);

            await _users.UpdateOneAsync(x => x.Id.Equals(user.Id), updateDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            await _users.DeleteOneAsync(u => u.Id.Equals(user.Id), token);
            return IdentityResult.Success;
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedUserName);

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedUserName;
            return Task.CompletedTask;
        }


        public virtual Task<TUser> FindByIdAsync(string userId, CancellationToken token)
            => IsObjectId(userId)
                ? _users.Find(u => u.Id.Equals(userId)).FirstOrDefaultAsync(token)
                : Task.FromResult<TUser>(null);


        private bool IsObjectId(string id)
        {
            ObjectId temp;
            return ObjectId.TryParse(id, out temp);
        }


        public virtual Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken token)
            => _users.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(token);

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(false);

        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.AddRole(roleName);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.RemoveRole(roleName);
            return Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            var roles = user.Roles?.ToArray() ?? Array.Empty<string>();
            return await Task.FromResult(roles);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(user, cancellationToken);
            return roles.Contains(roleName);
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;

            await UpdateAsync(user, x => x.SecurityStamp, user.SecurityStamp, cancellationToken);
        }

        public async Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(true))?.SecurityStamp ?? user.SecurityStamp;
        }

        public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            await SetNormalizedEmailAsync(user, _normalizer.NormalizeEmail(user.Email), cancellationToken).ConfigureAwait(false);

            user.Email = email;

            await UpdateAsync(user, x => x.Email, user.Email, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(false))?.Email ?? user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(false))?.EmailConfirmed ?? user.EmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.EmailConfirmed = confirmed;

            await UpdateAsync(user, x => x.EmailConfirmed, confirmed, cancellationToken);
        }

        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _users.AsQueryable().FirstOrDefaultAsync(a => a.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(true))?.NormalizedEmail ?? user.NormalizedEmail;
        }

        public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedEmail = normalizedEmail ?? _normalizer.NormalizeEmail(user.Email);

            await UpdateAsync(user, x => x.NormalizedEmail, user.NormalizedEmail, cancellationToken);
        }

        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumber = phoneNumber;
            await UpdateAsync(user, x => x.PhoneNumber, phoneNumber, cancellationToken);
        }

        public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(true))?.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(true))?.PhoneNumberConfirmed ?? false;
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumberConfirmed = confirmed;
            return UpdateAsync(user, x => x.PhoneNumberConfirmed, confirmed, cancellationToken);
        }

        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.TwoFactorEnabled = enabled;

            await UpdateAsync(user, x => x.TwoFactorEnabled, enabled, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            var foundUser = await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(true);

            return foundUser?.TwoFactorEnabled ?? user.TwoFactorEnabled;
        }


        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();
            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(false))?.LockoutEnd ?? user.LockoutEnd;
        }

        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.LockoutEnd = lockoutEnd;
            await UpdateAsync(user, x => x.LockoutEnd, user.LockoutEnd, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.AccessFailedCount++;
            await UpdateAsync(user, x => x.AccessFailedCount, user.AccessFailedCount, cancellationToken).ConfigureAwait(false);
            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.AccessFailedCount = 0;
            await UpdateAsync(user, x => x.AccessFailedCount, 0, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(false))?.AccessFailedCount ?? user.AccessFailedCount;
        }

        public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            return (await ByIdAsync(user.Id, cancellationToken).ConfigureAwait(false))?.LockoutEnabled ?? user.LockoutEnabled;
        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            user.LockoutEnabled = enabled;
            await UpdateAsync(user, x => x.LockoutEnabled, user.LockoutEnabled, cancellationToken).ConfigureAwait(false);
        }




    }
}
