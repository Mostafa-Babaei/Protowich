using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public static class RolePermissionSeed
    {
        public static void RolePermissions(this ModelBuilder modelBuilder)
        {
            var systemAdminPermissions = Enumerable.Range(1, 136).Select(id => new RolePermission
            {
                Id = id,
                RoleId = 1,
                PermissionId = id
            });
            var otherRoles = new List<RolePermission>
            {
                // =====================================================
            // ================== COMPANY ADMIN ====================
            // =====================================================
                new RolePermission { Id = 1000, RoleId = 2, PermissionId = 1 },   // Budgeting
                new RolePermission { Id = 1001, RoleId = 2, PermissionId = 2 },
                new RolePermission { Id = 1002, RoleId = 2, PermissionId = 3 },
                new RolePermission { Id = 1003, RoleId = 2, PermissionId = 4 },
                new RolePermission { Id = 1004, RoleId = 2, PermissionId = 5 },
                new RolePermission { Id = 1005, RoleId = 2, PermissionId = 6 },   // Budget Transactions
                new RolePermission { Id = 1006, RoleId = 2, PermissionId = 13 },  // CustomerBudget
                new RolePermission { Id = 1007, RoleId = 2, PermissionId = 18 },  // Customer
                new RolePermission { Id = 1008, RoleId = 2, PermissionId = 33 },  // Employee
                new RolePermission { Id = 1009, RoleId = 2, PermissionId = 52 },  // Mission
                new RolePermission { Id = 1010, RoleId = 2, PermissionId = 63 },  // Communication
                new RolePermission { Id = 1011, RoleId = 2, PermissionId = 96 },  // Dashboard
                new RolePermission { Id = 1012, RoleId = 2, PermissionId = 97 },  // Settings
                new RolePermission { Id = 1013, RoleId = 2, PermissionId = 133 }, // Reports

                // =====================================================
                // =================== DISPATCHER ======================
                // =====================================================
                new RolePermission { Id = 2000, RoleId = 3, PermissionId = 18 }, // Customer.View
                new RolePermission { Id = 2001, RoleId = 3, PermissionId = 33 }, // Employee.View
                new RolePermission { Id = 2002, RoleId = 3, PermissionId = 34 }, // Employee.Create
                new RolePermission { Id = 2003, RoleId = 3, PermissionId = 52 }, // Mission.View
                new RolePermission { Id = 2004, RoleId = 3, PermissionId = 53 }, // Mission.Create
                new RolePermission { Id = 2005, RoleId = 3, PermissionId = 54 }, // Mission.Edit
                new RolePermission { Id = 2006, RoleId = 3, PermissionId = 55 }, // Mission.Delete
                new RolePermission { Id = 2007, RoleId = 3, PermissionId = 61 }, // Mission.Reassign
                new RolePermission { Id = 2008, RoleId = 3, PermissionId = 96 }, // Dashboard.View

                // =====================================================
                // ==================== CAREGIVER ======================
                // =====================================================
                new RolePermission { Id = 3000, RoleId = 4, PermissionId = 57 }, // Mission.Start
                new RolePermission { Id = 3001, RoleId = 4, PermissionId = 58 }, // Mission.End
                new RolePermission { Id = 3002, RoleId = 4, PermissionId = 59 }, // Mission.Complete
                new RolePermission { Id = 3003, RoleId = 4, PermissionId = 42 }, // EmployeeTimesheet.View
                new RolePermission { Id = 3004, RoleId = 4, PermissionId = 43 }, // EmployeeTimesheet.Sign
                new RolePermission { Id = 3005, RoleId = 4, PermissionId = 91 }, // Profile.View
                new RolePermission { Id = 3006, RoleId = 4, PermissionId = 93 }, // Profile.ChangePassword
                new RolePermission { Id = 3007, RoleId = 4, PermissionId = 63 }, // Message.View
                new RolePermission { Id = 3008, RoleId = 4, PermissionId = 64 }, // Message.Send

                // =====================================================
                // =================== ACCOUNTANT ======================
                // =====================================================
                new RolePermission { Id = 4000, RoleId = 5, PermissionId = 1 },  // BudgetDefinition.View
                new RolePermission { Id = 4001, RoleId = 5, PermissionId = 6 },  // BudgetTransaction.View
                new RolePermission { Id = 4002, RoleId = 5, PermissionId = 11 }, // BudgetTransaction.Recharge
                new RolePermission { Id = 4003, RoleId = 5, PermissionId = 13 }, // CustomerBudget.View
                new RolePermission { Id = 4004, RoleId = 5, PermissionId = 134 },// Report.Export
                new RolePermission { Id = 4005, RoleId = 5, PermissionId = 129 },// SubscriptionPayment.View

                // =====================================================
                // =================== HR MANAGER ======================
                // =====================================================
                new RolePermission { Id = 5000, RoleId = 6, PermissionId = 33 }, // Employee.View
                new RolePermission { Id = 5001, RoleId = 6, PermissionId = 38 }, // EmployeeOffDay.View
                new RolePermission { Id = 5002, RoleId = 6, PermissionId = 40 }, // EmployeeOffDay.Approve
                new RolePermission { Id = 5003, RoleId = 6, PermissionId = 42 }, // EmployeeTimesheet.View
                new RolePermission { Id = 5004, RoleId = 6, PermissionId = 45 }, // EmployeeTimesheet.Approve

                // =====================================================
                // ================= CUSTOMER MANAGER ==================
                // =====================================================
                new RolePermission { Id = 6000, RoleId = 7, PermissionId = 18 }, // Customer.View
                new RolePermission { Id = 6001, RoleId = 7, PermissionId = 19 }, // Customer.Create
                new RolePermission { Id = 6002, RoleId = 7, PermissionId = 20 }, // Customer.Edit
                new RolePermission { Id = 6003, RoleId = 7, PermissionId = 28 }, // CustomerDocument.View
                new RolePermission { Id = 6004, RoleId = 7, PermissionId = 29 }, // CustomerDocument.Upload

                // =====================================================
                // ====================== VIEWER =======================
                // =====================================================
                new RolePermission { Id = 7000, RoleId = 8, PermissionId = 18 }, // Customer.View
                new RolePermission { Id = 7001, RoleId = 8, PermissionId = 33 }, // Employee.View
                new RolePermission { Id = 7002, RoleId = 8, PermissionId = 52 }, // Mission.View
                new RolePermission { Id = 7003, RoleId = 8, PermissionId = 133 },// Report.View
                new RolePermission { Id = 7004, RoleId = 8, PermissionId = 96 }, // Dashboard.View

                // =====================================================
                // ==================== DEVELOPER ======================
                // =====================================================
                new RolePermission { Id = 8000, RoleId = 9, PermissionId = 117 }, // ErrorLog.View
                new RolePermission { Id = 8001, RoleId = 9, PermissionId = 97 },  // Settings.View
                new RolePermission { Id = 8002, RoleId = 9, PermissionId = 90 },  // Menu.Sync
                new RolePermission { Id = 8003, RoleId = 9, PermissionId = 133 }, // Report.View
                new RolePermission { Id = 8004, RoleId = 9, PermissionId = 1 }    // BudgetDefinition.View
            };
            modelBuilder.Entity<RolePermission>().HasData(
                systemAdminPermissions.Concat(otherRoles).ToArray()
            );
        }
    }
}
