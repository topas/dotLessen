//-----------------------------------------------------------------------
// <copyright file="UriToken.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokens
{
    /// <summary>
    /// Token with Uri
    /// </summary>
    public class UriToken : StringValueToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriToken"/> class.
        /// </summary>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <param name="text">The text of token.</param>
        /// <param name="prefix">The prefix of url.</param>
        /// <param name="value">The string value of token.</param>
        public UriToken(int start, int end, string text, string prefix, string value)
            : base(TokenTypeEnum.Uri, start, end, text, value)
        {
            this.Prefix = prefix;
        }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public string Prefix
        {
            get;
            set;
        }
    }
}
