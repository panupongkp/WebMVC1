using Newtonsoft.Json;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebMVC1.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using HtmlToOpenXml;
using OpenXmlHelpers.Word;
using System.Xml.Linq;
using WebMVC1.DBContext;
using OpenXmlPowerTools;
using System.Data;

namespace WebMVC1.Services
{
    public class MailMergeService : IMailMergeService
    {
        private readonly FWContext _mailMergeContext;
        private static class MailMergeData
        {
            public static string FirstName { get; set; } = "TestFirstName";
            public static string LastName { get; set; } = "TestLastName";
        }

        public MailMergeService(FWContext mailMergeContext)
        {
            _mailMergeContext = mailMergeContext;
        }

        public ResponseModel UploadMailMerge()
        {
            ResponseModel response = new ResponseModel();

            string filename = $"{Directory.GetCurrentDirectory()}/template/Template001.docx";
            string fileSource1 = $"{Directory.GetCurrentDirectory()}/template/Data/Clause001.docx";
            string fileSource2 = $"{Directory.GetCurrentDirectory()}/template/Data/Clause002.docx";

            FileStream fs = new FileStream(filename, FileMode.Open);
            var ms = new MemoryStream();
            fs.CopyTo(ms);

            List<MailMergeEntity> mailMergeList = new List<MailMergeEntity> { };
            MailMergeEntity mailMergeEntity = new MailMergeEntity();
            mailMergeEntity.id = Guid.NewGuid();
            mailMergeEntity.fileName = Path.GetFileNameWithoutExtension(filename);
            mailMergeEntity.filePath = filename.Replace(Path.GetFileName(filename), "");
            mailMergeEntity.fileExtension = Path.GetExtension(filename);
            mailMergeEntity.fileData = ms.ToArray();
            mailMergeList.Add(mailMergeEntity);

            fs = new FileStream(fileSource1, FileMode.Open);
            ms = new MemoryStream();
            fs.CopyTo(ms);
            mailMergeEntity = new MailMergeEntity();
            mailMergeEntity.id = Guid.NewGuid();
            mailMergeEntity.fileName = Path.GetFileNameWithoutExtension(fileSource1);
            mailMergeEntity.filePath = fileSource1.Replace(Path.GetFileName(fileSource1), "");
            mailMergeEntity.fileExtension = Path.GetExtension(fileSource1);
            mailMergeEntity.fileData = ms.ToArray();
            mailMergeList.Add(mailMergeEntity);

            fs = new FileStream(fileSource2, FileMode.Open);
            ms = new MemoryStream();
            fs.CopyTo(ms);
            mailMergeEntity = new MailMergeEntity();
            mailMergeEntity.id = Guid.NewGuid();
            mailMergeEntity.fileName = Path.GetFileNameWithoutExtension(fileSource2);
            mailMergeEntity.filePath = fileSource1.Replace(Path.GetFileName(fileSource2), "");
            mailMergeEntity.fileExtension = Path.GetExtension(fileSource2);
            mailMergeEntity.fileData = ms.ToArray();
            mailMergeList.Add(mailMergeEntity);

            ms.Close();
            fs.Close();

            _mailMergeContext.mailMergeEntities.AddRange(mailMergeList);
            _mailMergeContext.SaveChanges();

            response.status = 200;
            response.success = true;
            response.data = "Success";
            response.message = "Upload Success";
            //var data = _mailMergeContext.mailMergeEntities.ToList();

            return response;
        }

