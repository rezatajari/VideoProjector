```csharp
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }


        // Relationships
        public virtual ICollection<Product> Products { get; set; }
    }
}
```
