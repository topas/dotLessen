//-----------------------------------------------------------------------
// <copyright file="Token.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokens
{
    using System;

    /// <summary>
    /// Text token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="type">The type of token.</param>
        /// <param name="start">The start of token.</param>
        /// <param name="end">The end of token.</param>
        /// <param name="text">The text of token.</param>
        public Token(TokenTypeEnum type, int start, int end, string text)
        {
            this.Type = type;
            this.Start = start;
            this.End = end;
            this.Text = text;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public TokenTypeEnum Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the start.
        /// </summary>
        public int Start
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the end.
        /// </summary>
        public int End
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// Types to string.
        /// </summary>
        /// <param name="type">The type of token.</param>
        /// <returns>Token name</returns>
        public static string TypeToString(TokenTypeEnum type)
        {
            switch (type)
            {
                case TokenTypeEnum.Invalid: return "invalid";

                case TokenTypeEnum.Whitespace: return "whitespace";
                case TokenTypeEnum.Comment: return "comment";
                case TokenTypeEnum.Delimiter: return "delimiter";
                case TokenTypeEnum.Operator: return "operator";

                case TokenTypeEnum.CDataOpen: return "cdata-open";
                case TokenTypeEnum.CDataClose: return "cdata-close";

                case TokenTypeEnum.Identifier: return "identifier";
                case TokenTypeEnum.AtIdentifier: return "@identifier";
                case TokenTypeEnum.HashName: return "#name";
                case TokenTypeEnum.Function: return "function";
                case TokenTypeEnum.Variable: return "variable";

                case TokenTypeEnum.String: return "string";
                case TokenTypeEnum.Uri: return "uri";

                case TokenTypeEnum.Number: return "number";
                case TokenTypeEnum.Percentage: return "percentage";
                case TokenTypeEnum.Dimension: return "dimension";
                case TokenTypeEnum.UnicodeRange: return "unicode-range";
                case TokenTypeEnum.Color: return "color";
            }

            return "unknown";
        } 

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.Type == TokenTypeEnum.Whitespace)
            {
                return TypeToString(this.Type);
            }

            return string.Format("{0}: {1}", TypeToString(this.Type), this.Text);
        }

        /// <summary>
        /// Gets the clean text.
        /// </summary>
        /// <returns>Clean text</returns>
        public virtual string GetCleanText()
        {
            return this.Text;
        } 
    }
}