        public ResponseModel GetMailMergeText()
        {
            ResponseModel response = new ResponseModel();

            string mailMergeText = "";
            string mailMergeInnerXML = "";
            string mailMergeOuterXML = "";

            string filename = $"{Directory.GetCurrentDirectory()}/template/Template001.docx";
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
            var fT = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Template001").Select(s => s.fileData).FirstOrDefault();
            var f1 = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Clause001").Select(s => s.fileData).FirstOrDefault();
            var f2 = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Clause002").Select(s => s.fileData).FirstOrDefault();
            var listByte = new List<byte[]> { f1, f2 };

            //var ms = new MemoryStream();
            //ms.Write(fT, 0, fT.Length);
            //ms.Position = 0;

            try
            {
                //WordprocessingDocument doc = WordprocessingDocument.Open(ms, true);
                //if (doc != null)
                //{
                //Merge Filestream
                //MainDocumentPart mainPart = doc.MainDocumentPart;
                foreach (byte[] b in listByte)
                {
                    using (var mss = new MemoryStream())
                    {
                        mss.Write(b, 0, b.Length);
                        mss.Position = 0;
                        WordprocessingDocument docData = WordprocessingDocument.Open(mss, true);
                        mailMergeText += docData.MainDocumentPart.Document.Body.InnerText + @"\r\n";
                        mailMergeInnerXML += docData.MainDocumentPart.Document.Body.InnerXml;
                        mailMergeOuterXML += docData.MainDocumentPart.Document.Body.OuterXml;

                        mss.Close();
                    }
                }
                //doc.Save();
                //}
                //StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream());
                //var docText = sr.ReadToEnd();
                //string docText = "";
                //using (StreamWriter sw = new StreamWriter($"{Directory.GetCurrentDirectory()}/template/Result/mailTxt.txt"))
                //{
                //    Body body = doc.MainDocumentPart.Document.Body;
                //    foreach (OpenXmlElement cBody in body.ChildElements)
                //    {
                //        docText = cBody.InnerText; //docText += cBody.InnerText + @"\r\n";
                //        //sw.WriteLine(docText);
                //    }
                //}

                //Response Model
                response.status = 200;
                MailMergeModel mmModel = new MailMergeModel
                {
                    innerText = mailMergeText,
                    innerXML = mailMergeInnerXML,
                    outerXML = mailMergeOuterXML
                    //bytesdata = ms.ToArray()
                };

                response.data = mmModel;

                //Writefile
                //ms.Seek(0, SeekOrigin.Begin);
                //using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                //{
                //    ms.CopyTo(savefs);
                //    savefs.Flush();
                //}
                //ms.Close();
            }
            catch (Exception ex)
            {
                //ms.Close();
                throw new Exception(ex.Message + ex?.InnerException.ToString());
            }

            return response;
        }

        public ResponseModel GetMailMerge()
        {
            ResponseModel response = new ResponseModel();
            string filename = $"{Directory.GetCurrentDirectory()}/template/Template001.docx";
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
            var fT = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Template001").Select(s => s.fileData).FirstOrDefault();
            var f1 = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Clause001").Select(s => s.fileData).FirstOrDefault();
            var f2 = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Clause002").Select(s => s.fileData).FirstOrDefault();
            var listByte = new List<byte[]> { f1, f2 };
            var ms = new MemoryStream();
            ms.Write(fT, 0, fT.Length);
            ms.Position = 0;

            try
            {
                WordprocessingDocument doc = WordprocessingDocument.Open(ms, true);
                if (doc != null)
                {
                    //Merge Filestream
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    int i = 0;
                    foreach (byte[] b in listByte)
                    {
                        using (var mss = new MemoryStream())
                        {
                            mss.Write(b, 0, b.Length);
                            mss.Position = 0;
                            //WordprocessingDocument docData = WordprocessingDocument.Open(mss, true);
                            //docData.SaveAs($"{Directory.GetCurrentDirectory()}/template/Result/data" + i.ToString() + ".docx");
                            //docData.Save();

                            //var fileStream = new FileStream($"{Directory.GetCurrentDirectory()}/template/Result/data" + i.ToString() + ".docx", FileMode.OpenOrCreate);
                            //mss.CopyTo(fileStream);
                            //fileStream.Flush();
                            //fileStream.Close();

                            //string altChunkId = "AltChunkId" + i.ToString();
                            //AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                            AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML);
                            string altChunkId = mainPart.GetIdOfPart(chunk);

                            chunk.FeedData(mss);

                            AltChunk altChunk = new AltChunk { Id = altChunkId };
                            //altChunk.Id = altChunkId;
                            mainPart.Document.Body.AppendChild(altChunk);
                            //mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().First());
                            mainPart.Document.Save();

                            i++;
                            //stm.Close();
                            mss.Close();
                        }
                    }
                    doc.Save();
                }
                //StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream());
                //var docText = sr.ReadToEnd();
                //string docText = "";
                //using (StreamWriter sw = new StreamWriter($"{Directory.GetCurrentDirectory()}/template/Result/mailTxt.txt"))
                //{
                //    Body body = doc.MainDocumentPart.Document.Body;
                //    foreach (OpenXmlElement cBody in body.ChildElements)
                //    {
                //        docText = cBody.InnerText; //docText += cBody.InnerText + @"\r\n";
                //        //sw.WriteLine(docText);
                //    }
                //}

                //Response Model
                response.status = 200;
                MailMergeModel mmModel = new MailMergeModel
                {
                    innerText = doc.MainDocumentPart.Document.Body.InnerText,
                    innerXML = doc.MainDocumentPart.Document.Body.InnerXml,
                    outerXML = doc.MainDocumentPart.Document.Body.OuterXml,
                    bytesdata = ms.ToArray()
                };

                response.data = mmModel;

                doc.Close();

                //Writefile
                ms.Seek(0, SeekOrigin.Begin);
                using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                {
                    ms.CopyTo(savefs);
                    savefs.Flush();
                }
                ms.Close();
            }
            catch (Exception ex)
            {
                ms.Close();
                throw new Exception(ex.Message + ex?.InnerException.ToString());
            }

            return response;
        }

