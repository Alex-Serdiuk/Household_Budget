
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/19/2023 18:50:47
-- Generated from EDMX file: E:\ФОТО\itstep\course project .net\Household_Budget\Household_Budget\dbBudget.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbBudget];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Expenditure_ExpenditureName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article_Expenditure] DROP CONSTRAINT [FK_Expenditure_ExpenditureName];
GO
IF OBJECT_ID(N'[dbo].[FK_Income_IncomeSource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article_Income] DROP CONSTRAINT [FK_Income_IncomeSource];
GO
IF OBJECT_ID(N'[dbo].[FK_Expenditure_ExpenditureType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article_Expenditure] DROP CONSTRAINT [FK_Expenditure_ExpenditureType];
GO
IF OBJECT_ID(N'[dbo].[FK_Expenditure_inherits_Article]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article_Expenditure] DROP CONSTRAINT [FK_Expenditure_inherits_Article];
GO
IF OBJECT_ID(N'[dbo].[FK_Income_inherits_Article]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Article_Income] DROP CONSTRAINT [FK_Income_inherits_Article];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ExpenditureName]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpenditureName];
GO
IF OBJECT_ID(N'[dbo].[Article]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Article];
GO
IF OBJECT_ID(N'[dbo].[IncomeSource]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IncomeSource];
GO
IF OBJECT_ID(N'[dbo].[ExpenditureType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpenditureType];
GO
IF OBJECT_ID(N'[dbo].[Article_Expenditure]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Article_Expenditure];
GO
IF OBJECT_ID(N'[dbo].[Article_Income]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Article_Income];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ExpenditureName'
CREATE TABLE [dbo].[ExpenditureName] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Article'
CREATE TABLE [dbo].[Article] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTime] datetime  NOT NULL
);
GO

-- Creating table 'IncomeSource'
CREATE TABLE [dbo].[IncomeSource] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'ExpenditureType'
CREATE TABLE [dbo].[ExpenditureType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Article_Expenditure'
CREATE TABLE [dbo].[Article_Expenditure] (
    [TypeId] int  NOT NULL,
    [NameId] int  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [Quantity] int  NOT NULL,
    [Summ] decimal(19,4)  NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Article_Income'
CREATE TABLE [dbo].[Article_Income] (
    [SourceId] int  NOT NULL,
    [Summ] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ExpenditureName'
ALTER TABLE [dbo].[ExpenditureName]
ADD CONSTRAINT [PK_ExpenditureName]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Article'
ALTER TABLE [dbo].[Article]
ADD CONSTRAINT [PK_Article]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IncomeSource'
ALTER TABLE [dbo].[IncomeSource]
ADD CONSTRAINT [PK_IncomeSource]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpenditureType'
ALTER TABLE [dbo].[ExpenditureType]
ADD CONSTRAINT [PK_ExpenditureType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Article_Expenditure'
ALTER TABLE [dbo].[Article_Expenditure]
ADD CONSTRAINT [PK_Article_Expenditure]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Article_Income'
ALTER TABLE [dbo].[Article_Income]
ADD CONSTRAINT [PK_Article_Income]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [NameId] in table 'Article_Expenditure'
ALTER TABLE [dbo].[Article_Expenditure]
ADD CONSTRAINT [FK_Expenditure_ExpenditureName]
    FOREIGN KEY ([NameId])
    REFERENCES [dbo].[ExpenditureName]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Expenditure_ExpenditureName'
CREATE INDEX [IX_FK_Expenditure_ExpenditureName]
ON [dbo].[Article_Expenditure]
    ([NameId]);
GO

-- Creating foreign key on [SourceId] in table 'Article_Income'
ALTER TABLE [dbo].[Article_Income]
ADD CONSTRAINT [FK_Income_IncomeSource]
    FOREIGN KEY ([SourceId])
    REFERENCES [dbo].[IncomeSource]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Income_IncomeSource'
CREATE INDEX [IX_FK_Income_IncomeSource]
ON [dbo].[Article_Income]
    ([SourceId]);
GO

-- Creating foreign key on [TypeId] in table 'Article_Expenditure'
ALTER TABLE [dbo].[Article_Expenditure]
ADD CONSTRAINT [FK_Expenditure_ExpenditureType]
    FOREIGN KEY ([TypeId])
    REFERENCES [dbo].[ExpenditureType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Expenditure_ExpenditureType'
CREATE INDEX [IX_FK_Expenditure_ExpenditureType]
ON [dbo].[Article_Expenditure]
    ([TypeId]);
GO

-- Creating foreign key on [Id] in table 'Article_Expenditure'
ALTER TABLE [dbo].[Article_Expenditure]
ADD CONSTRAINT [FK_Expenditure_inherits_Article]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Article]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Article_Income'
ALTER TABLE [dbo].[Article_Income]
ADD CONSTRAINT [FK_Income_inherits_Article]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Article]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------