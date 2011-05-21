//-----------------------------------------------------------------------
// <copyright file="TokenizerTest.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tests
{
    using System.Collections.Generic;

    using DotLessen.Tokenizers;
    using DotLessen.Tokens;

    using Xunit;

    /// <summary>
    /// Tokenizer tests
    /// </summary>
    public class TokenizerTest
    {
        /// <summary>
        /// Tests tokens in css example.
        /// </summary>
        [Fact]
        public void CssTokenize()
        {
            const string value = @"#someid .someclass {
                                                 font-size: 20px;
                                                 width: 50%; 
                                                 text-align: center; 
                                                 -webkit-border-radius: 5px;
                                                }";

            StringTokenizer t = new StringTokenizer(value);

            List<TokenTypeEnum> expectedTokens = new List<TokenTypeEnum>
                {
                    TokenTypeEnum.HashName,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Operator,
                    TokenTypeEnum.Identifier,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Identifier,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Dimension,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Identifier,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Percentage,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Identifier,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Identifier,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Identifier,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Dimension,
                    TokenTypeEnum.Delimiter,
                    TokenTypeEnum.Whitespace,
                    TokenTypeEnum.Delimiter
                };

            int i = 0;
            while (t.MoveNext())
            {
                var token = t.Current;
                Assert.Equal(expectedTokens[i++], token.Type);
            }
        }

        /// <summary>
        /// Tests delimeters parsing.
        /// </summary>
        [Fact]
        public void DelimitersTokenize()
        {
            const string value = @"{}()[]:;,";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Delimiter,  t.Current.Type);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests whitespace parsing.
        /// </summary>
        [Fact]
        public void WhitespaceTokenize()
        {
            const string value = " \t \r\n";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Whitespace, t.Current.Type);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests comment parsing.
        /// </summary>
        [Fact]
        public void MultilineCommentsTokenize()
        {
            const string value = @"/* 
                                    Multiline comment... 
                                    */";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Comment, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests comment parsing.
        /// </summary>
        [Fact]
        public void SinglelineCommentsTokenize()
        {
            const string value = @"// Singleline comment";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Comment, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests @clausules parsing.
        /// </summary>
        [Fact]
        public void ImportClausuleTokenize()
        {
            const string value = @"@import";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.AtIdentifier, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests identifier parsing.
        /// </summary>
        [Fact]
        public void IdentifierTokenize()
        {
            const string value = @"someidentifier";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Identifier, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests operator parsing.
        /// </summary>
        [Fact]
        public void OperatorTokenize()
        {
            const string value = @"+-.*/";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Operator, t.Current.Type);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests number parsing.
        /// </summary>
        [Fact]
        public void NumberTokenize()
        {
            const string value = @"123.45";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Number, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests percentage parsing.
        /// </summary>
        [Fact]
        public void PercentageTokenize()
        {
            const string value = @"123.45%";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Percentage, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests numbers with unit parsing.
        /// </summary>
        [Fact]
        public void NumberWithUnitTokenize()
        {
            const string value = @"123.45px";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Dimension, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests CData start parsing.
        /// </summary>
        [Fact]
        public void CDataStartTokenize()
        {
            const string value = @"<!--";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.CDataOpen, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests CData end parsing.
        /// </summary>
        [Fact]
        public void CDataEndTokenize()
        {
            const string value = @"-->";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.CDataClose, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests string with double quotes parsing.
        /// </summary>
        [Fact]
        public void DoubleQuotesStringTokenize()
        {
            const string value = "\"test\"";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.String, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests string with single quotes parsing.
        /// </summary>
        [Fact]
        public void SingleQuotesStringTokenize()
        {
            const string value = "'test'";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.String, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests function parsing.
        /// </summary>
        [Fact]
        public void FunctionTokenize()
        {
            const string value = "functionName(";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Function, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests url parsing.
        /// </summary>
        [Fact]
        public void UrlTokenize()
        {
            const string value = "url(someurl)";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Uri, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests url parsing.
        /// </summary>
        [Fact]
        public void UrlWithDoubleQuotesTokenize()
        {
            const string value = "url(\"someurl\")";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Uri, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests url parsing.
        /// </summary>
        [Fact]
        public void UrlWithSingleQuotesTokenize()
        {
            const string value = "url('someurl')";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Uri, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests variable parsing.
        /// </summary>
        [Fact]
        public void VariableTokenize()
        {
            const string value = "$variableName";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.Variable, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }

        /// <summary>
        /// Tests hash parsing.
        /// </summary>
        [Fact]
        public void HashTokenize()
        {
            const string value = "#someid";

            StringTokenizer t = new StringTokenizer(value);
            bool tokenized = false;
            while (t.MoveNext())
            {
                Assert.Equal(TokenTypeEnum.HashName, t.Current.Type);
                Assert.Equal(value, t.Current.Text);
                tokenized = true;
            }

            Assert.True(tokenized);
        }
    }
}
