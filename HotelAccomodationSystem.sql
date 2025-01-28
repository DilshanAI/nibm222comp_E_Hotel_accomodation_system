
CREATE DATABASE HotelAccomodationSystem;

Use HotelAccomodationSystem;

CREATE TABLE Login (
    ID varchar(50),
    Username varchar(50),
    Password varchar(50)
);

INSERT INTO Login (ID, Username, Password)
VALUES ('1', 'HASNIBM222', 'HAS22222');


INSERT INTO Login (ID, Username, Password)
VALUES ('2', 'Admin', '123');
CREATE TABLE Room (
    RoomID VARCHAR(10) PRIMARY KEY,
    RoomType VARCHAR(30),
    BedType VARCHAR(30),
    Price DECIMAL(8,2),
    CreateDate DATE,
	UpdatedDate DATE
);

ALTER TABLE Room
ADD UpdatedDate DATE NULL;



INSERT INTO Room (RoomID, RoomType, BedType, Price, CreateDate)
VALUES 
('R001', 'Single', 'Queen', 100.00, '2024-05-31'),
('R002', 'Double', 'King', 150.00, '2024-05-31'),
('R003', 'Suite', 'King', 250.00, '2024-05-31');

SELECT * FROM room;

SELECT Distinct RoomType FROM room;

SELECT RoomType FROM room;
SELECT BedType FROM room;

SELECT Cemail FROM Customer WHERE CustomerID = 'C0001';

CREATE TABLE Customer (
    CustomerID VARCHAR(10) PRIMARY KEY,
    CusName VARCHAR(30),
	C_Address varchar(60) not null,
    CusNic varchar(12) not null,
    Cus_Tele char(10) not null,
    Cemail varchar(50) not null,
    CreateDate DATE
);

INSERT INTO Customer VALUES 
('C0001','Dilsahn','Galle','922586089v','0775263522','sehelyamadumani@gmail.com','2024-05-31'),
('C0002','Kamal','Kakirawa','822586089v','0714254569','sehelyamadumani@gmail.com','2024-05-11'),
('C0003','Nimal','Kurunagala','962365089v','0725896458','sehelyamadumani@gmail.com','2024-05-12'),
('C0004','Ravindu','Colombo3', '922486089v','0702485632','sehelyamadumani@gmail.com','2024-05-13'),
('C0005','Pasan','Maradana','922167089v','0777145878','sehelyamadumani@gmail.com','2024-05-14');

Select * from Reservation;

create table Reservation
(ReID varchar(10) primary key ,
Re_date Date,
check_in Date,
check_out Date,
Remarks varchar(20),
No_Person int,
CusID VARCHAR(10) Foreign Key  References Customer(CustomerID),
RoID VARCHAR(10) Foreign Key  References Room(RoomID));


			
