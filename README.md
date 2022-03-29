# Hemnet Crawler

## How to use it

#### Setup
- Create a database.
- Refer to your database. You do this inside the value of the connection string that is located in project **HemnetCrawler.Data**, class **HemnetCrawlerDbContext**.
https://github.com/pineapple-exe/hemnet-crawler/blob/master/Presentation/connectionString.png?raw=true

#### Using it
1. Set **HemnetCrawler.ConsoleApp** as *Startup Project*.
2. Run the project until it is finished on its own accord. The data of all listings and final bids will now be stored in your database.
3. Set **HemnetCrawler.WebApp** as *Startup Project*.
4. Run the project to access the UI.

Listings and final bids are presented in separate tables, in the form of simplified data structures drawn from the database.
Use the search function to filter entities. Click on a property (table header) to order the results by given property. Click on the same property again to reverse the order.
https://github.com/pineapple-exe/hemnet-crawler/blob/master/Presentation/listings.png?raw=true

By clicking on the ID, you will arrive at the individual page of that entity.
https://github.com/pineapple-exe/hemnet-crawler/blob/master/Presentation/listing.png?raw=true

On a Listing page, you can rate the listing to increase the precision of price prediction. This data will also be stored in your database. You can change the rating anytime.
https://github.com/pineapple-exe/hemnet-crawler/blob/master/Presentation/rateListing.png?raw=true

The Estimation page is linked on the Listing page. If there are final bids with similar properties as the listing at hand, they will be shown on the Estimation page and act as the primary source of price prediction.
Click on "Estimate Final Price" to get the estimation.

***Disclaimer:*** The algorithm behind the estimation feature is quite naive at the moment and therefore most likely not very competent. 

## Architecture

Hemnet Crawler is built in the pattern of hexagonal architecture / ports and adapters architecture.
The following diagram shows the most central components and dependencies.
https://github.com/pineapple-exe/hemnet-crawler/blob/master/Presentation/hemnet-crawler_architecture1x.png?raw=true
