using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace OpenXmlHelpers.Word
{
    /// <summary>
    /// Provides extension methods for working with OpenXml document, particularly Word.
    /// </summary>
    public static class OpenXmlWordHelpers
    {
        /// <summary>
        /// Gets merge fields contained in a document, including the header and footer sections.
        /// </summary>
        /// <param name="mergeFieldName">Optional name for the merge fields to look for.</param>
        /// <returns>If a merge field name is specified, only merge fields with that name are returned. Otherwise, it returns all merge fields contained in the document.</returns>
        public static IEnumerable<FieldCode> GetMergeFields(this WordprocessingDocument doc, string mergeFieldName = null)
        {
            if (doc == null)
                return null;

            List<FieldCode> mergeFields = doc.MainDocumentPart.RootElement.Descendants<FieldCode>().ToList();
            foreach (var header in doc.MainDocumentPart.HeaderParts)
            {
                mergeFields.AddRange(header.RootElement.Descendants<FieldCode>());
            }
            foreach (var footer in doc.MainDocumentPart.FooterParts)
            {
                mergeFields.AddRange(footer.RootElement.Descendants<FieldCode>());
            }

            if (!string.IsNullOrWhiteSpace(mergeFieldName) && mergeFields != null && mergeFields.Count() > 0)
                return mergeFields.WhereNameIs(mergeFieldName);

            return mergeFields;
        }

        /// <summary>
        /// Gets merge fields contained in the given element.
        /// </summary>
        /// <param name="mergeFieldName">Optional name for the merge fields to look for..</param>
        /// <returns>If a merge field name is specified, only merge fields with that name are returned. Otherwise, it returns all merge fields contained in the given element.</returns>
        public static IEnumerable<FieldCode> GetMergeFields(this OpenXmlElement xmlElement, string mergeFieldName = null)
        {
            if (xmlElement == null)
                return null;

            if (string.IsNullOrWhiteSpace(mergeFieldName))
                return xmlElement
                    .Descendants<FieldCode>();

            return xmlElement
                .Descendants<FieldCode>()
                .Where(f => f.InnerText.StartsWith(GetMergeFieldStartString(mergeFieldName)));
        }

        /// <summary>
        /// Filters merge fields by the given name.
        /// </summary>
        /// <param name="mergeFieldName">The merge field name.</param>
        /// <returns>Returns all merge fields with the given name. If the merge field name is null or blank, it returns nothing.</returns>
        public static IEnumerable<FieldCode> WhereNameIs(this IEnumerable<FieldCode> mergeFields, string mergeFieldName)
        {
            if (mergeFields == null || mergeFields.Count() == 0)
                return null;

            return mergeFields
                .Where(f => f.InnerText.StartsWith(GetMergeFieldStartString(mergeFieldName)));
        }

        /// <summary>
        /// Gets the immediate containing paragraph of a given element.
        /// </summary>
        /// <returns>If the given element is a paragraph, that element is returned. Otherwise, it returns the immediate ancestor that is a paragraph, or null if none is found.</returns>
        public static Paragraph GetParagraph(this OpenXmlElement xmlElement)
        {
            if (xmlElement == null)
                return null;

            Paragraph paragraph = null;
            if (xmlElement is Paragraph)
                paragraph = (Paragraph)xmlElement;
            else if (xmlElement.Parent is Paragraph)
                paragraph = (Paragraph)xmlElement.Parent;
            else
                paragraph = xmlElement.Ancestors<Paragraph>().FirstOrDefault();
            return paragraph;
        }

        /// <summary>
        /// Removes a merge field from the containing document and replaces it with the given text content.
        /// </summary>
        /// <param name="replacementText">The content to replace the merge field with.</param>
        public static void ReplaceWithText(this FieldCode field, string replacementText)
        {
            if (field == null)
                return;

            Run rFldCode = field.Parent as Run;
            Run rBegin = rFldCode.PreviousSibling<Run>();
            Run rSep = rFldCode.NextSibling<Run>();
            Run rText = rSep.NextSibling<Run>();
            Run rEnd = rText.NextSibling<Run>();

            rFldCode.Remove();
            rBegin.Remove();
            rSep.Remove();
            rEnd?.Remove();

            Text t = rText.GetFirstChild<Text>();
            if (t != null)
            {
                t.Text = (replacementText != null) ? replacementText : string.Empty;
            }
        }

        /// <summary>
        /// Removes the merge fields from the containing document and replaces them with the given text content.
        /// </summary>
        /// <param name="replacementText">The content to replace the merge field with.</param>
        public static void ReplaceWithText(this IEnumerable<FieldCode> fields, string replacementText)
        {
            if (fields == null || fields.Count() == 0)
                return;

            foreach (var field in fields)
            {
                field.ReplaceWithText(replacementText);
            }
        }

        /// <summary>
        /// Removes the merge fields from the containing document and replaces them with the given texts.
        /// </summary>
        /// <param name="replacementTexts">The text values to replace the merge fields with</param>
        /// <param name="removeExcess">Optional value to indicate that excess merge fields are removes instead of replacing with blank values</param>
        public static void ReplaceWithText(this IEnumerable<FieldCode> fields, IEnumerable<string> replacementTexts, bool removeExcess = false)
        {
            if (fields == null || fields.Count() == 0)
                return;

            int replacementCount = replacementTexts.Count();
            int index = 0;
            foreach (var field in fields)
            {
                if (index < replacementCount)
                    field.ReplaceWithText(replacementTexts.ElementAt(index));
                else if (removeExcess)
                    field.GetParagraph().Remove();
                else
                    field.ReplaceWithText(string.Empty);

                index++;
            }
        }

        #region Private Methods

        private static string GetMergeFieldStartString(string mergeFieldName)
        {
            return " MERGEFIELD  " + (!string.IsNullOrWhiteSpace(mergeFieldName) ? mergeFieldName : "<NoNameMergeField>");
        }

        #endregion Private Methods

    }

    public class DocxToText
    {
        private const string ContentTypeNamespace =
            @"http://schemas.openxmlformats.org/package/2006/content-types";

        private const string WordprocessingMlNamespace =
            @"http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        private const string DocumentXmlXPath = @"/t:Types/t:Override[@ContentType=""application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml\""]";

        private const string BodyXPath = "/w:document/w:body";

        private MemoryStream docxFile = null;
        private string docxFileLocation = "";

        public DocxToText(MemoryStream fileName)
        {
            docxFile = fileName;
        }

        #region ExtractText()
        ///
        /// Extracts text from the Docx file.
        ///
        /// Extracted text.
        public string ExtractText()
        {
            //if (docxFile != null)
            //throw new Exception("Input file not specified.");

            // Usually it is "/word/document.xml"

            docxFileLocation = FindDocumentXmlLocation();

            if (string.IsNullOrEmpty(docxFileLocation))
                throw new Exception("It is not a valid Docx file.");

            return ReadDocumentXml();
        }
        #endregion

        #region FindDocumentXmlLocation()
        ///
        /// Gets location of the "document.xml" zip entry.
        ///
        /// Location of the "document.xml".
        private string FindDocumentXmlLocation()
        {
            MemoryStream contentTypes = docxFile;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(contentTypes);
            contentTypes.Close();

            //Create an XmlNamespaceManager for resolving namespaces

            XmlNamespaceManager nsmgr =
                new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("t", ContentTypeNamespace);

            // Find location of "document.xml"

            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(
                DocumentXmlXPath, nsmgr);

            if (node != null)
            {
                string location =
                    ((XmlElement)node).GetAttribute("PartName");
                return location.TrimStart(new char[] { '/' });
            }

            return null;
        }
        #endregion

        #region ReadDocumentXml()
        ///
        /// Reads "document.xml" zip entry.
        ///
        /// Text containing in the document.
        private string ReadDocumentXml()
        {
            StringBuilder sb = new StringBuilder();

            MemoryStream documentXml = docxFile;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(documentXml);
            documentXml.Close();

            XmlNamespaceManager nsmgr =
                new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("w", WordprocessingMlNamespace);

            XmlNode node =
                xmlDoc.DocumentElement.SelectSingleNode(BodyXPath, nsmgr);

            if (node == null)
                return string.Empty;

            sb.Append(ReadNode(node));

            return sb.ToString();
        }
        #endregion

        #region ReadNode()
        ///
        /// Reads content of the node and its nested childs.
        ///
        /// XmlNode.
        /// Text containing in the node.
        private string ReadNode(XmlNode node)
        {
            if (node == null || node.NodeType != XmlNodeType.Element)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                switch (child.LocalName)
                {
                    case "t":                           // Text
                        sb.Append(child.InnerText.TrimEnd());

                        string space =
                            ((XmlElement)child).GetAttribute("xml:space");
                        if (!string.IsNullOrEmpty(space) &&
                            space == "preserve")
                            sb.Append(' ');

                        break;

                    case "cr":                          // Carriage return
                    case "br":                          // Page break
                        sb.Append(Environment.NewLine);
                        break;

                    case "tab":                         // Tab
                        sb.Append("\t");
                        break;

                    case "p":                           // Paragraph
                        sb.Append(ReadNode(child));
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        break;

                    default:
                        sb.Append(ReadNode(child));
                        break;
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}