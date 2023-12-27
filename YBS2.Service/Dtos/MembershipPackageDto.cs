namespace YBS2.Service.Dtos
{
    public class MembershipPackageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }
        public float DiscountPercent { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}