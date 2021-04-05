CREATE OR ALTER view [dbo].[LastStatus] as 

with temp as 
(select  Status.*,
row_number() over(partition by hostid order by statusDate desc) as rowId
--row_number() over(partition by hostid order by id desc) as rowIdDate
from Status)
select * from temp where rowId = 1
GO

CREATE OR ALTER     view [dbo].[HostMonitoring] as 
select hosts.*,keys.productKey,keys.id as ProductKeyId,Confirmations.content as ConfirmationKey,
case 
when Activations.Id is not null then 'Activated' 
when Confirmations.Id is not null then 'In progress'
when Licenses.Id is not null then 'Requesting confirmation key'
when Licenses.Id is null then isnull(laststatus.message,'Subscribed')
end as 'Status'

from hosts 
left join Licenses on Licenses.HostId = hosts.Id
left join Activations on Licenses.Id = Activations.LicenseId
left join Confirmations on Confirmations.LicenseId = Licenses.Id
left join keys on Licenses.ProductKey = Keys.ProductKey
left join lastStatus on laststatus.Hostid = Hosts.Id
GO



CREATE OR ALTER       procedure [dbo].[GetStats]
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

----------------------------------------------Activated----------------------------------------------------------
activated as (
select count(distinct Hosts.Id) as activated from hosts left join licenses on hosts.id = licenses.hostid left join Activations on Activations.LicenseId = Licenses.Id
where Activations.ActivationDate Between @start and @end
and 
Activations.Id is not null
and 
Licenses.productKey in (select ProductKey from keys where @start between keys.StartDate and keys.endDate )
),

----------------------------------------------In Progress----------------------------------------------------------
inprogress as (
select count(distinct Hosts.Id) as inprogress from hosts left join licenses on hosts.id = licenses.hostid left join Activations on Activations.LicenseId = Licenses.Id
where 
Activations.Id is null 
and
Licenses.Id is not null
and
Licenses.productKey in (select ProductKey from keys where @start between keys.StartDate and keys.endDate )
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

