CREATE TABLE IF NOT EXISTS Company (
	company_id 	varchar(15) PRIMARY KEY,
	name		varchar(255) NOT NULL,
	city		varchar(100) NOT NULL,
	postal_code	char(5),
	address		varchar(100),
	-- bank
	bank_name	varchar(50),
	bank_bic	varchar(30),
	bank_account varchar(100),
	-- info
	biller_name varchar(100),
	email 		varchar(100),
	phone_number	varchar(20)
);

CREATE TABLE IF NOT EXISTS Client (
	client_id 	integer PRIMARY KEY,
	name		varchar(100), -- can be company so we aren't using first/surnames here
	city		varchar(100),
	postal_code char(5),
	address		varchar(100)

);

CREATE TABLE IF NOT EXISTS Bill (
	bill_id		integer PRIMARY KEY,
	company     varchar(15),
	client      integer,
	reference   varchar(100),
	due_date    integer, -- milliseconds
	comments 	text,
	FOREIGN KEY (client) REFERENCES Client(client_id),
 	FOREIGN KEY (company) REFERENCES Company(company_id)
) ;

CREATE TABLE IF NOT EXISTS Product (
	product_id	integer PRIMARY KEY,
	name		varchar(100) NOT NULL,
	quantity 	decimal,
	unit_price	integer, -- cents
	unit		varchar(30),
	vat			decimal
) ;

CREATE TABLE IF NOT EXISTS BillItems (
	product_id integer not NULL,
	bill_id integer not NULL, 
	PRIMARY KEY (product_id, bill_id),
	FOREIGN KEY (product_id) REFERENCES Product(product_id),
	FOREIGN KEY (bill_id) REFERENCES Bill(bill_id)
) WITHOUT ROWID;


-- company
INSERT INTO Company(company_id, name, city, postal_code, address, bank_name, bank_bic, bank_account, biller_name, email, phone_number)
VALUES (
	'Y-TUNNUS',
	'Jeesuksen Naulakauppa',
	'Kajaani',
	'87300',
	'Joku osote',
	'Nordea',
	'NDEAFIHH',
	'1234-123-123-12323',
	'Laskuttaja Pakastin',
	'wapsu.nussii@janinaa.vessassa',
	'040123123123'

);

-- client

INSERT INTO Client(client_id, name, city, postal_code, address)
VALUES (
	NULL,
	'Ville Pätsi',
	'Oulu',
	'12345',
	'Nussintakuja 69'
);

INSERT INTO Client(client_id, name, city, postal_code, address)
VALUES (
	NULL,
	'Erkki LOS',
	'Kasarmi',
	'44444',
	'Aamujen lato 347'
);

-- bill
INSERT INTO Bill(bill_id, company, client, reference, due_date, comments)
VALUES (
	NULL,
	'Y-TUNNUS', -- jeesuksen naulakauppa
	1, -- ville pätsi
	'12343234234',
	1408313084677,
	'Millon mansikanmakuiset kondoomit tulevat myyntiin?'
);

INSERT INTO Bill(bill_id, company, client, reference, due_date, comments)
VALUES (
	NULL,
	'Y-TUNNUS', -- jeesuksen naulakauppa
	1, -- ville pätsi
	'VITTUKUNVITUTTAA',
	1408313084677,
	'Nopea tapa tappaa itsensä?'
);

INSERT INTO Bill(bill_id, company, client, reference, due_date, comments)
VALUES (
	NULL,
	'Y-TUNNUS', -- jeesuksen naulakauppa
	2, -- erkki los
	'AAAAAAAAAAAAAA',
	1408313084677,
	'Saako täältä baretteja?'
);

-- Product

INSERT INTO Product(product_id, name, quantity, unit_price, unit, vat)
VALUES(
	NULL,
	'Suklaa kondoomi',
	50,
	2,
	'kpl',
	24
);


INSERT INTO Product(product_id, name, quantity, unit_price, unit, vat)
VALUES(
	NULL,
	'Banaani kondoomi',
	150,
	1,
	'kpl',
	24
);


INSERT INTO Product(product_id, name, quantity, unit_price, unit, vat)
VALUES(
	NULL,
	'Rynkky',
	1,
	3500,
	'kpl',
	24
);

-- billing products
-- 1 bill == 2 products
-- 2 bill == 3 products
-- 3 bill == 1 products

INSERT INTO BillItems(product_id, bill_id)
VALUES (
	1, --suklaa kortsu
	1 
);

INSERT INTO BillItems(product_id, bill_id)
VALUES (
	2, --banaani kortsu
	1 
);

INSERT INTO BillItems(product_id, bill_id)
VALUES (
	3, --rynkky 
	3
);

INSERT INTO BillItems(product_id, bill_id)
VALUES (
	1, --suklaa kortsu
	2 
);

INSERT INTO BillItems(product_id, bill_id)
VALUES (
	2, --banaani kortsu
	2 
);

INSERT INTO BillItems(product_id, bill_id)
VALUES (
	3, --rynkky kortsu
	2 
);