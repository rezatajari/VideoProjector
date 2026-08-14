using Domain.Orders.Exceptions;

namespace Domain.Orders;

public sealed record DateRange
{
    public DateOnly Start { get; }
    public DateOnly End { get; }
    public int LenghInDays { get; }

    public DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
            throw new InvalidDateRangeException("End date precedes start date");

        Start = startDate;
        End = endDate;
        LenghInDays = End.DayNumber - Start.DayNumber;
    }
}