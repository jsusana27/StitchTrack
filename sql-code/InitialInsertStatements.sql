INSERT INTO Yarns (Brand, Price, FiberType, FiberWeight, Color, YardagePerSkein, GramsPerSkein, NumberOfSkeinsOwned)
VALUES 
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'Baby Sand', 220, 300, 1),
('Bernat Blanket', 11.99, 'Chenille Polyester', 6, 'Frosted Blue', 220, 300, 1),
('Bernat Blanket', 11.99, 'Chenille Polyester', 6, 'Deep Sea', 220, 300, 1),
('Bernat Blanket', 11.99, 'Chenille Polyester', 6, 'Crimson', 220, 300, 1),
('Bernat Blanket', 11.99, 'Chenille Polyester', 6, 'Coal', 220, 300, 1),
('Bernat Blanket', 11.99, 'Chenille Polyester', 6, 'Taupe', 220, 300, 1),
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'Buttercup', 220, 300, 1),
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'Denim', 220, 300, 1),
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'Mellow Mauve', 220, 300, 1),
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'White', 220, 300, 1),
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'Baby Lilac', 220, 300, 1),
('Bernat Baby Blanket', 11.99, 'Chenille Polyester', 6, 'Misty Jungle Green', 220, 300, 1),
('Bernat Blanket Bright', 11.99, 'Chenille Polyester', 6, 'Pow Purple', 220, 300, 1),
('Bernat Blanket', 11.99, 'Chenille Polyester', 6, 'Crimson', 220, 300, 1),
('Bernat Velvet', 11.99, 'Chenille Polyester', 5, 'Mushroom', 315, 300, 2),
('Bernat Velvet', 11.99, 'Chenille Polyester', 5, 'White', 315, 300, 1),
('Bernat Velvet', 11.99, 'Chenille Polyester', 5, 'Gray Orchid', 315, 300, 1),
('Bernat Baby Velvet', 11.99, 'Chenille Polyester', 4, 'Chocolate', 492, 300, 2),
('Bernat Baby Velvet', 11.99, 'Chenille Polyester', 4, 'Emerald', 492, 300, 1),
('Big Twist Plush', 11.99, 'Chenille Polyester', 6, 'Cobalt', 153, 300, 1),
('Chenille Home Slim by Loops & Threads', 9.99, 'Chenille Polyester', 6, 'Cream', 218, 250, 2),
('Chenille Home Slim by Loops & Threads', 9.99, 'Chenille Polyester', 6, 'Agave', 218, 250, 1),
('Chenille Home Slim by Loops & Threads', 9.99, 'Chenille Polyester', 6, 'Ash', 218, 250, 1),
('Bernat Velvet', 11.99, 'Chenille Polyester', 5, 'Red', 315, 300, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Sage', 380, 170, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Pale Yellow', 380, 170, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Varsity Red', 380, 170, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Black', 380, 170, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Magenta', 380, 170, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Mustard', 380, 170, 3),
('Lion Brand Jiffy', 19.99, 'Acrylic', 5, 'Cream', 681, 410, 1),
('Red Heart Super Saver', 9.49, 'Acrylic', 4, 'Cafe Latte', 744, 396, 1),
('Value Yarn by Big Twist', 4.49, 'Acrylic', 4, 'Lilac', 380, 170, 1);

INSERT INTO SafetyEyes (Price, SizeinMM, Color, Shape, NumberOfEyesOwned)
VALUES
(7.99, 12, 'Black', 'Circle', 103),
(6.99, 10, 'Black', 'Circle', 95),
(6.99, 8, 'Black', 'Circle', 42),
(4.89, 6, 'Black', 'Circle', 30),
(4.99, 5, 'Black', 'Circle', 34),
(8.99, 18, 'Black', 'Circle', 42),
(8.99, 16, 'Black', 'Circle', 12);

INSERT INTO Stuffings (Brand, PricePerFivelbs, Type)
VALUES 
('Poly-fil', 20.99, 'Premium Polyester Fiber Fill');

INSERT INTO finishedproducts (name, timetomake, totalcosttomake, saleprice, numberinstock)
VALUES 
('Lavender Flower', '02:00:00', 1, 6, 10);

INSERT INTO finishedproductmaterials (finishedproductsid, materialtype, materialid, quantityused)
VALUES
(1, 'Yarn', 25, 0.03);

INSERT INTO Customers (Name, PhoneNumber, EmailAddress)
VALUES 
('Joshua Susana', '765-475-6724', 'kuyang27@gmail.com'),
('Jazmine Susana', '317-478-5119', 'jasusana0408@gmail.com'),
('Cassandra Okoye', '', '');

INSERT INTO Customerpurchases (customerid, finishedproductsid)
VALUES 
(3, 1);

INSERT INTO orders (customerid, orderdate, formofpayment, totalprice)
VALUES
(3, '2024-05-17', 'Zelle', 6);

INSERT INTO orderproducts(orderid, finishedproductsid, quantity)
VALUES
(1, 1, 1);