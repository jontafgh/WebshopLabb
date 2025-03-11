using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend;

public static class DtoExtensions
{
    public static UserDetailsDto ToUserDetailsDto(this WebshopUser user)
    {
        return new UserDetailsDto
        {
            Email = user.Email,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Address = (user.Address == null) ? new AddressDto() : user.Address.ToAddressDto()
        };
    }

    //public static OrderDto ToOrderDto(this Order order)
    //{
    //    return new OrderDto
    //    {
    //        Id = order.Id,
    //        OrderDate = order.OrderDate
    //    };
    //}

    public static OrderDto ToOrderDtoWithCartItems(this Order order, List<CartItemDto> cartItems)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            CartItems = cartItems
        };
    }

    public static OrderDto ToOrderDtoWithCartItems(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            CartItems = order.OrderLines.Select(ol => new CartItemDto
            {
                Name = ol.Product.Name,
                ArtNr = ol.Product.ArtNr,
                Quantity = ol.Quantity,
                Price = ol.Price
            }).ToList()
        };
    }

    public static Order ToOrder(this PlaceOrderDto placeOrderDto, string userId)
    {
        return new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            OrderLines = placeOrderDto.CartItems.Select(ci => new OrderLine
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Price
            }).ToList()
        };
    }
    public static CartItemDto ToCartItemDto(this CartItem cartItem)
    {
        return new CartItemDto
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            CartId = cartItem.CartId,
            Name = cartItem.Product.Name,
            ArtNr = cartItem.Product.ArtNr,
            Price = (cartItem.Product.Price!.Discount == null) ? cartItem.Product.Price!.Regular : cartItem.Product.Price!.Discount.DiscountPrice,
            Quantity = cartItem.Quantity
        };
    }

    public static CartItem ToCartItem(this CartItemDto cartItemDto)
    {
        return new CartItem
        {
            ProductId = cartItemDto.ProductId,
            CartId = cartItemDto.CartId,
            Quantity = cartItemDto.Quantity
        };
    }
    public static Cart ToCart(this CreateCartDto createCartDto)
    {
        return new Cart
        {
            Id = createCartDto.Id
        };
    }
    public static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto
        {
            Street = address.Street,
            PostalCode = address.PostalCode,
            City = address.City,
            Country = address.Country
        };
    }
    public static Address ToAddress(this AddressDto addressDto)
    {
        return new Address
        {
            Street = addressDto.Street ?? string.Empty,
            PostalCode = addressDto.PostalCode ?? string.Empty,
            City = addressDto.City ?? string.Empty,
            Country = addressDto.Country ?? string.Empty
        };
    }
    public static Boardgame ToBoardgame(this BoardgameDto boardgameDto)
    {
        return new Boardgame
        {
            Description = boardgameDto.Description,
            MinPlayers = boardgameDto.MinPlayers,
            MaxPlayers = boardgameDto.MaxPlayers,
            MinAge = boardgameDto.MinAge,
            PlayTime = boardgameDto.PlayTime,
            Publisher = new Publisher
            {
                Name = boardgameDto.Publisher.Name
            },
            Product = boardgameDto.ToProduct(),
            
        };
    }
    public static Product ToProduct(this BoardgameDto boardgameDto)
    {
        return new Product
        {
            ArtNr = boardgameDto.Product.ArtNr,
            Name = boardgameDto.Product.Name,
            Image = new Image
            {
                ImageText = boardgameDto.Product.Image.ImageText,
                ImageSmallUrl = boardgameDto.Product.Image.ImageSmallUrl,
                ImageMediumUrl = boardgameDto.Product.Image.ImageMediumUrl,
                ImageLargeUrl = boardgameDto.Product.Image.ImageLargeUrl
            },
            Price = new Price
            {
                Regular = boardgameDto.Product.Price.Regular,
                Discount = (boardgameDto.Product.Price.Discount == null) ? null : new Discount
                {
                    DiscountPrice = boardgameDto.Product.Price.Discount.DiscountPrice,
                    StartDate = boardgameDto.Product.Price.Discount.StartDate,
                    EndDate = boardgameDto.Product.Price.Discount.EndDate
                }
            },
            Stock = new Stock
            {
                Quantity = boardgameDto.Product.Stock.Quantity,
                NextRestock = (boardgameDto.Product.Stock.NextRestock == null) ? null : new Restock
                {
                    Quantity = boardgameDto.Product.Stock.NextRestock.Quantity,
                    RestockDate = boardgameDto.Product.Stock.NextRestock.RestockDate
                }
            }
        };
    }
}