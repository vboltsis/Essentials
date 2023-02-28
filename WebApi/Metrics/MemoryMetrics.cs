public readonly record struct MemoryMetrics {
    public readonly double Total;

    public MemoryMetrics(double total, double free) {
        Total = total;
        Free = free;
    }

    public readonly double Free;
    public double Used => Total - Free;
}
