using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS.Shared
{
    [Name("sharedrecord:txt")]
    public class sharedrecord_txt : BaseSharedRecord
    {
        private string _text;
        
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Encoding enc = Encoding.Unicode;
                    if (enc.GetByteCount(value.TrimValue()) <= 512)
                    {
                        Regex regex = new Regex("\".*?\"");
                        MatchCollection matches = regex.Matches(value);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (enc.GetByteCount(match.ToString()) <= 255)
                                {
                                    this._text += match.ToString();
                                }
                                else
                                {
                                    throw new ArgumentException(String.Format("Each substring in the text value must be less than 255 bytes, a substring of {0} bytes was provided.", enc.GetByteCount(match.ToString())));
                                }
                            }
                        }
                        else if (enc.GetByteCount(value.TrimValue()) <= 255)
                        {
                            this._text = value.TrimValue();
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("A single string in the text value must be less than 255 bytes, a string of {0} bytes was provided.", enc.GetByteCount(value)));
                        }
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("The total length of the text field for all substrings must be less than 512 bytes, a string of {0} bytes was provided.", enc.GetByteCount(value)));
                    }
                }
                else
                {
                    throw new ArgumentException("The text field value cannot be null or empty.");
                }
            }
        }

        public sharedrecord_txt(string name, string shared_record_group, string text) : base(shared_record_group)
        {
            base.name = name;
            this.text = text;
        }
    }
}
