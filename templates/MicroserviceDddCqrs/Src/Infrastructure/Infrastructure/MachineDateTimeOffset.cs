using System;
using Common;

namespace Infrastructure
{
    public class MachineDateTimeOffset : IDateTimeOffset
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
