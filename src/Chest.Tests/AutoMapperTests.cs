using AutoMapper;
using Chest.Mappers;
using Xunit;

namespace Chest.Tests
{
    public class AutoMapperTests
    {
        [Fact]
        public void Mapping_Configuration_Is_Correct()
        {
            // arrange
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(AutoMapperProfile).Assembly);
            });
            var mapper = mockMapper.CreateMapper();

            // act
            var ex = Record.Exception(() => mapper.ConfigurationProvider.AssertConfigurationIsValid());

            // assert
            Assert.Null(ex);
        }
    }
}