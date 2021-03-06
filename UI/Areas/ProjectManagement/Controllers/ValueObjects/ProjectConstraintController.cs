#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class ProjectConstraintController : ProjectAggregateController, IRule<ProjectConstraint>
    {
        #region Parents Chain

        #region Project

        private Project _project;

        public Project FirstEntity
        {
            get
            {
                return _project ??
                       (_project = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region ProjectConstraint

        private ProjectConstraint _projectConstraint;

        public ProjectConstraint SecondEntity
        {
            get
            {
                return _projectConstraint ??
                       (_projectConstraint =
                        FirstEntity.Constraints.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<ProjectConstraint> Members

        public ObjectRules<ProjectConstraint> Rules
        {
            get { return new ProjectConstraintRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Constraints.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Constraints != null
                       ? Rules.GetExpiredRules(FirstEntity.Constraints)
                       : new List<BrokenBusinessRule>();
        }

        public override void CleanUpModelState()
        {
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);
            CurrentlyInSecondLevel = selectedSubRowId;

            PrePublish();

            SaveTabIndex(4);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ProjectConstraint {Project = FirstEntity});
        }

        #endregion

        #region Create

        public JsonResult Save(ProjectConstraint ProjectConstraint)
        {
            PrePublish();

            #region Permission Check

            if (ProjectConstraint.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool) ViewData["CanCreate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Error")
                                    });
                }
            }
            else
            {
                if (ViewData["CanUpdate"] != null && !(bool) ViewData["CanUpdate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Error")
                                    });
                }
            }

            #endregion

            if (ProjectConstraint.IsTransient())
            {
                FirstEntity.Addconstraint(ProjectConstraint);
            }
            else
            {
                #region Retrieve Direct Parent

                ProjectConstraint.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(ProjectConstraint, SecondEntity);
            }

            if ((Rules.GetBrokenRules(ProjectConstraint).Count == 0) && (TryValidateModel(ProjectConstraint)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(ProjectConstraint));

                FirstEntity.Constraints.Remove(ProjectConstraint);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", ProjectConstraint)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, ProjectConstraint.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", ProjectConstraint)
                            });
        }

        #endregion

        #region Update

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool) ViewData["CanDelete"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanDeleteMessage);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            try
            {
                ProjectConstraint ProjectConstraint = FirstEntity.Constraints.SingleOrDefault(x => x.Id == id);

                FirstEntity.Constraints.Remove(ProjectConstraint);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Project");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        #endregion

        #endregion
    }
}