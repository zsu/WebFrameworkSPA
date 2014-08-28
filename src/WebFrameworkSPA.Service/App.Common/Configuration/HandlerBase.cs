using System;
using System.Globalization;
using System.Xml;
using System.Configuration;


namespace App.Common
{
    internal class HandlerBase
    {


        //
        // XML Attribute Helpers
        //

        private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool required)
        {
            XmlNode a = node.Attributes.RemoveNamedItem(attrib);

            // If the attribute is required and was not present, throw
            if (required && a == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format(AppCommon.Node_Missing_Required_Attr, node.Name,attrib),
                    node);
            }

            return a;
        }

        private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool required, ref string val)
        {
            XmlNode a = GetAndRemoveAttribute(node, attrib, required);
            if (a != null)
                val = a.Value;

            return a;
        }

        internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
        {
            return GetAndRemoveStringAttributeInternal(node, attrib, false /*required*/, ref val);
        }

        internal static XmlNode GetAndRemoveRequiredStringAttribute(XmlNode node, string attrib, ref string val)
        {
            return GetAndRemoveStringAttributeInternal(node, attrib, true /*required*/, ref val);
        }

        // input.Xml cursor must be at a true/false XML attribute
        private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool required, ref bool val)
        {
            XmlNode a = GetAndRemoveAttribute(node, attrib, required);
            if (a != null)
            {
                try
                {
                    val = bool.Parse(a.Value);
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(
                                    string.Format(AppCommon.Invalid_Bool_Attr, a.Name),
                                    e, a);
                }
            }

            return a;
        }

        internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
        {
            return GetAndRemoveBooleanAttributeInternal(node, attrib, false /*required*/, ref val);
        }

        internal static XmlNode GetAndRemoveRequiredBooleanAttribute(XmlNode node, string attrib, ref bool val)
        {
            return GetAndRemoveBooleanAttributeInternal(node, attrib, true /*required*/, ref val);
        }

        // input.Xml cursor must be an integer XML attribute
        private static XmlNode GetAndRemoveIntegerAttributeInternal(XmlNode node, string attrib, bool required, ref int val)
        {
            XmlNode a = GetAndRemoveAttribute(node, attrib, required);
            if (a != null)
            {
                try
                {
                    val = int.Parse(a.Value, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(
                        string.Format(AppCommon.Invalid_Integer_Attr, a.Name),
                        e, a);
                }
            }

            return a;
        }

        internal static XmlNode GetAndRemoveIntegerAttribute(XmlNode node, string attrib, ref int val)
        {
            return GetAndRemoveIntegerAttributeInternal(node, attrib, false /*required*/, ref val);
        }

        internal static XmlNode GetAndRemoveRequiredIntegerAttribute(XmlNode node, string attrib, ref int val)
        {
            return GetAndRemoveIntegerAttributeInternal(node, attrib, true /*required*/, ref val);
        }

        private static XmlNode GetAndRemovePositiveIntegerAttributeInternal(XmlNode node, string attrib, bool required, ref int val)
        {
            XmlNode a = GetAndRemoveIntegerAttributeInternal(node, attrib, required, ref val);

            if (a != null && val < 0)
            {
                throw new ConfigurationErrorsException(
                    string.Format(AppCommon.Invalid_Positive_Integer_Attr, attrib),
                    node);
            }

            return a;
        }

        internal static XmlNode GetAndRemovePositiveIntegerAttribute(XmlNode node, string attrib, ref int val)
        {
            return GetAndRemovePositiveIntegerAttributeInternal(node, attrib, false /*required*/, ref val);
        }

        internal static XmlNode GetAndRemoveRequiredPositiveIntegerAttribute(XmlNode node, string attrib, ref int val)
        {
            return GetAndRemovePositiveIntegerAttributeInternal(node, attrib, true /*required*/, ref val);
        }

        private static XmlNode GetAndRemoveTypeAttributeInternal(XmlNode node, string attrib, bool required, ref Type val)
        {
            XmlNode a = GetAndRemoveAttribute(node, attrib, required);

            if (a != null)
            {
                try
                {
                    val = Type.GetType(a.Value, true /*throwOnError*/);
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(
                        string.Format(AppCommon.Invalid_Type_Attr, a.Name),
                        e, a);
                }
            }

            return a;
        }

        internal static XmlNode GetAndRemoveTypeAttribute(XmlNode node, string attrib, ref Type val)
        {
            return GetAndRemoveTypeAttributeInternal(node, attrib, false /*required*/, ref val);
        }

        internal static XmlNode GetAndRemoveRequiredTypeAttribute(XmlNode node, string attrib, ref Type val)
        {
            return GetAndRemoveTypeAttributeInternal(node, attrib, true /*required*/, ref val);
        }

        internal static void CheckForUnrecognizedAttributes(XmlNode node)
        {
            if (node.Attributes.Count != 0)
            {
                throw new ConfigurationErrorsException(
                                string.Format(AppCommon.Unrecognized_Attr, node.Attributes[0].Name),
                                node);
            }
        }



        //
        // Obsolete XML Attribute Helpers
        //

        // if attribute not found return null
        internal static string RemoveAttribute(XmlNode node, string name)
        {

            XmlNode attribute = node.Attributes.RemoveNamedItem(name);

            if (attribute != null)
            {
                return attribute.Value;
            }

            return null;
        }

        // if attr not found throw standard message - "attribute x required"
        internal static string RemoveRequiredAttribute(XmlNode node, string name)
        {
            return RemoveRequiredAttribute(node, name, false/*allowEmpty*/);
        }

        internal static string RemoveRequiredAttribute(XmlNode node, string name, bool allowEmpty)
        {
            XmlNode attribute = node.Attributes.RemoveNamedItem(name);

            if (attribute == null)
            {
                throw new ConfigurationErrorsException(
                                string.Format(AppCommon.Required_Attr_Missing, name),
                                node);
            }

            if (attribute.Value == string.Empty && allowEmpty == false)
            {
                throw new ConfigurationErrorsException(
                                string.Format(AppCommon.Required_Attr_Can_Not_Be_Empty, name),
                                node);
            }

            return attribute.Value;
        }



        //
        // XML Element Helpers
        //

        internal static void CheckForNonElement(XmlNode node)
        {
            if (node.NodeType != XmlNodeType.Element)
            {
                throw new ConfigurationErrorsException(
                                AppCommon.Node_Type_Is_Not_Element,
                                node);
            }
        }


        internal static bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
            {
                return true;
            }

            CheckForNonElement(node);

            return false;
        }


        internal static void CheckForChildNodes(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                throw new ConfigurationErrorsException(
                                AppCommon.No_Child_Nodes,
                                node.FirstChild);
            }
        }

        internal static void ThrowUnrecognizedElement(XmlNode node)
        {
            throw new ConfigurationErrorsException(
                            AppCommon.Unrecognized_Element,
                            node);
        }

        // 
        // Parse Helpers
        // 

    }
}
