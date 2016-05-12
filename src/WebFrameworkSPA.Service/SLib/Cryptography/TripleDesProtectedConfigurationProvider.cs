using System;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;


namespace SLib.Cryptography
{

    public class TripleDesProtectedConfigurationProvider : ProtectedConfigurationProvider,IDisposable
    {

        private TripleDESCryptoServiceProvider _desProvider = new TripleDESCryptoServiceProvider();

        //private string pKeyFilePath;
        private string _name;

        //public string KeyFilePath
        //{
        //    get { return pKeyFilePath; }
        //}


        //
        // ProviderBase.Name
        //

        public override string Name
        {
            get { return _name; }
        }


        //
        // ProviderBase.Initialize
        //

        public override void Initialize(string name, NameValueCollection config)
        {
            _name = name;
            //pKeyFilePath = config["keyFilePath"];
            //ReadKey(KeyFilePath);
            SetKey();
        }


        //
        // ProtectedConfigurationProvider.Encrypt
        //

        public override XmlNode Encrypt(XmlNode node)
        {
            string encryptedData = EncryptString(node.OuterXml);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml("<EncryptedData>" + encryptedData + "</EncryptedData>");

            return xmlDoc.DocumentElement;
        }


        //
        // ProtectedConfigurationProvider.Decrypt
        //

        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            string decryptedData = DecryptString(encryptedNode.InnerText);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(decryptedData);

            return xmlDoc.DocumentElement;
        }


        //
        // EncryptString
        //    Encrypts a configuration section and returns the encrypted
        // XML as a string.
        //

        public string EncryptString(string encryptValue)
        {
            SetKey();
            byte[] valBytes = Encoding.Unicode.GetBytes(encryptValue);

            ICryptoTransform transform = _desProvider.CreateEncryptor();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();

            return Convert.ToBase64String(returnBytes);
        }


        //
        // DecryptString
        //    Decrypts an encrypted configuration section and returns the
        // unencrypted XML as a string.
        //

        public string DecryptString(string encryptedValue)
        {
            SetKey();
            byte[] valBytes = Convert.FromBase64String(encryptedValue);

            ICryptoTransform transform = _desProvider.CreateDecryptor();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();

            return Encoding.Unicode.GetString(returnBytes);
        }

        //
        // CreateKey
        //    Generates a new TripleDES key and vector and writes them
        // to the supplied file path.
        //

        public void CreateKey(string filePath)
        {
            _desProvider.GenerateKey();
            _desProvider.GenerateIV();

            StreamWriter sw = new StreamWriter(filePath, false);
            sw.WriteLine(ByteToHex(_desProvider.Key));
            sw.WriteLine(ByteToHex(_desProvider.IV));
            sw.Close();
        }


        //
        // ReadKey
        //    Reads in the TripleDES key and vector from the supplied
        // file path and sets the Key and IV properties of the 
        // TripleDESCryptoServiceProvider.
        //

        //private void ReadKey(string filePath)
        //{
        //    StreamReader sr = new StreamReader(filePath);
        //    string keyValue = sr.ReadLine();
        //    string ivValue = sr.ReadLine();
        //    des.Key = HexToByte(keyValue);
        //    des.IV = HexToByte(ivValue);
        //}


        //
        // ByteToHex
        //    Converts a byte array to a hexadecimal string.
        //
        private void SetKey()
        {
            _desProvider.Key = HexToByte("60B35C8AD168510F58F2E42C48F4B57BA813A7A350CE73AD");
            _desProvider.IV = HexToByte("5598A888706C25BE");
            _desProvider.Padding = PaddingMode.PKCS7;
        }
        private string ByteToHex(byte[] byteArray)
        {
            string outString = "";

            foreach (Byte b in byteArray)
                outString += b.ToString("X2");

            return outString;
        }

        //
        // HexToByte
        //    Converts a hexadecimal string to a byte array.
        //

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #region IDisposable Members
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_desProvider!=null)
                    _desProvider.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
