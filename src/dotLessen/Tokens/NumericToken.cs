//-----------------------------------------------------------------------
// <copyright file="NumericToken.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokens
{
    /// <summary>
    /// Token with number
    /// </summary>
    public class NumericToken : Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericToken"/> class.
        /// </summary>
        /// <param name="type">The type of token.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <param name="text">The text of token.</param>
        /// <param name="value">The value of number.</param>
        public NumericToken(TokenTypeEnum type, int start, int end, string text, decimal value)
            : base(type, start, end, text)
        {
            this.Number = value;
        }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public decimal Number
        {
            get;
            set;
        }
    }
}
