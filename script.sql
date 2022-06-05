--CREATE DATABASE Quizz

CREATE TABLE Subjects
(
	subject_id INT IDENTITY(1,1) PRIMARY KEY,
	subject_name NVARCHAR(50) NOT NULL,
	date_created DATETIME NULL
)

INSERT INTO Subjects VALUES (N'Mathematics','02 June 2022')
INSERT INTO Subjects VALUES (N'Software Engineering','02 June 2022')
INSERT INTO Subjects VALUES (N'English','02 June 2022')
INSERT INTO Subjects VALUES (N'Graphic Design','02 June 2022')

CREATE TABLE Exams
(
	exam_id INT IDENTITY(1,1) PRIMARY KEY,
	code NVARCHAR(200) NOT NULL,
	test_time time NOT NULL,
	num_of_ques INT NOT NULL,
	total_tested INT NOT NULL,
	date_created DATETIME NULL,
)

INSERT INTO Exams VALUES ('ABC121','00:30:00',5,0,'28 May 2022')
INSERT INTO Exams VALUES ('ABC122','00:45:00',5,0,'28 May 2022')
INSERT INTO Exams VALUES ('ABC123','01:00:00',5,0,'28 May 2022')

CREATE TABLE Accounts
(
    account_id NVARCHAR(30) PRIMARY KEY,
	full_name NVARCHAR(50) NULL,
	dob DATETIME NULL,
	gender BIT NULL,
	[username] NVARCHAR(20) NOT NULL,
	[password]  NVARCHAR(20) NOT NULL,
	[role] BIT NOT NULL,
)

INSERT INTO Accounts VALUES (N'1',N'Administrator','','','admin','admin',1)
INSERT INTO Accounts VALUES (N'MS002',N'Trần Thanh Hằng','20 Sep 2020',0,'abc','456',0)
INSERT INTO Accounts VALUES (N'MS003',N'Trần Thanh Vân','21 April 2000',0,'mra','456',0)
INSERT INTO Accounts VALUES (N'MS004',N'Trần Thị Thanh','09 Sep 2000',0,'mrb','456',0)

CREATE TABLE Scores
(
	score_id INT IDENTITY(1,1) PRIMARY KEY,
	score INT NOT NULL,
	exam_id INT FOREIGN KEY (exam_id) REFERENCES Exams(exam_id),
	account_id NVARCHAR(30) FOREIGN KEY (account_id) REFERENCES Accounts(account_id),
	[date_test] DATETIME NULL,	
)

INSERT INTO Scores VALUES (0,1,N'MS002','28 May 2022')
INSERT INTO Scores VALUES (0,2,N'MS003','28 May 2022')
INSERT INTO Scores VALUES (0,3,N'MS004','28 May 2022')

CREATE TABLE BankQuestions
(
	bank_id INT IDENTITY(1,1) PRIMARY KEY,
	bank_name NVARCHAR(100) NOT NULL,
	subject_id INT FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id),
	account_id NVARCHAR(30) FOREIGN KEY (account_id) REFERENCES Accounts(account_id),
)

INSERT INTO BankQuestions VALUES ('MAD101',1,'MS002')
INSERT INTO BankQuestions VALUES ('MAS291',1,'MS003')
INSERT INTO BankQuestions VALUES ('SWD392',2,'MS003')
INSERT INTO BankQuestions VALUES ('SWT301',2,'MS002')
INSERT INTO BankQuestions VALUES ('SWP391',2,'MS002')
INSERT INTO BankQuestions VALUES ('CAA201',4,'MS003')

CREATE TABLE Questions
(
    question_id INT IDENTITY(1,1) PRIMARY KEY,
	content NVARCHAR(max) NOT NULL,
	A NVARCHAR(max) NOT NULL,
	B NVARCHAR(max) NOT NULL,
	C NVARCHAR(max) NOT NULL,
	D NVARCHAR(max) NOT NULL,
	answer VARCHAR(5) NOT NULL,
	is_important BIT NULL,
	type_ques INT NULL, --> 0 is practice, 1 is exam
	bank_id INT FOREIGN KEY (bank_id) REFERENCES BankQuestions(bank_id),
)
--> Question Practice
--> MAE
INSERT INTO Questions VALUES (N'Transportation officials tell us that 70% of drivers wear seat belts while driving. Find the probability that more than 579 drivers in a sample of 800 drivers wear seat belts. Let P(Z < 1.5) = 0.0668; P(Z<0.25) = 0.6',N'0.0668', N'0.9332',N'0.4',N'0.6','B',0,0,2)
INSERT INTO Questions VALUES (N'Suppose X is a uniform random variable over the interval [40, 50). Find the probability that a randomly selected observation exceeds 43.',N'0.3', N'0.1',N'0.7',N'0.9','C',0,0,2)
INSERT INTO Questions VALUES (N'A card is drawn from a standard deck of 52 playing cards. Find the probability that the card is an ace or a king.',N'1/13', N'3/13',N'4/13',N'2/13','D',0,0,2)
INSERT INTO Questions VALUES (N'A pair of dice is thrown twice. What is the probability of getting totals of 7 and 11?',N'1/54', N'1/24',N'1/18',N'1/14','A',0,0,2)
--> DTG302
INSERT INTO Questions VALUES (N'This is any paid form of non-personal presentation and promotion of ideas, goods and services by an identified sponsor',N'Advertising', N'Marketing',N'Communication',N'Media','A',0,0,6)
INSERT INTO Questions VALUES (N'Focused on establishing a corporate identity or winning the public over to the organization''s point of view',N'Institutional Advertising', N'Nonprofit Advertising',N'Public Service Advertising',N'Brand Advertising','A',0,0,6)
INSERT INTO Questions VALUES (N'What are the effects behind effectiveness?',N'Attention', N'Interest',N'Desire',N'All others are correct','D',0,0,6)
INSERT INTO Questions VALUES (N'Which is not role of advertising?',N'Communication role', N'Economic role',N'Managerial role',N'Marketing role','C',0,0,6)
--> MAD
INSERT INTO Questions VALUES (N'Let P(Z < 1.5) = 0.0668; P(Z<0.25) = 0.6',N'0.0668', N'0.9332',N'0.4',N'0.6','B',0,0,1)

CREATE TABLE ExamsQues
(
	exam_ques_id INT IDENTITY(1,1) PRIMARY KEY,
	exam_id INT FOREIGN KEY (exam_id) REFERENCES Exams(exam_id),
	question_id INT FOREIGN KEY (question_id) REFERENCES Questions(question_id),
)

INSERT INTO ExamsQues VALUES (1,1)
INSERT INTO ExamsQues VALUES (1,4)
INSERT INTO ExamsQues VALUES (1,5)
INSERT INTO ExamsQues VALUES (1,6)

