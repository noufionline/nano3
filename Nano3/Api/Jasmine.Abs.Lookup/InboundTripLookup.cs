using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasmine.Abs.Lookup
{
    [NotifyPropertyChanged]
    [Serializable]
    public class InboundTripLookup
    {
        public IEnumerable<Lookup> SecurityGates { get; set; }
        public IEnumerable<VehicleLookup> Vehicles { get; set; }
        public IEnumerable<Lookup> Drivers { get; set; }
        public IEnumerable<Lookup> Divisions { get; set; }
        public IEnumerable<Lookup> Locations { get; set; }
        public IEnumerable<Lookup> TripPurposes { get; set; }
        public IEnumerable<Lookup> MovementTypes { get; set; }
        public IEnumerable<Lookup> Suppliers { get; set; }


    }

    [Serializable]
    public class VehicleLookup : Lookup
    {
        public string TransportCompany { get; set; }
    }
}