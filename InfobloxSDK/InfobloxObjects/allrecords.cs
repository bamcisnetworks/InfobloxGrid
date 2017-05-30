using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;
using System.Linq;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("allrecords")]
    public class allrecords : BaseNameCommentObject
    {
        private string _address;
        private string[] _recordTypes = new string[] { "ALL", "record:a", "record:aaaa", "record:cname", "record:dname", "record:host", "record:host_ipv4addr", "record:host_ipv6addr", "record:mx", "record:ptr", "record:srv", "record:txt" };
        private string _type;
        private string _view;

        [ReadOnlyAttribute]
        public string address
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPAddress(value, out this._address, false, true);
            }
        }
        [ReadOnlyAttribute]
        public bool disable { get; internal protected set; }
        [ReadOnlyAttribute]
        public string dtc_obscured { get; internal protected set; }
        [ReadOnlyAttribute]
        public string record { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint ttl { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string type 
        {
            get
            {
                return this._type;
            }
            internal protected set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (this._recordTypes.Contains(value.ToLower()))
                    {
                        this._type = value.ToLower();
                    }
                    else
                    {
                        throw new ArgumentException("The provided record type is not valid.");
                    }
                }
            }
        }
        [SearchableAttribute(Equality = true)]
        public string view
        {
            get
            {
                return this._view;
            }
            internal protected set
            {
                this._view = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string zone { get; internal protected set; }

        internal protected allrecords()
        { }
    }
}
