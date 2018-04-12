/*
' Copyright (c) 2017  virtualforce.io
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
using System.Web.UI.WebControls;
using VF.ANB.Modules.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Entities.Users;
using System.Globalization;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace VF.ANB.Modules
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from VF_POS_RequestFormModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : VF_POS_RequestFormModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
                IncludeScripts(culture.ToString());
                //if (!IsPostBack)
                {
                    var errorMessageText = LocalizeString("lblErrorMessage.Text");

                    reCaptcha recap = new reCaptcha();
                    this.LocalResourceFile += culture.ToString().ToLower().Contains("ar-") ? "-ar.ascx.resx" : ".ascx.resx";
                    if (culture.ToString().ToLower().Contains("ar-"))
                        rdArabic.Checked = true;
                    else
                        rdEnglish.Checked = true;
                    ReqCaptcha.ErrorMessage = errorMessageText;

                    EmailValidator.ErrorMessage = errorMessageText;
                    ContactNameValidator.ErrorMessage = errorMessageText;
                    MobileNumberValidator.ErrorMessage = errorMessageText;
                    NumPOSValidator.ErrorMessage = errorMessageText;
                    string redirectURL = LocalizeString("redirectURL.Text");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "setRedirectURL(\"" + redirectURL + "\");", true);//open Modal and redirect to main page
                    
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!(NumPOSValidator.IsValid && ReqCaptcha.IsValid && MobileNumberValidator.IsValid && ContactNameValidator.IsValid && EmailValidator.IsValid))                
                return;//if any one validator fails then return
            try
            {
                //throw new Exception("Debugging >> Testing Exceptional Pathway for POS");
                btn_submit3.Disabled = true;
                var t = new Item();
                var tc = new ItemController();
                string internalTeamTemplate = LocalizeString("lblPOSInternalNotification.Text");
                t = new Item()
                {
                    Name = txtName.Text.Trim(),
                    NumPOS = Convert.ToInt32(txtNumPOS.Text),
                    ContactName = txtContactName.Text.Trim(),
                    MobileNumber = txtMobileNumber.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PrefferedLanguage = rdArabic.Checked ? "ar-sa" : "en-us",
                    CreatedByUserId = UserId,
                    CreatedOnDate = DateTime.Now,
                };
                t.LastModifiedOnDate = DateTime.Now;
                t.LastModifiedByUserId = UserId;
                t.ModuleId = ModuleId;
                tc.CreateItem(t);
                Item lastRequest = tc.GetItemWithID(t, ModuleId);
                //Send Email using the culture specific text
                string refNumber = t.Name + "-" + lastRequest.ItemId;
                internalTeamTemplate = PlugInValues(t, internalTeamTemplate, refNumber);
                string customerEmailTemplate = PlugInValues(t, LocalizeString("lblEmailContents.Text"), refNumber);
                string subject = PlugInValues(t, LocalizeString("lblConfirmationEmailSubject.Text"), refNumber);
                SendEmail(ConfigurationManager.AppSettings["POSEmail"], t.Email, subject, customerEmailTemplate);//send email to client
                SendEmail("no-reply@anb.com.sa", ConfigurationManager.AppSettings["POSEmail"], subject, internalTeamTemplate);//send email to pos staff
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal(\"" + refNumber + "\");", true);//open Modal and redirect to main page                
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "submissionFailed();", true);//show error message to the user
            }
        }

        private static string PlugInValues(Item t, string internalTeamTemplate, string refNumber)
        {
            internalTeamTemplate = internalTeamTemplate.Replace("<MerchantName>", t.Name);
            internalTeamTemplate = internalTeamTemplate.Replace("<CustomerName>", t.ContactName);
            internalTeamTemplate = internalTeamTemplate.Replace("<NumOfPOS>", t.NumPOS.ToString());
            internalTeamTemplate = internalTeamTemplate.Replace("<CustomerMobile>", t.MobileNumber);
            internalTeamTemplate = internalTeamTemplate.Replace("<CustomerEmail>", t.Email);
            internalTeamTemplate = internalTeamTemplate.Replace("<Language>", t.PrefferedLanguage == "en-us" ? "English" : "Arabic");
            internalTeamTemplate = internalTeamTemplate.Replace("<ReferenceID>", refNumber);
            return internalTeamTemplate;
        }

        protected void IncludeScripts(string CurrentCulture)
        {
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<script language='javascript' >$(document).ready(function(){document.getElementById('" + txtCaptcha.ClientID+"').setAttribute('data-sitekey', '" + ConfigurationManager.AppSettings["reCaptchaKey"] + "');});</script>"));
            if (CurrentCulture.ToLower().Contains("ar-")) {
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("module-ar.css") + "\" />"));
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<script src='https://www.google.com/recaptcha/api.js?hl=ar' ></script>"));
            }
            else {
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("module-en.css") + "\" />"));
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<script src='https://www.google.com/recaptcha/api.js?hl=en' ></script>"));
            }
        }

        private static void SendEmail(string sendFromEmail, string sendToEmail, string sendToTitle, string msg)
        {
            try
            {                
                DotNetNuke.Services.Mail.Mail.SendEmail(sendFromEmail, sendToEmail, sendToTitle, msg);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }

        protected void EmailValidation(object source, ServerValidateEventArgs args)
        {
            bool isValid = false;
            string inputValue = txtEmail.Text.Trim();

            var regex = @"^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$";//<--this is from anb website, and that is what we found from web-->;@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            var match = Regex.Match(inputValue, regex, RegexOptions.IgnoreCase);

            args.IsValid = (match.Success) || isValid;
        }

        protected void ContactNameValidation(object source, ServerValidateEventArgs args)
        {
            bool isValid = false;
            string inputValue = txtContactName.Text.Trim();

            var regex = @"^([a-zA-Z0-9\s.-]|[\p{IsArabic}]|[_\s.-])*$";
            var match = Regex.Match(inputValue, regex, RegexOptions.IgnoreCase);

            args.IsValid = (match.Success) || isValid;
        }

        protected void MobileNumberValidation(object source, ServerValidateEventArgs args)
        {
            bool isValid = false;
            string inputValue = txtMobileNumber.Text.Trim();
            inputValue = Regex.Replace(inputValue, @" ", "");
            inputValue = inputValue.Replace("(0)", "").Replace("(", "").Replace(")", "").Replace("-", "");
            var regex = @"^(00966|966|\+966|05)(5|0|3|6|4|9|1|8|7)([0-9]{7,8})$";
            var match = Regex.Match(inputValue, regex, RegexOptions.IgnoreCase);

            args.IsValid = (match.Success) || isValid;
        }

        protected void NumPOSValidation(object source, ServerValidateEventArgs args)
        {
            int inputValue = -1;
            args.IsValid = false;
            try
            {
                inputValue = int.Parse(txtNumPOS.Text);
                args.IsValid = inputValue > 0 && inputValue < 100;
            }
            catch (Exception ex) {
                Exceptions.LogException(ex);
            }
        }
    }
}