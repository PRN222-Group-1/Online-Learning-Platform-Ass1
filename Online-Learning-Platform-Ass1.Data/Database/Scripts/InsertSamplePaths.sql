-- INSERT SAMPLE LEARNING PATHS (if not exists)

-- Check if we have any learning paths
IF NOT EXISTS (SELECT 1 FROM Learning_Paths)
BEGIN
    -- Get category IDs
    DECLARE @Cat_Web UNIQUEIDENTIFIER = (SELECT TOP 1 path_id FROM Learning_Paths WHERE title LIKE '%Full Stack%');
    
    -- If no learning paths at all, create sample ones
    IF @Cat_Web IS NULL
    BEGIN
        -- Create sample learning paths
        INSERT INTO Learning_Paths (path_id, title, description, price, status) VALUES
        (NEWID(), 'Full Stack .NET Developer', 'Master C#, ASP.NET Core, and Web Development', 199.99, 'published'),
        (NEWID(), 'Cloud Solutions with Azure', 'Learn Azure cloud services and deployment', 149.99, 'published'),
        (NEWID(), 'Data Science Fundamentals', 'Introduction to data analysis and visualization', 179.99, 'published');
        
        PRINT 'Sample learning paths created successfully!'
    END
END
ELSE
BEGIN
    PRINT 'Learning paths already exist!'
END
