// This file is part of the nexstandard YamlDotNet fork - A .NET library for YAML.
// Copyright (c) yfons123@gmail.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


using System;
using System.Collections.Generic;
using System.IO;

namespace YamlDotNet.RepresentationModel
{
    /// <summary>
    /// Defines a contract for classes that can serialize and deserialize YAML documents to and from objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize and deserialize.</typeparam>
    public interface IYamlSerializer<T>
        where T : new()
    {
        YamlMappingNode ConvertToYaml(T obj);
        /// <summary>
        /// Deserializes each <see cref="YamlDocument"/> in the provided <see cref="TextReader"/> to an object of type <see cref="T"/>.
        /// If the <see cref="TextReader"/> contains only one <see cref="YamlDocument"/>, the resulting <see cref="IEnumerable{T}"/> will contain only one element.
        /// </summary>
        /// <remarks>
        /// If there are no <see cref="YamlDocument"/>s in the <see cref="TextReader"/>, it will return <see cref="Enumerable.Empty{T}"/>.
        /// </remarks>
        /// <param name="reader">The <see cref="TextReader"/> containing the YAML to deserialize.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains all objects of the converted <see cref="YamlDocument"/>s.</returns>
        public IEnumerable<T> DeserializeMany(TextReader reader);
        /// <summary>
        /// Converts a <see cref="YamlMappingNode"/> to an object of type <see cref="T"/>.
        /// This method is used for recursive conversion of a nested <see cref="YamlMappingNode"/>.
        /// </summary>
        /// <param name="node">The <see cref="YamlMappingNode"/> to convert to an object of type <see cref="T"/>.</param>
        /// <returns>An object of type <see cref="T"/> that represents the converted <see cref="YamlMappingNode"/>.</returns>
        public T Deserialize(YamlMappingNode node);
        /// <summary>
        /// Deserializes the first <see cref="YamlDocument"/> in the provided <see cref="TextReader"/> to an object of type <see cref="T"/>.
        /// </summary>
        /// <remarks>
        /// The entire <see cref="TextReader"/> will be read.
        /// </remarks>
        /// <param name="reader">The <see cref="TextReader"/> containing the YAML to deserialize.</param>
        /// <returns>An object of type <see cref="T"/> that represents the deserialized YAML document.</returns>
        public T Deserialize(TextReader reader);
        public string IdentifierTag { get; }

        public Type IdentifierType { get; }
    }
}
