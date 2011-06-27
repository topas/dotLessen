//-----------------------------------------------------------------------
// <copyright file="StringTokenizer.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokenizers
{
    /// <summary>
    /// Tokenizer with string input
    /// </summary>
    public class StringTokenizer : ITokenizerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="text">The text for tokenizing.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <param name="localTextOffset">The local text offset.</param>
        /// <param name="options">The options.</param>
        public StringTokenizer(string text, int start, int end, int localTextOffset, TokenizerOptions options) 
            : base(localTextOffset, options)
        {
            this.Text = text;
            this.Start = start;
            this.End = end;

            this.Next = this.Start;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="text">The text for tokenizing.</param>
        /// <param name="localTextOffset">The local text offset.</param>
        public StringTokenizer(string text, int localTextOffset)
            : this(text, 0, text.Length, localTextOffset, TokenizerOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="text">The text for tokenizing.</param>
        /// <param name="localTextOffset">The local text offset.</param>
        /// <param name="options">The options.</param>
        public StringTokenizer(string text, int localTextOffset, TokenizerOptions options)
            : this(text, 0, text.Length, localTextOffset, options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="text">The text for tokenizing.</param>
        public StringTokenizer(string text)
            : this(text, 0, text.Length, 0, TokenizerOptions.None)
        {
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        protected string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start index of text.
        /// </value>
        protected int Start
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end index of text.
        /// </value>
        protected int End
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the next.
        /// </summary>
        /// <value>
        /// The next index.
        /// </value>
        protected int Next
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public override void Reset()
        {
            this.Next = this.Start;
        }

        /// <summary>
        /// Determines whether [has more char].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has more char]; otherwise, <c>false</c>.
        /// </returns>
        protected override bool HasMoreChar()
        {
            return this.Next < this.End;
        }

        /// <summary>
        /// Determines whether [has more char] [the specified offset].
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>
        ///   <c>true</c> if [has more char] [the specified offset]; otherwise, <c>false</c>.
        /// </returns>
        protected override bool HasMoreChar(int offset)
        {
            return this.Next + offset < this.End;
        }

        /// <summary>
        /// Gets the current char.
        /// </summary>
        /// <returns>Gets current char</returns>
        protected override char GetCurrentChar()
        {
            return this.Text[this.Next];
        }

        /// <summary>
        /// Gets the char relative.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Gets relative char</returns>
        protected override char GetCharRelative(int offset)
        {
            return this.Text[this.Next + offset];
        }

        /// <summary>
        /// Gets the current offset.
        /// </summary>
        /// <returns>Current text offset</returns>
        protected override int GetCurrentOffset()
        {
            return this.Next;
        }

        /// <summary>
        /// Advances this instance.
        /// </summary>
        protected override void Advance()
        {
            this.Advance(1, false);
        }

        /// <summary>
        /// Advances the specified by.
        /// </summary>
        /// <param name="by">The by index.</param>
        /// <param name="flush">if set to <c>true</c> [flush].</param>
        protected override void Advance(int by, bool flush)
        {
            this.Next += by;
            if (flush)
            {
                this.Flush(this.Next);
            } 
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="from">Text Index From.</param>
        /// <param name="to">Text Index To.</param>
        /// <returns>Text between range</returns>
        protected override string GetText(int from, int to)
        {
            return this.Text.Substring(from, to - from);
        }

        /// <summary>
        /// Flushes the specified up to index.
        /// </summary>
        /// <param name="upToIndex">Index of up to.</param>
        protected override void Flush(int upToIndex)
        {
            // do nothing
        }
    }
}
