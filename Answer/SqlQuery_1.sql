CREATE TABLE [dbo].[webpages_UsersInRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [fk_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [fk_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
);

CREATE TABLE [dbo].[webpages_Roles] (
    [RoleId]   INT            IDENTITY (1, 1) NOT NULL,
    [RoleName] NVARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC),
    UNIQUE NONCLUSTERED ([RoleName] ASC)
);

CREATE TABLE [dbo].[webpages_OAuthMembership] (
    [Provider]       NVARCHAR (30)  NOT NULL,
    [ProviderUserId] NVARCHAR (100) NOT NULL,
    [UserId]         INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Provider] ASC, [ProviderUserId] ASC)
);

CREATE TABLE [dbo].[webpages_Membership] (
    [UserId]                                  INT            NOT NULL,
    [CreateDate]                              DATETIME       NULL,
    [ConfirmationToken]                       NVARCHAR (128) NULL,
    [IsConfirmed]                             BIT            DEFAULT ((0)) NULL,
    [LastPasswordFailureDate]                 DATETIME       NULL,
    [PasswordFailuresSinceLastSuccess]        INT            DEFAULT ((0)) NOT NULL,
    [Password]                                NVARCHAR (128) NOT NULL,
    [PasswordChangedDate]                     DATETIME       NULL,
    [PasswordSalt]                            NVARCHAR (128) NOT NULL,
    [PasswordVerificationToken]               NVARCHAR (128) NULL,
    [PasswordVerificationTokenExpirationDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[MajorModels] (
    [MajorId]   INT            IDENTITY (1, 1) NOT NULL,
    [MajorType] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.MajorModels] PRIMARY KEY CLUSTERED ([MajorId] ASC)
);


CREATE TABLE [dbo].[UserProfile] (
    [UserId]        INT            IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (MAX) NULL,
    [Password]      NVARCHAR (MAX) NULL,
    [FirstName]     NVARCHAR (MAX) NULL,
    [LastName]      NVARCHAR (MAX) NULL,
    [Country]       NVARCHAR (MAX) NULL,
    [City]          NVARCHAR (MAX) NULL,
    [Email]         NVARCHAR (MAX) NULL,
    [Rate]          INT            NULL,
    [BirthDate]     DATETIME       NOT NULL,
    [CreationDate]  DATETIME       NOT NULL,
    [Balance]       INT            NOT NULL,
    [Salary]        INT            NOT NULL,
    [ActiveCode]    NVARCHAR (MAX) NULL,
    [Major_MajorId] INT            NULL,
    CONSTRAINT [PK_dbo.UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_dbo.UserProfile_dbo.MajorModels_Major_MajorId] FOREIGN KEY ([Major_MajorId]) REFERENCES [dbo].[MajorModels] ([MajorId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Major_MajorId]
    ON [dbo].[UserProfile]([Major_MajorId] ASC);

CREATE TABLE [dbo].[QuestionModels] (
    [QuestionId]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]               NVARCHAR (30)  NOT NULL,
    [Text]                NVARCHAR (250) NOT NULL,
    [Date]                DATETIME       NOT NULL,
    [Views]               INT            NOT NULL,
    [Rate]                INT            NOT NULL,
    [User_UserId]         INT            NULL,
    [Major_MajorId]       INT            NULL,
    [ReferredUser_UserId] INT            NULL,
    [UserProfile_UserId]  INT            NULL,
    [UserProfile_UserId1] INT            NULL,
    CONSTRAINT [PK_dbo.QuestionModels] PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    CONSTRAINT [FK_dbo.QuestionModels_dbo.UserProfile_User_UserId] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_dbo.QuestionModels_dbo.MajorModels_Major_MajorId] FOREIGN KEY ([Major_MajorId]) REFERENCES [dbo].[MajorModels] ([MajorId]),
    CONSTRAINT [FK_dbo.QuestionModels_dbo.UserProfile_ReferredUser_UserId] FOREIGN KEY ([ReferredUser_UserId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_dbo.QuestionModels_dbo.UserProfile_UserProfile_UserId] FOREIGN KEY ([UserProfile_UserId]) REFERENCES [dbo].[UserProfile] ([UserId]),
    CONSTRAINT [FK_dbo.QuestionModels_dbo.UserProfile_UserProfile_UserId1] FOREIGN KEY ([UserProfile_UserId1]) REFERENCES [dbo].[UserProfile] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_UserId]
    ON [dbo].[QuestionModels]([User_UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Major_MajorId]
    ON [dbo].[QuestionModels]([Major_MajorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReferredUser_UserId]
    ON [dbo].[QuestionModels]([ReferredUser_UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserProfile_UserId]
    ON [dbo].[QuestionModels]([UserProfile_UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserProfile_UserId1]
    ON [dbo].[QuestionModels]([UserProfile_UserId1] ASC);

	CREATE TABLE [dbo].[AnswerModels] (
    [AnswerId]            INT            IDENTITY (1, 1) NOT NULL,
    [AnswerText]          NVARCHAR (100) NOT NULL,
    [Date]                DATETIME       NOT NULL,
    [Question_QuestionId] INT            NULL,
    [User_UserId]         INT            NULL,
    CONSTRAINT [PK_dbo.AnswerModels] PRIMARY KEY CLUSTERED ([AnswerId] ASC),
    CONSTRAINT [FK_dbo.AnswerModels_dbo.QuestionModels_Question_QuestionId] FOREIGN KEY ([Question_QuestionId]) REFERENCES [dbo].[QuestionModels] ([QuestionId]),
    CONSTRAINT [FK_dbo.AnswerModels_dbo.UserProfile_User_UserId] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Question_QuestionId]
    ON [dbo].[AnswerModels]([Question_QuestionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_UserId]
    ON [dbo].[AnswerModels]([User_UserId] ASC);

CREATE TABLE [dbo].[RateModels] (
    [RateId]          INT IDENTITY (1, 1) NOT NULL,
    [Answer_AnswerId] INT NULL,
    [User_UserId]     INT NULL,
    CONSTRAINT [PK_dbo.RateModels] PRIMARY KEY CLUSTERED ([RateId] ASC),
    CONSTRAINT [FK_dbo.RateModels_dbo.AnswerModels_Answer_AnswerId] FOREIGN KEY ([Answer_AnswerId]) REFERENCES [dbo].[AnswerModels] ([AnswerId]),
    CONSTRAINT [FK_dbo.RateModels_dbo.UserProfile_User_UserId] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Answer_AnswerId]
    ON [dbo].[RateModels]([Answer_AnswerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_UserId]
    ON [dbo].[RateModels]([User_UserId] ASC);

