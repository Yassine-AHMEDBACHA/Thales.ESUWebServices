Create view HostOverview as 

select hosts.id as HostId,hosts.Name,hosts.Network,Hosts.[Identity],Hosts.Site, Licenses.id as licenseId , Confirmations.Id as confirmationid, Activations.Id as ActivationId, ProcessingStatus.Id as processingStatusId, ProcessingStatus.Message as errorMessage
from hosts 
left join Licenses on Licenses.HostId = hosts.Id
left join Confirmations on Confirmations.LicenseId = Licenses.id
left join Activations on Licenses.Id = Activations.LicenseId
left join ProcessingStatus on ProcessingStatus.HostId = Hosts.Id



