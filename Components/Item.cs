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
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;

namespace VF.ANB.Modules.Components
{
    [TableName("VF_POS_RequestForm_Items")]
    //setup the primary key for table
    [PrimaryKey("ItemId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Items", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    class Item
    {
        ///<summary>
        /// The UniqueId of the Merchant
        ///</summary>
        public int ItemId { get; set; }

        ///<summary>
        /// The Name of the Merchant
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// The number of POSs
        ///</summary>
        public int NumPOS { get; set; }

        ///<summary>
        /// The Contact Name
        ///</summary>
        public string ContactName { get; set; }

        ///<summary>
        /// The Mobile Number
        ///</summary>
        public string MobileNumber { get; set; }

        ///<summary>
        /// The Email
        ///</summary>
        public string Email { get; set; }

        ///<summary>
        /// The Preffered Language
        ///</summary>
        public string PrefferedLanguage { get; set; }

        ///<summary>
        /// The ModuleId of where the object was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        /// An integer for the user id of the user who created the object
        ///</summary>
        public int CreatedByUserId { get; set; }

        ///<summary>
        /// An integer for the user id of the user who last updated the object
        ///</summary>
        public int LastModifiedByUserId { get; set; }

        ///<summary>
        /// The date the object was created
        ///</summary>
        public DateTime CreatedOnDate { get; set; }

        ///<summary>
        /// The date the object was updated
        ///</summary>
        public DateTime LastModifiedOnDate { get; set; }
    }
}
