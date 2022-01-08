--Create Schemas
CREATE TABLE Person (
	id int PRIMARY KEY NOT NULL,
	dispName varchar(255),
	caste int
);

CREATE TABLE LoginData (
	userid int PRIMARY KEY REFERENCES Person(id),
	uname varchar(255) NOT NULL,
	pass varchar(255) NOT NULL
);

CREATE TABLE ChatMessage (
	sender int REFERENCES Person(id),
	sentTime datetime,
	content varchar(1000)
);
GO
--populate tables
INSERT INTO Person 
	VALUES 
	(0, 'Nrad', 0),
	(1, 'Chad', 1),
	(2, 'Mad', 0),
	(3, 'Sad', 0);
GO

INSERT INTO LoginData
	VALUES
	(0, 'NradTheMan', '1wish1wasChad'),
	(1, 'ChadLogin', 'lmao'),
	(2, 'MadLad69', 'GIVEMEFR!#ND%'),
	(3, 'SadBoi', '12wan7firends');

INSERT INTO ChatMessage
	VALUES (0, '2021-12-12T21:34:29', 'Hey, How are ya?'),
		(1, '2021-12-12T21:35:29', 'Fine.'),
		(1, '2021-12-12T21:35:31', 'WBU?.'),
		(0, '2021-12-12T21:37:02', 'I Can\''t Complain'),
		(2, '2021-12-12T21:39:34', 'No one''s gonna ask about how I feel?');
GO


SELECT dispName, caste as "type", content, sentTime
FROM Person JOIN ChatMessage ON Person.id=ChatMessage.sender
ORDER BY sentTime;
GO