        public ResponseModel GetMailMergePowerTools()
        {
            ResponseModel response = new ResponseModel();
            string filename = $"{Directory.GetCurrentDirectory()}/template/Template001.docx";
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
            //var fT = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Template001").Select(s => s.fileData).FirstOrDefault();
            //var f1 = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Clause001").Select(s => s.fileData).FirstOrDefault();
            //var f2 = _mailMergeContext.mailMergeEntities.Where(w => w.fileName == "Clause002").Select(s => s.fileData).FirstOrDefault();
            //var listByte = new List<byte[]> { f1, f2 };
            ////FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            //var ms = new MemoryStream();
            //ms.Write(fT, 0, fT.Length);
            //ms.Position = 0;

            string[] fileSources = new string[] { $"{Directory.GetCurrentDirectory()}/template/Data/Clause001.docx", $"{Directory.GetCurrentDirectory()}/template/Data/Clause002.docx" };
            var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;

            var f1 = File.ReadAllBytes(fileSources[0]);// new FileStream(fileSources[0], FileMode.Open, FileAccess.ReadWrite);
            var f2 = File.ReadAllBytes(fileSources[1]);// new FileStream(fileSources[1], FileMode.Open, FileAccess.ReadWrite);
            var listByte = new List<byte[]> { f1, f2 };

            var msRes = new MemoryStream();
            try
            {
                //var doct = WordprocessingDocument.Open(ms, true);
                WmlDocument docM = new WmlDocument("main", ms, true);
                if (docM != null)
                {
                    //Merge Filestream
                    //MainDocumentPart mainPart = doc.MainDocumentPart;
                    int i = 0;
                    List<Source> sources = new List<Source>();
                    sources.Add(new Source(docM, true));
                    foreach (byte[] b in listByte)
                    {
                        #region can use
                        //var mss = new MemoryStream();
                        //mss.Write(b, 0, b.Length);
                        //mss.Position = 0;
                        //Stream stm = mss;
                        //string altChunkId = "AltChunkId" + i.ToString();
                        //AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                        ////using (FileStream fileStream = File.Open(files, FileMode.Open))
                        ////{
                        //chunk.FeedData(stm);
                        ////}
                        //AltChunk altChunk = new AltChunk();
                        //altChunk.Id = altChunkId;
                        //mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                        ////mainPart.Document.Save();
                        //i++;
                        //stm.Close();
                        //mss.Close();
                        #endregion

                        var mss = new MemoryStream();
                        mss.Write(b, 0, b.Length);
                        mss.Position = 0;
                        //Stream stm = mss;
                        //WordprocessingDocument docData = WordprocessingDocument.Open(mss, true);
                        WmlDocument docData = new WmlDocument(i.ToString(), mss, true);
                        sources.Add(new Source(docData, true));

                        i++;
                        //stm.Close();
                        mss.Close();
                    }
                    DocumentBuilder.BuildDocument(sources).WriteByteArray(msRes);
                    //doc.Save();
                }
                WordprocessingDocument doc = WordprocessingDocument.Open(msRes, true);
                //StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream());
                //var docText = sr.ReadToEnd();
                //string docText = "";
                //using (StreamWriter sw = new StreamWriter($"{Directory.GetCurrentDirectory()}/template/Result/mailTxt.txt"))
                //{
                //    Body body = doc.MainDocumentPart.Document.Body;
                //    foreach (OpenXmlElement cBody in body.ChildElements)
                //    {
                //        docText = cBody.InnerText; //docText += cBody.InnerText + @"\r\n";
                //        //sw.WriteLine(docText);
                //    }
                //}

                //Response Model
                response.status = 200;
                MailMergeModel mmModel = new MailMergeModel
                {
                    innerText = doc.MainDocumentPart.Document.Body.InnerText,
                    innerXML = doc.MainDocumentPart.Document.Body.InnerXml,
                    outerXML = doc.MainDocumentPart.Document.Body.OuterXml,
                    bytesdata = msRes.ToArray()
                };

                response.data = mmModel;

                //doc.Close();
                //fs.Close();

                //Writefile
                msRes.Seek(0, SeekOrigin.Begin);
                using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                {
                    msRes.CopyTo(savefs);
                    savefs.Flush();
                }
                msRes.Close();
                //ms.Seek(0, SeekOrigin.Begin);
                //using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                //{
                //    ms.CopyTo(savefs);
                //    savefs.Flush();
                //}
                //ms.Close();
            }
            catch (Exception ex)
            {
                //fs.Close();
                ms.Close();
                throw new Exception(ex.Message + ex?.InnerException.ToString());
            }

            return response;
        }

