//-----------------------------------------------------------------------
// <copyright file="TokenizerOptions.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokenizers
{
    using System;

    /// <summary>
    /// Tokenizer options enum
    /// </summary>
    [Flags]
    public enum TokenizerOptions
    {
        /// <summary>
        /// Empty options
        /// </summary>
        None = 0,

        /// <summary>
        /// Parsing only comment start and end tokens in multiline tokens
        /// </summary>
        MultilineCommentBeginEndTokens 
    }
}
