using Eisk.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WS.Authorizers;

namespace WS.Interfaces
{
    public interface IAccessAuthorizer
    {
        // 3 types of Access
        // 1) RequestToken = with Device ID return a RequestToken
        // 2) AccessToken  = with RequestToken and UserName:Password return AccessToken
        // 3) RetreiveData = with AccessToken retreive date (IMPORTANT: Need Authorization)
        AccessAuthorizerResponse IsAuthorizedRequestToken(HttpRequestMessage request);
        AccessAuthorizerResponse IsAuthorizedAccessToken(HttpRequestMessage request);
        AccessAuthorizerResponse IsAuthorizedRetreiveData(HttpRequestMessage request, List<Role> roles);
    }
}
