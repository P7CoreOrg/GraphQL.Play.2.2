using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GraphQL.Client.Http {

	/// <summary>
	/// The Options that the <see cref="GraphQLHttpClient"/> will use
	/// </summary>
	public class GraphQLHttpClientOptions {

		/// <summary>
		/// The GraphQL EndPoint to be used
		/// </summary>
		public Uri EndPoint { get; set; }

		/// <summary>
		/// The <see cref="Newtonsoft.Json.JsonSerializerSettings"/> that is going to be used
		/// </summary>
		public JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings {
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new List<JsonConverter>
			{
				new StringEnumConverter()
			}
		};

		/// <summary>
		/// The <see cref="System.Net.Http.HttpMessageHandler"/> that is going to be used
		/// </summary>
		public HttpMessageHandler HttpMessageHandler { get; set; } = new HttpClientHandler();

		/// <summary>
		/// The <see cref="MediaTypeHeaderValue"/> that will be send on POST
		/// </summary>
		public MediaTypeHeaderValue MediaType { get; set; } = MediaTypeHeaderValue.Parse("application/json; charset=utf-8"); // This should be "application/graphql" also "application/x-www-form-urlencoded" is Accepted

	}

}
