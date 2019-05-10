using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using IdentityServer4.ResponseHandling;
using IdentityServer4Extras.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TokenExchange.Contracts;
using TokenMintingService;

namespace UnitTestProject_TokenExchange
{
    [TestClass]
    public class SimpleObjectCoverage
    {
        public static string GuidString => Guid.NewGuid().ToString();
        [TestMethod]
        public void TokenMintingService_ToTokenMintingResponse()
        {
            var tokenRawResult = new TokenRawResult
            {
                TokenErrorResult =
                    new TokenErrorResult(new TokenErrorResponse()
                    {
                        Custom = new Dictionary<string, object>() { { GuidString, GuidString } },
                        Error = GuidString,
                        ErrorDescription = GuidString
                    }),
                TokenResult = new TokenResult(new TokenResponseExtra()
                {
                    AccessToken = GuidString,
                    IdentityToken = GuidString,
                    RefreshToken = GuidString,
                    AccessTokenLifetime = 3,
                    Custom = new Dictionary<string, object>() { { GuidString, GuidString } }
                })
            };
            var kk = InProcTokenMintingService.ToTokenMintingResponse(tokenRawResult);
            kk.ShouldNotBeNull();

            tokenRawResult = new TokenRawResult
            {
                TokenErrorResult =
                   new TokenErrorResult(new TokenErrorResponse()),
                TokenResult = new TokenResult(new TokenResponseExtra())
            };
            kk = InProcTokenMintingService.ToTokenMintingResponse(tokenRawResult);
            kk.ShouldNotBeNull();
        }

        [TestMethod]
        public void TokenMintingService_ToArbitraryIdentityRequest_valid_clientId()
        {
            var fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => fakeConfiguration["inProcTokenMintingService:clientId"]).Returns(GuidString);
            var inProcTokenMintingService = new InProcTokenMintingService(fakeConfiguration, null, null);
            var d = inProcTokenMintingService.ToArbitraryIdentityRequest(new IdentityTokenRequest()
            {
                AccessTokenLifetime = 2,
                Subject = GuidString,
                Scope = $"a b c"
            });
            d.ShouldNotBeNull();

        }
        [TestMethod]
        public void TokenMintingService_ToArbitraryIdentityRequest_valid_clientId_IdentityTokenRequest()
        {
            var fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => fakeConfiguration["inProcTokenMintingService:clientId"]).Returns(null);
            var inProcTokenMintingService = new InProcTokenMintingService(fakeConfiguration, null, null);
            var d = inProcTokenMintingService.ToArbitraryIdentityRequest(new IdentityTokenRequest()
            {
                ClientId = GuidString,
                AccessTokenLifetime = 2,
                Subject = GuidString,
                Scope = $"a b c"
            });
            d.ShouldNotBeNull();

        }
        [TestMethod]
        public void TokenMintingService_ToArbitraryIdentityRequest_clientIds_all_null()
        {
            var fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => fakeConfiguration["inProcTokenMintingService:clientId"]).Returns(null);
            var inProcTokenMintingService = new InProcTokenMintingService(fakeConfiguration, null, null);
            Should.Throw<Exception>(() =>
            {
                inProcTokenMintingService.ToArbitraryIdentityRequest(new IdentityTokenRequest()
                {
                    ClientId = null,
                    AccessTokenLifetime = 2,
                    Subject = GuidString,
                    Scope = $"a b c"
                });
            });
            Should.Throw<Exception>(() =>
            {
                inProcTokenMintingService.ToArbitraryIdentityRequest(null);
            });
        }
        [TestMethod]
        public void TokenMintingService_ToArbitraryResourceOwnerRequest_valid_clientId()
        {
            var fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => fakeConfiguration["inProcTokenMintingService:clientId"]).Returns(GuidString);
            var inProcTokenMintingService = new InProcTokenMintingService(fakeConfiguration, null, null);
            var d = inProcTokenMintingService.ToArbitraryResourceOwnerRequest(new ResourceOwnerTokenRequest()
            {
                AccessTokenLifetime = 2,
                Subject = GuidString,
                Scope = $"a b c"
            });
            d.ShouldNotBeNull();



        }
        [TestMethod]
        public void TokenMintingService_ToArbitraryResourceOwnerRequest_valid_clientId_resourceTokenRequest()
        {
            var fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => fakeConfiguration["inProcTokenMintingService:clientId"]).Returns(null);
            var inProcTokenMintingService = new InProcTokenMintingService(fakeConfiguration, null, null);
            var d = inProcTokenMintingService.ToArbitraryResourceOwnerRequest(new ResourceOwnerTokenRequest()
            {
                ClientId = GuidString,
                AccessTokenLifetime = 2,
                Subject = GuidString,
                Scope = $"a b c"
            });
            d.ShouldNotBeNull();

        }
        [TestMethod]
        public void TokenMintingService_ToArbitraryResourceOwnerRequest_clientIds_all_null()
        {
            var fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => fakeConfiguration["inProcTokenMintingService:clientId"]).Returns(null);
            var inProcTokenMintingService = new InProcTokenMintingService(fakeConfiguration, null, null);
            Should.Throw<Exception>(() =>
            {
                inProcTokenMintingService.ToArbitraryResourceOwnerRequest(new ResourceOwnerTokenRequest()
                {
                    ClientId = null,
                    AccessTokenLifetime = 2,
                    Subject = GuidString,
                    Scope = $"a b c"
                });
            });
            Should.Throw<Exception>(() =>
            {
                inProcTokenMintingService.ToArbitraryResourceOwnerRequest(null);
            });

        }
    }
}
