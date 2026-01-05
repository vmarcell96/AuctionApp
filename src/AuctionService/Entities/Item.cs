
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;

[Table("Items")]
public class Item
{
  public Guid Id { get; set; }
  public Category Category { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public Condition Condition { get; set; }
  public string ImageName { get; set; }
  //nav properties for entity framework
  public Auction Auction { get; set; }
  public Guid AuctionId { get; set; }
}
