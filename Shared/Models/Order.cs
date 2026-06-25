namespace Shared.Models;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }

    // آیا این سفارش برای اجاره است؟ (true = اجاره، false = فروش قطعی)
    public bool IsRental { get; set; }

    // فیلدهای مخصوص اجاره (برای فروش قطعی null می‌مانند)
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // تعداد درخواستی (تعداد کالای خریداری شده یا تعداد دستگاه اجاره شده)
    public int Quantity { get; set; }

    // مبلغ کل سفارش (تعداد × قیمت فروش) یا (تعداد روزها × تعداد دستگاه × قیمت اجاره روزانه)
    public decimal TotalPrice { get; set; }

    // وضعیت فعلی سفارش از روی انام
    public OrderStatus Status { get; set; }
}