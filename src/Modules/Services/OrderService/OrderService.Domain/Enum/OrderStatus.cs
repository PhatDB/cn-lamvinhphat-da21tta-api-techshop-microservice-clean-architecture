namespace OrderService.Domain.Enum
{
    public enum OrderStatus : byte
    {
        AwaitingValidation = 0, // Đang chờ xác nhận (sau khi tạo đơn)
        Confirmed = 1, // Đã xác nhận (sẵn sàng xử lý)
        Paid = 2, // Đã thanh toán (áp dụng cho chuyển khoản trước)
        Shipping = 3, // Đang giao hàng
        Delivered = 4, // Đã giao thành công (COD hoặc Bank đều dùng)
        Cancelled = 5 // Đã huỷ
    }
}