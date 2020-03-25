using System;

namespace ESU.ImportWS.Models
{
    public class Message
    {
        private string row;

        public Message(string row)
        {
            this.row = row;
            var fields = this.row.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length != 10)
            {
                throw new Exception("Invalid row content");
            }

            this.LocalId = int.Parse(fields[0]);
            this.RemoteId = int.Parse(fields[1]);
            this.Owner = fields[2];
            this.Name = fields[3];
            this.Network = fields[4];
            this.Identity = fields[5];
            this.Mail = fields[6];
            this.Site = fields[7];
            this.OsBuild = fields[8];
            this.OsVersion = fields[9];
            this.Is64BitOperatingSystem =bool.Parse(fields[10]);
            this.ProductType = int.Parse(fields[11]);
            this.Date = DateTime.Parse(fields[12]);
            this.Data = fields[13];
            this.Tag = fields[14];
            this.Bag = fields[15];
        }

        public int LocalId { get; set; }

        public int RemoteId { get; set; }

        public string Owner { get; set; }

        public string Ressource { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Network { get; set; }

        public string Identity { get; set; }

        public string Site { get; set; }

        public string Mail { get; set; }

        public string OsBuild { get; set; }

        public string OsVersion { get; set; }

        public bool Is64BitOperatingSystem { get; set; }

        public int ProductType { get; set; }

        public string Data { get; set; }

        public string Tag { get; set; }

        public string Bag { get; set; }
    }
}
