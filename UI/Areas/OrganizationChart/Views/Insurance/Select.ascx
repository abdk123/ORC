<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Insurance>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<%@ Import Namespace="Infrastructure.Validation" %>
<div style="color: Maroon; font-size: smaller;">
    <%
        if (ViewData["ExpiredRules"] != null)
        {
            foreach (BrokenBusinessRule brokenBusinessRule in ViewData["ExpiredRules"] as IList<BrokenBusinessRule>)
            {%>
    <%:Html.DisplayTextFor(model => brokenBusinessRule.Rule)%>
    <br />
    <%
            }
        }%>
    <br />
</div>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <% foreach (var item in (IEnumerable<Insurance>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 25%; vertical-align: top">
            <%: Html.HiddenFor(model => model.Id) %>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.InsuranceCoverageRatio) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.InsuranceCoverageRatio)%>
                %
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.InsuranceCompany) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.InsuranceCompany.Name, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CompanyAddress) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.CompanyAddress, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.RepresentativeContact) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.RepresentativeContact, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Delete, "Delete", "Insurance", new { Id = item.Id }, new AjaxOptions { Confirm = Resources.Shared.Messages.General.DeleteConfirm, HttpMethod = "Delete", OnComplete = "JsonDelete_OnComplete" })%>
            <% } %>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Insurance", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
<script type="text/javascript">

    function JsonDelete_OnComplete(context) {

        var JsonDelete = context.get_response().get_object();
        if (JsonDelete.Success) {
            $("#ValueObjectsList").html(JsonDelete.PartialViewHtml);
        }
    };

    function JsonEdit_OnComplete(context) {

        var JsonEdit = context.get_response().get_object();
        if (JsonEdit.Success) {
            $("#addValueObjectArea").html(JsonEdit.PartialViewHtml);
            Toggle("edit");
        }
        else {
            $("#ValueObjectsList").html(JsonAdd.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    };

</script>
