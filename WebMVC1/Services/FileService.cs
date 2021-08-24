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
        private static readonly byte[] _EncryptionIV = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        //private static readonly byte[] _EncryptionIV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private const int iterations = 1042;
        private static readonly string fileExtension = "pdf";

        public ResponseModel UploadFile()
        {
            ResponseModel response = new ResponseModel();
            //string filename = $"{Directory.GetCurrentDirectory()}/template/Data/TestEncrypt.docx";
            //string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/TestEncrypt.docx";
            string filename = $"{Directory.GetCurrentDirectory()}/template/Data/Encrypt." + fileExtension;
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/Encrypt." + fileExtension;

            var ms = new MemoryStream();
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.CopyTo(ms);
            }

            using (var fs = new FileStream(fileResult, FileMode.Create, FileAccess.Write))
            {
                var res = Encrypt(ms.ToArray());
                fs.Write(res, 0, res.Length);
            }

            //UnicodeEncoding byteConverter = new UnicodeEncoding();
            //byte[] data = byteConverter.GetBytes(filename);

            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //{
            //    RSAParameters publicKey = rsa.ExportParameters(false);
            //    RSAParameters privateKey = rsa.ExportParameters(true);

            //    byte[] encrypted = Encrypt(data, publicKey, false);
            //    byte[] decrypted = Decrypt(encrypted, privateKey, false);

            //    using (var fs = new FileStream(fileResult, FileMode.Create, FileAccess.Write))
            //    {
            //        fs.Write(encrypted, 0, encrypted.Length);
            //    }

            //    using (var fs = new FileStream(fileResultD, FileMode.Create, FileAccess.Write))
            //    {

            //        fs.Write(decrypted, 0, decrypted.Length);
            //    }
            //}

            response.status = 200;
            response.success = true;
            response.message = "Encrypt Success";

            return response;
        }

        public ResponseModel DownloadFile()
        {
            ResponseModel response = new ResponseModel();
            //string filename = $"{Directory.GetCurrentDirectory()}/template/Data/TestDecrypt.docx";
            //string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/TestDecrypt.docx";
            string filename = $"{Directory.GetCurrentDirectory()}/template/Data/Decrypt." + fileExtension;
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/Decrypt." + fileExtension;

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

        #region utf8
        //private static byte[] Encrypt(byte[] data)
        //{
        //    using var aesAlg = Aes.Create();
        //    aesAlg.Key = Encoding.UTF8.GetBytes(_EncryptionKey);
        //    aesAlg.IV = Encoding.UTF8.GetBytes(_EncryptionIV);

        //    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //    using var ms = new MemoryStream();
        //    using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

        //    return ms.ToArray();
        //}

        //private static byte[] Decrypt(byte[] data)
        //{
        //    using var aesAlg = Aes.Create();
        //    aesAlg.Key = Encoding.UTF8.GetBytes(_EncryptionKey);
        //    aesAlg.IV = Encoding.UTF8.GetBytes(_EncryptionIV);

        //    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        //    using var result = new MemoryStream();
        //    using var ms = new MemoryStream(data);
        //    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        //    cs.CopyTo(result);

        //    return result.ToArray();
        //}
        #endregion

        #region Aes
        //private static byte[] Encrypt(byte[] data)
        //{
        //    var originalByteStream = new MemoryStream(data);
        //    var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
        //    var key = new Rfc2898DeriveBytes(_EncryptionKey, Encoding.ASCII.GetBytes(_EncryptionIV));

        //    algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
        //    algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);
        //    algorithm.Padding = PaddingMode.Zeros;

        //    var cryptoStream = new MemoryStream();
        //    using (var encryptedStream = new CryptoStream(cryptoStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
        //    {
        //        int dataLine;
        //        while ((dataLine = originalByteStream.ReadByte()) != -1)
        //            cryptoStream.WriteByte((byte)dataLine);
        //    }

        //    return cryptoStream.ToArray();        
        //}

        //private static byte[] Decrypt(byte[] data)
        //{
        //    var originalByteStream = new MemoryStream(data);
        //    var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
        //    var key = new Rfc2898DeriveBytes(_EncryptionKey, Encoding.ASCII.GetBytes(_EncryptionIV));

        //    algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
        //    algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);
        //    algorithm.Padding = PaddingMode.Zeros;

        //    var cryptoStream = new MemoryStream();
        //    try
        //    {
        //        using (var decryptedStream = new CryptoStream(cryptoStream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
        //        {
        //            int dataLine;
        //            while ((dataLine = originalByteStream.ReadByte()) != -1)
        //                cryptoStream.WriteByte((byte)dataLine);
        //        }
        //    }
        //    catch (CryptographicException ex)
        //    {
        //        throw new InvalidDataException("Please supply a correct password");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return cryptoStream.ToArray();
        //}
        #endregion

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

        #region rfc2898
        //private static byte[] Encrypt(byte[] data)
        //{
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, data);
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //        {
        //            cs.Write(data, 0, data.Length);
        //        }

        //        return ms.ToArray();
        //    }
        //}

        //private static byte[] Decrypt(byte[] data)
        //{
        //    using (Aes decryptor = Aes.Create())
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, data);
        //        decryptor.Key = pdb.GetBytes(32);
        //        decryptor.IV = pdb.GetBytes(16);
        //        using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Read))
        //        {
        //            cs.Write(data, 0, data.Length);
        //        }

        //        return ms.ToArray();
        //    }
        //}
        #endregion

        #region RSA
        //private static byte[] Encrypt(byte[] data, RSAParameters publicKey, bool fOAEP)
        //{
        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //    {
        //        rsa.ImportParameters(publicKey);

        //        return rsa.Encrypt(data, fOAEP);
        //    }
        //}

        //private static byte[] Decrypt(byte[] encrypted, RSAParameters privateKey, bool fOAEP)
        //{
        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //    {
        //        rsa.ImportParameters(privateKey);

        //        return rsa.Decrypt(encrypted, fOAEP);
        //    }
        //}
        #endregion
    }
}
