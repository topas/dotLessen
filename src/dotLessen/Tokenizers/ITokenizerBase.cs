//-----------------------------------------------------------------------
// <copyright file="ITokenizerBase.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokenizers
{
    using System;
    using System.Collections;

    using DotLessen.Tokens;

    /// <summary>
    /// Abstract base tokenizer
    /// </summary>
    public abstract class ITokenizerBase : ITokenizer
    {
        /// <summary>
        /// Is instance disposed?
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        public Token Current
        {
            get
            {
                return this.Token;
            }
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception><filterpriority>2</filterpriority>
        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        protected Token Token
        {
            get;
            set;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (!this._disposed)
            {
                this.OnDisposing(true);
                GC.SuppressFinalize(this);
                this._disposed = true;
            }
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully Advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            this.Token = null;
            if (this.HasMoreChar())
            {
                int tokenStart = this.GetCurrentOffset();
                char c = this.GetCurrentChar();

                int tokenEnd;
                if (IsWhitespace(c))
                {
                    int distance = this.LookOverWhitespace(tokenStart);
                    tokenEnd = tokenStart + distance;

                    this.Token = new Token(TokenTypeEnum.Whitespace, tokenStart, tokenEnd, this.GetText(tokenStart, tokenEnd));

                    this.Advance(distance);
                    return true;
                }

                if (Char.IsDigit(c))
                {
                    this.ParseNumber(tokenStart, 1);
                    return true;
                }

                if ("{}()[]:;,".IndexOf(c) >= 0)
                {
                    tokenEnd = tokenStart + 1;

                    this.Token = new Token(
                        TokenTypeEnum.Delimiter,
                        tokenStart,
                        tokenEnd,
                        this.GetText(tokenStart, tokenEnd));

                    this.Advance();
                    return true;
                }

                if (c == '~' || c == '|')
                {
                    if (this.HasMoreChar(1) && this.GetCharRelative(1) == '=')
                    {
                        this.Advance();
                    } // fall through                   
                }
                else if (c == '/')
                {
                    if (this.HasMoreChar(1))
                    {
                        char c2 = this.GetCharRelative(1);
                        if (c2 == '/')
                        {
                            this.Advance(2, false);

                            int commentStart = this.GetCurrentOffset();
                            while (this.HasMoreChar())
                            {
                                c2 = this.GetCurrentChar();
                                if (c2 == '\r' || c2 == '\n' || c2 == '\f')
                                {
                                    break;
                                }

                                this.Advance();
                            }

                            tokenEnd = this.GetCurrentOffset();

                            this.Token = new StringValueToken(
                                TokenTypeEnum.Comment,
                                tokenStart,
                                tokenEnd,
                                this.GetText(tokenStart, tokenEnd),
                                this.GetText(commentStart, tokenEnd));

                            return true;
                        }

                        if (c2 == '*')
                        {
                            this.Advance(2, false);

                            int commentStart = this.GetCurrentOffset();
                            int commentEnd = commentStart;

                            while (this.HasMoreChar())
                            {
                                c2 = this.GetCurrentChar();
                                if (c2 == '*' && this.HasMoreChar(1) && this.GetCharRelative(1) == '/')
                                {
                                    this.Advance(2, false);
                                    break;
                                }

                                commentEnd++;
                                this.Advance();
                            }

                            tokenEnd = this.GetCurrentOffset();

                            this.Token = new StringValueToken(
                                TokenTypeEnum.Comment,
                                tokenStart,
                                tokenEnd,
                                this.GetText(tokenStart, tokenEnd),
                                this.GetText(commentStart, commentEnd));

                            this.Flush(tokenEnd);

                            return true;
                        } // fall through                       
                    }  // fall through                  
                }
                else if (c == '@')
                {
                    int distance = this.LookOverIdentifier(1);
                    if (distance > 0)
                    {
                        tokenEnd = tokenStart + 1 + distance;

                        this.Token = new Token(
                            TokenTypeEnum.AtIdentifier,
                            tokenStart,
                            tokenEnd,
                            this.GetText(tokenStart, tokenEnd));

                        this.Advance(1 + distance);
                        return true;
                    } // fall through                   
                }
                else if (c == '#')
                {
                    int distance = this.LookOverName(1);
                    if (distance > 0)
                    {
                        tokenEnd = tokenStart + 1 + distance;

                        this.Token = new Token(
                            TokenTypeEnum.HashName,
                            tokenStart,
                            tokenEnd,
                            this.GetText(tokenStart, tokenEnd));

                        this.Advance(1 + distance);
                        return true;
                    }    // fall through
                }
                else if (c == '<' &&
                         this.HasMoreChar(3) &&
                         this.GetCharRelative(1) == '!' &&
                         this.GetCharRelative(2) == '-' &&
                         this.GetCharRelative(3) == '-')
                {
                    tokenEnd = tokenStart + 4;

                    this.Token = new Token(
                        TokenTypeEnum.CDataOpen,
                        tokenStart,
                        tokenEnd,
                        this.GetText(tokenStart, tokenEnd));

                    this.Advance(4);
                    return true;
                }
                else if (c == '-')
                {
                    if (this.HasMoreChar(2) &&
                        this.GetCharRelative(1) == '-' &&
                        this.GetCharRelative(2) == '>')
                    {
                        tokenEnd = tokenStart + 3;

                        this.Token = new Token(
                            TokenTypeEnum.CDataClose,
                            tokenStart,
                            tokenEnd,
                            this.GetText(tokenStart, tokenEnd));

                        this.Advance(3);
                        return true;
                    }

                    if (this.HasMoreChar(1))
                    {
                        if (Char.IsDigit(this.GetCharRelative(1)))
                        {
                            this.Advance(); // swallow -

                            this.ParseNumber(tokenStart, -1);
                            return true;
                        }
                        
                        int distance = this.LookOverName(1);
                        if (distance > 0)
                        {
                            tokenEnd = tokenStart + 1 + distance;

                            this.Token = new Token(
                                TokenTypeEnum.Identifier,
                                tokenStart,
                                tokenEnd, 
                                this.GetText(tokenStart, tokenEnd));

                            this.Advance(1 + distance);
                            return true;
                        }  // fall through               
                    }   // fall through         
                }
                else if (IsNameStartChar(c))
                {
                    if (c == 'u' && this.HasMoreChar(1) && this.GetCharRelative(1) == '+')
                    {
                        int unicodeCodeDistance = this.LookOverUnicodeCode(2);
                        if (unicodeCodeDistance > 0)
                        {
                            tokenEnd = tokenStart + 2 + unicodeCodeDistance;

                            this.Advance(2 + unicodeCodeDistance, false);

                            if (this.HasMoreChar() && this.GetCurrentChar() == '-')
                            {
                                int unicodeCodeDistance2 = this.LookOverUnicodeCode(1);
                                if (unicodeCodeDistance2 > 0)
                                {
                                    tokenEnd += 1 + unicodeCodeDistance2;

                                    this.Advance(1 + unicodeCodeDistance2, false);
                                }
                            }

                            this.Token = new Token(TokenTypeEnum.UnicodeRange, tokenStart, tokenEnd, this.GetText(tokenStart, tokenEnd));

                            this.Flush(tokenEnd);
                            return true;
                        }
                    }

                    int distance = this.LookOverName(0);
                    tokenEnd = tokenStart + distance;

                    string name = this.GetText(tokenStart, tokenEnd);

                    if (this.HasMoreChar(distance) &&
                        this.GetCharRelative(distance) == '(')
                    {
                        this.Advance(distance + 1, false);

                        if ("uri".Equals(name) || "url".Equals(name))
                        {
                            this.ParseRestOfURI(tokenStart, name);
                            return true;
                        }
                        
                        tokenEnd++;

                        this.Token = new Token(
                            TokenTypeEnum.Function,
                            tokenStart,
                            tokenEnd,
                            this.GetText(tokenStart, tokenEnd));
                        return true;
                    }

                    this.Token = new Token(
                        TokenTypeEnum.Identifier,
                        tokenStart,
                        tokenEnd,
                        this.GetText(tokenStart, tokenEnd));

                    this.Advance(distance);
                    return true;
                }
                else if (c == '"' || c == '\'')
                {
                    this.Advance();

                    string value = this.ParseString(c);

                    tokenEnd = this.GetCurrentOffset();

                    this.Token = new StringValueToken(TokenTypeEnum.String, tokenStart, tokenEnd, this.GetText(tokenStart, tokenEnd), value);

                    return true;
                }
                else if (c == '$' && this.HasMoreChar(1))
                {
                    char c2 = this.GetCharRelative(1);
                    if (c2 == '{')
                    {
                        int nameStart = tokenStart + 2;
                        int nameEnd = nameStart;

                        int distance = 2;
                        while (this.HasMoreChar(distance))
                        {
                            c2 = this.GetCharRelative(distance);
                            if (c2 == '}')
                            {
                                distance++;
                                break;
                            }

                            nameEnd++;
                            distance++;
                        }

                        tokenEnd = tokenStart + distance;

                        this.Token = new StringValueToken(
                            TokenTypeEnum.Variable,
                            tokenStart,
                            tokenEnd,
                            this.GetText(tokenStart, tokenEnd),
                            this.GetText(nameStart, nameEnd));

                        this.Advance(distance);

                        return true;
                    }

                    if (Char.IsLetter(c2))
                    {
                        int nameStart = tokenStart + 1;

                        int distance = 1;
                        while (this.HasMoreChar(distance))
                        {
                            c2 = this.GetCharRelative(distance);
                            if (Char.IsLetter(c2))
                            {
                                distance++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        tokenEnd = tokenStart + distance;

                        this.Token = new StringValueToken(
                            TokenTypeEnum.Variable,
                            tokenStart,
                            tokenEnd,
                            this.GetText(tokenStart, tokenEnd),
                            this.GetText(nameStart, tokenEnd));

                        this.Advance(distance);

                        return true;
                    }
                }

                this.Advance();

                tokenEnd = this.GetCurrentOffset();

                this.Token = new Token(
                    TokenTypeEnum.Operator,
                    tokenStart,
                    tokenEnd, 
                    this.GetText(tokenStart, tokenEnd));
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public abstract void Reset();

        /// <summary>
        /// Determines whether the specified c is whitespace.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>
        ///   <c>true</c> if the specified c is whitespace; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsWhitespace(char c)
        {
            return Char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Determines whether [is hex digit] [the specified c].
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>
        ///   <c>true</c> if [is hex digit] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsHexDigit(char c)
        {
            return Char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }

        /// <summary>
        /// Determines whether [is name char] [the specified c].
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>
        ///   <c>true</c> if [is name char] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsNameChar(char c)
        {
            return c == '_' || c == '-' || Char.IsDigit(c) || Char.IsLetter(c) || c > 177;
        }

        /// <summary>
        /// Determines whether [is name start char] [the specified c].
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>
        ///   <c>true</c> if [is name start char] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsNameStartChar(char c)
        {
            return c == '_' || Char.IsLetter(c) || c > 177;
        }

        /// <summary>
        /// Determines whether [has more char].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has more char]; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool HasMoreChar();

        /// <summary>
        /// Determines whether [has more char] [the specified offset].
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>
        ///   <c>true</c> if [has more char] [the specified offset]; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool HasMoreChar(int offset);

        /// <summary>
        /// Gets the current char.
        /// </summary>
        /// <returns>Gets current char</returns>
        protected abstract char GetCurrentChar();

        /// <summary>
        /// Gets the char relative.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Gets relative char</returns>
        protected abstract char GetCharRelative(int offset);

        /// <summary>
        /// Gets the current offset.
        /// </summary>
        /// <returns>Current text offset</returns>
        protected abstract int GetCurrentOffset();

        /// <summary>
        /// Advances this instance.
        /// </summary>
        protected abstract void Advance();

        /// <summary>
        /// Advances the specified by.
        /// </summary>
        /// <param name="by">The by index.</param>
        /// <param name="flush">if set to <c>true</c> [flush].</param>
        protected abstract void Advance(int by, bool flush);

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="from">Text Index From.</param>
        /// <param name="to">Text Index To.</param>
        /// <returns>Text between range</returns>
        protected abstract string GetText(int from, int to);

        /// <summary>
        /// Flushes the specified up to index.
        /// </summary>
        /// <param name="upToIndex">Index of up to.</param>
        protected abstract void Flush(int upToIndex);

        /// <summary>
        /// Advances the specified by.
        /// </summary>
        /// <param name="by">The by index.</param>
        protected void Advance(int by)
        {
            this.Advance(by, true);
        }

        /// <summary>
        /// Parses the number.
        /// </summary>
        /// <param name="tokenStart">The token start.</param>
        /// <param name="signum">The signum.</param>
        protected void ParseNumber(int tokenStart, int signum)
        {
            int n = 0;
            double divide = 1;

            char c;
            while (this.HasMoreChar())
            {
                c = this.GetCurrentChar();
                if (Char.IsDigit(c))
                {
                    n = (n * 10) + (c - '0');
                    this.Advance();
                }
                else
                {
                    break;
                }
            }

            if (this.HasMoreChar())
            {
                c = this.GetCurrentChar();
                if (c == '.')
                {
                    this.Advance();

                    while (this.HasMoreChar())
                    {
                        c = this.GetCurrentChar();
                        if (Char.IsDigit(c))
                        {
                            n = (n * 10) + (c - '0');
                            divide *= 10;
                            this.Advance();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            decimal num = new decimal(signum * (divide > 1 ? (n / divide) : n));

            int tokenEnd;
            if (this.HasMoreChar())
            {
                c = this.GetCurrentChar();
                if (c == '%')
                {
                    this.Advance();

                    tokenEnd = this.GetCurrentOffset();

                    this.Token = new NumericToken(
                        TokenTypeEnum.Percentage,
                        tokenStart,
                        tokenEnd,
                        this.GetText(tokenStart, tokenEnd),
                        num);

                    this.Flush(tokenEnd);
                    return;
                }

                if (Char.IsLetter(c))
                {
                    int unitStart = this.GetCurrentOffset();
                    this.Advance();

                    while (this.HasMoreChar() && Char.IsLetter(this.GetCurrentChar()))
                    {
                        this.Advance();
                    }

                    tokenEnd = this.GetCurrentOffset();

                    this.Token = new NumericWithUnitToken(
                        TokenTypeEnum.Dimension, 
                        tokenStart, 
                        tokenEnd,
                        this.GetText(tokenStart, tokenEnd), 
                        num, 
                        this.GetText(unitStart, tokenEnd));

                    this.Flush(tokenEnd);
                    return;
                }
            }

            tokenEnd = this.GetCurrentOffset();

            this.Token = new NumericToken(TokenTypeEnum.Number, tokenStart, tokenEnd, this.GetText(tokenStart, tokenEnd), num);

            this.Flush(tokenEnd);
        }

        /// <summary>
        /// Parses the rest of URI.
        /// </summary>
        /// <param name="tokenStart">The token start.</param>
        /// <param name="prefix">The prefix.</param>
        protected void ParseRestOfURI(int tokenStart, string prefix)
        {
            this.SwallowWhitespace(false);

            string value = string.Empty;
            if (this.HasMoreChar())
            {
                char c = this.GetCurrentChar();
                if (c == '"' || c == '\'')
                {
                    this.Advance();

                    value = this.ParseString(c);
                }
                else
                {
                    int valueStart = this.GetCurrentOffset();
                    this.Advance();

                    while (this.HasMoreChar())
                    {
                        c = this.GetCurrentChar();
                        if (IsWhitespace(c) || c == ')')
                        {
                            break;
                        }

                        this.Advance();
                    }

                    value = this.GetText(valueStart, this.GetCurrentOffset());
                }
            }

            this.SwallowWhitespace(false);

            if (this.HasMoreChar())
            {
                char c = this.GetCurrentChar();
                if (c == ')')
                {
                    this.Advance();
                }
            }

            int tokenEnd = this.GetCurrentOffset();
            this.Token = new UriToken(tokenStart, tokenEnd, this.GetText(tokenStart, tokenEnd), prefix, value);
        }

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>Parsed string</returns>
        protected string ParseString(char delimiter)
        {
            int start = this.GetCurrentOffset();
            int end = start;

            while (this.HasMoreChar())
            {
                char c = this.GetCurrentChar();
                if (c == delimiter)
                {
                    end = this.GetCurrentOffset();
                    this.Advance();
                    break;
                }
                
                if (c == '\r' || c == '\n' || c == '\f')
                {
                    end = this.GetCurrentOffset();
                    break;
                }

                if (c == '\\' && this.HasMoreChar(1))
                {
                    this.Advance();
                }

                this.Advance();
            }

            return this.GetText(start, end);
        }

        /// <summary>
        /// Swallows the whitespace.
        /// </summary>
        /// <param name="flush">if set to <c>true</c> [flush].</param>
        protected void SwallowWhitespace(bool flush)
        {
            this.Advance(this.LookOverWhitespace(0), flush);
        }

        /// <summary>
        /// Looks the over whitespace.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverWhitespace(int index)
        {
            int distance = 0;
            while (this.HasMoreChar(distance) && Char.IsWhiteSpace(this.GetCharRelative(distance)))
            {
                distance++;
            }

            return distance;
        }

        /// <summary>
        /// Looks the over identifier.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverIdentifier(int offset)
        {
            int distance = 0;
            if (this.HasMoreChar(offset + distance))
            {
                int distance2 = this.LookOverNameStartChar(offset + distance);
                if (distance2 > 0)
                {
                    distance += distance2;
                    distance += this.LookOverName(offset + distance);
                }
            }

            return distance;
        }

        /// <summary>
        /// Looks the name of the over.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverName(int offset)
        {
            int distance = 0;
            while (this.HasMoreChar(offset + distance))
            {
                int distance2 = this.LookOverNameChar(offset + distance);
                if (distance2 > 0)
                {
                    distance += distance2;
                }
                else
                {
                    break;
                }
            }

            return distance;
        }

        /// <summary>
        /// Looks the over name start char.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverNameStartChar(int offset)
        {
            int distance = 0;
            if (this.HasMoreChar(offset + distance))
            {
                char c = this.GetCharRelative(offset + distance);
                if (IsNameStartChar(c))
                {
                    distance++;
                }
                else if (c == '\\' && this.HasMoreChar(offset + distance + 1))
                {
                    char c2 = this.GetCharRelative(offset + distance + 1);
                    if (IsHexDigit(c2))
                    {
                        int distance2 = this.LookOverUnicodeCode(offset + distance + 1);
                        if (distance2 > 0)
                        {
                            distance += distance2;
                        }
                    }
                    else if (c2 != '\r' && c2 != '\n' && c2 != '\f')
                    {
                        distance += 2;
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Looks the over name char.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverNameChar(int offset)
        {
            int distance = 0;
            if (this.HasMoreChar(offset + distance))
            {
                char c = this.GetCharRelative(offset + distance);
                if (IsNameChar(c))
                {
                    distance++;
                }
                else if (c == '\\' && this.HasMoreChar(offset + distance + 1))
                {
                    char c2 = this.GetCharRelative(offset + distance + 1);
                    if (IsHexDigit(c2))
                    {
                        int distance2 = this.LookOverUnicodeCode(offset + distance + 1);
                        if (distance2 > 0)
                        {
                            distance += distance2;
                        }
                    }
                    else if (c2 != '\r' && c2 != '\n' && c2 != '\f')
                    {
                        distance += 2;
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Looks the over unicode char.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverUnicodeChar(int offset)
        {
            int distance = 0;
            if (this.HasMoreChar(offset + distance))
            {
                char c = this.GetCharRelative(offset + distance);
                if (c == '\\' && this.HasMoreChar(offset + distance + 1))
                {
                    char c2 = this.GetCharRelative(offset + distance + 1);
                    if (IsHexDigit(c2))
                    {
                        int distance2 = this.LookOverUnicodeCode(offset + distance + 1);
                        if (distance2 > 0)
                        {
                            distance += distance2;
                        }
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Called when [disposing].
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected virtual void OnDisposing(bool disposing)
        {
        }

        /// <summary>
        /// Looks the over unicode code.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Distance of look</returns>
        protected int LookOverUnicodeCode(int offset)
        {
            int distance = 0;
            while (distance < 6 && this.HasMoreChar(offset + distance) && IsHexDigit(this.GetCharRelative(offset + distance)))
            {
                distance++;
            }

            return distance;
        }
    }
}
