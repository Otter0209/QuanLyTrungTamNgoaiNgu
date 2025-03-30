--------------------------------------------------
-- 1. Tạo database (nếu chưa tồn tại)
--------------------------------------------------
IF DB_ID('TrungTamNgoaiNgu') IS NULL
BEGIN
    CREATE DATABASE TrungTamNgoaiNgu;
END
GO

USE TrungTamNgoaiNgu;
GO

--------------------------------------------------
-- 2. Xóa các bảng cũ theo thứ tự phụ thuộc (DROP từ bảng con đến bảng cha)
--------------------------------------------------
IF OBJECT_ID('dbo.FEEDBACK', 'U') IS NOT NULL DROP TABLE dbo.FEEDBACK;
GO
IF OBJECT_ID('dbo.ATTENDANCE', 'U') IS NOT NULL DROP TABLE dbo.ATTENDANCE;
GO
IF OBJECT_ID('dbo.ENROLLMENT', 'U') IS NOT NULL DROP TABLE dbo.ENROLLMENT;
GO
IF OBJECT_ID('dbo.BUOIHOC', 'U') IS NOT NULL DROP TABLE dbo.BUOIHOC;
GO
IF OBJECT_ID('dbo.STUDENT', 'U') IS NOT NULL DROP TABLE dbo.STUDENT;
GO
IF OBJECT_ID('dbo.PARENT', 'U') IS NOT NULL DROP TABLE dbo.PARENT;
GO
IF OBJECT_ID('dbo.CLASS', 'U') IS NOT NULL DROP TABLE dbo.CLASS;
GO
IF OBJECT_ID('dbo.TEACHER', 'U') IS NOT NULL DROP TABLE dbo.TEACHER;
GO
IF OBJECT_ID('dbo.[USER]', 'U') IS NOT NULL DROP TABLE dbo.[USER];
GO
IF OBJECT_ID('dbo.ROLE', 'U') IS NOT NULL DROP TABLE dbo.ROLE;
GO

--------------------------------------------------
-- 3. Tạo bảng ROLE
--------------------------------------------------
CREATE TABLE ROLE (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);
GO

--------------------------------------------------
-- 4. Tạo bảng [USER]
--------------------------------------------------
CREATE TABLE [USER] (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    RoleID INT NOT NULL,
    CONSTRAINT FK_User_Role FOREIGN KEY (RoleID) REFERENCES ROLE(RoleID)
);
GO

--------------------------------------------------
-- 5. Tạo bảng PARENT
--------------------------------------------------
CREATE TABLE PARENT (
    ParentID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100)
);
GO

--------------------------------------------------
-- 6. Tạo bảng STUDENT
--------------------------------------------------
CREATE TABLE STUDENT (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100),
    DateOfBirth DATE,
    Address NVARCHAR(200),
    ParentID INT,
    CONSTRAINT FK_Student_Parent FOREIGN KEY (ParentID)
        REFERENCES PARENT(ParentID)
);
GO

--------------------------------------------------
-- 7. Tạo bảng TEACHER
--------------------------------------------------
CREATE TABLE TEACHER (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100),
    Expertise NVARCHAR(100)
);
GO

--------------------------------------------------
-- 8. Tạo bảng CLASS
--------------------------------------------------
CREATE TABLE CLASS (
    ClassID INT IDENTITY(1,1) PRIMARY KEY,
    ClassName NVARCHAR(100) NOT NULL,
    Schedule NVARCHAR(100),
    Location NVARCHAR(100),
    TeacherID INT NOT NULL,
    ClassTime NVARCHAR(50) NOT NULL CONSTRAINT DF_CLASS_ClassTime DEFAULT (N'00:00'),
    CONSTRAINT FK_Class_Teacher FOREIGN KEY (TeacherID)
        REFERENCES TEACHER(TeacherID)
);
GO

--------------------------------------------------
-- 9. Tạo bảng ENROLLMENT
--------------------------------------------------
CREATE TABLE ENROLLMENT (
    EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    ClassID INT NOT NULL,
    RegistrationDate DATE NOT NULL,
    CONSTRAINT FK_Enrollment_Student FOREIGN KEY (StudentID)
        REFERENCES STUDENT(StudentID) ON DELETE CASCADE,
    CONSTRAINT FK_Enrollment_Class FOREIGN KEY (ClassID)
        REFERENCES CLASS(ClassID)
);
GO

