<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.Entities.Employee>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<% using (Html.BeginForm("Insert", "Employee"))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.EmployeeGeneralInfo %></legend>
    <script type="text/javascript">

        function JsonAdd_OnComplete(context) {

            var jsonAdd = context.get_response().get_object();
            if (jsonAdd.Success) {
                $("#result").html(jsonAdd.PartialViewHtml);
            }
        }
    </script>
    <table>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.FirstName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.FatherName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MiddleName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.LastName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MotherName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.FirstNameL2)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.FatherNameL2)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MiddleNameL2)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.LastNameL2)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MotherNameL2)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.DateOfBirth) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.PlaceOfBirth)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Nationality)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.OtherNationality)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Gender)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.BloodType)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Religion) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Race)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MaritalStatus)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.LoginName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MilitaryStatus)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.NoOfDependents)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.IdentificationNo)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.SocialSecurityNo)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.DisabilityExist)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.DisabilityDescription)%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <input type="submit" value="Save" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 33%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NameInFirstLanguage %></legend>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.FirstName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.FirstName) %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.FatherName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.FatherName)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.MiddleName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.MiddleName) %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.LastName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.LastName) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.MotherName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.MotherName) %>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top;">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NameInSecondLanguage %></legend>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.FirstNameL2) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.FirstNameL2) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.FatherNameL2) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.FatherNameL2)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.MiddleNameL2) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.MiddleNameL2) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.LastNameL2) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.LastNameL2) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.MotherNameL2) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.MotherNameL2) %>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top;">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NationalitiesAndBirthInformation %></legend>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.DateOfBirth) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DatePickerFor(model => model.DateOfBirth).Value(Model.DateOfBirth.Date).Min(DateTime.MinValue)%>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.CountryOfBirth) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.CountryOfBirth.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfCountries)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                     <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.PlaceOfBirth) %>
                    </div>
                    <div class="editor-field">
                         <%: Html.TextBoxFor(model => model.PlaceOfBirth) %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Nationality) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.Nationality.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfNationalities)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.OtherNationality) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.OtherNationality.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfNationalities)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.PersonalInformation %></legend>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Gender) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.Gender.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfGenders)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.BloodType) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.BloodType.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfBloodTypes)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Religion) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.Religion.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfReligions)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Race) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.Race.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfRaces)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.FamilyDetails %></legend>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.MaritalStatus) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.MaritalStatus.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfMaritalStatuses)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.LoginName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.LoginName)%>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Miscellaneous %></legend>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.MilitaryStatus) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.MilitaryStatus.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfMilitaryStatuses)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.IdentificationNo) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.IdentificationNo) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.SocialSecurityNo) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.SocialSecurityNo) %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.DisabilityExist) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.DisabilityExist.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfBoolCondition)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.DisabilityDescription) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextAreaFor(model => model.DisabilityDescription)%>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <a href="<%: Url.Action("Index", "Employee") %>">
                    <img src="<%: Url.Content("~/Content/Ribbon/Icons/48/90.png") %>" title='Back To Main Page'
                        alt="Back To Main Page" height="24" width="24" />
                </a>
            </td>
            <td>
                <input type="submit" value="Save" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
