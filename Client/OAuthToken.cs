using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TinyHttp
{
    public class OAuthAccessToken
    {

        private DateTime _CreatedOn = DateTime.Now;
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public DateTimeOffset ExpireDate
        {
            get
            {
                return CreatedOn.AddSeconds(expires_in);
            }
        }
        public bool IsExpired
        {
            get
            {
                return ExpireDate > DateTimeOffset.Now;
            }
        }

        private NameValueCollection _AuthorizationHeader = null;

        [XmlIgnore]
        [IgnoreDataMember]
        public NameValueCollection AuthorizationHeader
        {
            get
            {
                if (_AuthorizationHeader == null)
                {
                    _AuthorizationHeader = new NameValueCollection();
                    _AuthorizationHeader.Add("Authorization", "Bearer " + access_token);
                }
                return _AuthorizationHeader;
            }
        }




        public string access_token { get; set; }

        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public double expires_in { get; set; }

    }
}
