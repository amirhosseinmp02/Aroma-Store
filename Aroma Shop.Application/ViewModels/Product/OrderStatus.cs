using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public enum OrderStatus
    {
        [Description("لغو شده")]
        Canceled,
        [Description("در انتظار پرداخت")]
        AwaitingPayment,
        [Description("درحال انجام")]
        Doing,
        [Description("در انتظار بررسی")]
        AwaitingReview,
        [Description("تکمیل شده")]
        Completed,
        [Description("مسترد شده")]
        Returned
    }
}
