#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQL.Common.Response {

	/// <summary>
	/// Represents the error of a <see cref="GraphQLResponse"/>
	/// </summary>
	public class GraphQLError : IEquatable<GraphQLError?> {

		/// <summary>
		/// Additional error entries
		/// </summary>
		public IDictionary<string, dynamic>? Extensions { get; set; }

		/// <summary>
		/// The Location of an error
		/// </summary>
		public GraphQLLocation[]? Locations { get; set; }

		/// <summary>
		/// The error message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The Path of an error
		/// </summary>
		public dynamic[]? Path { get; set; }

		/// <summary>
		/// Initialize a new GraphQLError
		/// </summary>
		/// <param name="message">The Message</param>
		public GraphQLError(string message) {
			this.Message = message;
		}

		/// <inheritdoc />
		public override bool Equals(object? obj) => this.Equals(obj as GraphQLError);

		/// <inheritdoc />
		public bool Equals(GraphQLError? other) {
			if (other == null) { return false; }
			if (ReferenceEquals(this, other)) { return true; }
			if (!EqualityComparer<IDictionary<string, dynamic>?>.Default.Equals(this.Extensions, other.Extensions)) { return false; }
			{
				if(this.Locations!=null && other.Locations != null) {
					if (!Enumerable.SequenceEqual(this.Locations, other.Locations)) { return false; }
				}
				else if (this.Locations != null && other.Locations == null) { return false; }
				else if (this.Locations == null && other.Locations != null) { return false; }
			}
			if (!EqualityComparer<string>.Default.Equals(this.Message, other.Message)) { return false; }
			{
				if (this.Path != null && other.Path != null) {
					if (!Enumerable.SequenceEqual(this.Path, other.Path)) { return false; }
				}
				else if (this.Path != null && other.Path == null) { return false; }
				else if (this.Path == null && other.Path != null) { return false; }
			}
			return true;
		}

		/// <inheritdoc />
		public override int GetHashCode() {
			unchecked {
				var hashCode = EqualityComparer<IDictionary<string, dynamic>?>.Default.GetHashCode(this.Extensions);
				{
					if (this.Locations != null) {
						foreach(var element in this.Locations) {
							hashCode = (hashCode * 397) ^ EqualityComparer<GraphQLLocation?>.Default.GetHashCode(element);
						}
					}
					else {
						hashCode = (hashCode * 397) ^ 0;
					}
				}
				hashCode = (hashCode * 397) ^ EqualityComparer<string>.Default.GetHashCode(this.Message);
				{
					if (this.Path != null) {
						foreach (var element in this.Path) {
							hashCode = (hashCode * 397) ^ EqualityComparer<dynamic?>.Default.GetHashCode(element);
						}
					}
					else {
						hashCode = (hashCode * 397) ^ 0;
					}
				}
				return hashCode;
			}
		}

		/// <inheritdoc />
		public static bool operator ==(GraphQLError? error1, GraphQLError? error2) => EqualityComparer<GraphQLError?>.Default.Equals(error1, error2);

		/// <inheritdoc />
		public static bool operator !=(GraphQLError? error1, GraphQLError? error2) => !(error1 == error2);

	}

}
