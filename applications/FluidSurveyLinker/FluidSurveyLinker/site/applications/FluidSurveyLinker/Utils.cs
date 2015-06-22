using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISL.OneWeb.NSNU.FluidSurveyLinker
{
    [Serializable()]
    public enum Mode
    {
        Link
    }

    public static class Constants
    {
        public const string MODE = "mode";
        public const string SURVEY = "survey";
        public const string LINKTEXT = "linktext";
    }
}