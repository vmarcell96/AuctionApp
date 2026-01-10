namespace Contracts;

// same as AuctionDto
public class AuctionCreated
{
    public Guid Id { get; set; }
    public int ReservePrice { get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
    public string AuctionStatus { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Condition { get; set; }
    public string ImageName { get; set; }
}
