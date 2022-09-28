<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ControlsSection" runat="server">
<div class="content-container-fluid">
        <div class="row">
            <div>
                <div id="TextArea" contenteditable="true">
                    Facebook is a social networking service headquartered in Menlo Park, California. Its website was launched on February 4, 2004, by Mark Zuckerberg with his Harvard College roommates and fellow students Eduardo, Andrew McCollum, Dustin and Chris Hughes.
                    The fouders had initially limited the websites membrship to Harvard students, but later expanded it to collges in the Boston area, the Ivy League, and Stanford Univrsity. It graually added support for students at various other universities and later to high-school students.
                </div>
                <div>
                    <ej:Button ID="btn_Default" Type="Button" ClientSideOnClick="showInDialog" Text="Spell check using dialog" runat="server"></ej:Button>
                </div>
            </div>
        </div>
    </div>
    <ej:SpellCheck ID="SpellCheck" ClientIDMode="Static" runat="server" AjaxDataType="json" ActionBegin="onActionBegin" AjaxRequestType="POST"  ControlsToValidate="#TextArea">
        <DictionarySettings DictionaryUrl="/Default.aspx/CheckWords" CustomDictionaryUrl="/Default.aspx/AddToDictionary" />
    </ej:SpellCheck>  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptSection" runat="server">
    <script type="text/javascript">
            function showInDialog() {
            var spellObj = $("#SpellCheck").data("ejSpellCheck");
            spellObj.showInDialog();
        }
            function onActionBegin(e){
                e.webMethod = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="StyleSection" runat="server">
    <style type="text/css">
        .e-groupbutton>.e-ul>.e-grp-btn-item .e-btn-content.e-groupBtn-padding{
            height:20px;
        }
        .e-groupbutton .e-grp-btn-item .e-icon{
            padding:0px;
        }
    </style>
</asp:Content>



