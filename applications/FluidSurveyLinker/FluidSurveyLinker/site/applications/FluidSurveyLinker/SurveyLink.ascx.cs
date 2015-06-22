using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data.Linq;

namespace ISL.OneWeb.NSNU.FluidSurveyLinker
{
    public partial class SurveyLink : System.Web.UI.UserControl
    {
        protected Application _main;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Parent is Application)
            {
                _main = (Application)Parent;
            }

            if (_main == null)
            {
                return;
            }

            var _user = 0;

            try
            {
                _user = ISL.OneWeb4.UI.Components.Membership.MembershipManager.CurrentMember.MemberId;
            }
            catch (Exception ex)
            {
                hl_survey.Visible = false;
                return;
            }

            if (_user < 1) {
                hl_survey.Visible = false;
                return;
            }

            var FSLTA = new EntitiesTableAdapters.FluidSurveyLinkerTableAdapter();

            var links = FSLTA.GetDataByIds(_user, _main.Survey);

            if (links != null && links.Rows.Count > 0)
            {
                hl_survey.NavigateUrl = (string)links.Rows[0]["Link"];
                return;
            }

            try
            {
                var apiUrl = Properties.Settings.Default.BaseUrl + "/api/v2/surveys/" + _main.Survey + "/invite-codes/";
                var credentials = Properties.Settings.Default.ApiKey + ":" + Properties.Settings.Default.Password;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
                httpWebRequest.PreAuthenticate = true;

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                var postData = "count=1";
                var data = Encoding.ASCII.GetBytes(postData);

                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = data.Length;

                using (var stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var responseString = streamReader.ReadToEnd();
                    dynamic response = JsonConvert.DeserializeObject(responseString);
                    string inviteUrl = response.invites[0].invite_url;
                    hl_survey.NavigateUrl = inviteUrl;

                    FSLTA.Insert(_user, _main.Survey, inviteUrl);
                }
            }
            catch (Exception ex)
            {
                hl_survey.Visible = false;
            }
            
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}

