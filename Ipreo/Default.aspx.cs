using Ipreo.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ipreo
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((HttpRuntime.Cache["SubwayNames"] != null && HttpRuntime.Cache["SubwayStops"] != null))
                    PopulateDropdowns();
                else
                {
                    CacheHelper.LoadSubwayInfoIntoCache();

                    if ((HttpRuntime.Cache["SubwayNames"] == null || HttpRuntime.Cache["SubwayStops"] == null))
                    {
                        string errorCode = "0";
                        string startupScript = string.Format("displayError({0});", errorCode);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayError", startupScript, true);
                        btnCalculate.Enabled = false;
                    }
                    else
                        PopulateDropdowns();
                }
            }
            else
            {
                if ((HttpRuntime.Cache["SubwayNames"] == null || HttpRuntime.Cache["SubwayStops"] == null))
                {
                    CacheHelper.LoadSubwayInfoIntoCache();
                    PopulateDropdowns();
                }
            }

            btnCalculate.Click += BtnCalculate_Click;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlStationA.SelectedValue) && !string.IsNullOrEmpty(ddlStationB.SelectedValue))
            {
                Dictionary<string, Coordinates> subwayStops = HttpRuntime.Cache.Get("SubwayStops") as Dictionary<string, Coordinates>;
                Coordinates coordinatesA = subwayStops[ddlStationA.SelectedValue];
                Coordinates coordinatesB = subwayStops[ddlStationB.SelectedValue];

                
                string startupScript = string.Format("getDistanceFromLatLonInKm({0},{1},{2},{3});", coordinatesA.Latitude.ToString(), coordinatesA.Longitude.ToString(), coordinatesB.Latitude.ToString(), coordinatesB.Longitude.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "getDistanceFromLatLonInKm", startupScript, true);
            }
            else
            {
                string errorCode = "1";
                string startupScript = string.Format("displayError({0});", errorCode);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayError", startupScript, true);
            }
        }

        private void PopulateDropdowns()
        {
            List<string> subwayNames = HttpRuntime.Cache.Get("SubwayNames") as List<string>;
            ListItemCollection itemCollection = new ListItemCollection();

            subwayNames.Sort();

            ddlStationA.Items.Add(new ListItem("- Select a Station -", string.Empty));
            ddlStationB.Items.Add(new ListItem("- Select a Station -", string.Empty));

            foreach (string s in subwayNames)
            {
                ddlStationA.Items.Add(new ListItem(s, s));
                ddlStationB.Items.Add(new ListItem(s, s));
            }
        }
    }
}