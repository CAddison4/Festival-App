/* Before running, initialize a Concert database	*/
/* This script file creates the following tables:	*/
/* User, Order, Ticket, Festival, Artist,	*/
/* FestivalArtists, TicketType, FestivalTicketType	*/
/* and loads the default data rows			*/

BEGIN TRANSACTION;

/* Drop tables, children first, then parents */
DROP TABLE IF EXISTS Ticket;
DROP TABLE IF EXISTS FestivalTicketType;
DROP TABLE IF EXISTS TicketType;
DROP TABLE IF EXISTS CurrentFestival;
DROP TABLE IF EXISTS FestivalArtist;
DROP TABLE IF EXISTS Artist;
DROP TABLE IF EXISTS Festival;
DROP TABLE IF EXISTS [Order];
DROP TABLE IF EXISTS [User];

/* Create tables, parents first, then children */
CREATE TABLE [User] (
	email			VARCHAR(255)	PRIMARY KEY,
	lastName		VARCHAR(255)	NOT NULL,
	firstName		VARCHAR(255)	NOT NULL,
	[admin]			BIT	DEFAULT 0	NOT NULL
);

CREATE TABLE [Order] (
	orderID			INT IDENTITY	PRIMARY KEY,
	orderDate		DATE			NOT NULL	DEFAULT  CAST( GETDATE() AS DATE ),
	email			VARCHAR(255)	NOT NULL,
	payerEmail		VARCHAR(255)	NOT NULL,
	FOREIGN KEY (email)				REFERENCES [User](email)
);

CREATE TABLE Festival (
	festivalID		INT IDENTITY	PRIMARY KEY,
	[date]			DATE			NOT NULL,
	[location]		VARCHAR(255)	NOT NULL,
	isCurrent		BIT				NOT NULL
);

CREATE TABLE Artist (
	artistID		INT IDENTITY	PRIMARY KEY,
	artistName		VARCHAR(255)	NOT NULL,
	artistBio		VARCHAR(4095),
	imgPath			VARCHAR(255)
);

CREATE TABLE FestivalArtist (
	festivalID		INT				NOT NULL,
	artistID		INT				NOT NULL,
	FOREIGN KEY (festivalID)		REFERENCES Festival(festivalID),
	FOREIGN KEY (artistID)			REFERENCES Artist(artistID),
	PRIMARY KEY (festivalID, artistID)
);

CREATE TABLE TicketType (
	ticketTypeID	INT IDENTITY	PRIMARY KEY,
	[type]			VARCHAR(255)	NOT NULL,
	price			MONEY			NOT NULL,
);

CREATE TABLE FestivalTicketType (
	festivalID		INT				NOT NULL,
	ticketTypeID	INT				NOT NULL,
	quantity		INT				NOT NULL,
	FOREIGN KEY (festivalID)		REFERENCES Festival(festivalID),
	FOREIGN KEY (ticketTypeID)		REFERENCES TicketType([ticketTypeID]),
	PRIMARY KEY (festivalID, ticketTypeID)
);

CREATE TABLE Ticket (
	ticketID		INT IDENTITY	PRIMARY KEY,
	orderID			INT				NOT NULL,
	festivalID		INT				NOT NULL,
	ticketTypeID	INT				NOT NULL,
	FOREIGN KEY (festivalID)		REFERENCES Festival(festivalID),
	FOREIGN KEY (ticketTypeID)		REFERENCES TicketType(ticketTypeID),
	FOREIGN KEY (orderID)			REFERENCES [Order](orderID)
);

/* Seed tables */

/* Seed TicketType */
INSERT INTO TicketType VALUES('General Admission', 50.00);

/* Seed Artist */
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('Billy Talent', 'Billy Talent is a Canadian rock band from Mississauga, Ontario. They formed in 1993 with lead vocalist Benjamin Kowalewicz, guitarist Ian D Sa, bassist Jonathan Gallant, and drummer Aaron Solowoniuk.In the 28 years since their inception, Billy Talent has sold well over a million physical albums in Canada alone and nearly 3 million albums internationally.','billy-talent-2012.jpg');
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('The Wiggles', 'The Wiggles are an Australian childrens music group formed in Sydney in 1991. The group are currently composed of Anthony Field, Lachlan Gillespie, Simon Pryce and Tsehay Hawkins, as well as supporting members Evie Ferris, John Pearce, Caterina Mete and Lucia Field. The Wiggles were founded in 1991 and sing well known hits such as "Fruit Salad"!', 'WIGGLES30.jpeg');
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('Sum 41', 'Sum 41 is a Canadian rock band from Ajax, Ontario. Originally called Kaspir, the band was formed in 1996 and currently consists of Deryck Whibley, Dave Baksh, Jason "Cone" McCaslin, Tom Thacker, and Frank Zummo.', 'SUM41.png');
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('Drake', 'Aubrey Drake Graham is a Canadian rapper and singer. An influential figure in contemporary popular music, Drake has been credited for popularizing singing and R&B sensibilities in hip hop. Gaining recognition by starring as Jimmy Brooks in the CTV teen drama series Degrassi: The Next Generation (2001–08), Drake pursued a career in music releasing his debut mixtape Room for Improvement in 2006.', 'DRAKE.jpg');
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('Prince (Hologram)', 'Prince Rogers Nelson, commonly known mononymously as Prince, was an American singer-songwriter, musician, and record producer. The recipient of numerous awards and nominations, he is widely regarded as one of the greatest musicians of his generation.', 'prince.jpg');
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('Beethoven', 'Ludwig van Beethoven was a German composer and pianist. Beethoven remains one of the most admired composers in the history of Western music; his works rank amongst the most performed of the classical music repertoire and span the transition from the Classical period to the Romantic era in classical music.', 'Beethoven.jpg');
INSERT INTO Artist (artistName, artistBio, imgPath) VALUES('Toto', 'Toto is an American rock band formed in 1977 in Los Angeles, California. Toto is known for a musical style that combines elements of pop, rock, soul, funk, progressive rock, hard rock, R&B, blues, and jazz.', 'toto.jpg');

/* Seed Festival */
INSERT INTO Festival VALUES(CONVERT(DATE, '06/03/2023', 101), 'Vancouver', 1);

/* Seed FestivalTicketType */
INSERT INTO FestivalTicketType VALUES(1, 1, 20000);

/* Seed FestivalArtist */
INSERT INTO FestivalArtist VALUES(1, 1);
INSERT INTO FestivalArtist VALUES(1, 2);
INSERT INTO FestivalArtist VALUES(1, 3);
INSERT INTO FestivalArtist VALUES(1, 4);
INSERT INTO FestivalArtist VALUES(1, 5);
INSERT INTO FestivalArtist VALUES(1, 6);
INSERT INTO FestivalArtist VALUES(1, 7);

COMMIT;