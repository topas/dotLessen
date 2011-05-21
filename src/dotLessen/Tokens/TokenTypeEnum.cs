//-----------------------------------------------------------------------
// <copyright file="TokenTypeEnum.cs" company="IxoneCz">
//  Copyright (c) 2011 Tomas Pastorek, www.Ixone.Cz. All rights reserved.
//
//  Based on Lessen for Java http://code.google.com/p/lessen/
//  Copyright (c) 2010 Metaweb Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DotLessen.Tokens
{
    /// <summary>
    /// Token type enum
    /// Mostly http://www.w3.org/TR/CSS2/syndata.html, with some changes
    /// </summary>
    public enum TokenTypeEnum
    {
        /// <summary>
        /// Invalid token
        /// </summary>
        Invalid,

        /// <summary>
        /// Whitespace token
        /// </summary>
        Whitespace,

        /// <summary>
        /// Comment token 
        /// </summary>
        Comment,

        /// <summary>
        /// Delimiters { } [ ] ( ) : ;
        /// </summary>
        Delimiter,

        /// <summary>
        /// Operators ~= |=
        /// </summary>
        Operator,

        /// <summary>
        /// CData comment begin 
        /// </summary>
        CDataOpen,   

        /// <summary>
        /// CData comment end
        /// </summary>
        CDataClose,    

        /// <summary>
        /// optionally prefixed with a dash
        /// </summary>
        Identifier, 
        
        /// <summary>
        /// possibly a LESS identifier
        /// </summary>
        AtIdentifier,   

        /// <summary>
        /// such as color codes
        /// </summary>
        HashName,      

        /// <summary>
        /// such as expression(...)
        /// </summary>
        Function,    
        
        /// <summary>
        /// $name or ${name}, used for substitution
        /// </summary>
        Variable,   

        /// <summary>
        /// String token
        /// </summary>
        String,
        
        /// <summary>
        /// Uri token type
        /// </summary>
        Uri,

        /// <summary>
        /// Number token
        /// </summary>
        Number,

        /// <summary>
        /// Percentage token 
        /// </summary>
        Percentage,
        
        /// <summary>
        /// Dimension token 
        /// </summary>
        Dimension,

        /// <summary>
        /// Unicode range token
        /// </summary>
        UnicodeRange,

        /// <summary>
        /// Color token
        /// </summary>
        Color
    } 
}
