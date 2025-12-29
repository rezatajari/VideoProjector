# Domain – Product (Video Projector)

## 1️⃣ Meaning

**Definition:**
A Product is any video projector model that your business sells.
It represents the items customers can browse and purchase if available in the warehouse.
Accessories, cables, screens, or other related items are **not considered Products**.

**Purpose:**
To provide customers with a clear, browseable catalog of video projectors that they can buy directly.

**Boundaries / What Product is NOT:**

* Not accessories (cables, screens, mounts)
* Not services (installation, maintenance)
* Not second-hand units unless explicitly offered

---

## 2️⃣ Responsibilities

* Maintain identity of each Product (model-level, not physical units)
* Store and expose key information (brand, model, features, availability)
* Support business operations like listing for sale and inventory checks

---

## 3️⃣ Invariants

* Every Product must belong to a single brand and model
* A Product must exist in the catalog before a customer can purchase it
* Availability in warehouse must be known and up-to-date

---

## 4️⃣ Behaviors (conceptual, business level)

* Product can be listed for sale
* Product can be marked as available or unavailable depending on warehouse stock
* Product can be associated with categories (optional for filtering/search)

