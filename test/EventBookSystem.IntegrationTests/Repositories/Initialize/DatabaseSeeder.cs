using Bogus;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Entities;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.IntegrationTests.Repositories.Initialize
{
    public class DatabaseSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SeedDatabase(MainDBContext context, int count = 10)
        {
            var eventIds = new List<Guid>();
            var venueIds = new List<Guid>();
            var sectionIds = new List<Guid>();

            var eventFaker = new Faker<Event>()
                .RuleFor(e => e.Id, f =>
                {
                    var id = Guid.NewGuid();
                    eventIds.Add(id);
                    return id;
                })
                .RuleFor(e => e.Name, f => f.Commerce.ProductName());

            var venueFaker = new Faker<Venue>()
                .RuleFor(v => v.Id, f =>
                {
                    var id = Guid.NewGuid();
                    venueIds.Add(id);
                    return id;
                })
                .RuleFor(v => v.Name, f => f.Address.City())
                .RuleFor(v => v.Location, f => f.Address.FullAddress());

            var sectionFaker = new Faker<Section>()
                .RuleFor(s => s.Id, f =>
                {
                    var id = Guid.NewGuid();
                    sectionIds.Add(id);
                    return id;
                })
                .RuleFor(s => s.Name, f => f.Commerce.Department())
                .RuleFor(s => s.EventId, f => f.PickRandom(eventIds))
                .RuleFor(s => s.VenueId, f => f.PickRandom(venueIds));

            var seatFaker = new Faker<Seat>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.SectionId, f => f.PickRandom(sectionIds))
                .RuleFor(s => s.Row, f => f.Random.Int(1, 10))
                .RuleFor(s => s.Number, f => f.Random.Int(1, 20))
                .RuleFor(s => s.Price, f => new Price
                {
                    Name = f.Commerce.Price(),
                    Amount = f.Random.Decimal(50, 200)
                });

            var events = eventFaker.Generate(count);
            var venues = venueFaker.Generate(count);
            var sections = sectionFaker.Generate(count);
            var seats = seatFaker.Generate(count);

            context.Events.AddRange(events);
            context.Venues.AddRange(venues);
            context.Sections.AddRange(sections);
            context.Seats.AddRange(seats);

            context.SaveChanges();
        }
    }
}