--------------------------------------------------
-- 10. Tạo bảng ATTENDANCE
--------------------------------------------------
CREATE TABLE ATTENDANCE (
    AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentID INT NOT NULL,
    Date DATE NOT NULL,
    Status NVARCHAR(50),
    CONSTRAINT FK_Attendance_Enrollment FOREIGN KEY (EnrollmentID)
        REFERENCES ENROLLMENT(EnrollmentID) ON DELETE CASCADE
);
GO

--------------------------------------------------
-- 11. Tạo bảng FEEDBACK
--------------------------------------------------
CREATE TABLE FEEDBACK (
    FeedbackID INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentID INT NOT NULL,
    TeacherID INT NOT NULL,
    Comments NVARCHAR(MAX),
    Date DATE NOT NULL,
    CONSTRAINT FK_Feedback_Enrollment FOREIGN KEY (EnrollmentID)
        REFERENCES ENROLLMENT(EnrollmentID) ON DELETE CASCADE,
    CONSTRAINT FK_Feedback_Teacher FOREIGN KEY (TeacherID)
        REFERENCES TEACHER(TeacherID)
);
GO

--------------------------------------------------
-- 12. Tạo bảng BUOIHOC (Buổi học)
--------------------------------------------------
CREATE TABLE BUOIHOC (
    BuoiHocID INT IDENTITY(1,1) PRIMARY KEY,
    TenBuoiHoc NVARCHAR(100) NOT NULL,
    PhongHoc NVARCHAR(50),
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    TeacherID INT NOT NULL,
    ClassID INT NULL,  -- Có thể không gắn với lớp cụ thể
    CONSTRAINT FK_BuoiHoc_Teacher FOREIGN KEY (TeacherID)
        REFERENCES TEACHER(TeacherID),
    CONSTRAINT FK_BuoiHoc_Class FOREIGN KEY (ClassID)
        REFERENCES CLASS(ClassID)
);
GO

--------------------------------------------------
-- 13. (Tuỳ chọn) Reset Identity (không bắt buộc nếu đã DROP bảng)
--------------------------------------------------
/*
DBCC CHECKIDENT('ROLE', RESEED, 0);
DBCC CHECKIDENT('[USER]', RESEED, 0);
DBCC CHECKIDENT('PARENT', RESEED, 0);
DBCC CHECKIDENT('STUDENT', RESEED, 0);
DBCC CHECKIDENT('TEACHER', RESEED, 0);
DBCC CHECKIDENT('CLASS', RESEED, 0);
DBCC CHECKIDENT('ENROLLMENT', RESEED, 0);
DBCC CHECKIDENT('ATTENDANCE', RESEED, 0);
DBCC CHECKIDENT('FEEDBACK', RESEED, 0);
DBCC CHECKIDENT('BUOIHOC', RESEED, 0);
GO
*/

--------------------------------------------------
-- 14. Chèn dữ liệu mẫu (theo thứ tự từ bảng cha sang bảng con)
--------------------------------------------------

-- Insert ROLE mẫu
INSERT INTO ROLE (RoleName) VALUES 
(N'Admin'), 
(N'Teacher'), 
(N'Student');
GO

-- Insert USER mẫu
INSERT INTO [USER] (Username, PasswordHash, FullName, Email, RoleID) 
VALUES 
(N'admin', N'123456', N'Admin User', N'admin@example.com', 1),
(N'teacher1', N'123456', N'Teacher One', N'teacher1@example.com', 2),
(N'student1', N'123456', N'Student One', N'student1@example.com', 3);
GO

-- Insert TEACHER mẫu
INSERT INTO TEACHER (FullName, PhoneNumber, Email, Expertise)
VALUES 
(N'Giáo viên A', N'0123456789', N'teacherA@example.com', N'Tiếng Anh giao tiếp'),
(N'Giáo viên B', N'0987654321', N'teacherB@example.com', N'Tiếng Anh thương mại'),
(N'Giáo viên C', N'0911223344', N'teacherC@example.com', N'Tiếng Anh chuyên ngành');
GO

