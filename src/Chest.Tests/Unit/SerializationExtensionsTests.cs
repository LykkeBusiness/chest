// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Chest.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Client;
    using FluentAssertions;
    using Newtonsoft.Json;

    public class SerializationExtensionsTests
    {
        [Fact]
        public void DataShouldNotLostWhenSerializedDeserialized()
        {
            // arrange
            var expected = new ExampleClass
            {
                Id = 5,
                Name = "DAX_INDEX",
                Timestamp = DateTime.ParseExact(
                    DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), "yyyy-MM-dd HH:mm",
                    null),
                Price = 2500.25m,
                SomeEnumType = ExampleClass.SomeEnum.Value2,
                SubType = new SomeSubType
                {
                    OrderId = 26,
                    Name = "EUR"
                }
            };
            var dictionary = expected.ToMetadataDictionary();
            var data = JsonConvert.SerializeObject(dictionary);

            // act
            var actual = data.To<Dictionary<string, string>>().To<ExampleClass>();

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanSerializeDeserializeWatchList()
        {
            // arrange
            var now = DateTime.UtcNow;
            now = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond), now.Kind);
            var expected = new WatchList
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Default",
                Assets = new List<string> { "EURUSD", "EURGBP" },
                ViewType = "FE",
                ModifiedTimestamp = now,
            };
            var dictionary = expected.ToMetadataDictionary();
            var data = JsonConvert.SerializeObject(dictionary);
            
            // act
            var actual = data.To<Dictionary<string, string>>().To<WatchList>();

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

#pragma warning disable CA1034 // Nested types should not be visible
        private class WatchList
        {
            /// <summary>
            /// Gets or sets the watchlist unique identifier
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets watchlist display name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets asset pair ids in this watchlist
            /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
            public List<string> Assets { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

            /// <summary>
            /// Gets or sets view type
            /// </summary>
            public string ViewType { get; set; }

            public DateTime? ModifiedTimestamp { get; set; }
        }

        public class ExampleClass
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime Timestamp { get; set; }

            public decimal Price { get; set; }

            public SomeEnum SomeEnumType { get; set; }

            public SomeSubType SubType { get; set; }

            public enum SomeEnum
            {
                Value1 = 0,
                Value2,
                Value3
            }
        }

        public class SomeSubType
        {
            public int OrderId { get; set; }

            public string Name { get; set; }
        }
    }
}
