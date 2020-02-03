using ApiLdapIntegration.Models;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;

namespace ApiLdapIntegration.Services
{
    public class LdapAuthenticationService : IAuthenticationService
    {
        private const string Email = "mail";
        private const string DisplayName = "displayName";
        private const string UserName = "givenName";

        private readonly LdapConfig _config;

        public LdapAuthenticationService(IOptions<LdapConfig> options)
        {
            _config = options.Value;
        }

        public AuthResult Login(string userName, string password)
        {
            using LdapConnection _connection = new LdapConnection { SecureSocketLayer = _config.SecureSocketLayer };
            try
            {
                _connection.Connect(_config.ServerHost, _config.ServerPort);
                _connection.Bind(_config.BindDN, _config.BindPassword);

                string userFilter = string.Format(_config.UserFilter, userName);
                ILdapSearchResults result = _connection.Search(
                    _config.BaseDN,
                    LdapConnection.ScopeSub,
                    userFilter,
                    new[] { DisplayName, Email, UserName },
                    false
                );

                /* 
                 * WARNING: Do not check result.Count == 0;
                 * "Count doesn't return "correctly" because is not blocking and doesn't wait to get the results and is 
                 * returning whatever is available at that moment. It is true that this behavior 
                 * is not the most expected one :) - and it may have an easy fix.
                 * It will return correctly after calling hasMore - which is blocking (e.g. wait for the result).
                 * Probably will be useful to make the "async" methods match the .net style. 
                 * And even make the sync methods to return IEnumerable as will make the usage easier. Happy to take pull requests :)"
                 * https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard/issues/4
                */
                if (!result.HasMore())
                {
                    return new AuthResult
                    {
                        Errors = new List<string> { "Invalid user" }
                    };
                }

                LdapEntry user = result.Next();
                _connection.Bind(user.Dn, password);

                if (_connection.Bound)
                {
                    return new AuthResult
                    {
                        AppUser = new AppUser
                        {
                            Email = user.GetAttribute(Email).StringValue,
                            DisplayName = user.GetAttribute(DisplayName).StringValue,
                            UserName = user.GetAttribute(UserName).StringValue,
                        }
                    };
                }
                else
                {
                    return new AuthResult
                    {
                        Errors = new List<string> { "Invalid user" }
                    };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
