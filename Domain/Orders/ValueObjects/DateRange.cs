using Domain.Orders.Exceptions;

namespace Domain.Orders.ValueObjects;

public sealed record DateRange
{
    public DateOnly Start { get; }
    public DateOnly End { get; }
    public int NumberOfDays { get; }

    public DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
            throw new InvalidDateRangeException("End date precedes start date");

        Start = startDate;
        End = endDate;
        NumberOfDays = Math.Max(1, End.DayNumber - Start.DayNumber);
    }
}