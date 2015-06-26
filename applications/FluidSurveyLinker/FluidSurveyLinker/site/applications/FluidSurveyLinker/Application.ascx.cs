using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OWSys = ISL.OneWeb4.Sys;
using OWWeb = ISL.OneWeb4.UI;
using OWEnts = ISL.OneWeb4.Entities;

namespace ISL.OneWeb.NSNU.FluidSurveyLinker
{
    public partial class Application : ISL.OneWeb4.UI.Applications.BaseApplication
    {
        private System.Web.UI.UserControl _app;

        #region Application Parameters

        [OWWeb.Applications.ApplicationMode("Select the mode of the application to place.", "Mode")]
        public Mode Mode
        {
            get
            {
                if (this.ViewState[Constants.MODE] == null) {
                    this.ViewState[Constants.MODE] = Mode.Link;
                }
                    
                return (Mode)this.ViewState[Constants.MODE];
            }

            set
            {
                if (Enum.IsDefined(typeof(Mode), value)) {
                    this.ViewState[Constants.MODE] = value;

                }
                else {
                    this.ViewState[Constants.MODE] = Mode.Link;

                }
            }
        }

        [OWWeb.Applications.ApplicationParameter("Enter the ID of the survey to generate links for.", "Survey ID")]
        [OWWeb.Applications.ApplicableModes((int)Mode.Link)]
        public string Survey
        {
            get
            {
                if (this.ViewState[Constants.SURVEY] == null)
                    return String.Empty;
                return (String)this.ViewState[Constants.SURVEY];
            }
            set
            {
                this.ViewState[Constants.SURVEY] = value;
            }
        }

        [OWWeb.Applications.ApplicationParameter("Enter the text to use for the survey link.", "Link Text")]
        [OWWeb.Applications.ApplicableModes((int)Mode.Link)]
        public string LinkText
        {
            get
            {
                if (this.ViewState[Constants.LINKTEXT] == null)
                    return String.Empty;
                return (String)this.ViewState[Constants.LINKTEXT];
            }
            set
            {
                this.ViewState[Constants.LINKTEXT] = value;
            }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            switch (this.Mode)
            {
                case Mode.Link:
                    _app = this.LoadControl("SurveyLink.ascx") as System.Web.UI.UserControl;
                    break;
                default:
                    _app = this.LoadControl("SurveyLink.ascx") as System.Web.UI.UserControl;
                    break;
            }

            if (_app != null)
                this.Controls.Add(_app);
        }

    }
}