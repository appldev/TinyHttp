using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TinyHttp
{
    public abstract class OAuthTokenBase : IOAuthToken
    {
        public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual bool IsExpired
        {
            get
            {
                return ExpireDate > DateTimeOffset.Now;
            }
        }

        public virtual DateTimeOffset ExpireDate
        {
            get
            {
                return CreatedOn.AddSeconds(expires_in);
            }
        }

        private NameValueCollection _AuthorizationHeader = null;

        [XmlIgnore]
        [IgnoreDataMember]
        public virtual NameValueCollection AuthorizationHeader
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
        public double expires_in { get; set; } = 31536000D;
    }

    public interface IOAuthToken
    {
        string access_token { get; set; }
        string refresh_token { get; set; }
        string token_type { get; set; }
        double expires_in { get; set; }

        NameValueCollection AuthorizationHeader { get; }
    }
}
