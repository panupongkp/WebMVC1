using Newtonsoft.Json;
using RtfPipe;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebMVC1.Models;

namespace WebMVC1.Services
{
    public class ConvertService : IConvertService
    {
        public ResponseModel ConvertHTMLtoRTF(InputRequestModel html)
        {
            ResponseModel response = new ResponseModel();
            string input = html.input;
            if (!String.IsNullOrEmpty(input))
            {
                var rtf = Lib_ConvertHTMLToRTF(input);
                response.status = 200;
                response.success = true;
                response.data = rtf;
                response.message = "success";
            }
            else
            {
                response.status = 204;
                response.success = false;
                response.message = "Not input data.";
            }

            return response;
        }

        public ResponseModel ConvertRTFtoHTML(InputRequestModel rtf)
        {
            ResponseModel response = new ResponseModel();
            string input = rtf.input;
            if (!String.IsNullOrEmpty(input))
            {
                var html = Lib_ConvertRTFToHTML(input);
                response.status = 200;
                response.success = true;
                response.data = html;
                response.message = "success";
            }
            else
            {
                response.status = 204;
                response.success = false;
                response.message = "Not input data.";
            }
            return response;
        }

        public ResponseModel ShortenURL(string url)
        {
            ResponseModel response = new ResponseModel();
            if (!string.IsNullOrEmpty(url))
            {
                response.status = 200;
                response.success = true;
                response.message = "Success";
                response.data = GetShortUrl(url);
            }
            return response;
        }

        private string GetShortUrl(string longUrl)
        {
            return "";
        }

        private string Lib_ConvertHTMLToRTF(string html)
        {
            var rtf = "";
            //Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            //html = regex.Replace(html, string.Empty);
            //Byte[] bytes = Convert.FromBase64String(html);
            //var htmltext = Encoding.UTF8.GetString(bytes);
            //html = @"<div style="font - size:12pt; font - family:&quot; DB ThaiText X & quot;, serif; "><p style="margin: 0; ">&amp;nbsp; <strong style="font - size:14pt; ">แบบ อค./ทส.1.67</strong></p><p style="text - align:right; font - size:14pt; margin: 0; "><strong>&amp;nbsp; </strong><strong><u>เอกสารแนบท้ายว่าด้วยค่าใช้จ่ายในการบรรเทาความเสียหาย</u></strong></p><p style="font - size:14pt; text - align:justify; margin: 0; "><strong><u>&amp;nbsp; (Sue and Labour)</u></strong></p><p style="font - size:14pt; text - align:justify; margin: 0; "><strong>&amp;nbsp; </strong></p><p style="font - size:14pt; text - align:justify; margin: 0; "><strong>&amp;nbsp; <span style="display: inline - block; width: 48px"></span></strong>เป็นที่ตกลงว่า ถ้าข้อความใดในเอกสารนี้ขัดหรือแย้งกับข้อความที่ปรากฏในกรมธรรม์ประกันภัยนี้ ให้ใช้ข้อความตามที่ปรากฏในเอกสารนี้บังคับแทน</p><p style="font - size:14pt; text - align:justify; margin: 0; ">&amp;nbsp; <span style="display: inline - block; width: 48px"></span>การประกันภัยนี้ได้ขยายความคุ้มครองถึงค่าใช้จ่ายอันสมควรต่างๆที่เกิดขึ้นในการใช้ความพยายามเพื่อกู้คืน ป้องกันหรือรักษาทรัพย์สินที่เอาประกันภัย เพื่อลดความเสียหายที่ได้รับความคุ้มครองหรือเพื่อดำเนินคดีในนามของผู้เอาประกันภัย ในการเรียกร้องค่าสินไหมทดแทนหรือความเสียหายจากบุคคลอื่นสำหรับความเสียหายที่เกิดขึ้น ทั้งนี้ ค่าใช้จ่ายดังกล่าวนี้จะต้องได้รับความเห็นชอบจากผู้รับประกันภัยก่อน</p><p style="font - size:14pt; text - align:justify; margin: 0; ">&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; จำกัดวงเงินคุ้มครองสูงสุดไม่เกิน 100,000.00 บาท ต่ออุบัติเหตุแต่ละครั้ง และตลอดระยะเวลาเอาประกันภัย</p><p style="font - size:14pt; text - align:justify; margin: 0; ">&amp;nbsp; <span style="display: inline - block; width: 48px"></span>ค่าใช้จ่ายนี้เป็นส่วนเพิ่มเติมจากจำนวนเงินเอาประกันภัยที่ระบุไว้ในกรมธรรม์ประกันภัย</p><p style="font - size:14pt; text - align:justify; margin: 0; ">&amp;nbsp; <span style="display: inline - block; width: 48px"></span><span style="font - family:&quot; DB ThaiText X & quot; ; ">ส่วนเงื่อนไขและข้อความอื่นๆ ในกรมธรรม์ประกันภัยนี้คงใช้บังคับตามเดิม</span></p><p style="font - size:14pt; text - align:justify; margin: 0; ">&amp;nbsp; </p></div>"
            SautinSoft.HtmlToRtf h = new SautinSoft.HtmlToRtf();
            try
            {
                rtf = h.ConvertString(html);
            }
            catch (Exception ex)
            {
                rtf = ex.Message + ex?.InnerException?.Message;
            }

            return rtf;
        }

