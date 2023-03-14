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
INSERT INTO Artist (artistName, artistBio) VALUES('Billy Talent', 'sample description');
INSERT INTO Artist (artistName, artistBio) VALUES('The Wiggles', 'sample description');
INSERT INTO Artist (artistName, artistBio) VALUES('Sum 41', 'sample description');
INSERT INTO Artist (artistName, artistBio) VALUES('Drake', 'sample description');
INSERT INTO Artist (artistName, artistBio) VALUES('Prince (Hologram)', 'sample description');
INSERT INTO Artist (artistName, artistBio) VALUES('Beethoven', 'sample description');
INSERT INTO Artist (artistName, artistBio) VALUES('Toto', 'sample description');

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