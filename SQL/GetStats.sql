USE [ESU]
GO

/****** Object:  StoredProcedure [dbo].[GetStats]    Script Date: 16/03/2021 14:51:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   procedure [dbo].[GetStats]
@start  date,
@end date
as
with subscribed as (
select count(distinct Hosts.Id) as subscribed from hosts left join licenses on hosts.id = licenses.hostid
left join Status on status.HostId = hosts.Id
where 
hosts.subscriptionDate Between @start and @end
and
licenses.id is null
and 
Status.Id is null
),
activated as (
select count(distinct Hosts.Id) as activated from hosts left join licenses on hosts.id = licenses.hostid left join Activations on Activations.LicenseId = Licenses.Id
where Activations.ActivationDate Between @start and @end
and 
Activations.Id is not null
),
inprogress as (
select count(distinct Hosts.Id) as inprogress from hosts left join licenses on hosts.id = licenses.hostid left join Activations on Activations.LicenseId = Licenses.Id
where 
Activations.Id is null 
and
Licenses.Id is not null
and
Licenses.InstallationDate Between @start and @end
),
failed as (
select count(distinct Hosts.Id) as failed from hosts left join licenses on hosts.id = licenses.hostid left join Activations on Activations.LicenseId = Licenses.Id
left join [status] on [status].HostId = hosts.Id
where 
Activations.Id is null 
and
Licenses.Id is null
and
[status].Id is not null
and
[status].StatusDate Between @start and @end
)
select activated,subscribed,inprogress,failed from subscribed,activated,inprogress, failed
GO