        public ResponseModel GetMailMergeFile()
        {
            ResponseModel response = new ResponseModel();
            string filename = $"{Directory.GetCurrentDirectory()}/template/Template001.docx";
            string fileSource1 = $"{Directory.GetCurrentDirectory()}/template/Data/Clause001.docx";
            string fileSource2 = $"{Directory.GetCurrentDirectory()}/template/Data/Clause002.docx";
            string[] fileSources = new string[] { $"{Directory.GetCurrentDirectory()}/template/Data/Clause001.docx", $"{Directory.GetCurrentDirectory()}/template/Data/Clause002.docx" };
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
            var ms = new MemoryStream();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            fs.CopyTo(ms);
            try
            {
                WordprocessingDocument doc = WordprocessingDocument.Open(ms, true);
                //WordprocessingDocument docSource1 = WordprocessingDocument.Open(fileSource1, true);
                //WordprocessingDocument docSource2 = WordprocessingDocument.Open(fileSource2, true);
                //if (doc != null && docSource1 != null && docSource2 != null)
                if (doc != null)
                {
                    //Merge Filestream
                    //MainDocumentPart mainPart = doc.MainDocumentPart;
                    //mainPart.Document = new Document(new Body());

                    //foreach (string files in fileSources)
                    //{
                    //    AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML);
                    //    string altChunkId = mainPart.GetIdOfPart(chunk);

                    //    using (FileStream fileStream = File.Open(files, FileMode.Open))
                    //    {
                    //        chunk.FeedData(fileStream);
                    //    }

                    //    AltChunk altChunk = new AltChunk { Id = altChunkId };
                    //    mainPart.Document.Body.AppendChild(altChunk);
                    //}

                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    int i = 0;
                    foreach (string files in fileSources)
                    {
                        string altChunkId = "AltChunkId" + i.ToString();
                        AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                        using (FileStream fileStream = File.Open(files, FileMode.Open))
                        {
                            chunk.FeedData(fileStream);
                        }
                        AltChunk altChunk = new AltChunk();
                        altChunk.Id = altChunkId;
                        mainPart.Document.Body.AppendChild(altChunk);
                        //mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                        //mainPart.Document.Save();
                        i++;
                    }

                    //XElement newBody = XElement.Parse(doc.MainDocumentPart.Document.Body.OuterXml);
                    //XElement sourceBody1 = XElement.Parse(docSource1.MainDocumentPart.Document.Body.OuterXml);
                    //XElement sourceBody2 = XElement.Parse(docSource2.MainDocumentPart.Document.Body.OuterXml);

                    //newBody.Add(new object[] { sourceBody1, sourceBody2 });
                    //doc.MainDocumentPart.Document.Body = new Body(newBody.ToString());
                    //doc.MainDocumentPart.Document.Save();

                    //docSource1.Close();
                    //docSource2.Close();
                    doc.Save();
                }

                //Response Model
                response.status = 200;
                MailMergeModel mmModel = new MailMergeModel
                {
                    innerText = doc.MainDocumentPart.Document.Body.InnerText,
                    innerXML = doc.MainDocumentPart.Document.Body.InnerXml,
                    outerXML = doc.MainDocumentPart.Document.Body.OuterXml,
                    bytesdata = ms.ToArray()
                };

                response.data = mmModel;

                doc.Close();
                fs.Close();

                //Writefile
                ms.Seek(0, SeekOrigin.Begin);
                using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                {
                    ms.CopyTo(savefs);
                    savefs.Flush();
                }
                ms.Close();
            }
            catch (Exception ex)
            {
                fs.Close();
                ms.Close();
                throw new Exception(ex.Message + ex?.InnerException.ToString());
            }

            return response;
        }

