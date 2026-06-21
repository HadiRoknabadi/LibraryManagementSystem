using Application.DTOs.Common;
using Application.DTOs.Publisher;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

public class PublisherServiceTests
{
    private static ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static IMapper GetMapper()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Publisher, PublisherListItemDTO>();

            cfg.CreateMap<AddPublisherDTO, Publisher>();
            cfg.CreateMap<EditPublisherDTO, Publisher>();

        }, loggerFactory);

        return config.CreateMapper();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetPublishersAsync_ShouldReturnPublishers(Publisher publisher)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        publisher.IsDelete = false;

        await context.Publishers.AddAsync(publisher);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetPublishersAsync();

        // Assert
        result.Status.Should().Be(GetPublishersResult.Success);
        result.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetPublishersAsync_ShouldReturnEmptyList_WhenPublishersDoNotExist()
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        // Act
        var result = await service.GetPublishersAsync();

        // Assert
        result.Status.Should().Be(GetPublishersResult.Success);
        result.Data.Should().BeEmpty();
    }

    [Theory]
    [AutoDomainData]
    public async Task FilterPublisherAsync_ShouldReturnFilteredPublishers(Publisher publisher)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        publisher.IsDelete = false;
        publisher.Name = "OReilly";
        publisher.PhoneNumber = "09120000000";

        await context.Publishers.AddAsync(publisher);
        await context.SaveChangesAsync();

        var filter = new FilterPublisherDTO
        {
            Name = "OReilly",
            PhoneNumber = "0912",
            PageId = 1,
            TakeEntity = 10,
            HowManyShowPageAfterAndBefore = 3,
            OrderBy = FilterDataOrder.CreateDate_DES
        };

        // Act
        var result = await service.FilterPublisherAsync(filter);

        // Assert
        result.Publishers.Should().NotBeEmpty();
    }

    [Theory]
    [AutoDomainData]
    public async Task FilterPublisherAsync_ShouldReturnPublishers_WhenOnlyNameMatches(Publisher publisher)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        publisher.IsDelete = false;
        publisher.Name = "Test Publisher";
        publisher.PhoneNumber = "12345";

        await context.Publishers.AddAsync(publisher);
        await context.SaveChangesAsync();

        var filter = new FilterPublisherDTO
        {
            Name = "Test",
            PageId = 1,
            TakeEntity = 10,
            HowManyShowPageAfterAndBefore = 3,
            OrderBy = FilterDataOrder.CreateDate_DES
        };

        // Act
        var result = await service.FilterPublisherAsync(filter);

        // Assert
        result.Publishers.Should().NotBeEmpty();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetPublisherByIdAsync_ShouldReturnPublisher(Publisher publisher)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        publisher.IsDelete = false;

        await context.Publishers.AddAsync(publisher);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetPublisherByIdAsync(publisher.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(publisher.Id);
    }

    [Theory]
    [AutoDomainData]
    public async Task GetPublisherByIdAsync_ShouldReturnNull_WhenPublisherDoesNotExist(int id)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        // Act
        var result = await service.GetPublisherByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [AutoDomainData]
    public async Task AddPublisherAsync_ShouldAddPublisher(AddPublisherDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        // Act
        var result = await service.AddPublisherAsync(dto);

        // Assert
        result.Status.Should().Be(AddPublisherResult.Success);

        var publisher = await context.Publishers.FirstOrDefaultAsync();

        publisher.Should().NotBeNull();
        publisher.Name.Should().Be(dto.Name);
        publisher.PhoneNumber.Should().Be(dto.PhoneNumber);
    }

    [Theory]
    [AutoDomainData]
    public async Task EditPublisherAsync_ShouldEditPublisher(
        Publisher publisher,
        EditPublisherDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        publisher.IsDelete = false;

        await context.Publishers.AddAsync(publisher);
        await context.SaveChangesAsync();

        dto.Id = publisher.Id;
        dto.Name = "Updated Publisher";
        dto.PhoneNumber = "99999";

        // Act
        var result = await service.EditPublisherAsync(dto);

        // Assert
        result.Status.Should().Be(EditPublisherResult.Success);

        var updatedPublisher = await context.Publishers
            .FirstAsync(p => p.Id == publisher.Id);

        updatedPublisher.Name.Should().Be(dto.Name);
        updatedPublisher.PhoneNumber.Should().Be(dto.PhoneNumber);
    }

    [Theory]
    [AutoDomainData]
    public async Task EditPublisherAsync_ShouldReturnNotFound_WhenPublisherDoesNotExist(EditPublisherDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        // Act
        var result = await service.EditPublisherAsync(dto);

        // Assert
        result.Status.Should().Be(EditPublisherResult.NotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task DeletePublisherAsync_ShouldDeletePublisher(Publisher publisher)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        publisher.Id = 0;
        publisher.IsDelete = false;

        await context.Publishers.AddAsync(publisher);
        await context.SaveChangesAsync();

        var savedId = publisher.Id;

        // Act
        var result = await service.DeletePublisherAsync(savedId);

        // Assert
        result.Status.Should().Be(DeletePublisherResult.Success);

        var deletedPublisher = await context.Publishers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == savedId);

        deletedPublisher.Should().NotBeNull();
        deletedPublisher.IsDelete.Should().BeTrue();
    }



    [Theory]
    [AutoDomainData]
    public async Task DeletePublisherAsync_ShouldReturnNotFound_WhenPublisherDoesNotExist(int id)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new PublisherService(context, mapper);

        // Act
        var result = await service.DeletePublisherAsync(id);

        // Assert
        result.Status.Should().Be(DeletePublisherResult.NotFound);
    }
}
