//-----------------------------------------------------------------------
// <copyright file="StringValueToken.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokens
{
    /// <summary>
    /// String Value Token
    /// </summary>
    public class StringValueToken : Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueToken"/> class.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <param name="text">The token text.</param>
        /// <param name="value">The text value.</param>
        public StringValueToken(TokenTypeEnum type, int start, int end, string text, string value)
            : base(type, start, end, text)
        {
            this.UnquotedString = value;
        }

        /// <summary>
        /// Gets or sets the unquoted string.
        /// </summary>
        /// <value>
        /// The unquoted string.
        /// </value>
        public string UnquotedString
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return TypeToString(this.Type) + ": " + this.UnquotedString; 
        }

        /// <summary>
        /// Gets the clean text.
        /// </summary>
        /// <returns>
        /// Clean text
        /// </returns>
        public override string GetCleanText()
        {
            return this.Type == TokenTypeEnum.Comment ? ("/*" + this.UnquotedString + "*/") : this.Text; 
        }
    }
}
