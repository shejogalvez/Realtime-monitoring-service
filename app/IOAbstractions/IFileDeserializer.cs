using System.Text.Json;

namespace app.IOAbstractions
{
    public interface IJsonFileDeserializer
    {
        /// <summary>
        /// Open, reads file in <typeparamref name="filePath"/> and converts containing a single JSON value into a <typeparamref name="TValue"/>.
        /// The Stream will be read to completion.
        /// </summary>
        /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
        /// <returns>A <typeparamref name="TValue"/> representation of the JSON value.</returns>
        /// <param name="filePath">JSON data to parse.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="filePath"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="JsonException">
        /// The JSON is invalid,
        /// <typeparamref name="TValue"/> is not compatible with the JSON,
        /// or when there is remaining data in the Stream.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
        /// for <typeparamref name="TValue"/> or its serializable members.
        /// </exception>
        /// <exception cref="ArgumentException"> </exception>
        /// <exception cref="PathTooLongException"> </exception>
        /// <exception cref="DirectoryNotFoundException"> </exception>
        /// <exception cref="IOException"> </exception>
        /// <exception cref="UnauthorizedAccessException"> </exception>
        /// <exception cref="ArgumentOutOfRangeException"> </exception>
        /// <exception cref="FileNotFoundException"> </exception>
        /// <exception cref="NotSupportedException"> </exception>
        /// <exception cref="ArgumentException"> </exception>
        /// <exception cref="ArgumentNullException"> </exception>
        /// <exception cref="PathTooLongException"> </exception>
        /// <exception cref="DirectoryNotFoundException"> </exception>
        /// <exception cref="IOException"> </exception>
        /// <exception cref="UnauthorizedAccessException"> </exception>
        /// <exception cref="ArgumentOutOfRangeException"> </exception>
        /// <exception> cref="FileNotFoundException"> </exception>
        /// <exception> cref="NotSupportedException"> </exception>
        public TValue? Deserialize<TValue>(string filePath);
        
        /// <summary>
        /// Converts the <see cref="JsonElement"/> representing a single JSON value into a <paramref name="returnType"/>.
        /// </summary>
        /// <returns>A <paramref name="returnType"/> representation of the JSON value.</returns>
        /// <param name="element">The <see cref="JsonElement"/> to convert.</param>
        /// <param name="returnType">The type of the object to convert to and return.</param>
        /// <param name="options">Options to control the behavior during parsing.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="returnType"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="JsonException">
        /// <paramref name="returnType"/> is not compatible with the JSON.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
        /// for <paramref name="returnType"/> or its serializable members.
        /// </exception>
        public object? Deserialize(JsonElement element, Type returnType, JsonSerializerOptions options);
    }
}