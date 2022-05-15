using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.Enums;
using System.Collections;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult OrgChartDashboard()
        {
            return PartialView();
        }

        public ActionResult GetOrgCharts()
        {
            var data = new ArrayList();
            var nodes = ServiceFactory.ORMService.All<Node>().ToList();
            foreach (var node in nodes)
            {
                int employeeCount = GetEmployeesCount(nodes, node.Id); ;
                

                data.Add(new
                {
                    name = node.Name,
                    id = node.Id.ToString(),
                    parent = node.Parent != null ? node.Parent.Id.ToString() : string.Empty,
                    value = employeeCount
                });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public int GetEmployeesCount(IList<Node> nodes, int nodeId)
        {
            int employeeCount = 0;
            var jobDescriptionIds = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.Node.Id == nodeId).Select(x => x.Id).ToArray();

            var employees = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                .Where(x => jobDescriptionIds.Contains(x.Position.JobDescription.Id))
                .Select(x => x.Employee).Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned).ToList().Distinct();

            employeeCount += employees.Count();

            var childrenNodes = nodes.Where(x=> x.Parent!=null && x.Parent.Id==nodeId).ToList();
            if (childrenNodes.Any())
            {
                foreach (var node in childrenNodes)
                {
                    employeeCount += GetEmployeesCount(nodes, node.Id);
                }
                
            }

            return employeeCount;
        }

    }
}
