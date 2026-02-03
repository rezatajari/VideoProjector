## Ubiquitous Language — Video Projector Domain

### Product

**Definition**
A *Product* is a sellable business item identified by a unique identity, representing a commercial offering.

**Characteristics**

* Has a stable identity (`ProductId`)
* Has a name and description
* Has pricing rules
* May have availability constraints
* May represent either:

  * A physical device (e.g., a projector)
  * Or a bundled commercial offer (future possibility)

**Important Clarification**

> *Product is a business concept, not a database row and not a UI card.*

---

### Projector

**Definition**
A *Projector* is a **type of Product** representing a physical projection device with technical specifications.

**Characteristics**

* Is a **Product**, not separate from it
* Has technical attributes (e.g., resolution, brightness, lamp type)
* Represents something that can be physically stocked, sold, or rented

**Key Insight**

> The domain talks about *Projectors*, but the system sells *Products*.
> This prevents leaking technical concerns into sales logic.

---

### Availability

**Definition**
*Availability* expresses whether a Product can be committed to a customer **at a given point in time**.

**Characteristics**

* Time-sensitive
* Depends on:

  * Inventory (for sales)
  * Existing bookings (for rentals)
* Is **not** a boolean flag stored forever

**Important Rule**

> Availability is *calculated*, not owned.

This will matter later when deciding:

* Domain Service vs Entity logic

---

### Price

**Definition**
*Price* is the monetary value at which a Product is offered **under specific conditions**.

**Characteristics**

* Expressed in a currency
* May change over time
* May be influenced by discounts
* Is not necessarily a single number

**Explicit Decision**

> Price is a **Value**, not an identity-bearing concept.

---

### Discount

**Definition**
A *Discount* is a **pricing rule** that modifies the base price of a Product.

**Characteristics**

* Has rules (percentage, fixed amount, campaign-based)
* Has validity constraints (time, eligibility)
* Does **not** own money — it modifies price calculation

**Critical Boundary**

> Discount does **not** change Product.
> It changes **how price is calculated**.

---

### Booking (only if rentals exist)

**Definition**
A *Booking* is a **time-bound commitment** that reserves a Product for a customer.

**Characteristics**

* Has a start and end date
* Blocks availability
* Is transactional in nature
* Represents *intent*, not ownership

**Clarification**

> Booking is **not** an Order and **not** a Sale.

If rentals do **not** exist yet:

* Booking stays **out of the model**
