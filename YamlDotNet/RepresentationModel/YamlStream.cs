﻿// This file is part of YamlDotNet - A .NET library for YAML.
// Copyright (c) Antoine Aubry and contributors
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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
    /// <summary>
    /// Represents an YAML stream.
    /// </summary>
    public class YamlStream : IEnumerable<YamlDocument>
    {
        private readonly List<YamlDocument> documents;

        /// <summary>
        /// Gets the documents inside the stream.
        /// </summary>
        /// <value>The documents.</value>
        public List<YamlDocument> Documents
        {
            get
            {
                return documents;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlStream"/> class.
        /// </summary>
        public YamlStream()
        {
            documents = new List<YamlDocument>();
        }
        public YamlStream(int size)
        {
            documents = new List<YamlDocument>(size);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlStream"/> class.
        /// </summary>
        public YamlStream(params YamlDocument[] documents)
            : this(documents.ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlStream"/> class.
        /// </summary>
        public YamlStream(IList<YamlDocument> documents)
        {
            this.documents = documents;
        }

        /// <summary>
        /// Adds the specified document to the <see cref="Documents"/> collection.
        /// </summary>
        /// <param name="document">The document.</param>
        public void Add(YamlDocument document)
        {
            documents.Add(document);
        }

        /// <summary>
        /// Loads the stream from the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Load(TextReader input)
        {
            Load(new Parser(input));
        }

        /// <summary>
        /// Loads the stream from the specified <see cref="IParser"/>.
        /// </summary>
        public void Load(IParser parser)
        {
            documents.Clear();
            parser.Consume<StreamStart>();
            while (!parser.TryConsume<StreamEnd>(out var _))
            {
                var document = new YamlDocument(parser);
                documents.Add(document);
            }
        }

        /// <summary>
        /// Saves the stream to the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public void Save(TextWriter output)
        {
            Save(output, true);
        }

        /// <summary>
        /// Saves the stream to the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="assignAnchors">Indicates whether or not to assign node anchors.</param>
        public void Save(TextWriter output, bool assignAnchors)
        {
            Save(new Emitter(output), assignAnchors);
        }

        /// <summary>
        /// Saves the stream to the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="assignAnchors">Indicates whether or not to assign node anchors.</param>
        public void Save(IEmitter emitter, bool assignAnchors)
        {
            emitter.Emit(new StreamStart());

            for (int i=0; i < documents.Count; i++)
            {
                documents[i].Save(emitter, assignAnchors);
            }

            emitter.Emit(new StreamEnd());
        }

        /// <summary>
        /// Accepts the specified visitor by calling the appropriate Visit method on it.
        /// </summary>
        /// <param name="visitor">
        /// A <see cref="IYamlVisitor"/>.
        /// </param>
        public void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        #region IEnumerable<YamlDocument> Members

        /// <summary />
        public IEnumerator<YamlDocument> GetEnumerator()
        {
            return documents.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
