--============ STEP 1: Truncate tables ==== 
DROP TABLE IF EXISTS  dbo.EnrollmentHistory;
DROP TABLE IF EXISTS  dbo.Module;   
DROP TABLE IF EXISTS  dbo.Student;
DROP TABLE IF EXISTS  dbo.Course;
 


----========== STEP 2: Create Table ============
-- 1. Courses Table: Stores information about each course.
CREATE TABLE dbo.Course 
(
    [Id] INT PRIMARY KEY IDENTITY(1,1), -- AUTOINCREMENT 
    [CourseName] VARCHAR(100) NOT NULL,
    [CreatedBy] INT NULL,
	[CreatedDate] DATETIME NULL,
	-- Unique values on Name table 
	CONSTRAINT unq_Course UNIQUE ([CourseName])
);

-- 2. Modules Table: Stores information about modules, linked to a specific course.
CREATE TABLE dbo.Module
(
    Id INT PRIMARY KEY IDENTITY(1,1), -- Use AUTOINCREMENT 
	CourseId INT NOT NULL,
    [ModuleName] VARCHAR(100) NOT NULL, 
	[CreatedBy] INT NULL,
	[CreatedDate] DATETIME NULL,

    -- Foreign Key Constraint from Course Table against Id column
    FOREIGN KEY (CourseID) REFERENCES Course(Id),
	-- Unique constraint on two columns
    CONSTRAINT unq_CourseIdModuleName UNIQUE (CourseId, [ModuleName])
	
);
-- ALTER TABLE dbo.YourTableName ADD CONSTRAINT UQ_YourTableName_ColumnA_ColumnB UNIQUE (ColumnA, ColumnB);

---- Stundent table -------
CREATE TABLE dbo.Student
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(200) NOT NULL UNIQUE,
	[CreatedBy] INT NULL DEFAULT 1,
	[CreatedDate] DATETIME NULL DEFAULT GETDATE(),
);

---- Enrollment table ---
CREATE TABLE dbo.EnrollmentHistory
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    EnrollmentDate DATETIME NOT NULL DEFAULT(GETDATE()), 
	[CreatedBy] INT NULL DEFAULT 1,
	[CreatedDate] DATETIME NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Enrollment_Student 
        FOREIGN KEY (StudentId) REFERENCES dbo.Student(Id),

    CONSTRAINT FK_Enrollment_Course 
        FOREIGN KEY (CourseId) REFERENCES dbo.Course(Id),

    -- Prevent same student enrolling in same course twice
    CONSTRAINT UQ_Enrollment_Student_Course 
        UNIQUE (StudentId, CourseId)
);
GO
GO

 
	 
----========== STEP 3: Create Table ============
INSERT INTO dbo.Course ([CourseName], CreatedBy, CreatedDate)
VALUES
('.NET', 1, GETDATE()), 
('React', 1, GETDATE()),
('SQL', 1, GETDATE()),
('Maths', 1, GETDATE());

INSERT INTO dbo.Student (FirstName, LastName, Email)
VALUES
('John', 'Doe', 'john.doe@example.com'),
('Emma', 'Johnson', 'emma.johnson@example.com'),
('Michael', 'Smith', 'michael.smith@example.com'),
('Sophia', 'Brown', 'sophia.brown@example.com'),
('David', 'Wilson', 'david.wilson@example.com');

 
INSERT INTO dbo.Module (CourseId, ModuleName, CreatedBy, CreatedDate)
VALUES
-- Modules for Full Stack .NET Development 
(1, 'C# Basics & OOP', 1, GETDATE()),
(1, '.NET Core MVC', 1, GETDATE()),
(1, 'Entity Framework Core', 1, GETDATE()),
(1, 'REST API Development', 1, GETDATE()),
(1, 'Micro Services', 1, GETDATE()),
 

-- Modules for Web Development with React  
(2, 'JavaScript ES6+', 3, GETDATE()),
(2, 'React Components & Hooks', 3, GETDATE()),
(2, 'State Management with Redux', 3, GETDATE()),
(2, 'Interceptors', 3, GETDATE()),

-- Modules for SQL & Database Administration
(3, 'SQL Basics & Joins', 2, GETDATE()),
(3, 'Stored Procedures & Functions', 2, GETDATE()),
(3, 'Performance Tuning & Indexing', 2, GETDATE()),

(4, 'Mathematic I', 2, GETDATE()),
(4, 'Mathematic II', 2, GETDATE());;


---- Enrollment history data ------
INSERT INTO dbo.EnrollmentHistory (StudentId, CourseId, EnrollmentDate)
SELECT s.Id StudentId, c.Id CourseId, GETDATE()-s.Id-c.Id
FROM dbo.Student s 
LEFT JOIN dbo.Course c ON c.Id = c.Id   
WHERE ( (s.Id = 1  AND c.Id = c.Id) OR
		(s.Id = 2  AND c.Id < 4)  OR
		(s.Id >= 3 AND c.Id < 3)   
		)  
		-- AND s.id = 3
ORDER BY c.Id , s.Id   
-- Data manipulations ---
DELETE e FROM EnrollmentHistory e WHERE e.Id = 10 
GO
GO





--=========== STEP 4: Question ===========
--- List all courses with their module counts
SELECT c.[CourseName] CourseName, COUNT(m.Id) TotalModuleCount
FROM dbo.Course c WITH(NOLOCK) 
LEFT JOIN dbo.Module m WITH(NOLOCK) ON m.CourseId = c.Id
GROUP BY c.[CourseName]


--- List all students enrolled in a given course.
SELECT s.Id, s.FirstName, s.LastName, s.Email, c.[CourseName], e.EnrollmentDate
FROM dbo.EnrollmentHistory e WITH(NOLOCK) 
LEFT JOIN dbo.Course c WITH(NOLOCK)  ON c.Id = e.CourseId
LEFT JOIN dbo.Student s WITH(NOLOCK) ON s.Id = e.StudentId
 --WHERE c.[CourseName] = 'Web Development with React' -- Specific course
ORDER BY s.Id


--- List the top 3 courses with the highest number of enrollments.
SELECT TOP 3 c.[CourseName], COUNT(e.Id) AS TotalEnrollmentCount
FROM dbo.Course c
JOIN dbo.EnrollmentHistory e ON C.Id = E.CourseID 
--WHERE c.CourseName = 'Web Development with React'
GROUP BY c.[CourseName] -- Group by Name  
ORDER BY TotalEnrollmentCount DESC; 


---- For a given student, list all courses they are enrolled in along with the enrollment date.
SELECT s.Id, s.FirstName, s.LastName, s.Email, c.Id, C.[CourseName], E.EnrollmentDate
FROM dbo.Student s WITH(NOLOCK)  
  JOIN dbo.EnrollmentHistory e WITH(NOLOCK) ON s.Id = e.StudentId
  JOIN dbo.Course c WITH(NOLOCK) ON c.Id = e.CourseId
--WHERE S.Email = 'john.doe@example.com' -- Filter by email for example
ORDER BY s.Id, c.CourseName; -- Order by the most recent enrollment first