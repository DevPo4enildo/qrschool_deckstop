using Dapper;
using qrschool_deckstop.Models;
using System.Collections.Generic;
using System.Linq;

namespace qrschool_deckstop.DataAccess
{
    public class MonitorRepository : IRepository<Monitor>
    {
        public IEnumerable<Monitor> GetAll()
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                SELECT m.*, r.name as RoomName, e.full_name as ResponsibleEmployee
                FROM monitors m
                LEFT JOIN rooms r ON m.room_id = r.id
                LEFT JOIN employees e ON m.responsible_employee_id = e.id
                ORDER BY m.id";
            return connection.Query<Monitor>(sql);
            }
        }

        public Monitor GetById(int id)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = "SELECT * FROM monitors WHERE id = @Id";
            return connection.QueryFirstOrDefault<Monitor>(sql, new { Id = id });
            }
        }

        public int Add(Monitor entity)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                INSERT INTO monitors (
                    code, inventory_no, room_id, computer_id, brand, model, diagonal_inch,
                    serial_number, status, comment, cost, purchase_date, warranty_until,
                    responsible_employee_id, notes, created_at, updated_at, created_by,
                    updated_by, sync_status
                ) VALUES (
                    @Code, @InventoryNo, @RoomId, @ComputerId, @Brand, @Model, @DiagonalInch,
                    @SerialNumber, @Status, @Comment, @Cost, @PurchaseDate, @WarrantyUntil,
                    @ResponsibleEmployeeId, @Notes, @CreatedAt, @UpdatedAt, @CreatedBy,
                    @UpdatedBy, @SyncStatus
                ) RETURNING id";
            return connection.QuerySingle<int>(sql, entity);
            }
        }

        public bool Update(Monitor entity)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                UPDATE monitors SET
                    code = @Code,
                    inventory_no = @InventoryNo,
                    room_id = @RoomId,
                    computer_id = @ComputerId,
                    brand = @Brand,
                    model = @Model,
                    diagonal_inch = @DiagonalInch,
                    serial_number = @SerialNumber,
                    status = @Status,
                    comment = @Comment,
                    cost = @Cost,
                    purchase_date = @PurchaseDate,
                    warranty_until = @WarrantyUntil,
                    responsible_employee_id = @ResponsibleEmployeeId,
                    notes = @Notes,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy,
                    sync_status = @SyncStatus
                WHERE id = @Id";
            return connection.Execute(sql, entity) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = "DELETE FROM monitors WHERE id = @Id";
            return connection.Execute(sql, new { Id = id }) > 0;
            }
        }
    }
}
