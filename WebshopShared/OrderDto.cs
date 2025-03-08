﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class PlaceOrderDto
    {
        public string UserId { get; set; } = null!;
        public List<CartItemDto> CartItems { get; set; } = [];
    }
    public class OrderDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
