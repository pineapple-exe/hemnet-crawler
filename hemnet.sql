create table dbo.listing (
	[id] int not null identity,
	[address] nvarchar(96) not null,
	price int not null,
	[description] nvarchar(max) not null,
	home_type nvarchar(22) not null,
	ownership_type nvarchar(11) not null,
	rooms int null,
	[living_area] int not null,
	fee int null,
	[bi_area] int null,
	construction_year int null,
	utilities int null,
	visits int not null,
	days_on_hemnet int not null
	constraint pk_listing primary key (id)
);

create table dbo.[image] (
	[id] int not null identity,
	listing_id int not null,
	image_data varbinary(max) not null,
	content_type nvarchar(9) not null,
	foreign key (listing_id) references listing(id),
	constraint pk_image primary key (id)
);