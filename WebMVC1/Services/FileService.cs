using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using WebMVC1.Models;
using System.Text;

namespace WebMVC1.Services
{
    public class FileService : IFileService
    {
        private static readonly string _EncryptionKey = "FWins";
        //private static readonly string _EncryptionIV = "FWG";
        //private static readonly byte[] _EncryptionIV = Encoding.ASCII.GetBytes("FWGinsurance");
        private static readonly byte[] _EncryptionIV = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private const int iterations = 1042;
        private static readonly string fileExtension = "pdf";

        public ResponseModel UploadFile()
        {
            ResponseModel response = new ResponseModel();
            string filename = $"{Directory.GetCurrentDirectory()}/template/Data/Encrypt." + fileExtension;
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/Encrypt_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + fileExtension;

            var ms = new MemoryStream();
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.CopyTo(ms);
            }

            using (var fs = new FileStream(fileResult, FileMode.Create, FileAccess.Write))
            {
                var res = Encrypt(ms.ToArray());
                fs.Write(res, 0, res.Length);
                fs.Close();
            }

            response.status = 200;
            response.success = true;
            response.message = "Encrypt Success";

            return response;
        }

        public ResponseModel DownloadFile()
        {
            ResponseModel response = new ResponseModel();
            string filename = $"{Directory.GetCurrentDirectory()}/template/Data/Decrypt." + fileExtension;
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/Decrypt_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + fileExtension;

            try
            {
                var ms = new MemoryStream();
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.CopyTo(ms);
                }

                using (var fs = new FileStream(fileResult, FileMode.Create, FileAccess.Write))
                {
                    var res = Decrypt(ms.ToArray());
                    fs.Write(res, 0, res.Length);
                    fs.Close();
                }

                response.status = 200;
                response.success = true;
                response.message = "Decrypt Success";
            }
            catch(Exception ex)
            {
                response.status = 200;
                response.success = false;
                response.message = "Decrypt Failed";
                response.data = "DownloadFile Error: " + ex.Message + " - " + ex?.InnerException?.Message + " " + ex.ToString();
            }
            return response;
        }

        #region RijndaelManaged
        private static byte[] Encrypt(byte[] data)
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_EncryptionKey, _EncryptionIV);
            byte[] Key = pdb.GetBytes(32);
            byte[] IV = pdb.GetBytes(16);
            using (var aes = new RijndaelManaged { KeySize = 256, Key = Key, IV = IV, Padding = PaddingMode.PKCS7, Mode = CipherMode.CBC, BlockSize = 128 })
            //using (var aes = new RijndaelManaged { Key = Key, IV = IV })
            {
                using (var encryptedStream = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            //cryptoStream.FlushFinalBlock();
                            //using (var originalByteStream = new MemoryStream(data))
                            //{
                            //    int dataLine;
                            //    while ((dataLine = originalByteStream.ReadByte()) != -1)
                            //        cryptoStream.WriteByte((byte)dataLine);
                            //}                            
                        }
                    }

                    var encryptedBytes = encryptedStream.ToArray();
                    return encryptedBytes;
                }
            }
        }

        private static byte[] Decrypt(byte[] data)
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_EncryptionKey, _EncryptionIV);
            byte[] Key = pdb.GetBytes(32);
            byte[] IV = pdb.GetBytes(16);
            try
            {
                using (var encryptedStream = new MemoryStream(data))
                {
                    using (var decryptedStream = new MemoryStream())
                    {
                        using (var aes = new RijndaelManaged { KeySize = 256, Key = Key, IV = IV, Padding = PaddingMode.Zeros, BlockSize = 128 })
                        {
                            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                            {
                                using (var cryptoStream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(data, 0, data.Length);
                                    cryptoStream.Close();
                                    //encryptedStream.CopyTo(cryptoStream);
                                    //int dataLine;
                                    //while ((dataLine = cryptoStream.ReadByte()) != -1)
                                    //    decryptedStream.WriteByte((byte)dataLine);

                                    //cryptoStream.CopyTo(decryptedStream);

                                    //using (StreamReader srDecrypt = new StreamReader(cryptoStream))
                                    //{
                                    //    srDecrypt.BaseStream.CopyTo(decryptedStream);
                                    //}
                                }
                            }
                        }

                        //decryptedStream.Position = 0;
                        return decryptedStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Decrypt Error: " + ex.Message + " - " + ex?.InnerException?.Message + " " + ex.ToString());
            }
        }
        #endregion           
    }
}
