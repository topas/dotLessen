//-----------------------------------------------------------------------
// <copyright file="ITokenizer.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokenizers
{
    using System.Collections.Generic;
    using DotLessen.Tokens;

    /// <summary>
    /// Interface for tokenizers
    /// </summary>
    public interface ITokenizer : IEnumerator<Token>
    {
    }
}
