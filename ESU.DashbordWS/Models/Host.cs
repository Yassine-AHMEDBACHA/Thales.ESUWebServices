using System;
using System.Linq;

namespace ESU.Data.Models
{
    public class Host
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Network { get; set; }

        public string Identity { get; set; }

        public string Site { get; set; }

        public string Mail { get; set; }

        public string ProductKey { get; set; }

        public string ConfirmationKey { get; set; }

        public int? ProductKeyId { get; set; }

        public string Status { get; set; }

        internal static string[] Headers => typeof(Host).GetProperties().Select(x => x.Name).ToArray();
}
}