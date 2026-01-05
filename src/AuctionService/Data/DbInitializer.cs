using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
  public static void InitDb(WebApplication app)
  {
    using var scope = app.Services.CreateScope();

    SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
  }

    private static void SeedData(AuctionDbContext context)
    {
        context.Database.Migrate(); 

        if (            context.Auctions.Any())         
        {
          Console.WriteLine("Already have auctions in db, no need to seed");
          return;   
        }

        var auctions = new List<Auction>()
        {
            new Auction
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                AuctionStatus = AuctionStatus.Live,
                ReservePrice = 20,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Category = Category.Electronics,
                    Title = "Like-New Stainless Steel Air Fryer - Perfect Condition!",
                    Description = """
                                  Enjoy guilt-free crispy meals with this modern 5.8L stainless steel air fryer. 
                                  Featuring intuitive digital touchscreen controls for precise temperature (80-200°C) and 12 preset cooking programs including fries, chicken, bake, and dehydrate. 
                                  Rapid air circulation technology delivers 99% less fat than traditional frying while cooking up to 30% faster. 
                                  Non-stick basket dishwasher safe, cool-touch housing, and overheat protection for family safety. Barely used, fully tested, includes original manual and accessories.
                                  """,
                    Condition = Condition.LikeNew,
                    ImageName = "air_fryer",
                }
            },
            new Auction
            {
                Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
                AuctionStatus = AuctionStatus.Live,
                ReservePrice = 4000,
                Seller = "alice",
                AuctionEnd = DateTime.UtcNow.AddDays(60),
                Item = new Item
                {
                    Category = Category.Furniture,
                    Title = "Stunning Antique Victorian Sofa - Timeless Elegance!",
                    Description = """
                                  Experience luxury with this authentic Victorian-era sofa featuring a hand-carved mahogany frame and plush burgundy velvet upholstery with beautiful patina from gentle use. 
                                  Ornate cabriole legs and detailed armrest carvings make it a true statement piece for any classic or eclectic interior.Generously sized for family gatherings (seats 3 comfortably), 
                                  solid construction ensures decades of durability. Deep, comfortable cushions with subtle tufting; minor authentic wear adds character without compromising comfort. No damage, pet/smoke-free home.
                                  """,
                    Condition = Condition.Antique,
                    ImageName = "victorian_sofa",
                }
            },
            new Auction
            {
                Id = Guid.Parse("bbab4d5a-8565-48b1-9450-5ac2a5c4a654"),
                AuctionStatus = AuctionStatus.Live,
                Seller = "tom",
                AuctionEnd = DateTime.UtcNow.AddDays(30),
                Item = new Item
                {
                    Category = Category.Miscellaneous,
                    Title = "Classical Era Oil Painting - Museum Quality Heroic Scene!",
                    Description = """
                                  Authentic reproduction classical painting in the style of Jacques-Louis David, featuring a majestic Roman general in gleaming armor amidst dramatic ancient ruins under a stormy sky. 
                                  Rich chiaroscuro lighting, muscular anatomy, and flowing drapery capture the heroic grandeur of the Neoclassical era with meticulous brushwork on canvas. 
                                  Perfect focal point for a study, office, or gallery wall - dramatic earth tones and gold highlights create timeless drama. Framed and ready to hang, measures approx. 80x60cm, excellent condition with protective glass. 
                                  Minor frame patina adds authenticity.
                                  """,
                    Condition = Condition.Antique,
                    ImageName = "classical_painting",
                }
            },
        
        };

        context.AddRange(auctions);

        context.SaveChanges();
    }
}
