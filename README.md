# cSharpStoreAndBrands

####This is a website for Store and Brands application written in cSharp
####By: Neil Larion

## Description

###This is a project that allows the user to store shoe stores and the brands of shoes they carry. It also meets the following criteria:
* User can create, show, update and delete stores.
* User can create brands and assigned them to a store.
* There is a many to many relcationship between brands and shoe stores.
* When a user is viewing a single store, they can see all the brands at that store and allow them to add a brand to that store.
* When a user is viewing a single brand the website will list out all of the stores that carry that brand and allow them to add a store to that brand. 

This project was developed during a "Code Review" session at Epicodus, which is a solo project designed to test our knowledge of the materials learned over the previous week's study. It's an honor to be apart of a coding community where people are genuinely interested in what they're doing.

## Setup/Installation Requirements
- Clone this repository.
- Use the .sql in the root directory to make the databases. Or follow these commands in SQLCMD/SQL Server to create shoe_stores and shoe_stores_test:
  * CREATE DATABASE shoe_stores;
  - GO
  - CREATE TABLE shoes (id INT IDENTITY(1,1), name VARCHAR(255));
  - CREATE TABLE brands (id INT IDENTITY(1,1), name VARCHAR(255));
  - CREATE TABLE shoes_brands (id INT IDENTITY(1,1), shoes_id INT, brands_id INT);
  - GO
- Install Nancy the web viewer
- Build the project using "dnu restore".
- Run the project by calling "dnx kestrel"
- I suggest chrome or chromium to run the site.

## Support and contact details
* http://www.neillarion.com
* neil.larion@gmail.com
* [@nlarion](https://twitter.com/nlarion)

## Technologies Used
* SQL
* CSS
* HTML
* C#
* Bootstrap
* Nancy
* Razor web engine

### License

This work is licensed under a [Creative Commons Attribution 4.0 International License.](http://creativecommons.org/licenses/by/4.0/) 2016