-- Insert CLASS mẫu (TeacherID phải khớp với TEACHER)
INSERT INTO CLASS (ClassName, Schedule, Location, TeacherID, ClassTime)
VALUES 
(N'Lớp Tiếng Anh 1', N'Mon/Wed/Fri', N'Phòng A', 1, N'08:00'),
(N'Lớp Tiếng Anh 2', N'Tue/Thu', N'Phòng B', 2, N'09:30'),
(N'Lớp Tiếng Anh Nâng Cao', N'Mon/Tue/Thu', N'Phòng C', 1, N'14:00'),
(N'Lớp Tiếng Anh Giao Tiếp', N'Wed/Fri', N'Phòng D', 3, N'16:30');
GO

-- Insert PARENT mẫu
INSERT INTO PARENT (FullName, PhoneNumber, Email)
VALUES 
(N'Phụ huynh A', N'0123456789', N'parentA@example.com'),
(N'Phụ huynh B', N'0987654321', N'parentB@example.com');
GO

-- Insert STUDENT mẫu (ParentID phải khớp với PARENT)
INSERT INTO STUDENT (FullName, PhoneNumber, Email, DateOfBirth, Address, ParentID)
VALUES 
(N'Học sinh 1', N'0123456789', N'student1@example.com', '2005-01-01', N'Địa chỉ 1', 1),
(N'Học sinh 2', N'0987654321', N'student2@example.com', '2006-02-02', N'Địa chỉ 2', 2);
GO

-- Insert ENROLLMENT mẫu
INSERT INTO ENROLLMENT (StudentID, ClassID, RegistrationDate)
VALUES 
(1, 1, GETDATE()),
(2, 2, GETDATE());
GO

-- Insert ATTENDANCE mẫu
INSERT INTO ATTENDANCE (EnrollmentID, Date, Status)
VALUES 
(1, GETDATE(), N'Present'),
(2, GETDATE(), N'Absent');
GO

-- Insert FEEDBACK mẫu
INSERT INTO FEEDBACK (EnrollmentID, TeacherID, Comments, Date)
VALUES 
(1, 1, N'Tốt', GETDATE()),
(2, 2, N'Cần cải thiện', GETDATE());
GO

-- Insert BUOIHOC mẫu
INSERT INTO BUOIHOC (TenBuoiHoc, PhongHoc, StartTime, EndTime, TeacherID, ClassID)
VALUES 
(N'Buổi học buổi tối 1', N'Phòng 101', '19:00', '21:30', 1, 1),
(N'Buổi học buổi tối 2', N'Phòng 102', '19:00', '21:30', 2, NULL);
GO

--------------------------------------------------
-- Thiết lập ràng buộc ON DELETE cho các bảng con (nếu cần)
--------------------------------------------------
ALTER TABLE ENROLLMENT 
DROP CONSTRAINT FK_Enrollment_Student;
GO
ALTER TABLE ENROLLMENT 
ADD CONSTRAINT FK_Enrollment_Student
FOREIGN KEY (StudentID) REFERENCES STUDENT(StudentID) ON DELETE CASCADE;
GO

ALTER TABLE ATTENDANCE 
DROP CONSTRAINT FK_Attendance_Enrollment;
GO
ALTER TABLE ATTENDANCE 
ADD CONSTRAINT FK_Attendance_Enrollment
FOREIGN KEY (EnrollmentID) REFERENCES ENROLLMENT(EnrollmentID) ON DELETE CASCADE;
GO

ALTER TABLE FEEDBACK 
DROP CONSTRAINT FK_Feedback_Enrollment;
GO
ALTER TABLE FEEDBACK 
ADD CONSTRAINT FK_Feedback_Enrollment
FOREIGN KEY (EnrollmentID) REFERENCES ENROLLMENT(EnrollmentID) ON DELETE CASCADE;
GO

-- Fix teacher email
UPDATE TEACHER
SET Email = 'teacher1@example.com'
WHERE FullName = 'Giáo viên A';
GO

--------------------------------------------------
-- Kiểm tra quan hệ giữa STUDENT và CLASS qua Enrollment
--------------------------------------------------
SELECT s.StudentID, s.FullName, c.ClassName
FROM STUDENT s
JOIN ENROLLMENT e ON s.StudentID = e.StudentID
JOIN CLASS c ON e.ClassID = c.ClassID;
GO

--------------------------------------------------
-- Thêm cột AttendanceStatus vào bảng STUDENT để lưu trạng thái điểm danh
--------------------------------------------------
ALTER TABLE STUDENT
ADD AttendanceStatus NVARCHAR(50) NULL;
GO

SELECT StudentID, FullName, AttendanceStatus FROM STUDENT;
GO

select * from STUDENT
