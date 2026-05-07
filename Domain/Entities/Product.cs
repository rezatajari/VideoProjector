using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Product
{
    public ProductCode Code{ get;private set; }
    public ProductName Name{ get;private set; }
    public ProductType Type{ get; private set; }

    private Product(ProductCode code,ProductName name,ProductType type)
    {
        Code=code?? throw new ArgumentNullException(nameof(code));
        Name=name?? throw new ArgumentNullException( nameof(name));
        Type = type;
    }


    public static Product Create(
        ProductCode code,
        ProductName name,
        ProductType type)
    {
        return new Product(code,name,type);
    }
}
