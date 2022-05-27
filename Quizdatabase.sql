Create database QuizPracticeSystem;
Create table Users(
Username varchar(255) primary key,
Pass varchar(255),
Role varchar(30)
);
Create table Subjects(
SID int,
SName varchar(255),
CONSTRAINT PK_Subject PRIMARY KEY (SID,SName)
)
Create table Quiz(
QID int primary key,
SID int,
SName varchar(255),
TID int,
question varchar(255),
A varchar(255),
B varchar(255),
C varchar(255),
D varchar(255),
answer varchar(255),
FOREIGN KEY (SID,SName) references Subjects(SID,SName),
FOREIGN KEY (TID) references Topic(TID)
)
Create table Topic(
TID int primary key,
SID int,
SName varchar(255),
TName varchar(255),
FOREIGN KEY (SID,SName) references Subjects(SID,SName),
)
Insert into Subjects values (1,'Math')
Insert into Subjects values (2,'English')
Insert into Topic values (1, 1, 'Math','Math 7')
Insert into Topic values (2, 1, 'Math','Math 8')
Insert into Quiz values (1,1,'Math',1,'1+1=?','1','2','3','4','2')
Insert into Quiz values (2,1,'Math',1,'1+2=?','1','2','3','4','3')
Insert into Quiz values (3,1,'Math',1,'1+3=?','1','2','3','4','4')
Insert into Quiz values (4,1,'Math',1,'1+4=?','5','2','3','4','5')
Insert into Quiz values (5,1,'Math',1,'1+5=?','1','6','3','4','6')
Insert into Quiz values (6,1,'Math',1,'1+6=?','1','2','7','4','7')
Insert into Quiz values (7,1,'Math',1,'1+7=?','1','2','3','8','8')