        public ResponseModel GetMailMergeReplace()
        {
            ResponseModel response = new ResponseModel();
            string filename = $"{Directory.GetCurrentDirectory()}/template/devexmailmerge.docx"; //@"D:\MailmergeTest3.docx";
            //string filename = $"{Directory.GetCurrentDirectory()}/template/MailmergeTest3.docx"; //@"D:\MailmergeTest3.docx";
            //string csvPath = @"D:\MailMergeData.csv";
            string fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
            //string fileResult = filename.Substring(0, filename.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
            //File.Copy(filename, fileResult);
            var ms = new MemoryStream();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            fs.CopyTo(ms);
            try
            {
                //WordprocessingDocument doc = WordprocessingDocument.Create(filename, WordprocessingDocumentType.Document);
                WordprocessingDocument doc = WordprocessingDocument.Open(ms, true);
                if (doc != null)
                //using (WordprocessingDocument doc = WordprocessingDocument.Open(fileResult, true))
                {
                    //StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream());
                    //var docText = sr.ReadToEnd();
                    //using (StreamWriter sw = new StreamWriter($"{Directory.GetCurrentDirectory()}/template/Result/mailTxt.txt"))
                    //{
                    //    Body body = doc.MainDocumentPart.Document.Body;
                    //    string docText = "";
                    //    foreach (OpenXmlElement cBody in body.ChildElements)
                    //    {
                    //        docText = cBody.InnerText; //docText += cBody.InnerText + @"\r\n";
                    //        sw.WriteLine(docText);
                    //    }
                    //}
                    //var docText = body.InnerText.ToString();
                    //File.WriteAllText($"{Directory.GetCurrentDirectory()}/template/Result/mailTxt.txt", docText);

                    Body body = doc.MainDocumentPart.Document.Body;
                    string docText = "";
                    foreach (OpenXmlElement cBody in body.ChildElements)
                    {
                        docText += cBody.InnerText + @"\r\n";
                    }

                    //DocxToText dtt = new DocxToText(ms);
                    //string docxText = dtt.ExtractText();

                    //MailMerge
                    var mergeFields = doc.GetMergeFields().ToList();
                    foreach (var mergeField in mergeFields)
                    {
                        if (mergeField.InnerText.ToLower().Contains("first name"))
                        {
                            mergeField.ReplaceWithText(MailMergeData.FirstName);
                        }
                        else if (mergeField.InnerText.ToLower().Contains("last name"))
                        {
                            mergeField.ReplaceWithText(MailMergeData.LastName);
                        }
                        //mergeField.ReplaceWithText(MailMergeData.FirstName);
                    }
                    //doc.GetMergeFields("First Name").ReplaceWithText(MailMergeData.FirstName);
                    //doc.GetMergeFields("Last Name").ReplaceWithText(MailMergeData.LastName);
                    //doc.MainDocumentPart.Document.Save();
                    //Settings settings = doc.MainDocumentPart.DocumentSettingsPart.Settings;
                    //OpenXmlElement openXmlElement = null;

                    //foreach (OpenXmlElement element in settings.ChildElements)
                    //{
                    //    if (element is MailMerge mailMerge)
                    //    {
                    //        //mailMerge.ViewMergedData.Val = 0;
                    //        //mailMerge.Query.Val = "SELECT * FROM " + csvPath;
                    //        mailMerge.DataSourceObject.Remove();
                    //    }
                    //    else if (element is AttachedTemplate)
                    //    {
                    //        openXmlElement = element;
                    //    }
                    //}
                    //if (openXmlElement != null)
                    //{
                    //    openXmlElement.Remove();
                    //}
                    //doc.SaveAs(@"D:\MailMergeResultTest.docx");

                    //Response Model
                    //response.status = 200;
                    //MailMergeModel mmModel = new MailMergeModel
                    //{
                    //    innerText = doc.MainDocumentPart.Document.Body.InnerText,
                    //    innerXML = doc.MainDocumentPart.Document.Body.InnerXml,
                    //    outerXML = doc.MainDocumentPart.Document.Body.OuterXml
                    //};

                    //response.data = mmModel;

                    //Create new docx.
                    //string html = @"<div><p>&nbsp; <strong>แบบ อค./ทส.1.67</strong></p><p><strong>&nbsp; </strong><strong><u>เอกสารแนบท้ายว่าด้วยค่าใช้จ่ายในการบรรเทาความเสียหาย</u></strong></p><p><strong><u>&nbsp; (Sue and Labour)</u></strong></p><p><strong>&nbsp; </strong></p><p><strong>&nbsp; </strong>เป็นที่ตกลงว่า ถ้าข้อความใดในเอกสารนี้ขัดหรือแย้งกับข้อความที่ปรากฏในกรมธรรม์ประกันภัยนี้ ให้ใช้ข้อความตามที่ปรากฏในเอกสารนี้บังคับแทน</p><p>&nbsp; การประกันภัยนี้ได้ขยายความคุ้มครองถึงค่าใช้จ่ายอันสมควรต่างๆที่เกิดขึ้นในการใช้ความพยายามเพื่อกู้คืน ป้องกันหรือรักษาทรัพย์สินที่เอาประกันภัย เพื่อลดความเสียหายที่ได้รับความคุ้มครองหรือเพื่อดำเนินคดีในนามของผู้เอาประกันภัย ในการเรียกร้องค่าสินไหมทดแทนหรือความเสียหายจากบุคคลอื่นสำหรับความเสียหายที่เกิดขึ้น ทั้งนี้ ค่าใช้จ่ายดังกล่าวนี้จะต้องได้รับความเห็นชอบจากผู้รับประกันภัยก่อน</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; จำกัดวงเงินคุ้มครองสูงสุดไม่เกิน 100,000.00 บาท ต่ออุบัติเหตุแต่ละครั้ง และตลอดระยะเวลาเอาประกันภัย</p><p>&nbsp; ค่าใช้จ่ายนี้เป็นส่วนเพิ่มเติมจากจำนวนเงินเอาประกันภัยที่ระบุไว้ในกรมธรรม์ประกันภัย</p><p>&nbsp; <span>ส่วนเงื่อนไขและข้อความอื่นๆ ในกรมธรรม์ประกันภัยนี้คงใช้บังคับตามเดิม</span></p><p>&nbsp; </p></div><br>";
                    //var word = HtmlToWord(html);

                    //    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    //    Body body = mainPart.Document.AppendChild(new Body());
                    //    Paragraph para = body.AppendChild(new Paragraph());
                    //    Run run = para.AppendChild(new Run());

                    //    run.AppendChild(new Text("Hello world"));
                    doc.Save();

                }

                //Response Model
                response.status = 200;
                MailMergeModel mmModel = new MailMergeModel
                {
                    innerText = doc.MainDocumentPart.Document.Body.InnerText,
                    innerXML = doc.MainDocumentPart.Document.Body.InnerXml,
                    outerXML = doc.MainDocumentPart.Document.Body.OuterXml,
                    bytesdata = ms.ToArray()
                };

                response.data = mmModel;

                doc.Close();
                fs.Close();

                //Writefile
                ms.Seek(0, SeekOrigin.Begin);
                using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                {
                    ms.CopyTo(savefs);
                    savefs.Flush();
                }
                ms.Close();
            }
            catch (Exception ex)
            {
                fs.Close();
                ms.Close();
                throw new Exception(ex.Message + ex?.InnerException.ToString());
            }

            return response;
        }

