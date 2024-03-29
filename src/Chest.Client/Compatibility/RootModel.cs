﻿// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace Chest.Client.AutorestClient.Models 
{
    /// <summary>
    /// Represents the data model
    /// </summary>
    public class RootModel
    {
        /// <summary>
        /// Gets or sets title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets OS
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Gets or sets process id
        /// </summary>
        public int ProcessId { get; set; }
    }
}
