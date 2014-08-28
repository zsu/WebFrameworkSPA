//Author: Zhicheng Su
using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace SLib.Util
{
    /// <summary>
    /// Summary description for SqlXmlParametersConcatenator.
    /// </summary>
    public class SqlXmlParametersConcatenator
    {
        private StringBuilder _result;
        private string _rootTag, _elementTag;
        public SqlXmlParametersConcatenator(string rootTag,string elementTag)
        {
            if (rootTag == null || rootTag.Trim() == string.Empty)
                throw new ArgumentNullException("Root tag name cannot be null.");
            if (elementTag == null || elementTag.Trim() == string.Empty)
                throw new ArgumentNullException("Element tag name cannot be null.");

            _result = new StringBuilder();
            _rootTag = rootTag;
            _elementTag = elementTag;
        }

        public void Add(Dictionary<string,string> attributes)
        {
            if (attributes != null && attributes.Count > 0)
            {
                _result.AppendFormat("<{0} ", _elementTag);
                foreach (KeyValuePair<string, string> attribute in attributes)
                {
                    if (attribute.Key != null && attribute.Key.Trim() != string.Empty)
                        _result.AppendFormat("{0}=\"{1}\" ", attribute.Key, SecurityElement.Escape(attribute.Value));
                }
                _result.Append("/>");
            }
        } 

        public override string ToString()
        {
            if (_result != null && _result.ToString().Trim() != string.Empty)
            {
                StringBuilder result = new StringBuilder();
                result.AppendFormat("<{0}>{1}</{0}>", _rootTag, _result.ToString());
                return result.ToString();
            }
            return null;
        } 

    } //class SqlXmlParametersConcatenator

}
