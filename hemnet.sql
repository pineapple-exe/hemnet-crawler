create table dbo.listing (
	id int not null identity,
	[address] nvarchar not null,
	price int not null,
	[description] nvarchar not null,
	home_type nvarchar not null,
	type_of_ownership nvarchar not null,
	rooms int null,
	[living_area] int not null,
	fee int null,
	[bi_area] int null,
	year_of_construction int null,
	utilities int null,
	number_of_visits int not null,
	days_on_hemnet int not null
	constraint PK_listing primary key (id)
);

create table dbo.[image] (
	listing_id int not null,
	image_data varbinary(max) not null,
	content_type nvarchar not null,
	foreign key (listing_id) references listing(id)
);