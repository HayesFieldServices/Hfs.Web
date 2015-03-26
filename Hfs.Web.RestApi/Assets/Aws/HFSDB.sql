USE HFSDB
GO

/* Drop Existing Tables in Correct Order */

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='LegalName'))           DROP TABLE [User].[LegalName]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='PhoneBlacklist'))      DROP TABLE [User].[PhoneBlacklist]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='VerificationCode'))    DROP TABLE [User].[VerificationCode]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='Email'))               DROP TABLE [User].[Email]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='Phone'))               DROP TABLE [User].[Phone]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='PhoneRegion'))         DROP TABLE [User].[PhoneRegion]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='PhoneType'))           DROP TABLE [User].[PhoneType]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='GroupPermission'))     DROP TABLE [User].[GroupPermission]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='AccountGroup'))	     DROP TABLE [User].[AccountGroup]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='Account'))             DROP TABLE [User].[Account]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='NameSuffix'))	         DROP TABLE [User].[NameSuffix]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='Group'))	             DROP TABLE [User].[Group]
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='User' AND TABLE_NAME='Permission'))	         DROP TABLE [User].[Permission]

/* Drop Existing Schema */

IF EXISTS (SELECT * FROM sys.schemas WHERE name='User') EXEC('DROP SCHEMA [User]')

/* Create New Schema */

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name='User') EXEC('CREATE SCHEMA [User]')

/* Create New Tables */

