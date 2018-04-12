<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="VF.ANB.Modules.View" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>

    <div class="dnnForm dnnEditBasicSettings" id="dnnEditBasicSettings">
        <!--<div class="dnnFormExpandContent dnnRight "><a href=""><%=LocalizeString("ExpandAll")%></a></div>-->

        <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead dnnClear">
            <a href="" class="dnnSectionExpanded">
                <%# LocalizeString("BasicSettings.Text")%></a></h2>
        <fieldset>
            <!--<div>
                <label id="erorMessage"></label>
            </div>-->
            <div class="dnnFormItem">
                <dnn:label ID="lblName" runat="server" />
                <asp:TextBox ID="txtName" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblNumPOS" runat="server" />
                <asp:TextBox id="txtNumPOS" runat="server"/>
                <div class="Valcont error">
                    <asp:CustomValidator Display="Dynamic" ValidateEmptyText="true" ID="NumPOSValidator" runat="server" ValidationGroup="ntx_val" ControlToValidate="txtNumPOS" ClientValidationFunction="validateNumPOS" ErrorMessage="Invalid Email" ForeColor="#E11107" SetFocusOnError="True" Font-Size="13px" OnServerValidate="NumPOSValidation"></asp:CustomValidator>
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblContactName" runat="server" />
                <asp:TextBox ID="txtContactName" runat="server"/>
                <div class="Valcont error" style="">
                    <asp:CustomValidator Display="Dynamic" ValidateEmptyText="true" ID="ContactNameValidator" runat="server" ValidationGroup="ntx_val" ControlToValidate="txtContactName" ClientValidationFunction="validateContactName" ErrorMessage="Invalid Email" ForeColor="#E11107" SetFocusOnError="True" Font-Size="13px" OnServerValidate="ContactNameValidation"></asp:CustomValidator>
                </div>
            </div>
             <div class="dnnFormItem">
                <dnn:label ID="lblMobileNumber" runat="server" />
                <asp:TextBox ID="txtMobileNumber" runat="server"/>
                 <div class="Valcont error" style="">
                    <asp:CustomValidator Display="Dynamic" ValidateEmptyText="true" ID="MobileNumberValidator" runat="server" ValidationGroup="ntx_val" ControlToValidate="txtMobileNumber" ClientValidationFunction="validateMobileNumber" ErrorMessage="Invalid Email" ForeColor="#E11107" SetFocusOnError="True" Font-Size="13px" OnServerValidate="MobileNumberValidation"></asp:CustomValidator>
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblEmail" runat="server" />
                <asp:TextBox type="email" ID="txtEmail" runat="server"/>
                <div class="Valcont error" style="">
                    <asp:CustomValidator Display="Dynamic" ValidateEmptyText="true" ID="EmailValidator" runat="server" ValidationGroup="ntx_val" ControlToValidate="txtEmail" ClientValidationFunction="validateEmail" ErrorMessage="Invalid Email" ForeColor="#E11107" SetFocusOnError="True" Font-Size="13px" OnServerValidate="EmailValidation"></asp:CustomValidator>
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:label ID="lblPrefferedLanguage" runat="server" />
                <div class="dnnFormItemRadio">
                    <asp:RadioButton ID="rdEnglish" runat="server" GroupName="Language" Text="English"></asp:RadioButton>
                    <asp:RadioButton ID="rdArabic" runat="server" GroupName="Language" Text="عربى"></asp:RadioButton>
                </div>
            </div>
            <div>
                <br />                
                <div class="g-recaptcha" data-sitekey="jzjzjzjz" id="txtCaptcha" runat="server"></div>
                <div class="Valcont error errorCaptcha" style="margin-top:0;">
                    <asp:CustomValidator ValidateEmptyText="true" ID="ReqCaptcha" class="captchaEr" runat="server" Display="Dynamic"
                    ValidationGroup="ntx_val" ForeColor="#E11107" Font-Size="13px"
                    ClientValidationFunction="ntx_validateRecap"></asp:CustomValidator>
                    <div id="captchaEr" class="captchaEr" runat="server"></div>
                </div>
                <button id="btn_submit3" causesvalidation="true" validationgroup="ntx_val" class="btn btn-4 btn-4a btn-default btn-footer ComplaintMsgGrp dnnPrimaryAction" runat="server" onserverclick="btnSubmit_Click"><%=LocalizeString("btnSubmit.Text") %> </button>
            </div>
        </fieldset>
    </div>