        public byte[] HtmlToWord(String html)
        {
            const string filename = @"D:\testConvertHTMLtodocx.docx";
            const string filenamertf = @"D:\testConvertHTMLtodocx.rtf";
            if (File.Exists(filename)) File.Delete(filename);
            if (File.Exists(filenamertf)) File.Delete(filenamertf);

            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(
                       generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new DocumentFormat.OpenXml.Wordprocessing.Document(new Body()).Save(mainPart);
                    }

                    HtmlToOpenXml.HtmlConverter converter = new HtmlToOpenXml.HtmlConverter(mainPart);
                    Body body = mainPart.Document.Body;

                    var paragraphs = converter.Parse(html);
                    for (int i = 0; i < paragraphs.Count; i++)
                    {
                        body.Append(paragraphs[i]);
                    }

                    mainPart.Document.Save();
                }

                File.WriteAllBytes(filename, generatedDocument.ToArray());
                File.WriteAllBytes(filenamertf, generatedDocument.ToArray());

                return generatedDocument.ToArray();
            }
        }

        public ResponseModel BuildWordMerge()
        {
            var response = new ResponseModel();
            try
            {
                var filename = $"{Directory.GetCurrentDirectory()}/template/DevExTestWordMerge3.docx";
                //var filename = $"{Directory.GetCurrentDirectory()}/template/devexmailmerge.docx";
                var fileResult = $"{Directory.GetCurrentDirectory()}/template/Result/" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(filename);
                var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
                var memoryTemplate = new MemoryStream();
                fs.CopyTo(memoryTemplate);
                //fs.Close();

                var msResponse = new MemoryStream();
                var sources = new List<Source>();
                var dsSource = new DataSet();

                //Demo
                var dtSource = dsSource.Tables.Add("Table1");
                dtSource.Columns.Add("H1");
                dtSource.Columns.Add("H2");
                dtSource.Columns.Add("docbody");
                var drSource = dtSource.NewRow();
                drSource["H1"] = "Freewill Solutins";
                drSource["H2"] = "Insurances test data";
                drSource["BODY1"] = "Insurances test data bodddddd";

                //                drSource["docbody"] = @"แบบ อค./ทส.1.78
                //เอกสารแนบท้ายว่าด้วยเรื่องการแปลงสกุลเงิน
                //(Currency Conversion Clause)
                //	เป็นที่ตกลงว่า ถ้าข้อความใดในเอกสารนี้ขัดหรือแย้งกับข้อความที่ปรากฎในกรมธรรม์ประกันภัยนี้ให้ใช้ข้อความตามที่ปรากฎในเอกสารนี้บังคับแทน
                //	หากความเสียหายที่เกิดขึ้นซึ่งจะได้รับการชดใช้ค่าเสียหายโดยกรมธรรม์นี้ และเกิดขึ้นในสกุลเงินอื่นนอกเหนือจากสกุลเงินที่กำหนดไว้ในกรมธรรม์ ให้ใช้อัตราแลกเปลี่ยนเพื่อการชดใช้ค่าเสียหายตามสกุลเงินที่ระบุในกรมธรรม์ประกันภัย ณ …………………………………..หรือ ณ เวลาที่ทำการจ่ายค่าสินไหมทดแทนสำหรับความสูญหายหรือเสียหายนั้น

                //หมายเหตุ	:	ให้ระบุวันที่จะใช้อัตราแลกเปลี่ยนลงในช่องว่าง อาทิเช่น
                //			1) วันที่ตกลงการชดใช้ค่าสินไหมทดแทน
                //			2) วันที่จะชดใช้ค่าสินไหมทดแทน
                //			3) วันที่เกิดอุบัติเหตุ
                //เป็นต้น

                //	หากมิได้มีการระบุวันดังกล่าวให้ถือวันที่ตกลงการชดใช้ค่าสินไหมทดแทนเป็นวันที่จะใช้อัตราแลกเปลี่ยน
                //";
                var fileSource2 = $"{Directory.GetCurrentDirectory()}/template/Data/Clause002.docx";
                var fs2 = new FileStream(fileSource2, FileMode.Open, FileAccess.ReadWrite);
                var msSource2 = new MemoryStream();
                fs2.CopyTo(msSource2);
                //fs.Close();
                var docxBinary = msSource2.ToArray();
                drSource["docbody"] = msSource2.ToArray();
                dtSource.Rows.Add(drSource);

                for (int i = 1; i <= dsSource.Tables.Count; i++)
                {
                    var fileResultName = "merge" + i.ToString() + ".docx";
                    var dtProcess = dsSource.Tables["Table" + i.ToString()];
                    //var docxBinary = (byte[])dtProcess.Rows[0]["docbody"];//!= DBNull.Value ? (byte[])dtProcess.Rows[0]["docbody"] : null;

                    var sourceDoc = new List<Source>();
                    var msDoc = new MemoryStream();

                    memoryTemplate.Seek(0, SeekOrigin.Begin);
                    var doc = WordprocessingDocument.Open(memoryTemplate, true);
                    if (doc != null)
                    {
                        /// MailMerge replace field
                        var mergeFields = doc.GetMergeFields().ToList();

                        foreach (var mergeField in mergeFields)
                        {
                            var field = mergeField.InnerText.Replace("MERGEFIELD", "").Replace(@"\", "").Replace(@"""", "").Trim();
                            //var field = mergeField.InnerText.Replace(@" MERGEFIELD """, "").Replace(@"""\m", "").Replace("MERGEFIELD", "").Replace(@"\", "").Replace(@"""", "").Trim();
                            //if (mergeField.InnerText.Replace(@" MERGEFIELD """, "").Replace(@"""\m", "").Trim() == "docbody")
                            if (field == "docbody")
                            {
                                mergeField.Remove();
                            }
                            else
                            {
                                //var field = mergeField.InnerText.Replace(@" MERGEFIELD """, "").Replace(@"""\m", "").Trim();
                                mergeField.ReplaceWithText(dtProcess.Rows[0][field].ToString());
                            }
                        }
                    }
                    doc.MainDocumentPart.Document.Save();
                    doc.Save();
                    doc.Close();

                    /// Create new docx
                    var docX = new WmlDocument(fileResultName, memoryTemplate);
                    sourceDoc.Add(new Source(docX, false));

                    /// Merge docx
                    if (docxBinary != null)
                    {
                        var docData = new WmlDocument("data" + i.ToString(), docxBinary);
                        sourceDoc.Add(new Source(docData, false));
                    }
                    /// Build docx per page
                    DocumentBuilder.BuildDocument(sourceDoc).WriteByteArray(msDoc);

                    /// Add page docx to list
                    var docDoc = new WmlDocument(fileResultName, msDoc);
                    sources.Add(new Source(docDoc, true));

                    msDoc.Close();
                }

                /// Build merge all pages
                DocumentBuilder.BuildDocument(sources).WriteByteArray(msResponse);

                var mmModel = new MailMergeModel
                {
                    bytesdata = msResponse.ToArray()
                };

                //Writefile
                msResponse.Seek(0, SeekOrigin.Begin);
                using (FileStream savefs = new FileStream(fileResult, FileMode.OpenOrCreate))
                {
                    msResponse.CopyTo(savefs);
                    savefs.Flush();
                }
                msResponse.Close();

                response.data = mmModel;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex?.InnerException.ToString());
            }
        }
    }
}
