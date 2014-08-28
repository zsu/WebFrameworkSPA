using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Common.Test
{
    [TestClass]
    public class StringExtensionTest
    {
        [TestMethod]
        public void IsWebUrl_Should_Return_Correct_Result()
        {
            string[] targets = new string[] { 
                "http://dotnetshoutout.com" ,
                "htp://dotnetshoutout.com",
                "http://www.dotnetshoutout.com",
                "www.dotnetshoutout.com",
                ""}; 
            bool[] results=new bool[]{
                true,
                false,
                true,
                false,
                false,
            };
            Helper.TestInlineData<string,bool>(targets, results,StringExtension.IsWebUrl);           
        }

        [TestMethod]
        public void IsEmail_Should_Return_Correct_Result()
        {
            string[] targets = new string[] { 
                "admin@dotnetshoutout.com",
                "admin@dotnetshoutout.com.bd",
                "admin@dotnetshoutoutcom",
                "admin",
                ""};
            bool[] results = new bool[]{
                true,
                true,
                false,
                false,
                false,
            };
            Helper.TestInlineData<string,bool>(targets, results,StringExtension.IsEmail);           
        }

        [TestMethod]
        public void NullSafe_Should_Return_Empty_String_When_Null_String_Is_Passed()
        {
            const string nullString = null;

            Assert.AreEqual(string.Empty, nullString.NullSafe());
        }

        [TestMethod]
        public void FormatWith_Should_Replace_Place_Holder_Tokens_With_Provided_Value()
        {
            Assert.AreEqual("A-B-C-D", "{0}-{1}-{2}-{3}".FormatWith("A", "B", "C", "D"));
        }

        [TestMethod]
        public void Hash_Should_Return_Hashed_Value()
        {
            var plain = new string('x', 2048);
            string result = "XkLq9Yfgp4/OzryaEaAl8Q==";
            var hash = plain.Hash();

            Assert.AreEqual(result, hash);
        }

        [TestMethod]
        public void Hash_Should_Always_Return_Twenty_Four_Character_String()
        {
            string[] targets=new string[]{
                "abcd",
                "a dummy string",
                "another dummy string",
                "x"
            };
            foreach(string target in targets)
            {
                var hash = target.Hash();
                Assert.AreEqual(24, hash.Length);
            }
        }

        [TestMethod]
        public void WrapAt_Should_Returns_String_Which_Ends_With_Three_Dots()
        {
            Assert.AreEqual("a du...", "a dummy string".WrapAt(7));
        }

        [TestMethod]
        public void StripHtml_Should_Return_Only_Text()
        {
            const string Html = "<div style=\"border:1px #000\">This is a div</div>";

            Assert.AreEqual("This is a div", Html.StripHtml());
        }

        [TestMethod]
        public void ToGuid_Should_Return_Correct_Guid_When_Previously_Shrinked_String_Is_Passed()
        {
            string target = "JKgxzYZ2dEeRQz_D7XlRDw";
            string result = "cd31a824-7686-4774-9143-3fc3ed79510f";
            Assert.AreEqual(new Guid(result), target.ToGuid());
        }

        [TestMethod]
        public void ToGuid_Should_Throw_Exception_When_Incorrect_String_Is_Passed()
        {
            string[] targets = new string[]{
                "0P#x!=?dl;)x~cxza1@&Z$",
                "jdgndbaxhkgsaghdngtdhas"
            };
            foreach(string target in targets)
            {
                AssertExtension.Throws<Guid,FormatException>(target.ToGuid);
            }
        }
        private enum Roles
        {
            Administrator,
            User
        }
        [TestMethod]
        public void ToEnum_Should_Be_Able_To_Convert_From_String()
        {
            string[] targets = new string[]{
                "Administrator",
                "foo"
            };
            Roles[] results = new Roles[]{
                Roles.Administrator,
                Roles.User
            };
            if (targets.Length != results.Length)
                Assert.Fail("Test data error.");
            Roles role=Roles.User;
            for (int i = 0; i < targets.Length; i++)
            {
                Assert.AreEqual(results[i], targets[i].ToEnum<Roles>(role));
            }
        }

        [TestMethod]
        public void ToLegalUrl_Should_Return_String_Which_Does_Not_Contain_Any_Illegal_Url_Character()
        {
            string[] targets = new string[]{
                " abcd",
                "abcd",
                "ab;cd",
                "abc/d",
                "abc+d",
                "abc<d",
                "a&bcd",
                "ab   cd",
                "<>,+$",
                ""
            };
            string[] results = new string[]{
                "abcd",
                "abcd",
                "abcd",
                "abcd",
                "abcd",
                "abcd",
                "abcd",
                "ab-cd",
                "",
                ""
            };
            Helper.TestInlineData<string, string>(targets, results, StringExtension.ToLegalUrl);
        }

        [TestMethod]
        public void UrlEncode_Should_Return_Url_Encoded_String()
        {
            Assert.AreEqual("A%3fString", "A?String".UrlEncode());
        }

        [TestMethod]
        public void UrlDecode_Should_Return_Url_Decoded_String()
        {
            Assert.AreEqual("A?String", "A%3fString".UrlDecode());
        }

        [TestMethod]
        public void AttributeEncode_Should_Return_Attribute_Encoded_String()
        {
            Assert.AreEqual("&quot;quotes", "\"quotes".AttributeEncode());
        }

        [TestMethod]
        public void HtmlEncode_Should_Return_Html_Encoded_String()
        {
            Assert.AreEqual("&lt;A?String/&gt;", "<A?String/>".HtmlEncode());
        }

        [TestMethod]
        public void HtmlDecode_Should_Return_Html_Decoded_String()
        {
            Assert.AreEqual("<A?String/>", "&lt;A?String/&gt;".HtmlDecode());
        }
    }
}