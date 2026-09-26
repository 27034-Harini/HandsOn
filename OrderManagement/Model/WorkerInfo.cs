namespace OrderManagement.Model
{
    internal class WorkerInfo
    {
        public Guid WorkerId { get; init; }
        public Guid OrderId { get; init; }
        public bool IsAvailable { get; set; }
    }
}