CREATE TABLE [User].[NameSuffix](
	[name_suffix_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_NameSuffix PRIMARY KEY,
	[name_suffix] varchar(35) NOT NULL
)

INSERT INTO [User].[NameSuffix]([name_suffix])
VALUES ('Jr'),
	   ('Sr'),
	   ('II'),
       ('III'),
	   ('IV'),
	   ('Esq')

CREATE TABLE [User].[Account](
	[account_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_User PRIMARY KEY,
	[first_name] varchar(35) NOT NULL,
	[last_name] varchar(35) NOT NULL,
	[username] varchar(75) NOT NULL,
	[pbkdf2_iterations] int NOT NULL,
	[password_salt] binary(32) NOT NULL,
	[password_hash] binary(32) NOT NULL,
	[created_dt] datetime NOT NULL
)

CREATE TABLE [User].[LegalName](
	[legal_name_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_LegalName PRIMARY KEY,
	[account] int NOT NULL,
	[first_name] varchar(35) NOT NULL,
	[middle_name] varchar(35) NULL,
	[last_name] varchar(35) NOT NULL,
	[name_suffix] int NULL,
	CONSTRAINT fk_LegalName_account FOREIGN KEY ([account]) REFERENCES [User].[Account]([account_id]),
	CONSTRAINT fk_LegalName_name_suffix FOREIGN KEY ([name_suffix]) REFERENCES [User].[NameSuffix]([name_suffix_id])
)

CREATE TABLE [User].[Email](
	[email_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_Email PRIMARY KEY,
	[account] int NOT NULL,
	[address] varchar(255) NOT NULL,
	[is_primary] bit NOT NULL,
	[verify_dt] date NULL,
	CONSTRAINT fk_Email_account FOREIGN KEY ([account]) REFERENCES [User].[Account]([account_id])
)

CREATE TABLE [User].[PhoneRegion](
	[phone_region_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_PhoneRegion PRIMARY KEY,
	[region] varchar(50) NOT NULL,
	[code] varchar(30) NOT NULL
)

INSERT INTO [User].[PhoneRegion]([region],[code])
VALUES ('United States','+1'), 
	   ('Canada','+1'),
	   ('Mexico','+52'),
	   ('United Kingdom','+44'),
       ('Afghanistan','+93'),
       ('Albania','+355'),
       ('Algeria','+213'),
       ('Andorra','+376'),
       ('Angola','+244'),
       ('Antigua and Barbuda','+1'),
       ('Argentina','+54'),
       ('Armenia','+374'),
       ('Australia','+61'),
       ('Austria','+43'),
       ('Azerbaijan','+994'),
       ('Bahamas','+1'),
       ('Bahrain','+973'),
       ('Bangladesh','+880'),
       ('Barbados','+1'),
       ('Belarus','+375'),
       ('Belgium','+32'),
       ('Belize','+501'),
       ('Benin','+229'),
       ('Bhutan','+975'),
       ('Bolivia','+591'),
       ('Bosnia and Herzegovina','+387'),
       ('Botswana','+267'),
       ('Brazil','+55'),
       ('Brunei','+673'),
       ('Bulgaria','+359'),
       ('Burkina Faso','+226'),
       ('Burundi','+257'),
       ('Cambodia','+855'),
       ('Cameroon','+237'),
       ('Cape Verde','+238'),
       ('Central African Republic','+236'),
       ('Chad','+235'),
       ('Chile','+56'),
       ('China','+86'),
       ('Colombia','+57'),
       ('Comoros','+269'),
       ('Congo  Kinshasa','+243'),
       ('Congo  Brazzaville','+242'),
       ('Costa Rica','+506'),
       ('Cote d''Ivoire','+225'),
       ('Croatia','+385'),
       ('Cuba','+53'),
       ('Cyprus','+357'),
       ('Czech Republic','+420'),
       ('Denmark','+45'),
       ('Djibouti','+253'),
       ('Dominica','+1'),
       ('Dominican Republic','+1'),
       ('Ecuador','+593'),
       ('Egypt','+20'),
       ('El Salvador','+503'),
       ('Equatorial Guinea','+240'),
       ('Eritrea','+291'),
       ('Estonia','+372'),
       ('Ethiopia','+251'),
       ('Fiji','+679'),
       ('Finland','+358'),
       ('France','+33'),
       ('Gabon','+241'),
       ('Gambia','+220'),
       ('Georgia','+995'),
       ('Germany','+49'),
       ('Ghana','+233'),
       ('Greece','+30'),
       ('Grenada','+1'),
       ('Guatemala','+502'),
       ('Guinea','+224'),
       ('Guinea-Bissau','+245'),
       ('Guyana','+592'),
       ('Haiti','+509'),
       ('Honduras','+504'),
       ('Hungary','+36'),
       ('Iceland','+354'),
       ('India','+91'),
       ('Indonesia','+62'),
       ('Iran','+98'),
       ('Iraq','+964'),
       ('Ireland','+353'),
       ('Israel','+972'),
       ('Italy','+39'),
       ('Jamaica','+1'),
       ('Japan','+81'),
       ('Jordan','+962'),
       ('Kazakhstan','+7'),
       ('Kenya','+254'),
       ('Kiribati','+686'),
       ('North Korea','+850'),
       ('South Korea','+82'),
       ('Kuwait','+965'),
       ('Kyrgyzstan','+996'),
       ('Laos','+856'),
       ('Latvia','+371'),
       ('Lebanon','+961'),
       ('Lesotho','+266'),
       ('Liberia','+231'),
       ('Libya','+218'),
       ('Liechtenstein','+423'),
       ('Lithuania','+370'),
       ('Luxembourg','+352'),
       ('Macedonia','+389'),
       ('Madagascar','+261'),
       ('Malawi','+265'),
       ('Malaysia','+60'),
       ('Maldives','+960'),
       ('Mali','+223'),
       ('Malta','+356'),
       ('Marshall Islands','+692'),
       ('Mauritania','+222'),
       ('Mauritius','+230'),
       ('Micronesia','+691'),
       ('Moldova','+373'),
       ('Monaco','+377'),
       ('Mongolia','+976'),
       ('Montenegro','+382'),
       ('Morocco','+212'),
       ('Mozambique','+258'),
       ('Myanmar','+95'),
       ('Namibia','+264'),
       ('Nauru','+674'),
       ('Nepal','+977'),
       ('Netherlands','+31'),
       ('New Zealand','+64'),
       ('Nicaragua','+505'),
       ('Niger','+227'),
       ('Nigeria','+234'),
       ('Norway','+47'),
       ('Oman','+968'),
       ('Pakistan','+92'),
       ('Palau','+680'),
       ('Panama','+507'),
       ('Papua New Guinea','+675'),
       ('Paraguay','+595'),
       ('Peru','+51'),
       ('Philippines','+63'),
       ('Poland','+48'),
       ('Portugal','+351'),
       ('Qatar','+974'),
       ('Romania','+40'),
       ('Russia','+7'),
       ('Rwanda','+250'),
       ('Saint Kitts and Nevis','+1'),
       ('Saint Lucia','+1'),
       ('Saint Vincent and the Grenadines','+1'),
       ('Samoa','+685'),
       ('San Marino','+378'),
       ('Sao Tome and Principe','+239'),
       ('Saudi Arabia','+966'),
       ('Senegal','+221'),
       ('Serbia','+381'),
       ('Seychelles','+248'),
       ('Sierra Leone','+232'),
       ('Singapore','+65'),
       ('Slovakia','+421'),
       ('Slovenia','+386'),
       ('Solomon Islands','+677'),
       ('Somalia','+252'),
       ('South Africa','+27'),
       ('Spain','+34'),
       ('Sri Lanka','+94'),
       ('Sudan','+249'),
       ('Suriname','+597'),
       ('Swaziland','+268'),
       ('Sweden','+46'),
       ('Switzerland','+41'),
       ('Syria','+963'),
       ('Tajikistan','+992'),
       ('Tanzania','+255'),
       ('Thailand','+66'),
       ('Timor-Leste','+670'),
       ('Togo','+228'),
       ('Tonga','+676'),
       ('Trinidad and Tobago','+1'),
       ('Tunisia','+216'),
       ('Turkey','+90'),
       ('Turkmenistan','+993'),
       ('Tuvalu','+688'),
       ('Uganda','+256'),
       ('Ukraine','+380'),
       ('United Arab Emirates','+971'),
       ('Uruguay','+598'),
       ('Uzbekistan','+998'),
       ('Vanuatu','+678'),
       ('Vatican City','+379'),
       ('Venezuela','+58'),
       ('Vietnam','+84'),
       ('Yemen','+967'),
       ('Zambia','+260'),
       ('Zimbabwe','+263'),
       ('Abkhazia','+995'),
       ('Taiwan','+886'),
       ('Nagorno-Karabakh','+277'),
       ('Northern Cyprus','+1'),
       ('Pridnestrovie','+1'),
       ('Somaliland','+252'),
       ('South Ossetia','+995'),
       ('Christmas Island','+61'),
       ('Cocos Islands','+61'),
       ('Norfolk Island','+672'),
       ('New Caledonia','+687'),
       ('French Polynesia','+689'),
       ('Mayotte','+262'),
       ('Saint Barthelemy','+590'),
       ('Saint Martin','+590'),
       ('Saint Pierre and Miquelon','+508'),
       ('Wallis and Futuna','+681'),
       ('Cook Islands','+682'),
       ('Niue','+683'),
       ('Tokelau','+690'),
       ('Guernsey','+44'),
       ('Isle of Man','+44'),
       ('Jersey','+44'),
       ('Anguilla','+1'),
       ('Bermuda','+1'),
       ('British Indian Ocean Territory','+246'),
       ('British Sovereign Base Areas','+357'),
       ('British Virgin Islands','+1'),
       ('Cayman Islands','+1'),
       ('Falkland Islands','+500'),
       ('Gibraltar','+350'),
       ('Montserrat','+1'),
       ('Saint Helena','+290'),
       ('Turks and Caicos Islands','+1'),
       ('Northern Mariana Islands','+1'),
       ('Puerto Rico','+1'),
       ('American Samoa','+1'),
       ('Guam','+1'),
       ('U.S. Virgin Islands','+1'),
       ('Hong Kong','+852'),
       ('Macau','+853'),
       ('Faroe Islands','+298'),
       ('Greenland','+299'),
       ('French Guiana','+594'),
       ('Guadeloupe','+590'),
       ('Martinique','+596'),
       ('Reunion','+262'),
       ('Aland','+340'),
       ('Aruba','+297'),
       ('Netherlands Antilles','+599'),
       ('Svalbard','+47'),
       ('Ascension','+247'),
       ('Tristan da Cunha','+290')

CREATE TABLE [User].[PhoneType](
	[phone_type_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_PhoneType PRIMARY KEY,
	[type] varchar(10) NOT NULL
)

INSERT INTO [User].[PhoneType]([type])
VALUES ('home'),
	   ('office'),
	   ('mobile'),
       ('fax')

CREATE TABLE [User].[Phone](
	[phone_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_Phone PRIMARY KEY,
	[account] int NOT NULL,
	[number] varchar,
	[region] int NOT NULL,
	[type] int NOT NULL,
	[is_primary] bit NOT NULL,
	[verify_dt] date NULL,
	CONSTRAINT fk_Phone_account FOREIGN KEY ([account]) REFERENCES [User].[Account]([account_id]),
	CONSTRAINT fk_Phone_region FOREIGN KEY ([region]) REFERENCES [User].[PhoneRegion]([phone_region_id]),
	CONSTRAINT fk_Phone_type FOREIGN KEY ([type]) REFERENCES [User].[PhoneType]([phone_type_id])
)

CREATE TABLE [User].[Group](
	[group_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_Group PRIMARY KEY,
	[name] varchar(50) NOT NULL
)

CREATE TABLE [User].[Permission](
	[permission_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_Permission PRIMARY KEY,
	[name] varchar(50) NOT NULL
)

CREATE TABLE [User].[GroupPermission](
	[group] int NOT NULL,
	[permission] int NOT NULL,
	CONSTRAINT fk_GroupPermission_group FOREIGN KEY ([group]) REFERENCES [User].[Group]([group_id]),
	CONSTRAINT fk_GroupPermission_permission FOREIGN KEY ([permission]) REFERENCES [User].[Permission]([permission_id]),
	CONSTRAINT pk_GroupPermission PRIMARY KEY ([group],[permission])
)

CREATE TABLE [User].[AccountGroup](
	[account] int NOT NULL,
	[group] int NOT NULL,
	CONSTRAINT fk_AccountGroup_account FOREIGN KEY ([account]) REFERENCES [User].[Account]([account_id]),
	CONSTRAINT fk_AccountGroup_group FOREIGN KEY ([group]) REFERENCES [User].[Group]([group_id]),
	CONSTRAINT pk_AccountGroup PRIMARY KEY ([account],[group])
)

CREATE TABLE [User].[VerificationCode](
	[code_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_VerificationCode PRIMARY KEY,
	[code] int DEFAULT ROUND(RAND(CHECKSUM(NEWID())) * (99999999 - 10000000), 0) + 10000000,
	[phone_number] varchar(20) NOT NULL,
	[expiration_dt] datetime NOT NULL
)

CREATE TABLE [User].[PhoneBlacklist](
	[phone_blacklist_id] int NOT NULL IDENTITY(1,1) CONSTRAINT pk_PhoneBlacklist PRIMARY KEY,
	[phone_number] varchar(20) NOT NULL,
	[method] varchar(5) NOT NULL,
	[added_dt] datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
)