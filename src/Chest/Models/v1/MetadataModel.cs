﻿// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

#pragma warning disable SA1300 // Element must begin with upper-case letter

namespace Chest.Models.v1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the data model
    /// </summary>
    [Obsolete("MetadataModel is obsolete, please use v2/MetadataModel instead.")]
    public class MetadataModel
    {
#pragma warning disable CA2227

        /// <summary>
        /// Gets or sets data against the given key
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "Provide at least one key-value pair in data")]
        public Dictionary<string, string> Data { get; set; }

        public List<string> Keywords { get; set; }
    }
}
