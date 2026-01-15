-- CLEANUP EXISTING DATA (To avoid duplicates/errors on re-runs)
-- Disable constraints to make deletion easier, or delete in order.
-- Deleting in order:

DELETE FROM Path_Courses;
DELETE FROM Transactions;
DELETE FROM Certificates;
DELETE FROM Orders;
DELETE FROM Enrollments;
DELETE FROM Lesson_Progress;
DELETE FROM Quiz_Attempts;
DELETE FROM Options;
DELETE FROM Questions;
DELETE FROM Quizzes;
DELETE FROM Lessons;
DELETE FROM Modules;
DELETE FROM Courses;
DELETE FROM Learning_Paths;
DELETE FROM Categories;
DELETE FROM Blogs;
DELETE FROM profiles;
DELETE FROM Users;
DELETE FROM Roles;

-- 1. ROLES
INSERT INTO Roles (Id, Name, Description) VALUES
(NEWID(), 'Admin', 'Administrator with full access'),
(NEWID(), 'Instructor', 'Can create and manage courses'),
(NEWID(), 'Student', 'Can enroll in courses');

DECLARE @RoleId_Admin UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Roles WHERE Name = 'Admin');
DECLARE @RoleId_Instructor UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Roles WHERE Name = 'Instructor');
DECLARE @RoleId_Student UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Roles WHERE Name = 'Student');

-- 2. USERS (Password hash is a dummy 'Password123!')
-- Admin
INSERT INTO Users (Id, Username, Email, PasswordHash, RoleId, CreateAt) VALUES
(NEWID(), 'admin', 'admin@example.com', 'AQAAAAEAACcQAAAAEH...', @RoleId_Admin, GETDATE());

-- Instructors
INSERT INTO Users (Id, Username, Email, PasswordHash, RoleId, CreateAt) VALUES
(NEWID(), 'instructor1', 'john.doe@example.com', 'AQAAAAEAACcQAAAAEH...', @RoleId_Instructor, GETDATE()),
(NEWID(), 'instructor2', 'jane.smith@example.com', 'AQAAAAEAACcQAAAAEH...', @RoleId_Instructor, GETDATE());

DECLARE @Instructor1Id UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Users WHERE Username = 'instructor1');
DECLARE @Instructor2Id UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Users WHERE Username = 'instructor2');

-- Students
INSERT INTO Users (Id, Username, Email, PasswordHash, RoleId, CreateAt) VALUES
(NEWID(), 'student1', 'student1@example.com', 'AQAAAAEAACcQAAAAEH...', @RoleId_Student, GETDATE()),
(NEWID(), 'student2', 'student2@example.com', 'AQAAAAEAACcQAAAAEH...', @RoleId_Student, GETDATE());

-- 3. CATEGORIES
INSERT INTO Categories (category_id, name, parent_id) VALUES
(NEWID(), 'Web Development', NULL),
(NEWID(), 'Data Science', NULL),
(NEWID(), 'Design', NULL);

DECLARE @Cat_Web UNIQUEIDENTIFIER = (SELECT TOP 1 category_id FROM Categories WHERE name = 'Web Development');
DECLARE @Cat_Data UNIQUEIDENTIFIER = (SELECT TOP 1 category_id FROM Categories WHERE name = 'Data Science');

-- 4. COURSES
-- Course 1: Intro to C#
DECLARE @Course1Id UNIQUEIDENTIFIER = NEWID();
INSERT INTO Courses (course_id, instructor_id, category_id, title, description, price, status, created_at, image_url) VALUES
(@Course1Id, @Instructor1Id, @Cat_Web, 'Introduction to C#', 'Learn the basics of C# programming language.', 49.99, 'active', GETDATE(), 'https://via.placeholder.com/300x200?text=C%23+Basics');

-- Course 2: ASP.NET Core Web API
DECLARE @Course2Id UNIQUEIDENTIFIER = NEWID();
INSERT INTO Courses (course_id, instructor_id, category_id, title, description, price, status, created_at, image_url) VALUES
(@Course2Id, @Instructor1Id, @Cat_Web, 'Master ASP.NET Core Web API', 'Build robust RESTful APIs with .NET.', 89.99, 'active', GETDATE(), 'https://via.placeholder.com/300x200?text=Web+API');

-- Course 3: SQL Server Fundamentals
DECLARE @Course3Id UNIQUEIDENTIFIER = NEWID();
INSERT INTO Courses (course_id, instructor_id, category_id, title, description, price, status, created_at, image_url) VALUES
(@Course3Id, @Instructor2Id, @Cat_Data, 'SQL Server Fundamentals', 'Master SQL queries and database management.', 59.99, 'active', GETDATE(), 'https://via.placeholder.com/300x200?text=SQL+Server');

-- 5. LEARNING PATHS
DECLARE @PathId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Learning_Paths (path_id, title, description, price, status) VALUES
(@PathId, 'Full Stack .NET Developer', 'Become a comprehensive .NET developer from scratch to mastery.', 120.00, 'published');

-- 6. PATH_COURSES (Link courses to path)
INSERT INTO Path_Courses (path_id, course_id, order_index) VALUES
(@PathId, @Course1Id, 1), -- C#
(@PathId, @Course2Id, 2), -- Web API
(@PathId, @Course3Id, 3); -- SQL 

-- 7. MODULES (For Course 1: C#)
DECLARE @Module1Id UNIQUEIDENTIFIER = NEWID();
INSERT INTO Modules (module_id, course_id, title, order_index) VALUES
(@Module1Id, @Course1Id, 'Getting Started', 1);

-- 8. LESSONS (For Module 1)
INSERT INTO Lessons (lesson_id, module_id, title, type, content_url, "duration", order_index) VALUES
(NEWID(), @Module1Id, 'Installation', 'video', 'https://www.youtube.com/watch?v=sF7c14aQc3c', 300, 1),
(NEWID(), @Module1Id, 'Hello World', 'video', 'https://www.youtube.com/watch?v=sF7c14aQc3c', 600, 2);

-- Re-enable constraints
-- EXEC sp_msforeachtable "ALTER TABLE ? CHECK CONSTRAINT all"
