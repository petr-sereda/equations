﻿using System;
using System.Runtime.Serialization;

namespace EquationNormalizer.Core.Parsing
{
    [Serializable]
    public class ParsingException : Exception
    {
        public ParsingException() { }
        public ParsingException(string message) : base(message) { }
        public ParsingException(string message, Exception inner) : base(message, inner) { }
        protected ParsingException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
