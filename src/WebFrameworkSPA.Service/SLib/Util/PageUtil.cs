using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SLib.Util
{
	public abstract class PageUtil
	{
		private static string GetFilterValue( NameValueCollection collection, string value ) {
			if (collection == null || collection.Count == 0)
				return null;
			return collection[value];
		}

		public static string GetFilterValue( Page page, string key ) {
			return GetFilterValue( page, key, true );
		}

		public static string GetFilterValue( Page page, string key, bool checkSessionOnly ) {
			if (page == null)
				throw new ArgumentNullException( "Page object cannot be null." );
			string result = null;
			if (!checkSessionOnly)
				result = PageUtil.GetFilterValue( page.Request.Params, key );

			if (result == null && HttpContext.Current.Session!=null) {
				string fullKey = GetFullKey( page, key );
				result = page.Session.Contents[fullKey] as string;
			}

			return result;
		}

		public static void SaveSelectionInSession( Page page, NameValueCollection selections ) {
            if (HttpContext.Current.Session != null)
            {
                if (page == null)
                    throw new ArgumentNullException("Page object cannot be null.");
                if (selections == null || selections.Count == 0)
                    return;
                foreach (string key in selections.Keys)
                {
                    string fullKey = GetFullKey(page, key);
                    string value = selections[key];

                    if (value != null)
                    {
                        page.Session.Contents[fullKey] = value;
                    }
                    else
                    {
                        page.Session.Contents.Remove(fullKey);
                    }
                }
            }
		}

		public static void SaveSelectionInSession( Page page, string selectionKey, string value ) {
			if (selectionKey == null || selectionKey.Trim() == string.Empty)
				return;
			NameValueCollection oValues = new NameValueCollection();
			oValues.Add( selectionKey, value );
			SaveSelectionInSession( page, oValues );
		}

		public static void ClearSelectionInSession( Page page, string[] selectionKeys ) {
            if (HttpContext.Current.Session != null)
            {
                if (page == null)
                    throw new ArgumentNullException("Page object cannot be null.");
                if (selectionKeys == null || selectionKeys.Length == 0)
                    return;
                foreach (string key in selectionKeys)
                {
                    string fullKey = GetFullKey(page, key);
                    page.Session.Contents.Remove(fullKey);
                }
            }
		}

		public static void ClearSelectionInSession( Page page, string selectionKey ) {
            string[] selectionKeys = null;
			if (string.IsNullOrEmpty( selectionKey ))
				selectionKeys=new string[]{selectionKey};
            ClearSelectionInSession(page, selectionKeys);
		}

		public static string GetFullKey( Page page, string key ) {
			return String.Format( "{0}:{1}", GetPageName( page ), key ).ToLower();
		}

		/// <summary>
		///	Retrieves the relative path for a given page
		/// </summary>
		/// <param name="page">
		///	The page whose relative path is to be retrieved
		/// </param>
		/// <returns>
		///	The relative path for a given page
		/// </returns>
		private static string GetPageName( Page page ) {
            return page.AppRelativeVirtualPath;
		}
	}
}

