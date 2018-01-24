<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Ipreo._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin-top:20px">
        <h4>This application calculates the distance between two given subway stations in New York City. <br /> Please select
            the stations you would like to compare and click the Calculate button.
        </h4>
    </div>
    <div class="container" style="margin-top:20px">
        <div class="row">
            <div class="col-md-3">
                <label>Subway Station A</label>
                <asp:DropDownList ID="ddlStationA" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label>Subway Station B</label>
                <asp:DropDownList ID="ddlStationB" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-3" style="margin-top:15px;">
                <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <div ID="divResult" class="container" style="margin-top:15px;">
    </div>

    <script type="text/javascript" src='<%= ResolveUrl("~/Scripts/Ipreo.js") %>'></script>
</asp:Content>