
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/18/2021 13:36:48
-- Generated from EDMX file: D:\Documents\Git-repo\PIMtool\Repositories\Models\ProjectManagement.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PIMdb_ModelFirst];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GroupEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_GroupEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_ProjectGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeProject_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeProject] DROP CONSTRAINT [FK_EmployeeProject_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeProject_Project]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeProject] DROP CONSTRAINT [FK_EmployeeProject_Project];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[EmployeeProject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeProject];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] uniqueidentifier  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Visa] nvarchar(max)  NOT NULL,
    [Version] decimal(18,0)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [Group_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Version] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] uniqueidentifier  NOT NULL,
    [Version] decimal(18,0)  NOT NULL,
    [ProjectNumber] decimal(18,0)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Customer] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NULL,
    [Group_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'EmployeeProject'
CREATE TABLE [dbo].[EmployeeProject] (
    [Employees_Id] uniqueidentifier  NOT NULL,
    [Projects_Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Employees_Id], [Projects_Id] in table 'EmployeeProject'
ALTER TABLE [dbo].[EmployeeProject]
ADD CONSTRAINT [PK_EmployeeProject]
    PRIMARY KEY CLUSTERED ([Employees_Id], [Projects_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Group_Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_GroupEmployee]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupEmployee'
CREATE INDEX [IX_FK_GroupEmployee]
ON [dbo].[Employees]
    ([Group_Id]);
GO

-- Creating foreign key on [Group_Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_ProjectGroup]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectGroup'
CREATE INDEX [IX_FK_ProjectGroup]
ON [dbo].[Projects]
    ([Group_Id]);
GO

-- Creating foreign key on [Employees_Id] in table 'EmployeeProject'
ALTER TABLE [dbo].[EmployeeProject]
ADD CONSTRAINT [FK_EmployeeProject_Employee]
    FOREIGN KEY ([Employees_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Projects_Id] in table 'EmployeeProject'
ALTER TABLE [dbo].[EmployeeProject]
ADD CONSTRAINT [FK_EmployeeProject_Project]
    FOREIGN KEY ([Projects_Id])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeProject_Project'
CREATE INDEX [IX_FK_EmployeeProject_Project]
ON [dbo].[EmployeeProject]
    ([Projects_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------