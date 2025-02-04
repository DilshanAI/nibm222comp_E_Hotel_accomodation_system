
CREATE DATABASE HotelAccomodationSystem;

Use HotelAccomodationSystem;

-------------------------------------------------------------------------------------------------------

--Login--
CREATE TABLE Login (
    ID varchar(50),
    Username varchar(50),
    Password varchar(50)
);

INSERT INTO Login (ID, Username, Password)
VALUES ('1', 'HASNIBM222', 'HAS22222');

----------------------------------------------------------------------------------------------------------

--Room--
CREATE TABLE Room (
    RoomID VARCHAR(10) PRIMARY KEY,
    RoomType VARCHAR(30),
    BedType VARCHAR(30),
    Price DECIMAL(8,2),
    CreateDate DATE,
	UpdatedDate DATE NULL
);


INSERT INTO Room (RoomID, RoomType, BedType, Price, CreateDate, UpdatedDate)
VALUES 
('R001', 'Single', 'Queen', 100.00, '2024-05-31', '2024-05-31'),
('R002', 'Double', 'King', 150.00, '2024-05-31','2024-05-31'),
('R003', 'Suite', 'King', 250.00, '2024-05-31','2024-05-31');

----------------------------------------------------------------------------------------------------------------

--Customer--
CREATE TABLE Customer (
    CustomerID VARCHAR(10) PRIMARY KEY,
    CusName VARCHAR(30),
	C_Address varchar(60) not null,
    CusNic varchar(13) not null,
    Cus_Tele char(10) not null,
    Cemail varchar(50) not null,
    CreateDate DATE,
	UpdateDate varchar(15) NULL
);

INSERT INTO Customer VALUES 
('C001','Dilsahn','Galle','922586089v','0775263522','sehelyamadumani@gmail.com','2024-05-31','2024-05-31'),
('C002','Kamal','Kakirawa','822586089v','0714254569','sehelyamadumani@gmail.com','2024-05-11','2024-05-31'),
('C003','Nimal','Kurunagala','962365089v','0725896458','sehelyamadumani@gmail.com','2024-05-12','2024-05-31'),
('C004','Ravindu','Colombo3', '922486089v','0702485632','sehelyamadumani@gmail.com','2024-05-13','2024-05-31'),
('C005','Pasan','Maradana','922167089v','0777145878','sehelyamadumani@gmail.com','2024-05-14','2024-05-31');

----------------------------------------------------------------------------------------------------------------------------------------


--Employee--
CREATE TABLE Employee (
    EmployeeID VARCHAR(10) PRIMARY KEY,
    Name VARCHAR(30),
    Mobile varchar(12) not null,
    NIC char(15) not null,
    Address varchar(50) not null,
	Email varchar(50) not null,
	Designation varchar(50) not null,
    CreateDate DATE,
	UpdateDate DATE null
);

INSERT INTO Employee VALUES 
('E001','Dilsahn','0775263522','922586089v','Galle','sehelyamadumani@gmail.com','Chef','2024-05-31','2024-05-31'),
('E002','Kamal','0714254569','822586089v','Kakirawa','sehelyamadumani@gmail.com','Hotel Manager','2024-05-11','2024-05-31'),
('E003','Nimal','0725896458','962365089v','Kurunegala','sehelyamadumani@gmail.com','Waiter/Waitress','2024-05-12','2024-05-31'),
('E004','Ravindu','0702485632', '922486089v','Galle','sehelyamadumani@gmail.com','Security Officer','2024-05-13','2024-05-31'),
('E005','Pasan','0777145878','922167089v','Maradana','sehelyamadumani@gmail.com','Maintenance Staff','2024-05-14','2024-05-31');

-------------------------------------------------------------------------------------------------------------------------------------------------------

--Inventory--

CREATE TABLE Inventory (
    InventoryID VARCHAR(10) PRIMARY KEY, 
    RoomID VARCHAR(10) NOT NULL,  
    InventoryName VARCHAR(50) NOT NULL,  
    InventoryType VARCHAR(50),  
    Quantity INT , 
    CreateDate DATE , 
    UpdatedDate DATE NULL, 
    FOREIGN KEY (RoomID) REFERENCES Room(RoomID)  
);
select * from Inventory;

INSERT INTO Inventory (InventoryID,RoomID, InventoryName, InventoryType, Quantity, CreateDate, UpdatedDate)
VALUES 
('I001','R001', 'Television', 'Electronics', 1, '2024-06-01','2024-06-01'),
('I002','R001', 'Queen bed', 'Furniture', 1, '2024-06-01', '2024-06-01'),
('I003','R002', 'King Bed', 'Furniture', 1, '2024-06-01','2024-06-01'),
('I004','R002', 'Arm chair', 'Furniture', 1, '2024-06-01','2024-06-01'),
('I005','R003', 'King Bed', 'Furniture', 1, '2024-06-01','2024-06-01');


--------------------------------------------------------------------------------------------------------------


create table Reservation
(ReID varchar(10) primary key ,
Re_date Date,
check_in Date,
check_out Date,
Remarks varchar(20),
No_Person int,
CusID VARCHAR(10) Foreign Key  References Customer(CustomerID),
RoID VARCHAR(10) Foreign Key  References Room(RoomID));






