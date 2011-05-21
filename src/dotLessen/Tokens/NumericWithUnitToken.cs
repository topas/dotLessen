//-----------------------------------------------------------------------
// <copyright file="NumericWithUnitToken.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokens
{
    /// <summary>
    /// Numeric token with unit
    /// </summary>
    public class NumericWithUnitToken : NumericToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericWithUnitToken"/> class.
        /// </summary>
        /// <param name="type">The type of token.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <param name="text">The text value.</param>
        /// <param name="value">The number value.</param>
        /// <param name="unit">The unit of number.</param>
        public NumericWithUnitToken(TokenTypeEnum type, int start, int end, string text, decimal value, string unit)
            : base(type, start, end, text, value)
        {
            this.Unit = unit;
        }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit of number.
        /// </value>
        public string Unit
        {
            get;
            set;
        }
    }
}
