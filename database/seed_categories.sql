-- Run this against your 'swoop' database to ensure common categories exist.
USE `swoop`;

INSERT INTO categories (name)
SELECT 'Electronics' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM categories WHERE name = 'Electronics');

INSERT INTO categories (name)
SELECT 'Gardening' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM categories WHERE name = 'Gardening');

INSERT INTO categories (name)
SELECT 'Furniture' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM categories WHERE name = 'Furniture');

INSERT INTO categories (name)
SELECT 'Fashion' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM categories WHERE name = 'Fashion');

INSERT INTO categories (name)
SELECT 'Vehicles' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM categories WHERE name = 'Vehicles');