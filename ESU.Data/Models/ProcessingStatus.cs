using System;

namespace ESU.Data.Models
{
    public class ProcessingStatus
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime StatusDate { get; set; }

        public int HostId { get; set; }

        public Host Host { get; set; }
    }
}
