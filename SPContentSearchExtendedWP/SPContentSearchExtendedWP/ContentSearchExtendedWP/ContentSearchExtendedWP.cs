using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Search;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.Office.Server.Audience;
using Microsoft.SharePoint.Administration;

namespace SPContentSearchExtendedWP.ContentSearchExtendedWP
{
    [ToolboxItemAttribute(false)]
    public class ContentSearchExtendedWP : ContentBySearchWebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        public ContentSearchExtendedWP()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.AppManager != null)
            {
                if (this.AppManager.QueryGroups.ContainsKey(this.QueryGroupName) &&
                 this.AppManager.QueryGroups[this.QueryGroupName].DataProvider != null)
                {
                    this.AppManager.QueryGroups[this.QueryGroupName].DataProvider.BeforeSerializeToClient += new BeforeSerializeToClientEventHandler(UpdateQueryText);
                }
            }
            base.OnLoad(e);
        }

        private void UpdateQueryText(object sender, BeforeSerializeToClientEventArgs e)
        {
            try
            {
                DataProviderScriptWebPart dataProvider = sender as DataProviderScriptWebPart;

                string currentQueryText = dataProvider.QueryTemplate;
                string token = "TargetAudienceQuery";
                if (currentQueryText.Contains(token))
                {
                    dataProvider.QueryTemplate = currentQueryText.Replace(token, BuildTAQuery());
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("Content Search Ext Web Part", TraceSeverity.Medium, EventSeverity.Error),
                    TraceSeverity.Medium, "UpdateQueryText Failed: " + ex.Message, null);
            }
        }

        private string BuildTAQuery()
        {
            string query = string.Empty;
            string audienceGroups = string.Empty;
            try
            {
                SPSite site = new SPSite(SPContext.Current.Web.Url);
                SPServiceContext context = SPServiceContext.GetContext(site);

                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                string loginName = currentUser.ToString().Split('|')[1];

                AudienceManager audMgr = new AudienceManager(context);
                AudienceCollection audiences = audMgr.Audiences;

                for (int i = 0; i < audiences.Count; i++)
                {
                    if (audiences[i].AudienceName != "All site users")
                    {
                        if (audiences[i].IsMember(loginName))
                        {
                            //audienceGroups = audienceGroups + "," + audiences[i].AudienceID; 
                            //TargetAudience is the managed property name in the search schema
                            query += "TargetAudience:\"" + audiences[i].AudienceID + "\" OR ";
                        }
                    }
                }
                query = query.Substring(0, query.LastIndexOf(" OR "));

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("Content Search Ext Web Part", TraceSeverity.Medium, EventSeverity.Error),
                    TraceSeverity.Medium, "Audience Build Failed: " + ex.Message, null);
            }
            return query;
        }
    }
}
