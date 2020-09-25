----Confirmations
Alter table confirmations add HasSucceeded bit not null default 0
Go

Update Confirmations set HasSucceeded = 1 where Status = 1
Go

alter table confirmations drop column status
Go
----Creating activations table
CREATE TABLE [dbo].[Activations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LicenseId] [int] NOT NULL,
	[ActivationDate] [datetime] not NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Activations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Activations] ADD  DEFAULT (getdate()) FOR [Date]
GO

ALTER TABLE [dbo].[Activations]  WITH CHECK ADD  CONSTRAINT [UC_LicenseId] Unique([LicenseId])

ALTER TABLE [dbo].[Activations]  WITH CHECK ADD  CONSTRAINT [FK_Activations_Licenses] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[Licenses] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Activations] CHECK CONSTRAINT [FK_Activations_Licenses]
GO
-- Filling activations table.
with temp as (
select Licenses.Id as licenseId, ProcessingStatus.StatusDate, Hosts.Name,hosts.id,row_number() over(partition by licenses.id order by licenses.id) as RowId, ProcessingStatus.Date from Hosts 
inner join Licenses on Licenses.HostId = Hosts.Id
inner join ProcessingStatus on ProcessingStatus.HostId = Hosts.Id
where Message like '%activated%' )
insert into Activations(LicenseId, activationDate,Date)
select licenseId,StatusDate,Date from temp where RowId = 1  order by StatusDate asc
Go

--- Creating activatedHosts view

Create View [dbo].[ActivatedHosts] as 
with temp as 
(
select hosts.Id, 
count(Licenses.Id) over (partition by Hosts.id) as LicenseCount,
count(Activations.LicenseId) over(partition by hosts.id) as activationCount,
ActivationDate
from Hosts 
left join Licenses on Hosts.Id = HostId 
left join Activations on LicenseId = Licenses.Id)
select Id as HostId, Max(activationDate) as ActivationDate, Max(licenseCount) as LicenseCount from temp
where LicenseCount > 0 and LicenseCount = activationCount
group by Id
Go

--Spliting ProcessingStatus
CREATE TABLE [dbo].[Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HostId] [int] NOT NULL,
	[StatusDate][datetime] not NULL,
	[Message] [nvarchar](250) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Status] ADD  DEFAULT (getdate()) FOR [Date]
GO


ALTER TABLE [dbo].[Status]  WITH CHECK ADD  CONSTRAINT [FK_Status_Hosts] FOREIGN KEY([HostId])
REFERENCES [dbo].[Hosts] ([Id])
ON DELETE CASCADE
GO

-- Filling [Status] table

with temp as (
select 
id,
Row_Number() over (partition by Hostid order by id desc) as Last
from ProcessingStatus
where HostId not in (select HostId from ActivatedHosts) and Message not like '%activated%'
)

insert into status(hostId,Message,StatusDate,Date)
(
select HostId,Message,StatusDate,Date from ProcessingStatus
where Id in (select id from temp where last =1)
)



--Creating processing status view

Drop table [ProcessingStatus]
go
Create view [dbo].[ProcessingStatus] as
with temp as 
(select Status.HostId, 
Message,
StatusDate,
3 as Status,
ROW_NUMBER() over(partition by Message,Status.Hostid order by id desc) as lastMsg
from Status left join [ActivatedHosts] on [ActivatedHosts].HostId = Status.HostId
where ActivatedHosts.LicenseCount is null),
Result as 
(select HostId,Message,StatusDate,status from temp where lastMsg = 1 
union 
select [ActivatedHosts].HostId, 'License activated.' as Message, [ActivatedHosts].ActivationDate as StatusDate,1 as Status from activatedHosts)
select HostId as id,* from result
GO

