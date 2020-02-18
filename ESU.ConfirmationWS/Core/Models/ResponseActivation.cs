using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.ConfirmationWS.Core.Models
{
    public class ResponseActivation
    {
        //<?xml version = "1.0" encoding="utf-8"?>
        //<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //    <soap:Body>
        //        <AcquireConfirmationIdResponse xmlns = "http://tempuri.org/" >
        //            < AcquireConfirmationIdResult >
        //             530453693466059432467633325075009280408330787221 </ AcquireConfirmationIdResult >
        //        </ AcquireConfirmationIdResponse >
        //    </ soap:Body>
        //</soap:Envelope>

        [JsonProperty("soap:Envelope")]
        public Envelope envelope { get; set; }
    }

    public class Envelope
    {
        [JsonProperty("soap:Body")]
        public Body body { get; set; }
    }

    public class Body
    {
        [JsonProperty("AcquireConfirmationIdResponse")]
        public AcquireConfirmationIdResponse AcquireConfirmationIdResponse { get; set; }
    }

    public class AcquireConfirmationIdResponse
    {
        [JsonProperty("AcquireConfirmationIdResult")]
        public string AcquireConfirmationIdResult { get; set; }
    }
}
