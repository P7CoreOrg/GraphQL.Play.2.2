using System;
using System.Collections.Generic;
using FakeItEasy;
using GraphQLPlay.IdentityModelExtras;
using IdentityServer4.ResponseHandling;
using IdentityServer4Extras.Endpoints;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Shouldly;
using TokenExchange.Contracts;
using TokenMintingService;
using Utils.Models;
using Xunit;

namespace XUnitServer_TokenExchange
{
    public class SimpleObjectCoverage
    {
        public static string GuidString => Guid.NewGuid().ToString();


        [Fact]
        public void Utils_WellknownAuthority()
        {
            var dd = new WellknownAuthority()
            {
                Scheme = SimpleObjectCoverage.GuidString,
                Authority = SimpleObjectCoverage.GuidString,
                AdditionalEndpointBaseAddresses = new List<string>()
                {
                    SimpleObjectCoverage.GuidString
                }
            };
            dd.Scheme.ShouldNotBeNullOrWhiteSpace();
            dd.Authority.ShouldNotBeNullOrWhiteSpace();
            dd.AdditionalEndpointBaseAddresses.ShouldNotBeNull();
            dd.AdditionalEndpointBaseAddresses.Any().ShouldBeTrue();
        }

        [Fact]
        public void Utils_Header()
        {
            var dd = new HttpHeader()
            {
                Name = SimpleObjectCoverage.GuidString,
                Value = SimpleObjectCoverage.GuidString
            };
            dd.ToString().ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
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

        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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