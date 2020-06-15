﻿using System;

namespace Domain.Common
{
    public class Auditable
    {
        public string CreatedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset? LastModified { get; set; }
    }
}