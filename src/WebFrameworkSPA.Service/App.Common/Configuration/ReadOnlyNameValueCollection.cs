using System;
using System.Collections;
using System.Collections.Specialized;


namespace App.Common
{
     internal class ReadOnlyNameValueCollection : NameValueCollection {
         public ReadOnlyNameValueCollection(IEqualityComparer equalityComparer): base(equalityComparer)
         {
         }
         public ReadOnlyNameValueCollection(ReadOnlyNameValueCollection value) : base(value) {
         }

         public void SetReadOnly() {
             IsReadOnly = true;
         }
     }

}
