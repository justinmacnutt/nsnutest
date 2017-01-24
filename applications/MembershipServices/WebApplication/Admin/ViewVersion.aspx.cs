using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nsnu.DataAccess;
using Nsnu.DataAccess.Enumerations;
using Nsnu.MembershipServices;
using System.Xml;
using System.Text;

using WebApplication.Utilities;
using CommitteeEnum = Nsnu.DataAccess.Enumerations.Committee;
using PositionEnum = Nsnu.DataAccess.Enumerations.CommitteePosition;
using FacilityTypeEnum = Nsnu.DataAccess.Enumerations.FacilityType;
using RegionEnum = Nsnu.DataAccess.Enumerations.Region;
using DistrictEnum = Nsnu.DataAccess.Enumerations.District;
using System.Web.Script.Serialization;



namespace WebApplication.Admin
{
    public partial class ViewVersion : System.Web.UI.Page
    {

        private MembershipBs _mBs = new MembershipBs();



        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["id"] != null)
            {
                var versionid = int.Parse(Request.QueryString["id"]);
                VersionHistory version = _mBs.GetVersion(versionid);

                if (version != null)
                {
                    string s = version.versionXml;
                    XmlReader reader;
                    XmlReaderSettings rs = new XmlReaderSettings();
                    StringBuilder sb = new StringBuilder();
                    rs.CloseInput = true;
                    rs.IgnoreComments = true;
                    rs.ConformanceLevel = ConformanceLevel.Fragment;

                    reader = XmlReader.Create(new System.IO.StringReader(s), rs);

                    // Create the XslCompiledTransform and load the stylesheet.
                    System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
                    String path = Server.MapPath(ResolveUrl("Version.xslt"));
                    xslt.Load(path);

                    // Create the XsltArgumentList.
                    System.Xml.Xsl.XsltArgumentList xslArg = new System.Xml.Xsl.XsltArgumentList();
   
                    xslArg.AddParam("modificationdate", "", version.modificationDate.ToString());
                    xslArg.AddParam("versionid", "", version.id.ToString());
                    xslArg.AddParam("modifiedby", "", version.modifiedBy);

                    System.IO.TextWriter tw = new System.IO.StringWriter(sb);

                    //Transform the version xml.
                    xslt.Transform(reader, xslArg, tw);
                    litVersionData.Text = sb.ToString();

                }

            }

        }

    }
}