using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Library
{
    public class SessionData
    {
        private static Dictionary<string, IPrincipal> _currentThreadContext = new Dictionary<string, IPrincipal>();
        
        private static IHttpContextAccessor _httpContextAccessor;

        public SessionData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void CurrentPrincipalDispose()
        {
            string threadName = Thread.CurrentThread?.Name != null ? Thread.CurrentThread.Name.ToString() : string.Empty;

            lock(_currentThreadContext)
            {
                if(_currentThreadContext.ContainsKey(threadName))
                {
                    _currentThreadContext.Remove(threadName);  
                }
            }
        }

        public IPrincipal CurrentPrincipal
        {
            get
            {
                try
                {
                    IPrincipal principal;
                    string threadName = Thread.CurrentThread?.Name != null ? Thread.CurrentThread.Name.ToString() : string.Empty;

                    if (string.IsNullOrWhiteSpace(threadName))
                    {
                        return _httpContextAccessor.HttpContext.User;
                    }
                    lock(_currentThreadContext)
                    {
                        _currentThreadContext.TryGetValue(threadName, out principal); 
                    }
                    return principal ?? _httpContextAccessor.HttpContext.User;

                }
                catch (Exception)
                {
                    return _httpContextAccessor.HttpContext.User;
                }
            }
            set {
                string threadName = Thread.CurrentThread?.Name != null ? Thread.CurrentThread.Name.ToString() : string.Empty;
                if(!string.IsNullOrWhiteSpace(threadName) && _currentThreadContext != null)
                {
                    lock (_currentThreadContext)
                    {
                        if(!_currentThreadContext.ContainsKey(threadName))
                        {
                            _currentThreadContext.Add(threadName,value); 
                        }
                        else
                        {
                            _currentThreadContext[threadName] = value; 
                        }
                    }
                }
            }
        }

        public IIdentity? Identity
        {
            get
            {
                if(this.CurrentPrincipal.Identity?.IsAuthenticated ?? false)
                {
                    return this.CurrentPrincipal.Identity;
                }
                return null;
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                var claimsIdentity = (ClaimsIdentity) this.Identity;
                return claimsIdentity.Claims;
            }
        }

        public long ID
        {
            get
            {
                var claim = this.Claims.ToList().Find(x => x.Type == "id");
                if(claim == null)
                {
                    return 0;
                }
                return long.Parse(claim.Value);
            }
        }

        public long RoleID
        {
            get
            {
                var claim = this.Claims.ToList().Find(x => x.Type == "roleid");
                if (claim == null)
                {
                    return 0;
                }
                return long.Parse(claim.Value);
            }
        }

        public string Email
        {
            get
            {
                var claim = this.Claims.ToList().Find(x => x.Type == "email");
                if (claim == null)
                {
                    return "";
                }
                return claim.Value.ToString();
            }
        }

        public string Username
        {
            get
            {
                var claim = this.Claims.ToList().Find(x => x.Type == "username");
                if (claim == null)
                {
                    return "";
                }
                return claim.Value.ToString();
            }
        }
    }
}
