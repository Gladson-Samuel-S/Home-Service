using HomeServiceWebApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeServiceWebApp.Controllers
{
    public class ExcelController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public ExcelController()
        {
            _context = new ApplicationDbContext();
        }

        public ExcelController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public FileResult ExportFeedbackReportToExcel()
        {
            var userOrder = _context.Orders.Where(u => u.UserReviewId != null).ToList();

            if (userOrder.Any())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excelPackage = new ExcelPackage();
                var workSheet = excelPackage.Workbook.Worksheets.Add("UserFeedbackReport");

                workSheet.Cells["A1"].Value = "User";
                workSheet.Cells["B1"].Value = "Service Name";
                workSheet.Cells["C1"].Value = "Vendor";
                workSheet.Cells["D1"].Value = "Rating";
                workSheet.Cells["E1"].Value = "Review";
                int row = 2;

                foreach (var item in userOrder)
                {
                    workSheet.Cells[$"A{row}"].Value = $"{item.ApplicationUser.FirstName} {item.ApplicationUser.LastName}";
                    workSheet.Cells[$"B{row}"].Value = item.Service.ServiceName;
                    workSheet.Cells[$"C{row}"].Value = item.Service.ApplicationUser.FirstName;
                    workSheet.Cells[$"D{row}"].Value = item.Review.Rating;
                    workSheet.Cells[$"E{row}"].Value = item.Review.Review;
                    row++;
                }
                workSheet.Cells["A:AZ"].AutoFitColumns();

                return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserFeedbackReport.xlsx");
            }
            else
            {
                var excelPackage = new ExcelPackage();
                var workSheet = excelPackage.Workbook.Worksheets.Add("UserFeedbackReport");

                workSheet.Cells["A1"].Value = "No data";

                return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserFeedbackReport.xlsx");
            }
        }

        [Authorize(Roles = "Admin")]
        public FileResult ExportOrderDetailsToExcel()
        {
            var userOrders = _context.Orders.ToList();

            var maxOrdersForAService = userOrders.GroupBy(n => n.ServiceId)
                                         .Select(n => new
                                         {
                                             ServiceName = n.Select(s => s.Service.ServiceName),
                                             ServiceVendor = n.Select(u => u.ApplicationUser.FirstName),
                                             ServiceCount = n.Count()
                                         })
                                         .OrderByDescending(n => n.ServiceCount).Take(1);

            var maxServiceOrderedByUser = userOrders.GroupBy(n => n.ApplicationUserId)
                                             .Select(n => new
                                             {
                                                 UserName = n.Select(u => u.ApplicationUser.FirstName),
                                                 UserCount = n.Count()
                                             })
                                             .OrderByDescending(n => n.UserCount).Take(1);

            if (userOrders.Any())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excelPackage = new ExcelPackage();
                var workSheet = excelPackage.Workbook.Worksheets.Add("OrderDetails");

                workSheet.Cells["A1"].Value = "User";
                workSheet.Cells["B1"].Value = "Service Name";
                workSheet.Cells["C1"].Value = "Vendor";
                workSheet.Cells["D1"].Value = "OrderId";
                workSheet.Cells["E1"].Value = "TransactionId";
                workSheet.Cells["F1"].Value = "Maximum Orders for Service Name";
                workSheet.Cells["G1"].Value = "Maximum Orders By a User";

                int row = 2;

                foreach (var item in userOrders)
                {
                    workSheet.Cells[$"A{row}"].Value = $"{item.ApplicationUser.FirstName} {item.ApplicationUser.LastName}";
                    workSheet.Cells[$"B{row}"].Value = item.Service.ServiceName;
                    workSheet.Cells[$"C{row}"].Value = item.Service.ApplicationUser.FirstName;
                    workSheet.Cells[$"D{row}"].Value = item.OrderId;
                    workSheet.Cells[$"E{row}"].Value = item.TransactionId;
                    row++;
                }

                workSheet.Cells[$"F2"].Value = maxOrdersForAService.First().ServiceName;
                workSheet.Cells[$"G2"].Value = maxServiceOrderedByUser.First().UserName;

                workSheet.Cells["A:AZ"].AutoFitColumns();

                return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserFeedbackReport.xlsx");
            }
            else
            {
                var excelPackage = new ExcelPackage();
                var workSheet = excelPackage.Workbook.Worksheets.Add("UserFeedbackReport");

                workSheet.Cells["A1"].Value = "No data";

                return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserFeedbackReport.xlsx");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}