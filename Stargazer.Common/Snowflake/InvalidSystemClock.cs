﻿using System;

namespace Stargazer.Common.Snowflake
{
    public class InvalidSystemClock : Exception
    {      
        public InvalidSystemClock(string message) : base(message) { }
    }
}