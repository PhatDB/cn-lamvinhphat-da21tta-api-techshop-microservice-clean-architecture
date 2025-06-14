﻿namespace CustomerService.Application.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<AddressDto>? Address { get; set; }
    }
}