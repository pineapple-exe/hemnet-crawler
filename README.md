# Hemnet Crawler

## How to use it

#### Setup
- Create a database.
- Refer to your database. You do this inside the value of the connection string that is located in project **HemnetCrawler.Data**, class **HemnetCrawlerDbContext**.

![image](https://user-images.githubusercontent.com/70913967/160587312-67cc75e8-7774-40ff-992c-fd7eaba1d3f2.png)

#### Using it
1. Set **HemnetCrawler.ConsoleApp** as *Startup Project*.
2. Run the project until it is finished on its own accord. The data of all listings and final bids will now be stored in your database.
3. Set **HemnetCrawler.WebApp** as *Startup Project*.
4. Run the project to access the UI.

Listings and final bids are presented in separate tables, in the form of simplified data structures drawn from the database.
Use the search function to filter entities. Click on a property (table header) to order the results by given property. Click on the same property again to reverse the order.

![image](https://user-images.githubusercontent.com/70913967/160587273-7f59af4b-a372-47fa-8f6e-33789ab67aef.png)


By clicking on the ID, you will arrive at the individual page of that entity.

![image](https://user-images.githubusercontent.com/70913967/160587241-c6664786-c603-4bbb-b57a-514a61808cf8.png)


On a Listing page, you can rate the listing to increase the precision of price prediction. This data will also be stored in your database. You can change the rating anytime.

![image](https://user-images.githubusercontent.com/70913967/160587150-99dd157e-6ce9-4b67-a7ed-625cee172913.png)


The Estimation page is linked on the Listing page. If there are final bids with similar properties as the listing at hand, they will be shown on the Estimation page and act as the primary source of price prediction.
Click on "Estimate Final Price" to get the estimation.

***Disclaimer:*** The algorithm behind the estimation feature is quite naive at the moment and therefore most likely not very competent. 

## Architecture

Hemnet Crawler is built in the pattern of hexagonal architecture / ports and adapters architecture.
The following diagram illustrates the most important components and dependencies.

![image](https://user-images.githubusercontent.com/70913967/160587075-1bc0f1d6-aa54-41c3-add8-5016a4d5cd3f.png)

