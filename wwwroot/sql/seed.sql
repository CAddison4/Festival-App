/* Seed tables */
/* Creates one default admin user 'admin@home.com' with password 'P@ssw0rd!' */
/* Creates dummy data for one festival */

BEGIN TRANSACTION;

/* Seed TicketType */
INSERT INTO TicketType VALUES('General Admission', 50.00);
INSERT INTO TicketType VALUES('VIP', 100.00);

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
INSERT INTO FestivalTicketType VALUES(1, 2, 10);

/* Seed FestivalArtist */
INSERT INTO FestivalArtist VALUES(1, 1);
INSERT INTO FestivalArtist VALUES(1, 2);
INSERT INTO FestivalArtist VALUES(1, 3);
INSERT INTO FestivalArtist VALUES(1, 4);
INSERT INTO FestivalArtist VALUES(1, 5);
INSERT INTO FestivalArtist VALUES(1, 6);
INSERT INTO FestivalArtist VALUES(1, 7);

/* Seed User table with a default admin user */
INSERT INTO [User] VALUES('admin@home.com', 'Smith', 'Bob', 1);

/* Seed AspNetUsers table with the default admin identity user */
INSERT INTO AspNetUsers VALUES(
	'f903e641-5e27-4eb9-bd68-dbf111815b11',
	'admin@home.com',
	'ADMIN@HOME.COM',
	'admin@home.com',
	'ADMIN@HOME.COM',
	1,
	'AQAAAAIAAYagAAAAEDGgzbGWc2E30xB51HWXoAmREx38AjYk0smd4PuVTRzpAzSpFyDlpvVk+i4E2dVI3Q==',
	'NNT5KYXRUHESXLER7BWRWAQ2IDX3OHPQ',
	'f1a5fffb-e034-4dd0-8a6c-66e3be7327b4',
	NULL,
	0,
	0,
	NULL,
	1,
	0
);

/* Seed the AspNetRoles table with the admin role as an identity role */
INSERT INTO AspNetRoles VALUES('Admin', 'Admin', 'ADMIN', NULL);

/* Seed the AspNetUserRoles bridge table to assign the admin identity role to the default admin identity user */
INSERT INTO AspNetUserRole VALUES('f903e641-5e27-4eb9-bd68-dbf111815b11', 'Admin');

COMMIT;