<script>   
    document.addEventListener("DOMContentLoaded", function () {
        var elements = document.getElementsByTagName("INPUT");
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].name.indexOf("_View_") > -1) {//work only for our added fields...
                elements[i].oninvalid = function (e) {
                    e.target.setCustomValidity("");
                    if (!e.target.validity.valid) {
                        e.target.setCustomValidity('<%#LocalizeString("lblErrorMessage")%>');
                        //TOOD: apply a class to this element to highlight it.
                    }
                };
                elements[i].oninput = function (e) {
                    e.target.setCustomValidity("");
                };
            }
            
            
        }
    });

    function ntx_validateRecap(oSrc, args) {

        if (grecaptcha.getResponse() == "")
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    function validateNumPOS (oSrc, args) {
        args.IsValid = /^[0-9]{1,2}$/i.test(args.Value) && Number(args.Value) > 0 && Number(args.Value) < 100;
        if (!args.IsValid) {
            $("input[id$='txtNumPOS']").css("border-color", "#e11107");
            $("label[id$='lblNumPOS']").css("color", "#e11107");
            $("input[id$='txtNumPOS']").css("background-color", "#f2f2f2");
        }
        else {
            $("input[id$='txtNumPOS']").css("border-color", "#12af37");
            $("input[id$='txtNumPOS']").css("background-color", "#e6f3e9");
            $("label[id$='lblNumPOS']").css("color", "#4f4f4f");
        }
    }

    function validateEmail(oSrc, args) {
        //args.IsValid = /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i.test(args.Value);
        args.IsValid = /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i.test(args.Value); //this is from anb website
        //this was what we found on the internet. args.IsValid = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/i.test(args.Value);
        if (!args.IsValid) {
            $("input[id$='txtEmail']").css("border-color", "#e11107");
            $("label[id$='lblEmail']").css("color", "#e11107");
            $("input[id$='txtEmail']").css("background-color", "#f2f2f2");
        }
        else {
            $("input[id$='txtEmail']").css("border-color", "#12af37");
            $("input[id$='txtEmail']").css("background-color", "#e6f3e9");
            $("label[id$='lblEmail']").css("color", "#4f4f4f");
        }
    }

    function validateContactName(oSrc, args) {
        //completely theirs!
        args.IsValid = $.trim(args.Value) != "" && !/[^\w\s\u0621-\u064A\u0660-\u0669-]/.test(args.Value);
        //args.IsValid = /[^\w\s\u0621-\u064A\u0660-\u0669-]/.test(args.Value);//<--theirs ;ours-->/^([a-zA-Z0-9\s.-]|[\p{Arabic}]|[_\s.-])*$/i.test(args.Value);
        if (!args.IsValid) {
            $("input[id$='txtContactName']").css("border-color", "#e11107");
            $("label[id$='lblContactName']").css("color", "#e11107");
            $("input[id$='txtContactName']").css("background-color", "#f2f2f2");
        }
        else {
            $("input[id$='txtContactName']").css("border-color", "#12af37");
            $("input[id$='txtContactName']").css("background-color", "#e6f3e9");
            $("label[id$='lblContactName']").css("color", "#4f4f4f");
        }
    }

    function validateMobileNumber(oSrc, args) {
        args.Value = args.Value.replace(/ /g, "").replace("(0)","").replace("(","").replace(")","").replace("-",""); 
        args.IsValid = /^(00966|966|\+966|05)(5|0|3|6|4|9|1|8|7)([0-9]{7,8})$/.test(args.Value);
        if (!args.IsValid) {
            $("input[id$='txtMobileNumber']").css("border-color", "#e11107");
            $("label[id$='lblMobileNumber']").css("color", "#e11107");
            $("input[id$='txtMobileNumber']").css("background-color", "#f2f2f2");
        }
        else {
            $("input[id$='txtMobileNumber']").css("border-color", "#12af37");
            $("input[id$='txtMobileNumber']").css("background-color", "#e6f3e9");
            $("label[id$='lblMobileNumber']").css("color", "#4f4f4f");
        }
    }

    function ntx_validateMobile(oSrc, args) {
        if ($("input[id$='ntx_mobileInput']").val().length > 0) {
            args.IsValid = /^(009665|9665|\+9665|05)(5|0|3|6|4|9|1|8|7)([0-9]{7,8})$/.test(args.Value);
            if (!args.IsValid) {
                $("input[id$='ntx_mobileInput']").css("border-color", "#e11107");
                $("label[id$='mobileLbl']").css("color", "#e11107");
                $("input[id$='ntx_mobileInput']").css("background-color", "#f2f2f2");
            }
            else {
                $("input[id$='ntx_mobileInput']").css("border-color", "#12af37");
                $("input[id$='ntx_mobileInput']").css("background-color", "#e6f3e9");
                $("label[id$='mobileLbl']").css("color", "#4f4f4f");
            }
        }
        else {
            args.IsValid = false;
            $("label[id$='mobileLbl']").attr('style', 'color:#e11107;');
            $("input[id$='ntx_mobileInput']").css("border-color", "#e11107");
            $("input[id$='ntx_mobileInput']").css("background-color", "none");
        }
    }

    function openModal(refNumber) {
        $('#success').show();
        $('#failure').hide();
        $('#closeModel').attr("onclick", redirectURL);
        $('#refNumber').html(refNumber);
        $('#myModal').modal('show');
    }
    function submissionFailed() {
        $('#success').hide();
        $('#failure').show();
        $('#closeModel').removeAttr("onclick");
        $('#myModal').modal('show');
    }
    function setRedirectURL(theURL) {
        redirectURL = theURL;
    }
    var redirectURL = "";
</script>

<!-- Trigger the modal with a button -->
<!--<button id="modalButtonClick" type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal">Open Modal</button>-->

<!-- Modal -->
<div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title"><%=LocalizeString("lblConfirmationHeading.Text")%></h4>
      </div>
      <div class="modal-body">
        <div id="success"><p><%=LocalizeString("lblConfirmationMessage.Text")%> <div id="refNumber"></div></p></div>
        <div id="failure"><p><%=LocalizeString("lblFailureMessage.Text")%> </p></div>
      </div>
      <div class="modal-footer">
        <button id="closeModel" type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>
