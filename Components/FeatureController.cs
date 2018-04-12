/*
' Copyright (c) 2017 virtualforce.io
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search.Entities;

namespace VF.ANB.Modules.Components
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for VF_POS_RequestForm
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class FeatureController : ModuleSearchBase, IPortable, IUpgradeable
    {
        // feel free to remove any interfaces that you don't wish to use
        // (requires that you also update the .dnn manifest file)

        #region Optional Interfaces

        /// <summary>
        /// Gets the modified search documents for the DNN search engine indexer.
        /// </summary>
        /// <param name="moduleInfo">The module information.</param>
        /// <param name="beginDate">The begin date.</param>
        /// <returns></returns>
        public override IList<SearchDocument> GetModifiedSearchDocuments(ModuleInfo moduleInfo, DateTime beginDate)
        {
            var searchDocuments = new List<SearchDocument>();
            var controller = new ItemController();
            var items = controller.GetItems(moduleInfo.ModuleID);
            if (items == null)//JZ20170524 trying to handle empy list gracefully.
                return searchDocuments;
            foreach (var item in items)
            {
                if (item.LastModifiedOnDate.ToUniversalTime() <= beginDate.ToUniversalTime() ||
                    item.LastModifiedOnDate.ToUniversalTime() >= DateTime.UtcNow)
                    continue;
            }
            return searchDocuments;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="moduleId">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int moduleId)
        {
            var controller = new ItemController();
            var items = controller.GetItems(moduleId);
            var sb = new StringBuilder();

            var itemList = items as IList<Item> ?? items.ToList();

            if (!itemList.Any()) return string.Empty;

            sb.Append("<Items>");

            foreach (Item item in itemList)
            {
                sb.Append("<Item>");

                //sb.AppendFormat("<AssignedUserId>{0}</AssignedUserId>", item.AssignedUserId);
                //sb.AppendFormat("<CreatedByUserId>{0}</CreatedByUserId>", item.CreatedByUserId);
                //sb.AppendFormat("<CreatedOnDate>{0}</CreatedOnDate>", item.CreatedOnDate);
                //sb.AppendFormat("<ItemId>{0}</ItemId>", item.ItemId);
                //sb.AppendFormat("<ItemDescription>{0}</ItemDescription>", XmlUtils.XMLEncode(item.ItemDescription));
                //sb.AppendFormat("<ItemName>{0}</ItemName>", XmlUtils.XMLEncode(item.ItemName));
                //sb.AppendFormat("<LastModifiedByUserId>{0}</LastModifiedByUserId>", item.LastModifiedByUserId);
                //sb.AppendFormat("<LastModifiedOnDate>{0}</LastModifiedOnDate>", item.LastModifiedOnDate);
                //sb.AppendFormat("<ModuleId>{0}</ModuleId>", item.ModuleId);

                sb.Append("</Item>");
            }

            sb.Append("</Items>");

            // you might consider doing something similar here for any important module settings

            return sb.ToString();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="moduleId">The Id of the module to be imported</param>
        /// <param name="content">The content to be imported</param>
        /// <param name="version">The version of the module to be imported</param>
        /// <param name="userId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int moduleId, string content, string version, int userId)
        {
            var controller = new ItemController();
            var items = DotNetNuke.Common.Globals.GetContent(content, "Items");
            var xmlNodeList = items.SelectNodes("Item");

            if (xmlNodeList == null) return;

            foreach (XmlNode item in xmlNodeList)
            {
                var newItem = new Item()
                {
                    ModuleId = moduleId,
                    // assigning everything to the current UserID, because this might be a new DNN installation
                    // your use case might be different though
                    CreatedByUserId = userId,
                    LastModifiedByUserId = userId,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                };

                //// NOTE: If moving from one installation to another, this user will not exist
                //newItem.AssignedUserId = int.Parse(item.SelectSingleNode("AssignedUserId").InnerText, NumberStyles.Integer);
                //newItem.ItemDescription = item.SelectSingleNode("ItemDescription").InnerText;
                //newItem.ItemName = item.SelectSingleNode("ItemName").InnerText;

                controller.CreateItem(newItem);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        public string UpgradeModule(string version)
        {
            try
            {
                switch (version)
                {
                    case "00.00.01":
                        // run your custom code here
                        return "success";
                    default:
                        return "success";
                }
            }
            catch
            {
                return "failure";
            }
        }

        #endregion
    }
}