        private string Lib_ConvertRTFToHTML(string rtf)
        {
            rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033\fs24{\fonttbl{\f0\fnil\fcharset0 Times New Roman;}{\f1\fnil\fcharset0 Arial;}{\f2\fnil\fcharset0 Courier New;}{\f3\fnil\fcharset2 Symbol;}{\f4\fnil\fcharset0 Helvetica;}{\f5\fnil\fcharset0 Courier;}{\f10\fnil\fcharset2 Wingdings;}{\f11\fnil\fcharset0 MS Mincho;}{\f13\fnil\fcharset0 SimSun;}{\f21\fnil\fcharset0 Century;}{\f34\fnil\fcharset0 Cambria Math;}{\f36\fnil\fcharset0 Cambria;}{\f37\fnil\fcharset0 Calibri;}}
            {\colortbl ;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;\red132\green95\blue145;\red79\green129\blue189;\red36\green63\blue96;\red23\green54\blue93;\red192\green128\blue77;\red75\green172\blue198;\red192\green0\blue0;\red255\green192\blue0;\red146\green208\blue80;\red0\green176\blue128;\red0\green176\blue240;\red0\green112\blue192;\red0\green32\blue96;\red112\green48\blue160;\red84\green141\blue212;\red227\green108\blue10;\red95\green73\blue122;\red118\green146\blue60;\red141\green179\blue226;\red148\green138\blue84;\red229\green184\blue183;\red89\green89\blue89;}
            {\stylesheet {\s1 heading 1;}{\s2 heading 2;}{\s3 heading 3;}{\s4 heading 4;}{\s5 heading 5;}{\s6 heading 6;}}
            {\info{\doccomm Created by the \\'abHTML to RTF .Net\\'bb 7.4.4.30}}
            \paperw12241\paperh15841\margl567\margr567\margt283\margb567\viewkind1\viewscale100\viewzk2

            \pard\fs24\lang1033\f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \b \u3649?\u3610?\u3610? \u3629?\u3588?./\u3607?\u3626?.1.67\par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \ul \u3648?\u3629?\u3585?\u3626?\u3634?\u3619?\u3649?\u3609?\u3610?\u3607?\u3657?\u3634?\u3618?\u3623?\u3656?\u3634?\u3604?\u3657?\u3623?\u3618?\u3588?\u3656?\u3634?\u3651?\u3594?\u3657?\u3592?\u3656?\u3634?\u3618?\u3651?\u3609?\u3585?\u3634?\u3619?\u3610?\u3619?\u3619?\u3648?\u3607?\u3634?\u3588?\u3623?\u3634?\u3617?\u3648?\u3626?\u3637?\u3618?\u3627?\u3634?\u3618?\par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; (Sue and Labour)\par
            \f0\fs24\cf0\ul0\ltrpar\ql\sb241\sa241 &nbsp; \par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \b0 \u3648?\u3611?\u3655?\u3609?\u3607?\u3637?\u3656?\u3605?\u3585?\u3621?\u3591?\u3623?\u3656?\u3634? \u3606?\u3657?\u3634?\u3586?\u3657?\u3629?\u3588?\u3623?\u3634?\u3617?\u3651?\u3604?\u3651?\u3609?\u3648?\u3629?\u3585?\u3626?\u3634?\u3619?\u3609?\u3637?\u3657?\u3586?\u3633?\u3604?\u3627?\u3619?\u3639?\u3629?\u3649?\u3618?\u3657?\u3591?\u3585?\u3633?\u3610?\u3586?\u3657?\u3629?\u3588?\u3623?\u3634?\u3617?\u3607?\u3637?\u3656?\u3611?\u3619?\u3634?\u3585?\u3599?\u3651?\u3609?\u3585?\u3619?\u3617?\u3608?\u3619?\u3619?\u3617?\u3660?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\u3609?\u3637?\u3657? \u3651?\u3627?\u3657?\u3651?\u3594?\u3657?\u3586?\u3657?\u3629?\u3588?\u3623?\u3634?\u3617?\u3605?\u3634?\u3617?\u3607?\u3637?\u3656?\u3611?\u3619?\u3634?\u3585?\u3599?\u3651?\u3609?\u3648?\u3629?\u3585?\u3626?\u3634?\u3619?\u3609?\u3637?\u3657?\u3610?\u3633?\u3591?\u3588?\u3633?\u3610?\u3649?\u3607?\u3609?\par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \u3585?\u3634?\u3619?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\u3609?\u3637?\u3657?\u3652?\u3604?\u3657?\u3586?\u3618?\u3634?\u3618?\u3588?\u3623?\u3634?\u3617?\u3588?\u3640?\u3657?\u3617?\u3588?\u3619?\u3629?\u3591?\u3606?\u3638?\u3591?\u3588?\u3656?\u3634?\u3651?\u3594?\u3657?\u3592?\u3656?\u3634?\u3618?\u3629?\u3633?\u3609?\u3626?\u3617?\u3588?\u3623?\u3619?\u3605?\u3656?\u3634?\u3591?\u3654?\u3607?\u3637?\u3656?\u3648?\u3585?\u3636?\u3604?\u3586?\u3638?\u3657?\u3609?\u3651?\u3609?\u3585?\u3634?\u3619?\u3651?\u3594?\u3657?\u3588?\u3623?\u3634?\u3617?\u3614?\u3618?\u3634?\u3618?\u3634?\u3617?\u3648?\u3614?\u3639?\u3656?\u3629?\u3585?\u3641?\u3657?\u3588?\u3639?\u3609? \u3611?\u3657?\u3629?\u3591?\u3585?\u3633?\u3609?\u3627?\u3619?\u3639?\u3629?\u3619?\u3633?\u3585?\u3625?\u3634?\u3607?\u3619?\u3633?\u3614?\u3618?\u3660?\u3626?\u3636?\u3609?\u3607?\u3637?\u3656?\u3648?\u3629?\u3634?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618? \u3648?\u3614?\u3639?\u3656?\u3629?\u3621?\u3604?\u3588?\u3623?\u3634?\u3617?\u3648?\u3626?\u3637?\u3618?\u3627?\u3634?\u3618?\u3607?\u3637?\u3656?\u3652?\u3604?\u3657?\u3619?\u3633?\u3610?\u3588?\u3623?\u3634?\u3617?\u3588?\u3640?\u3657?\u3617?\u3588?\u3619?\u3629?\u3591?\u3627?\u3619?\u3639?\u3629?\u3648?\u3614?\u3639?\u3656?\u3629?\u3604?\u3635?\u3648?\u3609?\u3636?\u3609?\u3588?\u3604?\u3637?\u3651?\u3609?\u3609?\u3634?\u3617?\u3586?\u3629?\u3591?\u3612?\u3641?\u3657?\u3648?\u3629?\u3634?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618? \u3651?\u3609?\u3585?\u3634?\u3619?\u3648?\u3619?\u3637?\u3618?\u3585?\u3619?\u3657?\u3629?\u3591?\u3588?\u3656?\u3634?\u3626?\u3636?\u3609?\u3652?\u3627?\u3617?\u3607?\u3604?\u3649?\u3607?\u3609?\u3627?\u3619?\u3639?\u3629?\u3588?\u3623?\u3634?\u3617?\u3648?\u3626?\u3637?\u3618?\u3627?\u3634?\u3618?\u3592?\u3634?\u3585?\u3610?\u3640?\u3588?\u3588?\u3621?\u3629?\u3639?\u3656?\u3609?\u3626?\u3635?\u3627?\u3619?\u3633?\u3610?\u3588?\u3623?\u3634?\u3617?\u3648?\u3626?\u3637?\u3618?\u3627?\u3634?\u3618?\u3607?\u3637?\u3656?\u3648?\u3585?\u3636?\u3604?\u3586?\u3638?\u3657?\u3609? \u3607?\u3633?\u3657?\u3591?\u3609?\u3637?\u3657? \u3588?\u3656?\u3634?\u3651?\u3594?\u3657?\u3592?\u3656?\u3634?\u3618?\u3604?\u3633?\u3591?\u3585?\u3621?\u3656?\u3634?\u3623?\u3609?\u3637?\u3657?\u3592?\u3632?\u3605?\u3657?\u3629?\u3591?\u3652?\u3604?\u3657?\u3619?\u3633?\u3610?\u3588?\u3623?\u3634?\u3617?\u3648?\u3627?\u3655?\u3609?\u3594?\u3629?\u3610?\u3592?\u3634?\u3585?\u3612?\u3641?\u3657?\u3619?\u3633?\u3610?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\u3585?\u3656?\u3629?\u3609?\par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; \u3592?\u3635?\u3585?\u3633?\u3604?\u3623?\u3591?\u3648?\u3591?\u3636?\u3609?\u3588?\u3640?\u3657?\u3617?\u3588?\u3619?\u3629?\u3591?\u3626?\u3641?\u3591?\u3626?\u3640?\u3604?\u3652?\u3617?\u3656?\u3648?\u3585?\u3636?\u3609? 100,000.00 \u3610?\u3634?\u3607? \u3605?\u3656?\u3629?\u3629?\u3640?\u3610?\u3633?\u3605?\u3636?\u3648?\u3627?\u3605?\u3640?\u3649?\u3605?\u3656?\u3621?\u3632?\u3588?\u3619?\u3633?\u3657?\u3591? \u3649?\u3621?\u3632?\u3605?\u3621?\u3629?\u3604?\u3619?\u3632?\u3618?\u3632?\u3648?\u3623?\u3621?\u3634?\u3648?\u3629?\u3634?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \u3588?\u3656?\u3634?\u3651?\u3594?\u3657?\u3592?\u3656?\u3634?\u3618?\u3609?\u3637?\u3657?\u3648?\u3611?\u3655?\u3609?\u3626?\u3656?\u3623?\u3609?\u3648?\u3614?\u3636?\u3656?\u3617?\u3648?\u3605?\u3636?\u3617?\u3592?\u3634?\u3585?\u3592?\u3635?\u3609?\u3623?\u3609?\u3648?\u3591?\u3636?\u3609?\u3648?\u3629?\u3634?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\u3607?\u3637?\u3656?\u3619?\u3632?\u3610?\u3640?\u3652?\u3623?\u3657?\u3651?\u3609?\u3585?\u3619?\u3617?\u3608?\u3619?\u3619?\u3617?\u3660?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\par
            \f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \u3626?\u3656?\u3623?\u3609?\u3648?\u3591?\u3639?\u3656?\u3629?\u3609?\u3652?\u3586?\u3649?\u3621?\u3632?\u3586?\u3657?\u3629?\u3588?\u3623?\u3634?\u3617?\u3629?\u3639?\u3656?\u3609?\u3654? \u3651?\u3609?\u3585?\u3619?\u3617?\u3608?\u3619?\u3619?\u3617?\u3660?\u3611?\u3619?\u3632?\u3585?\u3633?\u3609?\u3616?\u3633?\u3618?\u3609?\u3637?\u3657?\u3588?\u3591?\u3651?\u3594?\u3657?\u3610?\u3633?\u3591?\u3588?\u3633?\u3610?\u3605?\u3634?\u3617?\u3648?\u3604?\u3636?\u3617?\par
            //\f0\fs24\cf0\ltrpar\ql\sb241\sa241 &nbsp; \par}";
            //rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033\fs24{\fonttbl{\f0\fnil\fcharset0 Times New Roman;}{\f1\fnil\fcharset0 Arial;}{\f2\fnil\fcharset0 Courier New;}{\f3\fnil\fcharset2 Symbol;}{\f4\fnil\fcharset0 Helvetica;}{\f5\fnil\fcharset0 Courier;}{\f10\fnil\fcharset2 Wingdings;}{\f11\fnil\fcharset0 MS Mincho;}{\f13\fnil\fcharset0 SimSun;}{\f21\fnil\fcharset0 Century;}{\f34\fnil\fcharset0 Cambria Math;}{\f36\fnil\fcharset0 Cambria;}{\f37\fnil\fcharset0 Calibri;}{\f38\fnil\fcharset222 DB ThaiText X Bd;}}{\colortbl ;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;\red132\green95\blue145;\red79\green129\blue189;\red36\green63\blue96;\red23\green54\blue93;\red192\green128\blue77;\red75\green172\blue198;\red192\green0\blue0;\red255\green192\blue0;\red146\green208\blue80;\red0\green176\blue128;\red0\green176\blue240;\red0\green112\blue192;\red0\green32\blue96;\red112\green48\blue160;\red84\green141\blue212;\red227\green108\blue10;\red95\green73\blue122;\red118\green146\blue60;\red141\green179\blue226;\red148\green138\blue84;\red229\green184\blue183;\red89\green89\blue89;}{\stylesheet {\s1 heading 1;}{\s2 heading 2;}{\s3 heading 3;}{\s4 heading 4;}{\s5 heading 5;}{\s6 heading 6;}}\paperw12241\paperh15841\margl567\margr567\margt283\margb567\viewkind1\viewscale100\viewzk2\pard\fs24\lang1033\f38\fs45\cf0\b\i\ul\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par\f38\fs45\cf0\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par\f38\fs45\cf0\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par\f38\fs45\cf0\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par}{\rtf1\ansi\ansicpg1252\deff0\deflang1033\fs24{\fonttbl{\f0\fnil\fcharset0 Times New Roman;}{\f1\fnil\fcharset0 Arial;}{\f2\fnil\fcharset0 Courier New;}{\f3\fnil\fcharset2 Symbol;}{\f4\fnil\fcharset0 Helvetica;}{\f5\fnil\fcharset0 Courier;}{\f10\fnil\fcharset2 Wingdings;}{\f11\fnil\fcharset0 MS Mincho;}{\f13\fnil\fcharset0 SimSun;}{\f21\fnil\fcharset0 Century;}{\f34\fnil\fcharset0 Cambria Math;}{\f36\fnil\fcharset0 Cambria;}{\f37\fnil\fcharset0 Calibri;}{\f38\fnil\fcharset222 DB ThaiText X Bd;}}{\colortbl ;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;\red132\green95\blue145;\red79\green129\blue189;\red36\green63\blue96;\red23\green54\blue93;\red192\green128\blue77;\red75\green172\blue198;\red192\green0\blue0;\red255\green192\blue0;\red146\green208\blue80;\red0\green176\blue128;\red0\green176\blue240;\red0\green112\blue192;\red0\green32\blue96;\red112\green48\blue160;\red84\green141\blue212;\red227\green108\blue10;\red95\green73\blue122;\red118\green146\blue60;\red141\green179\blue226;\red148\green138\blue84;\red229\green184\blue183;\red89\green89\blue89;}{\stylesheet {\s1 heading 1;}{\s2 heading 2;}{\s3 heading 3;}{\s4 heading 4;}{\s5 heading 5;}{\s6 heading 6;}}\paperw12241\paperh15841\margl567\margr567\margt283\margb567\viewkind1\viewscale100\viewzk2\pard\fs24\lang1033\f38\fs45\cf0\b\i\ul\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par\f38\fs45\cf0\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par\f38\fs45\cf0\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par\f38\fs45\cf0\ltrpar\ql\sb241\sa241 \u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\u3615?\par}";
            //var iso = Encoding.GetEncoding("ISO-8859-1");
            //rtf = rtf.Replace("/", "").Replace("=", "") + "=";
            //Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            //rtf = regex.Replace(rtf, string.Empty);
            //Byte[] bytes = Convert.FromBase64String(rtf);
            //var rtftext = Encoding.UTF8.GetString(bytes);
            //var rtftext = Encoding.UTF8.GetString(bytes);

            string html = "";
            try
            {
                //html = Rtf.ToHtml(rtf);
                SautinSoft.RtfToHtml r = new SautinSoft.RtfToHtml();
                html = r.ConvertString(rtf);
                html = html.Replace("amp;", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex?.InnerException?.Message);
            }

            return html;
        }
    }
}
