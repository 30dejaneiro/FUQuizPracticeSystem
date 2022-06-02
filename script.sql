--CREATE DATABASE Quizz

CREATE TABLE Subjects
(
	subject_id INT IDENTITY(1,1) PRIMARY KEY,
	subject_name NVARCHAR(50) NOT NULL,
	date_created DATETIME NULL
)

INSERT INTO Subjects VALUES (N'Toán','02 June 2022')
INSERT INTO Subjects VALUES (N'Văn','02 June 2022')
INSERT INTO Subjects VALUES (N'Anh','02 June 2022')
INSERT INTO Subjects VALUES (N'Sử','02 June 2022')


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
	subject_id INT FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id),
)
--> Question Practice
--> Math
INSERT INTO Questions VALUES (N'Số phức nào dưới đây có môđun bằng 5',N'3 + 4i', N'3 + 5i',N'6 - i',N'4 - 7i','A',0,0,1)
INSERT INTO Questions VALUES (N'Số phức nào dưới đây có môđun bằng 5',N'3 + 4i', N'3 + 5i',N'6 - i',N'4 - 7i','A',0,0,1)
INSERT INTO Questions VALUES (N'Số phức nào dưới đây có môđun bằng 5',N'3 + 4i', N'3 + 5i',N'6 - i',N'4 - 7i','A',0,0,1)
INSERT INTO Questions VALUES (N'Số phức nào dưới đây có môđun bằng 5asd',N'3 + 4i', N'3 + 5i',N'6 - i',N'4 - 7i','A',0,0,1)
--> Văn
INSERT INTO Questions VALUES (N'Ai là sáng tác của bài Người Lái Đò Sông Đà?',N'Hồ Xuân Hương', N'Nguyễn Tuân',N'Bác Hồ',N'Nam Cao','B',0,0,2)
INSERT INTO Questions VALUES (N'Ai là sáng tác của bài Người Lái Đò Sông Đà?',N'Hồ Xuân Hương', N'Nguyễn Tuân',N'Bác Hồ',N'Nam Cao','B',0,0,2)
INSERT INTO Questions VALUES (N'Ai là sáng tác của bài Người Lái Đò Sông Đà?',N'Hồ Xuân Hương', N'Nguyễn Tuân',N'Bác Hồ',N'Nam Cao','B',0,0,2)
--> Anh
INSERT INTO Questions VALUES (N'What is the main color of USA flag?',N'Red', N'Blue',N'Green',N'Yellow','B',0,0,3)
INSERT INTO Questions VALUES (N'What is the main color of USA flag?',N'Red', N'Blue',N'Green',N'Yellow','B',0,0,3)
INSERT INTO Questions VALUES (N'What is the main color of USA flag?',N'Red', N'Blue',N'Green',N'Yellow','B',0,0,3)
--> Sử
INSERT INTO Questions VALUES (N'Ai là người đã lợi dụng thủy chiều để đánh phá tàu thuyền của địch?',N'Trần Thánh Tông', N'Quang Trung',N'Ngô Quyền',N'Võ Nguyên Giáp','C',0,0,4)
INSERT INTO Questions VALUES (N'Ai là người đã lợi dụng thủy chiều để đánh phá tàu thuyền của địch?',N'Trần Thánh Tông', N'Quang Trung',N'Ngô Quyền',N'Võ Nguyên Giáp','C',0,0,4)
INSERT INTO Questions VALUES (N'Ai là người đã lợi dụng thủy chiều để đánh phá tàu thuyền của địch?',N'Trần Thánh Tông', N'Quang Trung',N'Ngô Quyền',N'Võ Nguyên Giáp','C',0,0,4)

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

